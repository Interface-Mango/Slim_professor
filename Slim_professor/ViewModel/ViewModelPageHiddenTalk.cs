using MVVMBase.Command;
using MVVMBase.ViewModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using SocketGlobal;
using Slim_professor.View;
using Slim_professor.Model;
using System.Windows.Media.Animation;

namespace Slim_professor.ViewModel
{
    #region 유저에게 연결할 델리게이트
    //델리게이트 선언
    /// 유저 접속 끊김
    public delegate void dgDisconnect(SocketUser sender);

    /// 유저 메시지 요청
    public delegate void dgMessage(SocketUser sender, MessageEventArgs e);
    #endregion

    class ViewModelPageHiddenTalk : ViewModelBase
    {
        /// 접속한 유저 리스트(로그인 완료전 포함)
        private List<SocketUser> m_listUser = null;

        /// 서버 소켓
        private Socket socketServer;
        private PageHiddenTalk pht;
        private TextBox txtMsg;
        private int portNum;
        private TextBlock IDText;
        private String NickName;
        private Button ServerConnectBtn;

        private DB_Subject dbSubject;

        /// 명령어 클래스
        private claCommand m_insCommand = new claCommand();
        /// 내 소켓
        private Socket m_socketMe = null;

        /// ServerConnectBtn 버튼 상태
        enum typeState
        {
            /// 연결하세요
            Connecting,

            /// 끊으세요
            DisConnecting,
        }

        /// 나의 상태
        private typeState m_typeState = typeState.Connecting;

        public ViewModelPageHiddenTalk
            (PageHiddenTalk _pht, TextBox _txtMsg, TextBlock _IDText, Button _ServerConnectBtn)
        {
            pht = _pht;
            txtMsg = _txtMsg;

            IDText = _IDText;
            NickName = randomID();
            ServerConnectBtn = _ServerConnectBtn;

            UI_Setting(typeState.Connecting);

            dbSubject = new DB_Subject(new DBManager());
            portNum = dbSubject.SelectPort(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)));
        }

        // UI 세팅
        private void UI_Setting(typeState typeSet)
        {
            //들어온 값을 세팅하고
            m_typeState = typeSet;
            

            switch (typeSet)
            {
                case typeState.Connecting:	// 연결 전
                    ServerConnectBtn.Content = "채팅 열기";
                    txtMsg.IsEnabled = false;
                    
                    break;

                case typeState.DisConnecting:	// 연결완료
                    pht.Dispatcher.BeginInvoke(new Action(
                        delegate()
                        {
                            ServerConnectBtn.Content = "채팅 종료";
                        }));
                    break;
            }
            
        }

        // ------------------------
        // professor에는 서버와 클라이언트 2개가 존재.
        // 서버지역
        // ------------------------
        #region Server

        #region ServerConnect
        private ICommand _ServerConnect;
        public ICommand ServerConnect
        {
            get { return _ServerConnect ?? (_ServerConnect = new AppCommand(ServerConnectFunc)); }
        }

        private void ServerConnectFunc(Object o)
        {
            switch (m_typeState)
            {
                case typeState.Connecting:
                    {
                        //UI 세팅
                        UI_Setting(typeState.DisConnecting);


                        //Random random = new Random();
                        //portNum = random.Next(1000) + 8124;
                        //서버 세팅
                        socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                        IPEndPoint ipepServer = new IPEndPoint(IPAddress.Any, portNum);
                        socketServer.Bind(ipepServer);
                        socketServer.Listen(20);


                        System.Net.Sockets.SocketAsyncEventArgs saeaUser = new System.Net.Sockets.SocketAsyncEventArgs();
                        //유저가 연결되었을때 이벤트
                        saeaUser.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(Accept_Completed);
                        //유저 접속 대기 시작
                        socketServer.AcceptAsync(saeaUser);


                        //유저 리스트 생성
                        m_listUser = new List<SocketUser>();
                        //서버 시작 로그 표시
                        //pht.DisplayLog("* [ " + portNum + " ] ");
                        pht.DisplayLog("* 채팅 시작 ");

                        // ip와 포트 추가
                        //dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), getMyIp, portNum);

                        ServerConnectionFunction();
                        break;
                    }
                case typeState.DisConnecting:
                    {
                        UI_Setting(typeState.Connecting);
                        //pht.DisplayLog("* [ " + portNum + " ] 채팅 종료 * \n");
                        pht.DisplayMsg("* [채팅이 종료되었습니다.] *\n");
                        pht.Dispatcher.BeginInvoke(new Action(
                        delegate()
                        {
                            IDText.Text = "ID";
                        }));
                        Commd_Server_Disconnetion();
                        socketServer.Close();
                        m_listUser.Clear();

                        dbSubject.UpdateIpaddr(Convert.ToInt32(PageMainSubject.SubjectInfo.ElementAt((int)DB_Subject.FIELD.sub_id)), "", 0);
                        break;
                    }
            }
        }

        /*
         * 자신 PC의 IP를 받아오는 프로퍼티
         */
        public string getMyIp
        {
            get
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ClientIP = host.AddressList[i].ToString();
                    }
                }
                return ClientIP;
            }
        }
        #endregion

        /// 클라이언트가 연결되면 발생
        private void Accept_Completed(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        {
            try
            {

                //클라이언트 접속
                //유저 객체를 만든다.
                SocketUser insUser = new SocketUser(e.AcceptSocket);

                //각 이벤트 연결
                insUser.OnDisconnected += insUser_OnDisconnected;
                insUser.OnMessaged += insUser_OnMessaged;

                //리스트에 클라이언트 추가
                m_listUser.Add(insUser);

                //다시 클라이언트 접속을 기다린다.
                Socket socketServer = (Socket)sender;
                e.AcceptSocket = null;
                socketServer.AcceptAsync(e);
            }

            catch(ObjectDisposedException)
            {
                // 소켓종료 탈출구
            }

        }


        /// 유저 끊김 이벤트
        private void insUser_OnDisconnected(SocketUser sender)
        {
            //리스트에서 유저를 지움
            m_listUser.Remove(sender);

        }

        /// 유저 메시지 이벤트
        public void insUser_OnMessaged(SocketUser sender, MessageEventArgs e)
        {
            StringBuilder sbMsg = new StringBuilder();

            switch (e.m_typeCommand)
            {
                case claCommand.Command.Msg:	//메시지
                    sbMsg.Append(sender.UserID);
                    sbMsg.Append(" : ");
                    sbMsg.Append(e.m_strMsg);
                    Commd_SendMsg(sbMsg.ToString());
                    break;

                case claCommand.Command.ID_Check:	//id체크
                    Commd_IDCheck(sender, e.m_strMsg);
                    break;

                //case claCommand.Command.Server_Disconnection: // 서버에서 접속 끊을때
                   // Commd_Server_Disconnetion();
                    //break;
                    
            }
        }

        // 명령처리 - 서버 접속 종료를 모든 유저들에게 알림
        private void Commd_Server_Disconnetion()
        {
            StringBuilder sbMsg = new StringBuilder();

            //명령어 부착
            sbMsg.Append(claCommand.Command.Server_Disconnection.GetHashCode().ToString());
            //구분자 부착
            sbMsg.Append(claGlobal.g_Division);

            //전체 유저에게 메시지 전송
            AllUser_Send(sbMsg.ToString());
        }


        /// 명령 처리 - 메시지 보내기
        private void Commd_SendMsg(string sMsg)
        {
            StringBuilder sbMsg = new StringBuilder();

            //-------------------------------
            //서버에도, 클라이언트에도 Log 보이게끔
            //-------------------------------
            //명령어 부착
            sbMsg.Append(claCommand.Command.Msg.GetHashCode().ToString());
            //구분자 부착
            sbMsg.Append(claGlobal.g_Division);
            //메시지 완성
            sbMsg.Append(sMsg);

            //전체 유저에게 메시지 전송
            AllUser_Send(sbMsg.ToString());

        }

        /// 명령 처리 - ID체크
        private void Commd_IDCheck(SocketUser insUser, string sID)
        {
            //사용 가능 여부
            bool bReturn = true;

            //모든 유저의 아이디 체크
            foreach (SocketUser insUserTemp in m_listUser)
            {
                if (insUserTemp.UserID == sID)
                {
                    //같은 유저가 있다!
                    //같은 유저가 있으면 그만 검사한다.
                    bReturn = false;
                    break;
                }
            }

            if (true == bReturn)
            {
                //사용 가능

                //아이디를 지정하고
                insUser.UserID = sID;

                //유저에게 로그인이 성공했음을 알림
                StringBuilder sbMsg = new StringBuilder();
                //접속자에게 먼저 로그인이 성공했음을 알린다.
                sbMsg.Append(claCommand.Command.ID_Check_Ok.GetHashCode());
                sbMsg.Append(claGlobal.g_Division);
                insUser.SendMsg_User(sbMsg.ToString());

                //유저가 접속 했음을 직접 알리지 말고 'ID_Check_Ok'를 받은
                //클라이언트가 직접 요청한다.
            }
            else
            {
                //검사 실패를 알린다.
                StringBuilder sbMsg = new StringBuilder();

                sbMsg.Append(claCommand.Command.ID_Check_Fail.GetHashCode().ToString());
                sbMsg.Append(claGlobal.g_Division);

                insUser.SendMsg_User(sbMsg.ToString());
            }
        }

        /// 접속중인 모든 유저에게 메시지를 보냅니다.
        private void AllUser_Send(string sMsg)
        {
            if (m_listUser != null)
            {
                //모든 유저에게 메시지를 전송 한다.
                foreach (SocketUser insUser in m_listUser)
                {
                    insUser.SendMsg_User(sMsg);
                }
            }

            //로그 출력
            //pht.DisplayLog(sMsg);
        }

        #endregion


        // ------------------------
        // 클라이언트 지역
        // ------------------------
        #region Cilent

        #region msgSend
        private ICommand _msgSend;
        public ICommand msgSend
        {
            get { return _msgSend ?? (_msgSend = new AppCommand(msgSendFunc)); }
        }

        private void msgSendFunc(Object o)
        {
            //메시지를 보낸다.
            StringBuilder sbData = new StringBuilder();
            sbData.Append(claCommand.Command.Msg.GetHashCode());
            sbData.Append(claGlobal.g_Division);
            sbData.Append(txtMsg.Text);

            SendMsg(sbData.ToString());
            txtMsg.Text = "";
        }
        #endregion

        private void ServerConnectionFunction()
        {
            txtMsg.IsEnabled = true;

            //소켓 생성
            Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portNum);

            SocketAsyncEventArgs saeaServer = new SocketAsyncEventArgs();
            saeaServer.RemoteEndPoint = ipepServer;
            //연결 완료 이벤트 연결
            saeaServer.Completed += new EventHandler<SocketAsyncEventArgs>(Connect_Completed);

            //서버 메시지 대기
            socketServer.ConnectAsync(saeaServer);
        }

        /// 연결 완료
        private void Connect_Completed(object sender, SocketAsyncEventArgs e)
        {
            m_socketMe = (Socket)sender;

            if (true == m_socketMe.Connected)
            {
                MessageData mdReceiveMsg = new MessageData();

                //서버에 보낼 객체를 만든다.
                SocketAsyncEventArgs saeaReceiveArgs = new SocketAsyncEventArgs();
                //보낼 데이터를 설정하고
                saeaReceiveArgs.UserToken = mdReceiveMsg;
                //데이터 길이 세팅
                saeaReceiveArgs.SetBuffer(mdReceiveMsg.GetBuffer(), 0, 4);
                //받음 완료 이벤트 연결
                saeaReceiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Recieve_Completed);
                //받음 보냄
                m_socketMe.ReceiveAsync(saeaReceiveArgs);

                pht.DisplayMsg("* [채팅 시작] *");
                //서버 연결이 성공하면 id체크를 시작한다.
                Login(NickName);
            }
            else
            {
                Disconnection();
            }
        }

        private void Recieve_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socketClient = (Socket)sender;
            MessageData mdRecieveMsg = (MessageData)e.UserToken;
            mdRecieveMsg.SetLength(e.Buffer);
            mdRecieveMsg.InitData();

            if (true == socketClient.Connected)
            {
                //연결이 되어 있다.

                //데이터 수신
                socketClient.Receive(mdRecieveMsg.Data, mdRecieveMsg.DataLength, SocketFlags.None);

                //명령을 분석 한다.
                MsgAnalysis(mdRecieveMsg.GetData());

                //다음 메시지를 받을 준비를 한다.
                socketClient.ReceiveAsync(e);
            }
            else
            {
                Disconnection();
            }
        }

        /// 넘어온 데이터를 분석 한다.
        private void MsgAnalysis(string sMsg)
        {
            //구분자로 명령을 구분 한다.
            string[] sData = sMsg.Split(claGlobal.g_Division);

            //데이터 개수 확인
            if ((1 <= sData.Length))
            {
                //0이면 빈메시지이기 때문에 별도의 처리는 없다.

                //넘어온 명령
                claCommand.Command typeCommand
                    = m_insCommand.StrIntToType(sData[0]);

                switch (typeCommand)
                {
                    case claCommand.Command.None:	//없다
                        break;
                    case claCommand.Command.Msg:	//메시지인 경우
                        Command_Msg(sData[1]);
                        break;
                    case claCommand.Command.ID_Check_Ok:	//아이디 성공
                        SendMeg_IDCheck_Ok();
                        break;
                    case claCommand.Command.ID_Check_Fail:	//아이디 실패
                        SendMeg_IDCheck_Fail();
                        break;
                }
            }
        }

        /// 메시지 출력
        private void Command_Msg(string sMsg)
        {
            pht.DisplayMsg(sMsg);
        }

        /// 아이디 성공
        private void SendMeg_IDCheck_Ok()
        {
            pht.Dispatcher.BeginInvoke(new Action(
                        delegate()
                        {
                            IDText.Text = NickName;
                        }));

            //아이디확인이 되었으면 서버에 로그인 요청을 하여 로그인을 끝낸다.
            StringBuilder sbData = new StringBuilder();
            sbData.Append(claCommand.Command.Login.GetHashCode());
            sbData.Append(claGlobal.g_Division);
            SendMsg(sbData.ToString());

        }

        /// 아이디체크 실패
        private void SendMeg_IDCheck_Fail()
        {
            String Nick1 = randomID();
            Login(Nick1);

        }

        /// 접속이 끊겼다.
        private void Disconnection()
        {
            //접속 끊김
            m_socketMe = null;
            pht.DisplayMsg("* [Server DisConnecting] *\n");
        }

        // 서버에 접속시 랜덤으로 아이디 부여
        private string randomID()
        {
            string[] NickNames = 
                { "Apple ", "Banana ", "Korea ", "Babo ", "Mouse ",
                "Book ", "Lemon ", "Orange ", "멍청이 ", "이순신 ",
                "장영실 ", "Melon ", "Moon ", "Star ", "Cloud ",
                "Winter ", "Spring ", "Summer ","Fall ","광해군 ",
                "연산군 ", "논개 ", "장희빈 ", "논병아리 ", "뱁새" 
            };
            Random rand = new Random();
            int r = rand.Next(0, NickNames.Length);
            return NickNames[r];
        }

        /// 아이디 체크 요청
        private void Login(String Nick)
        {
            StringBuilder sbData = new StringBuilder();
            sbData.Append(claCommand.Command.ID_Check.GetHashCode());
            sbData.Append(claGlobal.g_Division);

            sbData.Append(Nick);

            SendMsg(sbData.ToString());
        }

        /// 서버로 메시지를 전달 합니다.
        private void SendMsg(string sMsg)
        {
            MessageData mdSendMsg = new MessageData();

            //데이터를 넣고
            mdSendMsg.SetData(sMsg);

            //서버에 보낼 객체를 만든다.
            SocketAsyncEventArgs saeaServer = new SocketAsyncEventArgs();
            //데이터 길이 세팅
            saeaServer.SetBuffer(BitConverter.GetBytes(mdSendMsg.DataLength), 0, 4);
            //보내기 완료 이벤트 연결
            saeaServer.Completed += new EventHandler<SocketAsyncEventArgs>(Send_Completed);
            //보낼 데이터 설정
            saeaServer.UserToken = mdSendMsg;
            try
            {
            //보내기
            m_socketMe.SendAsync(saeaServer);
            }
            catch (NullReferenceException ex)
            {//TODO: 예외처리..ㅠ
                //UI_Setting(typeState.DisConnecting);

                ServerConnectFunc(null);
                Console.WriteLine(ex.Message);
            }
        }

        /// 메시지 보내기 완료
        private void Send_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socketSend = (Socket)sender;
            MessageData mdMsg = (MessageData)e.UserToken;
            //데이터 보내기 마무리
            socketSend.Send(mdMsg.Data);
        }
        #endregion
    }
}

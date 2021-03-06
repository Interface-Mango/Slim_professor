﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocketGlobal;
using SocketGlobal.CustomClass;
using System.Net;
using System.Net.Sockets;
using Slim_professor.ViewModel;

namespace Slim_professor
{
    public class SocketUser
    {
        #region 연결할 이벤트

        /// 끊김
        public event dgDisconnect OnDisconnected;

        /// 메시지
        public event dgMessage OnMessaged;
        #endregion

        /// 명령어 클래스
        private claCommand m_insCommand = new claCommand();
        /// 이 유저의 소켓정보
        public Socket m_socketMe;
        /// 이 유저의 아이디
        public string UserID { get; set; }

        /// 유저 객체를 생성합니다.
        /// 접속된 유저의 소켓
        public SocketUser(Socket socketClient)
        {
            //소켓 저장
            m_socketMe = socketClient;

            //데이터 구조 생성
            MessageData MsgData = new MessageData();

            //리시브용 인스턴스 생성
            System.Net.Sockets.SocketAsyncEventArgs saeaReceiveArgs = new System.Net.Sockets.SocketAsyncEventArgs();

            //리시브용 데이터 구조 지정
            saeaReceiveArgs.UserToken = MsgData;

            //리시브용 데이터버퍼 설정
            saeaReceiveArgs.SetBuffer(MsgData.GetBuffer(), 0, 4);

            //유저한테서 넘어온 데이터 받음 완료 이벤트 연결
            saeaReceiveArgs.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(Recieve_Completed);

            //데이터 받기 시작
            m_socketMe.ReceiveAsync(saeaReceiveArgs);
        }



        /// 유저한테서 넘어온 데이터 받음 완료
        private void Recieve_Completed(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        {
            //서버에서 넘어온 정보
            Socket socketClient = (Socket)sender;
            //서버에서 넘어온 데이터
            MessageData MsgData = (MessageData)e.UserToken;
            MsgData.SetLength(e.Buffer);
            MsgData.InitData();

            //유저가 연결 상태인지?
            if (true == socketClient.Connected)
            {
                //연결 상태이다
                socketClient.Receive(MsgData.Data, MsgData.DataLength, SocketFlags.None);
                //넘어온 메시지를 분석의뢰!
                MsgAnalysis(MsgData.GetData());

                //다음 데이터를 기다린다.
                socketClient.ReceiveAsync(e);
            }
            else
            {
                //아니다
                //접속 끊김을 알린다.
                Disconnected();
            }
        }


        /// 나 끊김
        public void Disconnected()
        {
            OnDisconnected(this);
        }

        /// 서버로 메시지를 보냅니다.
        private void SendMeg_Main(string sMag, claCommand.Command typeCommand)
        {
            MessageEventArgs e = new MessageEventArgs(sMag, typeCommand);
            OnMessaged(this, e);
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
                claCommand.Command typeCommand = m_insCommand.StrIntToType(sData[0]);

                switch (typeCommand)
                {
                    case claCommand.Command.None:	//없다
                        break;
                    case claCommand.Command.Msg:	//메시지인 경우
                        SendMeg_Main(sData[1], typeCommand);
                        break;
                    case claCommand.Command.ID_Check:	//아이디 체크
                        SendMeg_Main(sData[1], typeCommand);
                        break;

                }
            }
        }

        /// 이 유저에게 메시지를 보낸다.
        public void SendMsg_User(string sMsg)
        {
            MessageData mdMsg = new MessageData();
            mdMsg.SetData(sMsg);

            //유저에게 보낼 객체를 만든다.
            System.Net.Sockets.SocketAsyncEventArgs saeaSendArgs = new System.Net.Sockets.SocketAsyncEventArgs();
            //데이터 길이 세팅
            saeaSendArgs.SetBuffer(BitConverter.GetBytes(mdMsg.DataLength), 0, 4);
            //보내기 완료 이벤트 연결
            saeaSendArgs.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(Send_Completed);
            //보낼 데이터 설정
            saeaSendArgs.UserToken = mdMsg;
            //보내기
            this.m_socketMe.SendAsync(saeaSendArgs);
        }


        /// 메시지 보내기 완료
        private void Send_Completed(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        {
            //유저 소켓
            Socket socketClient = (Socket)sender;
            MessageData mdMsg = (MessageData)e.UserToken;
            //데이터 보내기 마무리
            socketClient.Send(mdMsg.Data);
        }

    }
}

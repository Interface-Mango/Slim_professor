using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGlobal;

namespace Slim_professor
{
    public class MessageEventArgs : EventArgs
    {
        /// 메시지
        public readonly string m_strMsg = "";

        /// 메시지 타입
        public claCommand.Command m_typeCommand = claCommand.Command.None;


        /// 메시지 설정
        public MessageEventArgs(string strMsg, claCommand.Command typeCommand)
        {
            this.m_strMsg = strMsg;
            this.m_typeCommand = typeCommand;
        }
    }
}

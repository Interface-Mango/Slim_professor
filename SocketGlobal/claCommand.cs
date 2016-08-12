using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal
{
	/// <summary>
	/// 소켓에 사용될 명령어
	/// </summary>
	public class claCommand
	{
		public enum Command
		{
			/// 기본 상태
			None = 0,

			/// 아이디 체크
			ID_Check,

			/// 아이디 체크 성공
			ID_Check_Ok,

			/// 아이디 체크 실패
			ID_Check_Fail,


			/// 아이디 무결성이 확인 된후 호출함
			Login,

			/// 서버에서 모든 로그인 과정이 완료 되었다고 클라이언트에게 알림
			Login_Complete,

			/// 서버에서 연결을 끊을 때
            Server_Disconnection,

			/// 메시지 전송
			Msg,
		}

		private CustomClass.claNumber m_insNumber = new CustomClass.claNumber();

		/// 문자열로된 숫자를 명령어 타입으로 바꿔줍니다.
		/// 입력된 문자열이 올바르지 않다면 기본상태를 줍니다.
		public Command StrIntToType(string sData)
		{
			//넘어온 명령
			claCommand.Command typeCommand = claCommand.Command.None;

			if (true == m_insNumber.IsNumeric(sData))
			{
				//입력된 명령이 숫자라면 명령 타입으로 변환한다.
				//입력된 명령이 숫자가 아니면 명령 없음 처리(기본값)를 한다.
				typeCommand = (claCommand.Command)Convert.ToInt32(sData);
			}

			return typeCommand;
		}

	}
}

using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Messenger
{
	internal sealed class SendMsgMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string text = Essential.FilterString(Event.PopFixedString());
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().GetMessenger() != null && Session.GetHabbo().PassedSafetyQuiz)
			{
                Session.GetHabbo().CheckForUnmute();
				if (num == 0u && Session.GetHabbo().HasFuse("cmd_sa"))
				{
                    ServerMessage Message = new ServerMessage(Outgoing.InstantChat);
					Message.AppendUInt(0u);
					Message.AppendString(Session.GetHabbo().Username + ": " + text);
                    Message.AppendString(Essential.GetUnixTimestamp() + string.Empty);
					Essential.GetGame().GetClientManager().SendToStaffs(Session, Message, false);
				}
				else
				{
					if (num == 0u)
					{
                       /* ServerMessage Message2 = new ServerMessage(1133);
						Message2.AppendInt32(4);
						Message2.AppendUInt(0u);
						Session.SendMessage(Message2);*/
					}
					else
					{
                        if (Session != null && Session.GetHabbo() != null)
                        {
                            Session.GetHabbo().GetMessenger().method_18(num, text);
                        }
					}
				}
			}
		}
	}
}

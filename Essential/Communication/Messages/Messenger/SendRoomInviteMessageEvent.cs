using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Messenger
{
	internal sealed class SendRoomInviteMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			List<uint> list = new List<uint>();
			for (int i = 0; i < num; i++)
			{
				list.Add(Event.PopWiredUInt());
			}
			string text = Event.PopFixedString();
			
				text = Essential.DoFilter(text, true, false);
				text = ChatCommandHandler.ApplyFilter(text);
                ServerMessage Message = new ServerMessage(Outgoing.InstantInvite); // Update
				Message.AppendUInt(Session.GetHabbo().Id);
				Message.AppendStringWithBreak(text);
				foreach (uint current in list)
				{
					if (Session.GetHabbo().GetMessenger().method_9(Session.GetHabbo().Id, current))
					{
						GameClient @class = Essential.GetGame().GetClientManager().GetClient(current);
						if (@class == null)
						{
							break;
						}
						@class.SendMessage(Message);
					}
				}
		}
	}
}

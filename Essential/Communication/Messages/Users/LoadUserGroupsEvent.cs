using System;
using System.Data;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Users
{
	internal sealed class LoadUserGroupsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DataTable dataTable_ = Session.GetHabbo().dataTable_0;
			if (dataTable_ != null)
			{
                ServerMessage Message = new ServerMessage(Outgoing.UserGuilds);
				Message.AppendInt32(dataTable_.Rows.Count);
				foreach (DataRow dataRow in dataTable_.Rows)
				{
					GroupsManager @class = Groups.GetGroupById((int)dataRow["groupid"]);
					Message.AppendInt32(@class.Id);
                    Message.AppendStringWithBreak(@class.Name);
                    Message.AppendStringWithBreak(@class.Badge);
					if (Session.GetHabbo().FavouriteGroup == @class.Id) // is favorite group?
					{
						Message.AppendBoolean(true);
					}
					else
					{
						Message.AppendBoolean(false);
					}
				}
				Session.SendMessage(Message);
			}
		}
	}
}

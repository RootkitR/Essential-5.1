using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Settings
{
	internal sealed class GetRoomSettingsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
                Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                
                if ((room != null) && room.CheckRights(Session, true))
                {
                    ServerMessage RoomDataEdit = new ServerMessage(Outgoing.RoomDataEdit);
                    RoomDataEdit.AppendInt32(room.Id);
                    RoomDataEdit.AppendString(room.Name);
                    RoomDataEdit.AppendString(room.Description);
                    RoomDataEdit.AppendInt32(room.State);
                    RoomDataEdit.AppendInt32(room.Category);
                    RoomDataEdit.AppendInt32(room.UsersMax);
                    RoomDataEdit.AppendInt32(100);
                    RoomDataEdit.AppendInt32(room.Tags.Count);
                    foreach (string str in room.Tags.ToArray())
                    {
                        RoomDataEdit.AppendString(str);
                    }
                    RoomDataEdit.AppendInt32(room.UsersWithRights.Count);
                    RoomDataEdit.AppendInt32(room.AllowPet ? 1 : 0);
                    RoomDataEdit.AppendInt32(room.AllowPetsEating ? 1 : 0);
                    RoomDataEdit.AppendInt32(room.AllowWalkthrough ? 1 : 0);
                    RoomDataEdit.AppendInt32(room.Hidewall ? 1 : 0);
                    RoomDataEdit.AppendInt32(room.Wallthick);
                    RoomDataEdit.AppendInt32(room.Floorthick);
                    RoomDataEdit.AppendInt32(0);//muto penso
                    RoomDataEdit.AppendInt32(0);//muto penso
                    RoomDataEdit.AppendInt32(0);//muto penso
                    Session.SendMessage(RoomDataEdit);
                    if (room.UsersWithRights.Count > 0)
                    {
                        ServerMessage msg = new ServerMessage(Outgoing.GetPowerList);
                        msg.AppendInt32(room.RoomData.Id);
                        msg.AppendInt32(room.UsersWithRights.Count);
                        foreach (uint num in room.UsersWithRights)
                        {
                            msg.AppendInt32(num);
                            msg.AppendString(Essential.GetGame().GetClientManager().GetNameById(num));
                        }
                        Session.SendMessage(msg);
                    }
                }
		}
	}
}

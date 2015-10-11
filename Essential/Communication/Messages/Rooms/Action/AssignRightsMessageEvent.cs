using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
using System;
namespace Essential.Communication.Messages.Rooms.Action
{
    internal sealed class AssignRightsMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            uint num = Event.PopWiredUInt();
            Room room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(num);
                if (room != null && room.CheckRights(Session, true) && roomUserByHabbo != null && !roomUserByHabbo.IsBot && !room.UsersWithRights.Contains(num))
                {
                    room.UsersWithRights.Add(num);
                    using (DatabaseClient client = Essential.GetDatabase().GetClient())
                    {
                        client.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO room_rights (room_id,user_id) VALUES (",
							room.Id,
							",",
							num,
							")"
						}));
                    }
                    ServerMessage serverMessage = new ServerMessage(Outgoing.GivePowers); // Updated
                    serverMessage.AppendUInt(room.Id);
                    serverMessage.AppendUInt(num);
                    serverMessage.AppendStringWithBreak(roomUserByHabbo.GetClient().GetHabbo().Username);
                    Session.SendMessage(serverMessage);

                    roomUserByHabbo.AddStatus("flatctrl", "");
                    roomUserByHabbo.UpdateNeeded = true;

                    ServerMessage Rights = new ServerMessage(Outgoing.RoomRightsLevel); // Updated
                    Rights.AppendInt32(1);
                    roomUserByHabbo.GetClient().SendMessage(Rights);

                  
                    

                }
                else if (room.CheckRights(Session, true) && !room.UsersWithRights.Contains(num) && Session.GetHabbo().GetMessenger().UserInFriends(num))
                {
                    room.UsersWithRights.Add(num);
                    using (DatabaseClient client = Essential.GetDatabase().GetClient())
                    {
                        client.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO room_rights (room_id,user_id) VALUES (",
							room.Id,
							",",
							num,
							")"
						}));
                    }
                    ServerMessage serverMessage = new ServerMessage(Outgoing.GivePowers); // Updated
                    serverMessage.AppendUInt(room.Id);
                    serverMessage.AppendUInt(num);
                    serverMessage.AppendStringWithBreak(Essential.GetGame().GetClientManager().GetNameById(num));
                    Session.SendMessage(serverMessage);

                }
            }
        }
    }
}

using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Settings
{
    internal sealed class GetFlatControlsMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room Room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (Room != null && Room.CheckRights(Session, true))
            {

                        ServerMessage LoadUsersWithRights = new ServerMessage(Outgoing.FlatControllerAdded);
                        LoadUsersWithRights.AppendUInt(Room.Id);
                        LoadUsersWithRights.AppendInt32(Room.UsersWithRights.Count);
                        foreach (uint current2 in Room.UsersWithRights)
                        {

                            LoadUsersWithRights.AppendUInt(current2);
                            LoadUsersWithRights.AppendString(Essential.GetGame().GetClientManager().GetNameById(current2));
                        }
                        Session.SendMessage(LoadUsersWithRights);

                        foreach (uint current3 in Room.UsersWithRights)
                        {
                            ServerMessage serverMessage = new ServerMessage(Outgoing.GivePowers); // Updated
                            serverMessage.AppendUInt(Room.Id);
                            serverMessage.AppendUInt(current3);
                            serverMessage.AppendStringWithBreak(Essential.GetGame().GetClientManager().GetNameById(current3));
                            Session.SendMessage(serverMessage);
                        }
        
            }
        }
    }
}

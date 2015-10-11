using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.Pets;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Bots
{
	internal sealed class PickupBotMessageEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null)
            {
                uint botId = Event.PopWiredUInt();
                RoomUser class2 = @class.getBot(botId);
                if (class2 != null)
                {
                    using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("UPDATE user_bots SET room_id=0 WHERE id=" + botId);
                    }
                    Session.GetHabbo().GetInventoryComponent().AddBot(botId);
                    @class.method_6(class2.VirtualId, false);
                    
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pets;
using Essential.Util;
using Essential.Messages;
using Essential.HabboHotel.Users.Inventory;
using Essential.HabboHotel.RoomBots;
using Essential.HabboHotel.Rooms;
using Essential.Storage;

namespace Essential.Communication.Messages.Rooms.Bots
{
    internal sealed class PlaceBotMessageEvent : Interface
    {
        public void Handle(GameClient session, ClientMessage message)
        {
            Room room = Essential.GetGame().GetRoomManager().GetRoom(session.GetHabbo().CurrentRoomId);

            if (room != null && (room.CheckRights(session, true)))
            {
                uint botId = message.PopWiredUInt();
                UserBot bot = session.GetHabbo().GetInventoryComponent().GetBotById(botId);
                if (bot != null && !bot.PlacedInRoom)
                {
                    int num = message.PopWiredInt32();
                    int num2 = message.PopWiredInt32();

                    if (room.method_30(num, num2, 0.0, true, false))//&& !room.ContainsBot())
                    {

                        bot.PlacedInRoom = true;
                        bot.RoomId = (int)room.Id;
                        List<RandomSpeech> list = new List<RandomSpeech>();
                        List<BotResponse> list2 = new List<BotResponse>();
                        try
                        {
                            room.AddBotToRoom(new RoomBot(bot.BotId, (uint)bot.RoomId, AIType.UserBot, "freeroam", bot.Name, "", bot.Look, num, num2, 0, 0, 0, 0, 0, 0, ref list, ref list2, 0), bot);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        session.GetHabbo().GetInventoryComponent().RemoveBotById(bot.BotId);
                        using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery("UPDATE user_bots SET room_id = '" + bot.RoomId + "', x = " + num + " , y = " + num2 + " WHERE id = '" + botId + "' LIMIT 1");
                        }
                        session.SendMessage(session.GetHabbo().GetInventoryComponent().ComposeBotInventoryListMessage());
                    }
                }
            }
        }
    }
}

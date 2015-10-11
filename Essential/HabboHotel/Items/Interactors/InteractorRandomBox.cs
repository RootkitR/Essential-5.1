using Essential.HabboHotel.SurpriseBoxes;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorRandomBox : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
            // DIeser Code wird ausgeführt, wenn das Item im Raum platziert wird.
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
            // Dieser Code wird ausgeführt, wenn das Item aufgenommen wird.
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            // Dieser Code wird bei einem Doppelklick ausgeführt.
            if(Session.GetHabbo().CurrentRoom.Owner == Session.GetHabbo().Username) // Wenn der Raumbesitzer den gleichen Namen hat wie der User...
            {
                // Zufallsitem...
                SurpriseBox sb = Essential.GetGame().GetCatalog().GetSurpriseBoxManager().GetRandomSurpriseBox(); // zufällige Box
                Item itm = sb.GetItem(); // item von Box
                int x = RoomItem_0.GetX;
                int y = RoomItem_0.Int32_1;
                int rot = RoomItem_0.int_3;
                Session.GetHabbo().CurrentRoom.method_29(Session,RoomItem_0.uint_0, true, true); //löscht Box ausm Raum (& DB)
                Essential.GetGame().GetCatalog().AddItemToInventory(Session,itm, 1, "", true, 0u, 0, 0, ""); // platziert Item in Inventar
                UserItem ui = Session.GetHabbo().GetInventoryComponent().GetLastItem();
                RoomItem RoomItem_ = new RoomItem(ui.uint_0, Session.GetHabbo().CurrentRoom.Id, ui.uint_1, ui.string_0, 0, 0, 0.0, 0, "", Session.GetHabbo().CurrentRoom, ui.LtdId, ui.LtdCnt, ui.GuildData);
                if (Session.GetHabbo().CurrentRoom.method_79(Session, RoomItem_, x, y, rot, true, false, false))
                {
                    Session.GetHabbo().GetInventoryComponent().method_12(ui.uint_0, 1u, false);
                    using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
                    {
                        class3.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '",
									Session.GetHabbo().CurrentRoom.Id,
									"' WHERE Id = '",
									ui.uint_0,
									"' LIMIT 1"
								}));
                    }
                }
                try
                {
                    Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("38|" + sb.Name + "|" + sb.Image);
                }
                catch { }
            }
        }
    }
}

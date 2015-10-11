using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorWiredFXBotChangesLook : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if(bool_0)
            {
                string oldlook = Session.GetHabbo().Figure;
                string botname = "";
                string Gender = "M";
                try
                {
                    oldlook = RoomItem_0.string_2.Split(';')[1];
                    botname = RoomItem_0.string_2.Split(';')[0];
                    Gender = RoomItem_0.string_2.Split(';')[2].ToLower();
                    Gender = Gender.ToLower() == "m" ? "M" : (Gender.ToLower() == "f" ? "F" : "M");
                }
                catch { }
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("27|" + RoomItem_0.uint_0 + "|"+ botname + "|" + oldlook + "|" + Gender);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorImageWired : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (bool_0)
            {
                if (!IsValidFile(RoomItem_0.string_2))
                    RoomItem_0.string_2 = "http://habbo.tl/public/images/logo.png";
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("37|" + RoomItem_0.uint_0 + "|" + RoomItem_0.string_2);
            }
        }
        public bool IsValidFile(string url)
        {
            return url.StartsWith("http://") && (url.EndsWith(".png") || url.EndsWith(".jpg") || url.EndsWith(".gif"));
        }
    }
}

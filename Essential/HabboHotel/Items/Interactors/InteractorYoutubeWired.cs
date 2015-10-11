using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorYoutubeWired : FurniInteractor
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
                try { Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("31|" + RoomItem_0.uint_0 + "|" + "https://youtube.com/watch?v=" + RoomItem_0.string_2); }
                catch { }
            }
        }
    }
}

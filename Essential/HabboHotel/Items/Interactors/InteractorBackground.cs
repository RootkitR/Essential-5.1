using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorBackground : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if(RoomItem_0.GetRoom().CheckRights(Session,true))
            {
                string imageUrl = "";
                int offsetX = 0;
                int offsetY = 0;
                int offsetZ = 0;
                int count = 0;
                string[] splitted = RoomItem_0.ExtraData.Split(Convert.ToChar(9));
                foreach(string s in RoomItem_0.ExtraData.Split(Convert.ToChar(9)))
                {
                    try {
                        if (s == "imageUrl")
                            imageUrl = splitted[count +1];
                        if (s == "offsetX")
                            offsetX = int.Parse(splitted[count + 1]);
                        if (s == "offsetY")
                            offsetY = int.Parse(splitted[count + 1]);
                        if (s == "offsetZ")
                            offsetZ = int.Parse(splitted[count + 1]);
                        }catch{}
                    count++;
                }
                string tosend = "17|" + RoomItem_0.uint_0 + "|" + imageUrl.Replace("https://","http://") + "|" + offsetX + "|" + offsetY + "|" + offsetZ;
                Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send(tosend);
            }
        }
    }
}

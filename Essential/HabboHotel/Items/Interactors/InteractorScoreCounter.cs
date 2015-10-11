using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorScoreCounter : FurniInteractor
    {
        public override void OnPlace(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnRemove(GameClients.GameClient Session, RoomItem RoomItem_0)
        {
        }

        public override void OnTrigger(GameClients.GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (!bool_0)
            {
                return;
            }
            int num = 0;
            int.TryParse(RoomItem_0.ExtraData, out num);

            {
                switch (int_0)
                {
                    case 1:
                        num++;
                        break;

                    case 2:
                        num--;
                        break;

                    case 3:
                        num = 0;
                        break;
                }
                RoomItem_0.ExtraData = num < 0 ? "0" : num.ToString();
                RoomItem_0.UpdateState(false, true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pathfinding;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorBancomat : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            Essential.getWebSocketManager().getWebSocketByName(Session.GetHabbo().Username).Send("4|Bancomat");
            /*  if (!Session.GetHabbo().bankingModeEnabled)
               {
                   if (Session.GetHabbo().PIN != "NOACCOUNT")
                   {
                       Session.Whisper(Essential.GetBankingConfig().getData("text.welcome"));
                       Session.Whisper(Essential.GetBankingConfig().getData("text.pincode").Replace("%leave%", Essential.GetBankingConfig().getData("cmd.leave")));
                       Session.GetHabbo().bankingModeEnabled = true;
                       Session.GetHabbo().nextStep = "PIN";
                       RoomUser class4 = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                       if (class4 != null)
                       {
                           class4.bool_5 = true;
                       }
                   }
                   else
                   {
                       Session.Whisper(Essential.GetBankingConfig().getData("text.noaccount"));
                   }
               }*/
        }
    }
}

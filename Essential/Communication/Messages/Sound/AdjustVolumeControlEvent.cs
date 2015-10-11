using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Sound
{
    class AdjustVolumeControlEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            int num = Event.PopWiredInt32();
            int num1 = Event.PopWiredInt32();
            int num2 = Event.PopWiredInt32();
            using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
            {
                    adapter.ExecuteQuery("REPLACE INTO user_volume (userid, system_volume, furni_volume, trax_volume) VALUES (" + Session.GetHabbo().Id + "," + num + "," + num1 + ", " + num2 + " )");
            }
        }
    }
}

using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Navigator
{
    internal sealed class ReceptionWidgetMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string ReceptionData = Event.PopFixedString();

            string[] ReceptionParts = ReceptionData.Split(';');

            for (int i = 0; i < ReceptionParts.Length; i++)
            {

                if (!ReceptionParts[i].Contains(","))
                {
                    return;
                }

                ServerMessage InitReception = new ServerMessage(Outgoing.SendCampaingData);
                InitReception.AppendString(ReceptionParts[i].Split(',')[0]);
                InitReception.AppendString(ReceptionParts[i].Split(',')[1]);
                Session.SendMessage(InitReception);

           
            }
        
        }
    }
}

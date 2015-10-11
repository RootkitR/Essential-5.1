using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Relationships
{
    class SetRelationshipsStatusMessage : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            uint requester = Event.PopWiredUInt();
            uint newStatus = Event.PopWiredUInt();
            if (Session.GetHabbo().GetRelationshipComposer().GetRelationshipList.ContainsKey(requester))
            {
                if (Session.GetHabbo().GetRelationshipComposer().GetRelationshipList[requester] != newStatus)
                    Session.SendMessage(Session.GetHabbo().GetRelationshipComposer().SerializeRelationshipUpdate(requester, newStatus));
            }
            else if ((!Session.GetHabbo().GetRelationshipComposer().GetRelationshipList.ContainsKey(requester)) && (newStatus != 0))
                Session.SendMessage(Session.GetHabbo().GetRelationshipComposer().SerializeRelationshipUpdate(requester, newStatus));
            else
                return;
        }
    }
}

using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users.Relationship;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Relationships
{
    class GetRelationshipsProfile : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            uint userId = Event.PopWiredUInt();
            GameClient gc = Essential.GetGame().GetClientManager().GetClientByHabbo(Essential.GetGame().GetClientManager().GetNameById(userId));
            if (gc != null)
                Session.SendMessage(gc.GetHabbo().GetRelationshipComposer().SerializeRelationshipsProfile());
            else
                Session.SendMessage(new RelationshipComposer(userId).SerializeRelationshipsProfile());
        }
    }
}

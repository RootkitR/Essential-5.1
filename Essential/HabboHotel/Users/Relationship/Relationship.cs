using System;

namespace Essential.HabboHotel.Users.Relationship
{
    internal class Relationship
    {
        internal uint targetID;
        internal uint relationshipStatus;

        internal Relationship(uint target, uint status)
        {
            this.targetID = target;
            this.relationshipStatus = status;
        }

    }
}

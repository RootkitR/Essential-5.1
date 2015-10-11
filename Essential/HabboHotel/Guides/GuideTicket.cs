using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Users;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Guides
{
    class GuideTicket
    {
        public uint CreatorId;
        public string CreatorName;
        public string CreatorLook;
        public uint GuideId;
        public string GuideName;
        public string GuideLook;
        public bool Answered;
        public GuideTicket(Habbo creator, Habbo guide)
        {
            this.CreatorId = creator.Id;
            this.GuideId = guide.Id;
            this.CreatorLook = creator.Figure;
            this.CreatorName = creator.Username;
            this.GuideLook = guide.Figure;
            this.GuideName = guide.Username;
            this.Answered = false;
        }
        public void Serialize(ServerMessage Message)
        {
            Message.AppendInt32(CreatorId);
            Message.AppendString(CreatorName);
            Message.AppendString(CreatorLook);
            Message.AppendInt32(GuideId);
            Message.AppendString(GuideName);
            Message.AppendString(GuideLook);
        }
        public void StoreMessage(string message, uint userid)
        {
            using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("message", message);
                dbClient.ExecuteQuery("INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('" + userid + "','" + (userid == CreatorId ? GuideId : CreatorId) + "','" + DateTime.Now.Hour + "','" + DateTime.Now.Minute + "',UNIX_TIMESTAMP(),@message,'" + (userid == CreatorId ? CreatorName : GuideName) + "','" + DateTime.Now.ToLongDateString() + "')");
            }
        }
        public void SendToTicket(ServerMessage message)
        {
            Essential.GetGame().GetClientManager().GetClientByHabbo(CreatorName).SendMessage(message);
            Essential.GetGame().GetClientManager().GetClientByHabbo(GuideName).SendMessage(message);
        }
        public GameClient GetOtherClient(uint userid)
        {
            string otherid = userid == GuideId ? CreatorName : GuideName;
            return Essential.GetGame().GetClientManager().GetClientByHabbo(otherid);
        }
        public void SendToOther(ServerMessage message, uint Id)
        {
            GetOtherClient(Id).SendMessage(message);
        }
    }
}

/* Made for Essential 5*/
using Essential.HabboHotel.Users;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Guides
{
    class GuideManager
    {
        List<uint> Guides;
        Dictionary<uint, Guide> GuidesOnDuty;
        List<GuideTicket> Tickets;
        ServerMessage errorMessage;
        ServerMessage detachedMessage;
        public int GuidesOnDutyCount
        {
            get
            {
                return GuidesOnDuty.Count;
            }
        }
        public ServerMessage ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }
        public ServerMessage DetachedMessage
        {
            get
            {
                return detachedMessage;
            }
        }
        public int Timer
        {
            get
            {
                return 60;
            }
        }
        public GuideManager(DatabaseClient dbClient)
        {
            this.Guides = new List<uint>();
            this.GuidesOnDuty = new Dictionary<uint, Guide>();
            this.Tickets = new List<GuideTicket>();
            DataTable dt = dbClient.ReadDataTable("SELECT * FROM users WHERE is_guide='1'");
            foreach(DataRow dr in dt.Rows)
            {
                this.Guides.Add((uint)dr["id"]);
            }
            errorMessage = new ServerMessage(2553);
            errorMessage.AppendInt32(0);
            detachedMessage = new ServerMessage(1719);
        }
        #region "Guide User related things"
        public void ToggleState(bool onDuty, uint userid)
        {
            if (onDuty)
                this.AddToDuty(userid);
            else
                this.RemoveFromDuty(userid);
            this.UpdateGuidesOnDuty();
        }
        public void UpdateGuidesOnDuty()
        {
            foreach(Guide g in GuidesOnDuty.Values)
            {
                try
                {
                    ServerMessage GuideTool = new ServerMessage(Outgoing.SerializeGuideTool);
                    GuideTool.AppendBoolean(true); //Im dienst?
                    GuideTool.AppendInt32(this.GuidesOnDutyCount); //Helper im Dienst
                    GuideTool.AppendInt32(0);
                    Essential.GetGame().GetClientManager().GetClientByHabbo(Essential.GetGame().GetClientManager().GetNameById(g.Id)).SendMessage(GuideTool);
                }
                catch 
                {
                    /*Why do I ignore this Exception ? Because it's not a big problem if a GameClient is null.
                     *It's just shit if the whole Queue breaks because of a Gameclient.
                     */
                }
            }
        }
        public void RemoveFromDuty(uint userid)
        {
            if (this.GuidesOnDuty.ContainsKey(userid))
                this.GuidesOnDuty.Remove(userid);
        }
        public void AddToDuty(uint userid)
        {
            if (!this.GuidesOnDuty.ContainsKey(userid))
                this.GuidesOnDuty.Add(userid, new Guide(userid));
        }
        public bool OnDuty(uint userid)
        {
            return this.GuidesOnDuty.ContainsKey(userid);
        }
        public Guide GetRandomGuide()
        {
            try
            {
                foreach (Guide g in this.GuidesOnDuty.Values.Where(o => !o.IsInUse).ToArray())
                {
                    return g;
                }
            }
            catch
            { }
            return null;
        }
        public bool isGuide(uint userid)
        {
            if (!this.Guides.Contains(userid))
            {
                using (DatabaseClient dbClient = Essential.GetDatabase().GetClient())
                {
                    bool b = Essential.StringToBoolean(dbClient.ReadString("SELECT is_guide FROM users WHERE id=" + userid));
                    if (b)
                    {
                        this.Guides.Add(userid);
                        return true;
                    }
                }
            }
            else
                return true;
            return false;
        }
        public Guide GetGuideById(uint UserId)
        {
            return GuidesOnDuty[UserId];
        }
        #endregion
        #region "Tickets"
        public bool UserMadeTicket(uint userid)
        {
                if (Tickets.Where(o => o.CreatorId == userid).ToArray()[0] != null)
                    return true;
            return false;
        }
        public void CreateTicket(Habbo Creator, Habbo Guide)
        {
            try
            {
                this.Tickets.Add(new GuideTicket(Creator, Guide));
                this.GuidesOnDuty.Values.Where(o => o.Id == Guide.Id).ToArray()[0].IsInUse = true;
            }
            catch { }
        }
        public void RemoveTicket(uint UserId)
        {
            this.Tickets.Remove(this.Tickets.Where(o => o.GuideId == UserId || o.CreatorId == UserId).ToArray()[0]);
        }
        public GuideTicket GetTicket(uint userid)
        {
            return this.Tickets.Where(o => o.GuideId == userid || o.CreatorId == userid).ToArray()[0];
        }
        #endregion
    }
}

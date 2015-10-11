using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Handshake
{
    class LoginWithUsernameAndPasswordEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            using(DatabaseClient dbClient = Essential.GetDatabase().GetClient())
            {
                string username = Event.PopFixedString();
                string password = Event.PopFixedString(); //TODO: Hash undso..
                dbClient.AddParamWithValue("username", username);
                
                string currentpassword = "";
                try
                {
                    currentpassword = dbClient.ReadString("SELECT password FROM users WHERE username=@username");
                }
                catch { }
                if (currentpassword == "")
                { Session.SendMessage(new ServerMessage(Outgoing.InvalidUsername)); return; }
                if (currentpassword != password)
                { Session.SendMessage(new ServerMessage(Outgoing.InvalidPassword)); return; }
                ServerMessage asdf = new ServerMessage(12345);
                asdf.AppendBoolean(true);
                asdf.AppendString("Hi");
                asdf.AppendInt32(1337);
                asdf.AppendUInt(12345);
                Session.SendMessage(asdf);
                Session.tryLogin(dbClient.ReadString("SELECT auth_ticket FROM users WHERE username=@username"));
            }
        }
    }
}

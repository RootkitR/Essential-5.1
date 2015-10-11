using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
    internal sealed class SaveMannequinMessageEvent : Interface
    {
        internal string GetHair(string _Figure)
        {

            string FigurePartHair = _Figure;
            string GetHairPart;

            GetHairPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hr")[1];
            FigurePartHair = GetHairPart.Split('.')[0];
            string FigurePartBody = _Figure;
            string GetBodyPart;

            GetBodyPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hd")[1];
            FigurePartBody = GetBodyPart.Split('.')[0];

            string _Uni = Convert.ToString("hr" + FigurePartHair);

            return _Uni;
        }
         internal string GetBody(string _Figure)
        {

            string FigurePartHair = _Figure;
            string GetHairPart;

            GetHairPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hr")[1];
            FigurePartHair = GetHairPart.Split('.')[0];
            string FigurePartBody = _Figure;
            string GetBodyPart;

            GetBodyPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hd")[1];
            FigurePartBody = GetBodyPart.Split('.')[0];

            string _Uni = Convert.ToString("hd" + FigurePartBody);

            return _Uni;
        }

        public void Handle(GameClient Session, ClientMessage Event)
        {
            try
            {
                uint ItemId = Event.PopWiredUInt();
                string ClothingName = Event.PopFixedString();
                Room Room = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                if (Room != null)
                {
                    RoomItem Item = Room.method_28(ItemId);

                    if (Item == null)
                    {
                        return;
                    }
                    string Lookki = Session.GetHabbo().Figure;
                    Lookki = Lookki.Replace(GetHair(Session.GetHabbo().Figure), "");
                    Lookki = Lookki.Replace(GetBody(Session.GetHabbo().Figure), "");
                    Item.ExtraData = Lookki + (char)5 + Session.GetHabbo().Gender + (char)5 + ChatCommandHandler.ApplyFilter(ClothingName);
      
                     using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
                     {
                         class2.ExecuteQuery("DELETE FROM items_extra_data WHERE item_id = '" + Item.uint_0 + "'");
                         class2.AddParamWithValue("extradata", Item.ExtraData);
                         class2.ExecuteQuery("INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + Item.uint_0 + "' , @extradata);");
                     }
                   Room.method_79(Session, Item, Item.GetX, Item.Int32_1, Item.int_3, true, false, true);
                   Item.ReqUpdate(1);
                   Item.UpdateState(true, true, false);
                  //  Item.UpdateState(true, true);
                }
            }
            catch
            {
            }
        }
    }
}

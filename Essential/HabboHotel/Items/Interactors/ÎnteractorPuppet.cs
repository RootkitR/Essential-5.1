using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Essential.Storage;
using Essential.Messages;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Pathfinding;
using Essential.HabboHotel.Rooms;
using System.Threading;
using System.Threading.Tasks;
using Essential.HabboHotel.Items.Interactors;
using Essential.HabboHotel.Items;

namespace Essential.HabboHotel.Items.Interactors
{
    class InteractorPuppet : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem Item)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            try
            {
                string[] figureParts = Session.GetHabbo().Figure.Split('.');
                switch (Item.GetBaseItem().Name)
                {
                    case "clothing_goggles":

                        if (Session.GetHabbo().Figure.Contains("he"))
                        {
                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("he"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "he-3376-62-106");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".he-3376-62-106";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyhood":
                        if (Session.GetHabbo().Figure.Contains("ha"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("ha"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "ha-3382-64-105");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".ha-3382-64-105";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyphones":
                        if (Session.GetHabbo().Figure.Contains("he"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("he"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "he-3379-62-62");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".he-3379-62-62";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cymask":
                        if (Session.GetHabbo().Figure.Contains("fa"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("fa"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "fa-3378-62-82");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".fa-3378-62-82";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyquif":
                        if (Session.GetHabbo().Figure.Contains("hr"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("hr"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "hr-3386-53");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".hr-3386-53";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyglass":
                        if (Session.GetHabbo().Figure.Contains("ea"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("ea"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "ea-3388-62-62");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".ea-3388-62-62";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyhair":
                        if (Session.GetHabbo().Figure.Contains("hr"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("hr"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "hr-3377-42-55");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".hr-3377-42-55";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cycircuit":
                        if (Session.GetHabbo().Figure.Contains("he"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("he"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "he-3385-62-62");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".he-3385-62-62";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyboots":
                        if (Session.GetHabbo().Figure.Contains("sh"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("sh"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "sh-3375-79-106");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".sh-3375-79-106";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyskirt":
                        if (Session.GetHabbo().Figure.Contains("lg"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("lg"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "lg-3387-104-64");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".lg-3387-104-64";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cystrapboots":
                        if (Session.GetHabbo().Figure.Contains("sh"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("sh"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "sh-3383-64-62");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".sh-3383-64-62";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyleather":
                        if (Session.GetHabbo().Figure.Contains("cc"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("cc"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "cc-3374-64");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".cc-3374-64";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cyzipped":
                        if (Session.GetHabbo().Figure.Contains("cc"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("cc"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "cc-3380-64-62");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".cc-3380-64-62";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cystrappants":
                        if (Session.GetHabbo().Figure.Contains("lg"))
                        {

                            foreach (string Part in figureParts)
                            {
                                if (Part.StartsWith("lg"))
                                {
                                    Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "lg-3384-64-104");
                                }
                            }

                        }
                        else
                        {
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".lg-3384-64-104";
                        }
                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cystraphood":


                        foreach (string Part in figureParts)
                        {
                            if (Part.StartsWith("lg"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "lg-3384-64-104");
                            }
                            else if (Part.StartsWith("sh"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "sh-3383-64-62");
                            }
                            else if (Part.StartsWith("cc"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "cc-3380-64-62");
                            }
                            else if (Part.StartsWith("ha"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "ha-3382-64-105");
                            }
                        }

                        if (!Session.GetHabbo().Figure.Contains("lg"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".lg-3384-64-104";

                        if (!Session.GetHabbo().Figure.Contains("sh"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".sh-3383-64-62";

                        if (!Session.GetHabbo().Figure.Contains("cc"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".cc-3380-64-62";

                        if (!Session.GetHabbo().Figure.Contains("ha"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".ha-3382-64-105";


                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    case "clothing_cygirl":
                        foreach (string Part in figureParts)
                        {
                            if (Part.StartsWith("lg"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "lg-3387-104-64");
                            }
                            else if (Part.StartsWith("hr"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "hr-3377-42-55");
                            }
                            else if (Part.StartsWith("cc"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "cc-3374-64");
                            }
                            else if (Part.StartsWith("he"))
                            {
                                Session.GetHabbo().Figure = Session.GetHabbo().Figure.Replace(Part, "he-3376-62-106");
                            }
                        }

                        if (!Session.GetHabbo().Figure.Contains("lg"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".lg-3387-104-64";

                        if (!Session.GetHabbo().Figure.Contains("hr"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".hr-3377-42-55";

                        if (!Session.GetHabbo().Figure.Contains("cc"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".cc-3374-64";

                        if (!Session.GetHabbo().Figure.Contains("he"))
                            Session.GetHabbo().Figure = Session.GetHabbo().Figure + ".he-3376-62-106";


                        Session.GetHabbo().UpdateLook(false, Session);
                        break;
                    default:
                        break;
                }
                Session.GetHabbo().UpdateLook(false, Session);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

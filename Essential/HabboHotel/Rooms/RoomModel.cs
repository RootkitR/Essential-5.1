using System;
using System.Globalization;
using System.Text;
using Essential.Messages;
using Essential.Core;
using System.Collections.Generic;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.HabboHotel.Rooms
{
	internal sealed class RoomModel
	{
		public string Name;
		public int DoorX;
		public int DoorY;
		public double double_0;
		public int int_2;
		public string string_1;
		public SquareState[,] squareState;
		public double[,] double_1;
		public int[,] int_3;
		public int int_4;
		public int int_5;
		public string string_2;
		public bool bool_0;
		public RoomModel(string string_3, int int_6, int int_7, double double_2, int int_8, string string_4, string string_5, bool bool_1)
		{
            try
            {
                this.Name = string_3;
                this.DoorX = int_6;
                this.DoorY = int_7;
                this.double_0 = double_2;
                this.int_2 = int_8;
                this.string_1 = string_4.ToLower();
                this.string_2 = string_5;
                string[] array = string_4.Split(new char[]
			{
				Convert.ToChar(13)
			});
                this.int_4 = array[0].Length;
                this.int_5 = array.Length;
                this.bool_0 = bool_1;
                this.squareState = new SquareState[this.int_4, this.int_5];
                this.double_1 = new double[this.int_4, this.int_5];
                this.int_3 = new int[this.int_4, this.int_5];
                for (int i = 0; i < this.int_5; i++)
                {
                    if (i > 0)
                    {
                        array[i] = array[i].Substring(1);
                    }
                    for (int j = 0; j < this.int_4; j++)
                    {
                        string text = array[i].Substring(j, 1).Trim().ToLower();
                        if (text == "x")
                        {
                            this.squareState[j, i] = SquareState.BLOCKED;
                        }
                        else
                        {
                            if (this.method_0(text, NumberStyles.Integer))
                            {
                                this.squareState[j, i] = SquareState.OPEN;
                                this.double_1[j, i] = double.Parse(text);
                            }
                        }
                    }
                }
                this.double_1[int_6, int_7] = double_2;
                
             
           if (string_5 != "" && string_5.Contains("|"))
                {
                foreach (string PublicIt in string_5.Split('|'))
                {

                    string text2 = PublicIt.Split(' ')[1];
                    int j = int.Parse(PublicIt.Split(' ')[2]);
                     int i = int.Parse(PublicIt.Split(' ')[3]);
                       this.squareState[j, i] = SquareState.BLOCKED;
                     

                       if (text2.Contains("bench") || text2.Contains("chair") || text2.Contains("stool") || text2.Contains("seat") || text2.Contains("sofa") || text2.Contains("shift"))
                       {
                           this.squareState[j, i] = SquareState.SEAT;
                           this.int_3[j, i] = int.Parse(PublicIt.Split(' ')[5]);
                       }
                }
                }
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
            }
		}
		public bool method_0(string string_3, NumberStyles numberStyles_0)
		{
			double num;
			return double.TryParse(string_3, numberStyles_0, CultureInfo.InvariantCulture, out num);
		}
        public ServerMessage method_1(Room Room)
        {
            try
            {
                StringBuilder thatMessage = new StringBuilder();
                ServerMessage Message = new ServerMessage(Outgoing.HeightMap); // P
                string[] array = this.string_1.Split(new char[]
			{
				Convert.ToChar(13)
			});
                for (int i = 0; i < this.int_5; i++)
                {
                    if (i > 0)
                    {
                        array[i] = array[i].Substring(1);
                    }
                    for (int j = 0; j < this.int_4; j++)
                    {
                        string text = array[i].Substring(j, 1).Trim().ToLower();
                       
                        try
                        {
                            List<RoomItem> list = Room.method_93(j, i);
                            double num2 = Room.method_84(j, i, list);

                            if ((int)num2 < 10)
                            {
                                text = ((int)num2).ToString();
                            }
                            else
                            {
                                text = "0";
                            }
                        }
                      
                        catch
                        {

                        }
                      /*  foreach (RoomItem @class in Room.Hashtable_0.Values)
                        {
                            if (@class.GetX == i && @class.Int32_1 == j)
                            {
                                if (!@class.GetBaseItem().Stackable)
                                {
                                    text = "A";
                                    continue;
                                }

                            }
                        }
                        */
                        thatMessage.Append(text);
                    }
                    thatMessage.Append(string.Concat(Convert.ToChar(13)));
                }
                Message.AppendString(thatMessage.ToString());
             
                return Message;
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
                return null;
            }
        }
        private String Number2String(int number, bool isCaps)
        {
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }
        public ServerMessage RelativeHeightmap(Room Room)
        {
            try
            {
                StringBuilder thatMessage = new StringBuilder();
                ServerMessage Message = new ServerMessage(Outgoing.FloorHeightMap); // P
                string[] array = this.string_1.Split(new char[]
			{
				Convert.ToChar(13)
			});
                for (int i = 0; i < this.int_5; i++)
                {
                    if (i > 0)
                    {
                        array[i] = array[i].Substring(1);
                    }
                    for (int j = 0; j < this.int_4; j++)
                    {
                        string text = array[i].Substring(j, 1).Trim().ToLower();
                        if (this.DoorX == j && this.DoorY == i)
                        {
                            text = string.Concat((int)this.double_0);
                        }
                        else if (Room.method_93(j, i).Count > 0)
                         {
                             int Korkeus = 0;
                             List<RoomItem> list = Room.method_93(j, i);

                             foreach (RoomItem @class in Room.Hashtable_0.Values)
                             {
                                 if (@class.GetX == j && @class.Int32_1 == i)
                                 {
                                     if (!@class.GetBaseItem().Stackable)
                                     {
                                         text = "X";
                                         continue;
                                     }
                                     Korkeus = Korkeus + Convert.ToInt32(@class.Double_1);
                                  
                                 }
                                 Dictionary<int, AffectedTile> dictionary = Room.method_94(@class.GetBaseItem().Length, @class.GetBaseItem().Width, @class.GetX, @class.Int32_1, @class.int_3);
                                 foreach (AffectedTile current in dictionary.Values)
                                 {
                                     if (current.Int32_0 == j && current.Int32_1 == i)
                                     {
                                         if (!@class.GetBaseItem().Stackable)
                                         {
                                             text = "X";
                                             continue;
                                         }
                                         Korkeus = Korkeus + Convert.ToInt32(@class.Double_1);
                                     }
                                 }
                                 text = Korkeus.ToString();
                             }

                               
                           
                             
                         }
                        
                        thatMessage.Append(text);
                    }
                    thatMessage.Append(string.Concat(Convert.ToChar(13)));
                }
                Message.AppendString(thatMessage.ToString());
               /* using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\filut\testit.txt", true))
                {
                    file.WriteLine(Message.ToString());
                }*/
                return Message;
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
                return null;
            }
        }
		public ServerMessage method_2(Room Room)
		{
            try
            {
                StringBuilder thatMessage = new StringBuilder();
                ServerMessage Message = new ServerMessage(Outgoing.FloorHeightMap); // P
                string[] array = this.string_1.Split(new char[]
			{
				Convert.ToChar(13)
			});
                for (int i = 0; i < this.int_5; i++)
                {
                    if (i > 0)
                    {
                        array[i] = array[i].Substring(1);
                    }
                    for (int j = 0; j < this.int_4; j++)
                    {

                        string text = array[i].Substring(j, 1).Trim().ToLower();
                        if (this.DoorX == j && this.DoorY == i)
                        {
                            text = string.Concat((int)this.double_0);
                        }
                     
                        thatMessage.Append(text);
                    }
                    thatMessage.Append(string.Concat(Convert.ToChar(13)));
                }
                Message.AppendString(thatMessage.ToString());
              
                return Message;
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
                return null;
            }
		}
	}
}

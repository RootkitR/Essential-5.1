using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Essential.API
{
    class FurniImageAsset
    {
        public string Name;
        public int X;
        public int Y;
        public string furniname;
        public int Height
        {
            get
            {
                try
                {
                    return Image.FromFile("API\\" + furniname + "\\" + furniname + "_" + Name + ".png").Height;
                }
                catch { }
                return 0;
            }
        }
        public int Width
        {
            get
            {
                try
                {
                    return Image.FromFile("API\\" + furniname + "\\" + furniname + "_" + Name + ".png").Width;
                }
                catch { }
                return 0;
            }
        }
        public Image Image
        {
            get
            {
                return Image.FromFile("API\\" + furniname + "\\" + furniname + "_" + Name + ".png");
            }
        }
        public FurniImageAsset(string Name, int x, int y, string furniname)
        {
            this.Name = Name;
            this.X = x;
            this.Y = y;
            this.furniname = furniname;
        }
    }
}

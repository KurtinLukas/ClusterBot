using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Top_down_shooter
{
    public class Consumable
    {
        public int X;
        public int Y;
        public string texturePath;

        public Consumable(int posX, int posY, string path)
        {
            X = posX;
            Y = posY;
            texturePath = path;
        }
    }
}

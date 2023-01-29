using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Top_down_shooter
{
    class Bullet
    {
        public double posX;
        public double posY;
        public double rotation;
        private double moveX;
        private double moveY;
        
        public Bullet(int x, int y, float rotation)
        {
            posX= x;
            posY = y;
            this.rotation = rotation;
            moveX = x;
            moveY = y;
        }

        public void Move()
        {
            moveX = Math.Cos(rotation) * 5f;
            moveY = Math.Sin(rotation) * 5f;
            posX -= (int)moveX;
            posY -= (int)moveY;
        }
    }
}

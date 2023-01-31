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
        public int X;
        public int Y;
        public int speedX;
        public int speedY;
        public double rotation;
        public int speed = 50;
        private double moveX;
        private double moveY;
        
        public Bullet(int x, int y, double rotation)
        {
            X = x;
            Y = y;
            this.rotation = rotation;
            moveX = x;
            moveY = y;
        }

        public void Move()
        {
            moveX = Math.Cos(rotation) * 5f;
            moveY = Math.Sin(rotation) * 5f;
            X -= (int)moveX;
            Y -= (int)moveY;
        }
    }
}

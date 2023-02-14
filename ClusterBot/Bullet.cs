using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Top_down_shooter
{
    public class Bullet
    {
        public int X;
        public int Y;
        public int speedX;
        public int speedY;
        public float rotation;
        public int speed = 35;
        private double moveX;
        private double moveY;
        public bool damagePlayer;
        public Character originChar;
        
        public Bullet(int x, int y, float rotation, bool damagePlayer)
        {
            X = x;
            Y = y;
            this.rotation = rotation;
            moveX = x;
            moveY = y;
            this.damagePlayer = damagePlayer;
        }

        public void Move(GridItem[,] mapGrid)
        {
            moveX = Math.Cos(rotation) * 5f;
            moveY = Math.Sin(rotation) * 5f;
            X -= (int)moveX;
            Y -= (int)moveY;


        }
        public void Draw(Image img, double angle, Graphics g)
        {
            var state = g.Save();

            g.TranslateTransform(img.Width / 2, img.Height / 2);
            g.RotateTransform((float)angle);
            g.DrawImage(img, this.X, this.Y);
            g.Restore(state);
        }
    }
}

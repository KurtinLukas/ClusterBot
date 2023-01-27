using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Top_down_shooter
{
    class Player
    {
        //proměnný
        public Point position;
        public int X = 0; //default position
        public int Y = 0;
        public int width = 200;
        public int height = 200;
        public int health = 100;

        //  metody
        //pohyb
        public void MoveBy(int posX, int posY)
        {
            X = posX;
            Y = posY;
            position = new Point(posX, posY);
        }
        public void MoveBy(Point posPoint)
        {
            X = posPoint.X;
            Y = posPoint.Y;
            position = posPoint;
        }

        //akce
        public void Shoot(Point direction)
        {

        }

        public void TakeDamage(int value)
        {
            health -= value;
        }

        //ostatní


        //deklarace
        public Player()
        {
            position = new Point(X, Y);
        }
        public Player(int posX, int posY)
        {
            X = posX;
            Y = posY;
            position = new Point(posX, posY);
        }
        public Player(Point posPoint)
        {
            X = posPoint.X;
            Y = posPoint.Y;
            position = posPoint;
        }
    }
}

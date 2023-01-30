using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Top_down_shooter
{
    class Character
    {
        //proměnný
        public Point position;
        public int X = 0; //default position
        public int Y = 0;
        public int centerX; 
        public int centerY; 
        public int width = 100;
        public int height = 100;
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

        //ostatní


        //deklarace
        public Character()
        {
            position = new Point(X, Y);
        }
        public Character(int posX, int posY)
        {
            X = posX;
            Y = posY;
            position = new Point(posX, posY);
        }
        public Character(Point posPoint)
        {
            X = posPoint.X;
            Y = posPoint.Y;
            position = posPoint;
        }
    }
}

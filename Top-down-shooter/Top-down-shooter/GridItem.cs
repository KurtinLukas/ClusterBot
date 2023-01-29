using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_down_shooter
{
    class GridItem
    {
        public int X = 0;
        public int Y = 0;

        public Material material = Material.Air;
        public enum Material {
            Air,
            Wall,
            Enemy,
            Player,
            Object //see-through non-walkable (e.g. barrel)
        };

        /// <summary>
        /// Vytvoří nové grid pole
        /// základní hodnota materiálu = Air
        /// </summary>
        /// <param name="posX">pozice X</param>
        /// <param name="posY">pozice Y</param>
        public GridItem(int posX, int posY)
        {
            X = posX;
            Y = posY;
            material = Material.Air;
        }
        /// <summary>
        /// Vytvoří nové grid pole
        /// základní hodnota materiálu = Air
        /// </summary>
        /// <param name="posX">pozice X</param>
        /// <param name="posY">pozice Y</param>
        /// <param name="mat">materiál (typ) pole</param>
        public GridItem(int posX, int posY, Material mat)
        {
            X = posX;
            Y = posY;
            material = mat;
        }
        public GridItem() { }

        public override string ToString()
        {
            string output = "X=" + X + ", Y=" + Y + ", Mat=" + material;
            return output;
        }
    }
}

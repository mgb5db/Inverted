using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class Layer
    {
        //Declare the layer array
        public int[,] layer;

        //Declare the map and tile dimensions
        int mapWidth, mapHeight, tileWidth, tileHeight;

        public Layer(int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            this.tileHeight = tileHeight;
            this.tileWidth = tileWidth;
            layer = new int[mapWidth, mapHeight];
        }

        public void LoadLayer(System.IO.StreamReader objReader)
        {
            try
            {
                //Populate the layer array
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        layer[i, j] = Convert.ToInt32(objReader.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error loading the map.");
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class Level
    {
        //Declare map and tile size variables
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public int tileWidth { get; set; }
        public int tileHeight { get; set; }

        //Declare new layers
        public Layer TileLayer1;
        public Layer TileLayer2;
        public Layer SolidLayer;

        //Declare a rectangle list to hold the tile bounds
        public List<Rectangle> tileSet = new List<Rectangle>();

        //Declare a rectangle list for collision
        public List<Rectangle> collisionRects = new List<Rectangle>();

        //Declare a rectangle to temporarily hold the tile bounds
        Rectangle bounds;

        //Declare the draw offset (map scroll values)
        Vector2 drawOffset = Vector2.Zero;

        public void LoadMap(String loadFileName)
        {
            try
            {
                //Declare and Initialize the stream reader object
                System.IO.StreamReader objReader;
                objReader = new System.IO.StreamReader(@loadFileName);

                //Find the map height and width from file
                mapHeight = Convert.ToInt32(objReader.ReadLine());
                mapWidth = Convert.ToInt32(objReader.ReadLine());
                tileHeight = Convert.ToInt32(objReader.ReadLine());
                tileWidth = Convert.ToInt32(objReader.ReadLine());
                //Reinitialize the map layers
                TileLayer1 = new Layer(mapWidth, mapHeight, tileWidth, tileHeight);
                //TileLayer2 = new Layer(mapWidth, mapHeight, tileWidth, tileHeight);
                //SolidLayer = new Layer(mapWidth, mapHeight, tileWidth, tileHeight);

                //Load the layers
                TileLayer1.LoadLayer(objReader);
                //TileLayer2.LoadLayer(objReader);
                //SolidLayer.LoadLayer(objReader);

                //close the text file and dispose of the stream reader object
                objReader.Close();
                objReader.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error loading the map, is the file name correct??");
            }
        }
        public void LoadTileSet(Texture2D tileset)
        {
            //Get the tile sheet dimensions
            int noOfTilesX = (int)tileset.Width / tileWidth;
            int noOfTilesY = (int)tileset.Height / tileHeight;

            //Initilise the tileset list
            tileSet = new List<Rectangle>(noOfTilesX * noOfTilesY);

            //Split the tile sheet into separate tiles
            for (int j = 0; j < noOfTilesY; ++j)
            {
                for (int i = 0; i < noOfTilesX; ++i)
                {
                    bounds = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                    tileSet.Add(bounds);
                }
            }
        }
        public void DrawMap()
        {
            //Get the camera coords
            //drawOffset.X = (player.position.X / tileWidth) - (player.drawPosition.X / tileWidth);
            //drawOffset.Y = (player.position.Y / tileHeight) - (player.drawPosition.Y / tileHeight);

            try
            {
                // For each tile position
                for (int x = 0; x < mapHeight; ++x)
                {
                    for (int y = 0; y < mapWidth; ++y)
                    {
                        //Tile Layer 1
                        // If there is a visible tile in that position
                        if (TileLayer1.layer[y, x] != 0)
                        {
                            //get the tileSheet bounds so the correct tile is drawn
                            bounds = tileSet[TileLayer1.layer[y, x] - 1];
                            //Console.WriteLine(TileLayer1.layer[y, x]);
                            // Draw it in screen space
                            GameLoop.spriteBatch.Draw(GameLoop.tileSheet, new Vector2(((y - drawOffset.X) * tileWidth),
                                ((x - drawOffset.Y) * tileHeight)), bounds, Color.White);
                        }
                    }
                }

                //Draw the player
                //player.Draw();

                
            }
            catch
            {
                Console.WriteLine("There was a problem drawing the map");
            }
        }

        public void PopulateCollisionLayer()
        {
            //Redeclare the rect list for collision
            collisionRects = new List<Rectangle>();

            //Loop through the array
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    //There is a collidable tile
                    if (TileLayer1.layer[x, y] == 1)
                    {
                        Console.WriteLine(mapHeight);
                        //Console.WriteLine(SolidLayer.layer[x,y]);
                        collisionRects.Add(new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
                    }
                }
            }
        }
    }
}   

#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Tao.Sdl;
#endregion

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PlatformerMain : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Texture2D tileSheet;
        Level map;
        Vector2 normal;
        Vector2 collisionDist = Vector2.Zero;
        Vector2 collisionDist2 = Vector2.Zero;
        Player player1;
        Player player2;
        Controls controls;
        Texture2D background;
        public PlatformerMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player1 = new Player(100, 100, 50, 75, false);
            player2 = new Player(100, 100, 50, 75, true);
            //player2 = new Player2(100, 100, 50, 75);
            player1.setP2(player2);
            player2.setP2(player1);
            map = new Platformer.Level();

            base.Initialize();

            Joystick.Init();
            Console.WriteLine("Number of joysticks: " + Sdl.SDL_NumJoysticks());
            controls = new Controls();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("map");
            tileSheet = Content.Load<Texture2D>("FloorPanelTiles");
            map.LoadMap("Content/test.txt");
            map.LoadTileSet(tileSheet);
            map.PopulateCollisionLayer();

            player1.LoadContent(this.Content);
            player2.LoadContent(this.Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //set our keyboardstate tracker update can change the gamestate on every cycle
            controls.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite
            player1.Hold(controls, player2);
            player1.Drop(controls, player2);
            player2.Hold(controls, player1);
            player2.Drop(controls, player1);
            player1.Update(controls, gameTime, map.collisionRects);
            player2.Update(controls, gameTime, map.collisionRects);

            //Reset collision dist
            //collisionDist = Vector2.Zero;
            //collisionDist2 = Vector2.Zero;
            //Check for collisions            
            
            //for (int i = 0; i < map.collisionRects.Count(); i++)
            //{
            //    if (IsColliding(player1.rect, map.collisionRects[i]))
            //    {
            //        //If there are multiple collision make sure we only react to the most severe
            //        if (normal.Length() > collisionDist.Length())
            //            collisionDist = normal;

            //        player1.setGrounded(true);
                        
            //    }
            //}
            ////Update the players position
            //float a = player1.getX() + collisionDist.X;
            //float b = player1.getY() + collisionDist.Y;
            //player1.setX((int)a);
            //player1.setY((int)b);

            //for (int i = 0; i < map.collisionRects.Count(); i++)
            //{
            //    if (IsColliding(player2.rect, map.collisionRects[i]))
            //    {
            //        //If there are multiple collision make sure we only react to the most severe
            //        if (normal.Length() > collisionDist2.Length())
            //            collisionDist2 = normal;
            //    }
            //}
            ////Update the players position

            //float c = player2.getX() + collisionDist2.X;
            //float d = player2.getY() + collisionDist2.Y;
            //player2.setX((int)c);
            //player2.setY((int)d);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(200, 200, 200, 255));

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(background, new Rectangle(-50, 40, 1000, 400), Color.White);

            map.DrawMap();

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        //private bool IsColliding(Rectangle body1, Rectangle body2)
        //{
        //    //Reset the normal vector
        //    normal = Vector2.Zero;

        //    //Get the centre of each body
        //    Vector2 body1Centre = new Vector2(body1.X + (body1.Width / 2), body1.Y + (body1.Height / 2));
        //    Vector2 body2Centre = new Vector2(body2.X + (body2.Width / 2), body2.Y + (body2.Height / 2));

        //    //Declare 2 local vectors
        //    Vector2 distance, absDistance;

        //    //xMag and yMag represent the magnitudes of the x and y components of the normal vector
        //    float xMag, yMag;

        //    //Calculate the difference in position of the two rectangles
        //    distance = body1Centre - body2Centre;

        //    //Get the combined half heights/widths of the rects
        //    float xAdd = ((body1.Width) + (body2.Width)) / 2.0f;
        //    float yAdd = ((body1.Height) + (body2.Height)) / 2.0f;

        //    //Calculate absDistance, according to distance
        //    absDistance.X = (distance.X < 0) ? -distance.X : distance.X;
        //    absDistance.Y = (distance.Y < 0) ? -distance.Y : distance.Y;

        //    //Check if there is a collision
        //    if (!((absDistance.X < xAdd) && (absDistance.Y < yAdd)))
        //        return false;

        //    //The magnitude of the normal vector is determined by the overlap in the rectangles.
        //    xMag = xAdd - absDistance.X;
        //    yMag = yAdd - absDistance.Y;

        //    //Only adjust the normal vector in the direction of the least significant overlap.
        //    if (xMag < yMag)
        //        normal.X = (distance.X > 0) ? xMag : -xMag;
        //    else
        //        normal.Y = (distance.Y > 0) ? yMag : -yMag;

        //    //There was a collision, return true
        //    return true;
        //}
    }

}


#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
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
        SpriteBatch spriteBatch;
        // public static Texture2D tileSheet;
        
        //Level map;
        //Player player1;
        //Player player2;
        //Controls controls;
        //Texture2D background;
        public PlatformerMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 768; //Fits 24x40 tiles
            graphics.PreferredBackBufferWidth = 1280;// ^
            graphics.ApplyChanges();

            Components.Add(new Menu(this, null));

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

            //player1 = new Player(100, 200, 32, 64, false);
            //player2 = new Player(100, 100, 32, 64, true);
            //player1.setP2(player2);
            //player2.setP2(player1);
            //map = new Platformer.Level();

            base.Initialize();

            //Joystick.Init();
            //Console.WriteLine("Number of joysticks: " + Sdl.SDL_NumJoysticks());
            //controls = new Controls();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            Services.AddService(typeof(GraphicsDeviceManager), graphics);

            //background = Content.Load<Texture2D>("map");
            //tileSheet = Content.Load<Texture2D>("FloorPanelTiles");
            //map.LoadMap("Content/test.txt");
            //map.LoadTileSet(tileSheet);
            //map.PopulateCollisionLayer();

            //player1.LoadContent(this.Content);
            //player2.LoadContent(this.Content);

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
            ////set our keyboardstate tracker update can change the gamestate on every cycle
            //controls.Update();

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            //if (controls.onPress(Keys.Back, Buttons.Back))
            //    Initialize();

            //// TODO: Add your update logic here
            ////Up, down, left, right affect the coordinates of the sprite
            //player1.Update(controls, gameTime, map.collisionRects);
            //player2.Update(controls, gameTime, map.collisionRects);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(200, 200, 200, 255));

            //// TODO: Add your drawing code here
            //spriteBatch.Begin();
            ////spriteBatch.Draw(background, new Rectangle(-50, 40, 1000, 400), Color.White);

            //map.DrawMap();

            //player1.Draw(spriteBatch);
            //player2.Draw(spriteBatch);
            //spriteBatch.End();

            base.Draw(gameTime);
        }
       
    }

}


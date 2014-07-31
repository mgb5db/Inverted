using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Tao.Sdl;

namespace Platformer
{
    public class GameLoop : DrawableGameComponent
    {
        Level map;
        Player player1;
        Player player2;
        Controls controls;
        Texture2D background;

        public static Texture2D tileSheet;
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public GameLoop(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            player1 = new Player(100, 200, 32, 64, false);
            player2 = new Player(100, 100, 32, 64, true);
            player1.setP2(player2);
            player2.setP2(player1);
            map = new Platformer.Level();

            Joystick.Init();
            Console.WriteLine("Number of joysticks: " + Sdl.SDL_NumJoysticks());
            controls = new Controls();

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));

            // TODO: use this.Content to load your game content here
            background = Game.Content.Load<Texture2D>("map");
            tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
            map.LoadMap("Content/test.txt");
            map.LoadTileSet(tileSheet);
            map.PopulateCollisionLayer();

            player1.LoadContent(this.Game.Content);
            player2.LoadContent(this.Game.Content);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //set our keyboardstate tracker update can change the gamestate on every cycle
            controls.Update();

            if (controls.onPress(Keys.Back, Buttons.Back))
            {
                this.Initialize();   
            }

            if (controls.onPress(Keys.Escape, Buttons.Start))
            {
                Game.Components.Add(new Menu(this.Game, null));
                this.Dispose();
            }
                
            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite
            player1.Update(controls, gameTime, map.collisionRects);
            player2.Update(controls, gameTime, map.collisionRects);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(200, 200, 200, 255));
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            map.DrawMap();

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
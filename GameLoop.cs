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
using Microsoft.Xna.Framework.Media;
using Tao.Sdl;

namespace Platformer
{
    public class GameLoop : DrawableGameComponent
    {
        Level map;
        Player player1;
        Player player2;
        Endline end;
        Controls controls;
        Texture2D background;
        int level;

        public static Texture2D tileSheet;
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        SoundEffect death;
        SoundEffect reset;
        SoundEffect clear;

        SoundEffect theme;
        SoundEffectInstance bgm;

        public GameLoop(Game game, int level)
            : base(game)
        {
            // TODO: Construct any child components here
            this.level = level;
        }

        public override void Initialize()
        {
            // TODO: use this.Content to load your game content here
            death = Game.Content.Load<SoundEffect>("death.wav");
            reset = Game.Content.Load<SoundEffect>("reset.wav");
            clear = Game.Content.Load<SoundEffect>("clear.wav");

            if (bgm == null)
            {
                theme = Game.Content.Load<SoundEffect>("space.wav");
                bgm = theme.CreateInstance();
                bgm.IsLooped = true;
                bgm.Volume = 0.5f;
                bgm.Play();
            }

            if (level == -3)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level-3bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level-3.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            if (level == -2)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level-2bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level-2.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            if (level == -1)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level-1bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level-1.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            if (level == 0)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level0bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level0.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            else if (level == 1)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level1bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level1.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            else if (level == 2)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level2bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level2.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            else if (level == 3)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level0bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level3.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            else if (level == 4)
            {
                player1 = new Player(50, 200, 32, 64, false);
                player2 = new Player(50, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level0bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level4.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }
            else if (level == 5)
            {
                player1 = new Player(100, 10, 32, 64, false);
                player2 = new Player(100, 100, 32, 64, true);
                player1.setP2(player2);
                player2.setP2(player1);

                end = new Endline(1216, 32, 32, 256);
                background = Game.Content.Load<Texture2D>("level0bg");

                map = new Platformer.Level();
                tileSheet = Game.Content.Load<Texture2D>("FloorPanelTiles");
                map.LoadMap("Content/level5.txt");
                map.LoadTileSet(tileSheet);
                map.PopulateCollisionLayer();
            }

            Joystick.Init();
            Console.WriteLine("Number of joysticks: " + Sdl.SDL_NumJoysticks());
            controls = new Controls();
            
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));


            end.LoadContent(this.Game.Content);
            player1.LoadContent(this.Game.Content);
            player2.LoadContent(this.Game.Content);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //set our keyboardstate tracker update can change the gamestate on every cycle
            controls.Update();

            //Reset level
            if (controls.onPress(Keys.Back, Buttons.Back))
            {
                reset.Play();
                this.Initialize();   
            }

            if (end.checkCollision(player1, player2))
            {
                if (level == 5)
                {
                    Game.Components.Add(new Screen(this.Game, "WinScreen"));
                    bgm.Stop();
                    Game.Components.Remove(this);
                }
                else
                {
                    clear.Play();
                    level++;
                    this.Initialize();
                }
                
            }

            if (player1.spriteY > 768 || player2.spriteY < -64)
            {
                death.Play();
                this.Initialize();
            }

            if (controls.onPress(Keys.J, Buttons.LeftShoulder) && controls.onPress(Keys.M, Buttons.RightShoulder))
            {
                if (level == 5)
                {
                    bgm.Stop();
                    Game.Components.Add(new Screen(this.Game, "WinScreen"));
                    Game.Components.Remove(this);
                }
                else
                {
                    level++;
                    //bgmi.Stop();
                    this.Initialize();
                }
            }

            //Back to menu
            //TODO: Fix garbage collection. Dispose doesn't work apparently.
            if (controls.onPress(Keys.Escape, Buttons.Start))
            {
                bgm.Stop();
                Game.Components.Add(new Menu(this.Game, null));
                Game.Components.Remove(this);
            }
                
            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite
            player1.Update(controls, gameTime, map.collisionRects);
            player2.Update(controls, gameTime, map.collisionRects);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 768), Color.White);
            map.DrawMap();
            end.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
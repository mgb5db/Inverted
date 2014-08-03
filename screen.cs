using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Platformer
{
    public class Screen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D image;

        SoundEffect start;
        Song theme;
        Controls controls;

        String name;

        public Screen(Game game, String name)
            : base(game)
        {
            this.name = name;
        }

        public override void Initialize()
        {
            controls = new Controls();
            start = Game.Content.Load<SoundEffect>("start.wav");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));//new SpriteBatch(GraphicsDevice);
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));

            image = Game.Content.Load<Texture2D>(name);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            controls.Update();

            if (controls.onPress(Keys.Enter, Buttons.Start))
            {
                start.Play();
                if (name.Equals("Tutorial2"))
                {
                    Game.Components.Add(new GameLoop(Game, 0));
                    Game.Components.Remove(this);
                }
                else if (name.Equals("Tutorial1"))
                {
                    Game.Components.Add(new Screen(Game, "Tutorial2"));
                    Game.Components.Remove(this);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, new Rectangle(0, 0, 1280, 768), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}

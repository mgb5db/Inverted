using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
namespace Platformer
{
	abstract class Sprite
	{
		public int spriteX, spriteY;
		public int spriteWidth, spriteHeight;
		public Texture2D image;

		public Sprite ()
		{
		}
	}
}


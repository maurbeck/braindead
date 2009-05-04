using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Beta
{
    class Credits
    {
        // Enumeration for game states
        enum State
        {
            Menu = 1,
            Instructions = 2,
            Game = 3,
            Quit = 4,
            Credits = 5
        }

        // Background image
        Image background = new Image();

        // Cursor
        Cursor cursor = new Cursor();

        public Credits()
        {
        }

        public void Initialize()
        {
            background.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 0f);
            cursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0f);
        }

        public void Click(ref int state)
        {
            state = (int)State.Quit;
        }

        public void Update(GameTime gameTime, ref int state)
        {
            cursor.Update();
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D bgTex, Texture2D curTex)
        {
            this.background.LoadContent(spriteBatch, ref bgTex);
            this.cursor.LoadContent(spriteBatch, ref curTex);
        }

        public void Draw()
        {
            background.Draw();
            cursor.Draw();
        }
    }
}

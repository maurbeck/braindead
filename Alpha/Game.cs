using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Alpha
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Board board = new Board();

        Texture2D boardTex;
        Texture2D redTex;
        Texture2D greenTex;
        Texture2D redToGreen;
        Texture2D greenToRed;
        Texture2D greenCursor;
        Texture2D redCursor;

        Rectangle click;
        bool clickEnabled = true;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set windows size
            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();

            // Set mouse to visible
            //this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            board.Initialize();

            click = new Rectangle(0, 0, 700, 700);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            boardTex = Content.Load<Texture2D>("Board");
            redTex = Content.Load<Texture2D>("Red");
            greenTex = Content.Load<Texture2D>("Green");
            redToGreen = Content.Load<Texture2D>("RedToGreen");
            greenToRed = Content.Load<Texture2D>("GreenToRed");
            redCursor = Content.Load<Texture2D>("RedCursor");
            greenCursor = Content.Load<Texture2D>("GreenCursor");

            board.LoadContent(spriteBatch, boardTex, redTex, greenTex, redToGreen, greenToRed, redCursor, greenCursor);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (clickEnabled)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    board.Click(mouseState);
                    clickEnabled = false;
                }
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    clickEnabled = true;
                }
            }
            board.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            board.Draw();

            base.Draw(gameTime);
        }
    }
}

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

namespace Beta
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Menu menu = new Menu();

        Board board = new Board();

        // Cursor textures
        Texture2D greenCursor;
        Texture2D redCursor;

        // Textures for the menu state
        Texture2D menuBgTex;
        Texture2D gameBtnTex;
        Texture2D tutorialBtnTex;
        Texture2D quitBtnTex;

        // Textures for the game state
        Texture2D boardTex;
        Texture2D redTex;
        Texture2D greenTex;
        Texture2D redToGreen;
        Texture2D greenToRed;

        bool clickEnabled = true;

        enum State
        {
            Menu = 1,
            Tutorial = 2,
            Game = 3,
            Quit = 4
        }

        int gameState;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set windows size
            graphics.PreferredBackBufferWidth = 798;
            graphics.PreferredBackBufferHeight = 798;
            graphics.ApplyChanges();

            // Set mouse to visible
            //this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameState = (int)State.Menu;
            menu.Initialize();
            board.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load the content for the menu state
            menuBgTex = Content.Load<Texture2D>("Menu");
            gameBtnTex = Content.Load<Texture2D>("GameButton");
            tutorialBtnTex = Content.Load<Texture2D>("TutorialButton");
            quitBtnTex = Content.Load<Texture2D>("QuitButton");
            redCursor = Content.Load<Texture2D>("RedCursor"); // This is used as the normal cursor, will also be used in other states

            menu.LoadContent(spriteBatch, menuBgTex, redCursor, gameBtnTex, tutorialBtnTex, quitBtnTex);

            // Load the content for the game state
            boardTex = Content.Load<Texture2D>("Board");
            redTex = Content.Load<Texture2D>("Red");
            greenTex = Content.Load<Texture2D>("Green");
            redToGreen = Content.Load<Texture2D>("RedToGreen");
            greenToRed = Content.Load<Texture2D>("GreenToRed");
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
                    switch (gameState)
                    {
                        case (int)State.Menu:
                            menu.Click(mouseState, ref gameState);
                            break;
                        case (int)State.Tutorial:
                            break;
                        case (int)State.Game:
                            board.Click(mouseState);
                            break;
                    }
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

            // Find which update logic to perform
            switch (gameState)
            {
                case (int)State.Menu:
                    menu.Update(gameTime, ref gameState);
                    break;
                case (int)State.Tutorial:
                    break;
                case (int)State.Game:
                    board.Update(gameTime, ref gameState);
                    break;
            }

            // Quit the game if told to do so
            if (gameState == (int)State.Quit)
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case (int)State.Menu:
                    menu.Draw();
                    break;
                case (int)State.Tutorial:
                    break;
                case (int)State.Game:
                    board.Draw();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}

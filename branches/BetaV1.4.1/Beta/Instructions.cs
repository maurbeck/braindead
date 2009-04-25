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
    class Instructions
    {
        // Enumeration for game states
        enum State
        {
            Menu = 1,
            Instructions = 2,
            Game = 3,
            Quit = 4
        }

        // Background image
        Image background1 = new Image();
        Image background2 = new Image();
        Image background3 = new Image();

        // Cursor
        Cursor cursor = new Cursor();

        // Buttons
        Image mainButton = new Image();
        Image backButton = new Image();
        Image nextButton = new Image();

        //Sounds
        SoundEffect mouseClick;

        // Animations
        Animation moveAnim = new Animation();
        Animation attAnim = new Animation();

        // Which screen to draw
        int screen;

        public Instructions()
        {
        }

        public void Initialize()
        {
            // Initialze which screen to draw
            screen = 1;

            // Initialize background images
            background1.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 1.0f);
            background2.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 1.0f);
            background3.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 1.0f);

            // initialize the cursor
            cursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0.0f);

            // instruction animations
            moveAnim.Initialize(new Vector2(249/2, 249/2), new Rectangle(0, 0, 300, 300), Vector2.Zero, new Vector2(0.5f), 0f, 1, 2, true);
            attAnim.Initialize(new Vector2(249/2, 249/2), new Rectangle(0, 0, 300, 300), Vector2.Zero, new Vector2(0.5f), 0f, 1, 6, true);

            // Initialize the buttons
            mainButton.Initialize(new Vector2(10/2, 680/2), new Rectangle(0, 0, 180, 110), Color.White, Vector2.Zero, new Vector2(0.5f), 0.5f);
            backButton.Initialize(new Vector2(400/2, 680/2), new Rectangle(0, 0, 180, 110), Color.White, Vector2.Zero, new Vector2(0.5f), 0.5f);
            nextButton.Initialize(new Vector2(610/2, 680/2), new Rectangle(0, 0, 180, 110), Color.White, Vector2.Zero, new Vector2(0.5f), 0.5f);
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D bg1Tex, Texture2D bg2Tex, Texture2D bg3Tex, Texture2D moveAnim, Texture2D attAnim, Texture2D cursor, Texture2D mainBtnTex, Texture2D backBtnTex, Texture2D nextBtnTex)
        {
            background1.LoadContent(spriteBatch, bg1Tex);
            background2.LoadContent(spriteBatch, bg2Tex);
            background3.LoadContent(spriteBatch, bg3Tex);

            this.moveAnim.LoadContent(spriteBatch, moveAnim);
            this.attAnim.LoadContent(spriteBatch, attAnim);

            this.cursor.LoadContent(spriteBatch, cursor);

            mainButton.LoadContent(spriteBatch, mainBtnTex);
            backButton.LoadContent(spriteBatch, backBtnTex);
            nextButton.LoadContent(spriteBatch, nextBtnTex);
        }

        public void LoadAudio(SoundEffect mouseClick)
        {
            this.mouseClick = mouseClick;
        }

        public void Update(GameTime gameTime, ref int state)
        {
            switch (screen)
            {
                case 1:
                    moveAnim.Update(gameTime);
                    break;
                case 2:
                    attAnim.Update(gameTime);
                    break;
            }
            cursor.Update();
        }

        public void Click(MouseState mouseState, ref int state)
        {
            if (mouseState.X > 10/2 && mouseState.X < 190/2 && mouseState.Y > 680/2 && mouseState.Y < 900/2)
            {
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                screen = 1;
                state = (int)State.Menu;
                Initialize();
            }

            switch (screen)
            {
                case 1:
                    // Next button
                    if (mouseState.X > 610 / 2 && mouseState.X < 790 / 2 && mouseState.Y > 680 / 2 && mouseState.Y < 900 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        moveAnim.Reset();
                        moveAnim.Start();
                        screen = 2;
                    }
                    break;
                case 2:
                    // Next button
                    if (mouseState.X > 610 / 2 && mouseState.X < 790 / 2 && mouseState.Y > 680 / 2 && mouseState.Y < 900 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        attAnim.Reset();
                        attAnim.Start();
                        screen = 3;
                    }
                    // Back button
                    if (mouseState.X > 400 / 2 && mouseState.X < 580 / 2 && mouseState.Y > 680 / 2 && mouseState.Y < 900 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        attAnim.Reset();
                        attAnim.Start();
                        screen = 1;
                    }
                    break;
                case 3:
                    // Back button
                    if (mouseState.X > 400 / 2 && mouseState.X < 580 / 2 && mouseState.Y > 680 / 2 && mouseState.Y < 900 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 2;
                    }
                    break;
            }
            

        }

        public void Draw()
        {
            switch (screen)
            {
                case 1:
                    background1.Draw();
                    nextButton.Draw();
                    moveAnim.Draw();
                    break;
                case 2:
                    background2.Draw();
                    nextButton.Draw();
                    backButton.Draw();
                    attAnim.Draw();
                    break;
                case 3:
                    background3.Draw();
                    backButton.Draw();
                    break;
            }
            mainButton.Draw();
            cursor.Draw();
        }
    }
}

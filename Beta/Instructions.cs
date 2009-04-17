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

            // initialize the cursor
            cursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0.0f);

            // instruction animations
            moveAnim.Initialize(new Vector2(448/2, 40/2), new Rectangle(0, 0, 300, 300), Vector2.Zero, new Vector2(0.5f), 0f, 1, 2, true);
            attAnim.Initialize(new Vector2(78/2, 448/2), new Rectangle(0, 0, 300, 300), Vector2.Zero, new Vector2(0.5f), 0f, 1, 2, true);

            // Initialize the buttons
            mainButton.Initialize(new Vector2(500/2, 680/2), new Rectangle(0, 0, 180, 110), Color.White, Vector2.Zero, new Vector2(0.5f), 0.5f);
            //backButton.Initialize(new Vector2(250/2, 680/2), new Rectangle(0, 0, 180, 110), Color.White, Vector2.Zero, new Vector2(0.5f), 0.5f);
            //nextButton.Initialize(new Vector2(250/2, 680/2), new Rectangle(0, 0, 180, 110), Color.White, Vector2.Zero, new Vector2(0.5f), 0.5f);
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D bg1Tex, Texture2D bg2Tex, Texture2D moveAnim, Texture2D attAnim, Texture2D cursor, Texture2D mainBtnTex, Texture2D backBtnTex, Texture2D nextBtnTex)
        {
            background1.LoadContent(spriteBatch, bg1Tex);
            background2.LoadContent(spriteBatch, bg2Tex);

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
            this.moveAnim.Update(gameTime);
            this.attAnim.Update(gameTime);
            cursor.Update();
        }

        public void Click(MouseState mouseState, ref int state)
        {
            if (mouseState.X > 500/2 && mouseState.X < 680/2 && mouseState.Y > 680/2 && mouseState.Y < 900/2)
            {
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                screen = 1;
                state = (int)State.Menu;
            }
 /*           if (mouseState.X > 250/2 && mouseState.X < 430/2 && mouseState.Y > 680/2 && mouseState.Y < 790/2)
            {
                if (screen == 1)
                {
                    screen = 2;
                }
                else
                {
                    screen = 1;
                }
            }
 */
        }

        public void Draw()
        {
            switch (screen)
            {
                case 1:
                    background1.Draw();
                    nextButton.Draw();
                    moveAnim.Draw();
                    attAnim.Draw();
                    break;
                case 2:
                    background2.Draw();
                    backButton.Draw();
                    break;
            }
            mainButton.Draw();
            cursor.Draw();
        }
    }
}

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
            Quit = 4,
            Credits = 5
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
        Animation tipsAnim = new Animation();
        Animation tipsAnimLeft = new Animation();
        Animation tipsAnimRight = new Animation();

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
            background1.Initialize( Vector2.Zero, new Rectangle(0, 0, 798, 798), 
                                    Color.White, Vector2.Zero,
                                    new Vector2(0.5f), 1.0f);
            background2.Initialize( Vector2.Zero, new Rectangle(0, 0, 798, 798),
                                    Color.White, Vector2.Zero,
                                    new Vector2(0.5f), 1.0f);
            background3.Initialize( Vector2.Zero, new Rectangle(0, 0, 798, 798),
                                    Color.White, Vector2.Zero,
                                    new Vector2(0.5f), 1.0f);

            // initialize the cursor
            cursor.Initialize(  Vector2.Zero, new Rectangle(0, 0, 50, 50),
                                Color.White, Vector2.Zero,
                                new Vector2(0.5f), 0.0f);

            // instruction animations
            moveAnim.Initialize(new Vector2(249 / 2, 249 / 2), new Rectangle(0, 0, 300, 394),
                                Vector2.Zero, new Vector2(0.5f),
                                0f, 1, 2, true);
            
            attAnim.Initialize( new Vector2(249 / 2, 249 / 2), new Rectangle(0, 0, 300, 300),
                                Vector2.Zero, new Vector2(0.5f),
                                0f, 1, 6, true);

            tipsAnim.Initialize(new Vector2(249 / 2, 249 / 2), new Rectangle(0, 0, 300, 394),
                                Vector2.Zero, new Vector2(0.5f),
                                0f, 0.41f, 6, true);

            tipsAnimLeft.Initialize(new Vector2(34 / 2, 262 / 2), new Rectangle(0, 0, 190, 270),
                                    Vector2.Zero, new Vector2(0.5f),
                                    0f, 0.41f, 6, true);
            tipsAnimRight.Initialize(   new Vector2(572 / 2, 262 / 2), new Rectangle(0, 0, 190, 270),
                                        Vector2.Zero, new Vector2(0.5f),
                                        0f, 0.41f, 6, true);

            // Initialize the buttons
            mainButton.Initialize(  new Vector2(580 / 2, 660 / 2), new Rectangle(0, 0, 180, 110),
                                    Color.White, Vector2.Zero,
                                    new Vector2(0.5f), 0.5f);
            backButton.Initialize(  new Vector2(30 / 2, 660 / 2), new Rectangle(0, 0, 180, 110),
                                    Color.White, Vector2.Zero, 
                                    new Vector2(0.5f), 0.5f);
            nextButton.Initialize(  new Vector2(580 / 2, 660 / 2), new Rectangle(0, 0, 180, 110),
                                    Color.White, Vector2.Zero,
                                    new Vector2(0.5f), 0.5f);
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D bg1Tex, 
                                Texture2D bg2Tex, Texture2D bg3Tex, 
                                Texture2D moveAnim, Texture2D attAnim,
                                Texture2D tipsAnim, Texture2D tipsAnimLeft,
                                Texture2D tipsAnimRight, Texture2D cursor,
                                Texture2D mainBtnTex, Texture2D backBtnTex, 
                                Texture2D nextBtnTex)
        {
            background1.LoadContent(spriteBatch, ref bg1Tex);
            background2.LoadContent(spriteBatch, ref bg2Tex);
            background3.LoadContent(spriteBatch, ref bg3Tex);

            this.moveAnim.LoadContent(spriteBatch, ref moveAnim);
            this.attAnim.LoadContent(spriteBatch, ref attAnim);
            this.tipsAnim.LoadContent(spriteBatch, ref tipsAnim);
            this.tipsAnimLeft.LoadContent(spriteBatch, ref tipsAnimLeft);
            this.tipsAnimRight.LoadContent(spriteBatch, ref tipsAnimRight);

            this.cursor.LoadContent(spriteBatch, ref cursor);

            mainButton.LoadContent(spriteBatch, ref mainBtnTex);
            backButton.LoadContent(spriteBatch, ref backBtnTex);
            nextButton.LoadContent(spriteBatch, ref nextBtnTex);
        }

        public void LoadAudio(SoundEffect mouseClick)
        {
            this.mouseClick = mouseClick;
        }

        public void Update(GameTime gameTime, ref int state)
        {

#if XBOX

            /*
             * XBOX port code
             * 
             * 
             * 
             * Modified/Created by: Miles Aurbeck
             * May 06 2009
            */

            // If left trigger was depressed, set the state to the main menu
            // Then reset this board
            // Then return so nothing gets processed
            if (Game.previousGamePadState.Buttons.B == ButtonState.Pressed &&
                Game.gamePadState.Buttons.B == ButtonState.Released)
            {
                state = (int)State.Menu;
                this.Initialize();
                return;
            }
#endif

            switch (screen)
            {
                case 1:
                    moveAnim.Update(gameTime);
                    break;
                case 2:
                    attAnim.Update(gameTime);
                    break;
                case 3:
                    tipsAnim.Update(gameTime);
                    tipsAnimLeft.Update(gameTime);
                    tipsAnimRight.Update(gameTime);
                    break;
            }
            cursor.Update();
        }



        public void Click(MouseState mouseState, ref int state)
        {
            /* if (mouseState.X > 10/2 && mouseState.X < 190/2 && mouseState.Y > 680/2 && mouseState.Y < 900/2)
             {
                 mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                 screen = 1;
                 state = (int)State.Menu;
                 Initialize();
             }
            */
            switch (screen)
            {
                case 1:
                    // Next button
                    if (mouseState.X > 580 / 2 && mouseState.X < 760 / 2 && 
                        mouseState.Y > 660 / 2 && mouseState.Y < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        moveAnim.Reset();
                        moveAnim.Start();
                        screen = 2;
                    }
                    if (mouseState.X > 30 / 2 && mouseState.X < 210 / 2 &&
                        mouseState.Y > 660 / 2 && mouseState.Y < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 1;
                        state = (int)State.Menu;
                        Initialize();
                    }
                    break;
                case 2:
                    // Next button
                    if (mouseState.X > 580 / 2 && mouseState.X < 760 / 2 &&
                        mouseState.Y > 660 / 2 && mouseState.Y < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        attAnim.Reset();
                        attAnim.Start();
                        screen = 3;
                    }
                    // Back button
                    if (mouseState.X > 30 / 2 && mouseState.X < 210 / 2 &&
                        mouseState.Y > 660 / 2 && mouseState.Y < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        attAnim.Reset();
                        attAnim.Start();
                        screen = 1;
                    }
                    break;
                case 3:
                    // Back button
                    if (mouseState.X > 30 / 2 && mouseState.X < 210 / 2 &&
                        mouseState.Y > 660 / 2 && mouseState.Y < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 2;
                    }
                    if (mouseState.X > 580 / 2 && mouseState.X < 760 / 2 &&
                        mouseState.Y > 660 / 2 && mouseState.Y < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 1;
                        state = (int)State.Game;
                        Initialize();
                    }
                    break;
            }
        }

        public void aButtClick(ref int state)
        {

            switch (screen)
            {
                case 1:
                    // Next button
                    if (Game.xbCursorX > 580 / 2 && Game.xbCursorX < 760 / 2 &&
                        Game.xbCursorY > 660 / 2 && Game.xbCursorY < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        moveAnim.Reset();
                        moveAnim.Start();
                        screen = 2;
                    }
                    if (Game.xbCursorX > 30 / 2 && Game.xbCursorX < 210 / 2 &&
                        Game.xbCursorY > 660 / 2 && Game.xbCursorY < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 1;
                        state = (int)State.Menu;
                        Initialize();
                    }
                    break;
                case 2:
                    // Next button
                    if (Game.xbCursorX > 580 / 2 && Game.xbCursorX < 760 / 2 &&
                        Game.xbCursorY > 660 / 2 && Game.xbCursorY < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        attAnim.Reset();
                        attAnim.Start();
                        screen = 3;
                    }
                    // Back button
                    if (Game.xbCursorX > 30 / 2 && Game.xbCursorX < 210 / 2 &&
                        Game.xbCursorY > 660 / 2 && Game.xbCursorY < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        attAnim.Reset();
                        attAnim.Start();
                        screen = 1;
                    }
                    break;
                case 3:
                    // Back button
                    if (Game.xbCursorX > 30 / 2 && Game.xbCursorX < 210 / 2 &&
                        Game.xbCursorY > 660 / 2 && Game.xbCursorY < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 2;
                    }
                    if (Game.xbCursorX > 580 / 2 && Game.xbCursorX < 760 / 2 &&
                        Game.xbCursorY > 660 / 2 && Game.xbCursorY < 880 / 2)
                    {
                        mouseClick.Play(1.0f, 0.0f, 0.0f, false);
                        screen = 1;
                        state = (int)State.Game;
                        Initialize();
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
                    backButton.Draw();
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
                    mainButton.Draw();
                    tipsAnim.Draw();
                    tipsAnimLeft.Draw();
                    tipsAnimRight.Draw();
                    break;
            }
            //  mainButton.Draw();
            cursor.Draw();
        }
    }
}

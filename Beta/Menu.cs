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
    class Menu
    {
        // Enumeration for game states
        enum State
        {
            Menu = 1,
            Instructions = 2,
            Game = 3,
            Quit = 4,
            Credits = 5,
            AIGame = 6,
            Othello = 7
        }

        // Background image
        Image background = new Image();

        // Cursor
        Cursor cursor = new Cursor();

        // Buttons
        // Are animations to handle mouse overs
        //Animation gameButton = new Animation();
        //Animation tutorialButton = new Animation();
        //Animation quitButton = new Animation();

        Animation onePlayerJojamianButton = new Animation();
        Animation twoPlayerJojamianButton = new Animation();
        Animation rulesJojamianButton = new Animation();
        Animation endGameButton = new Animation();
        Animation onePlayerOthelloButton = new Animation();

        //Sounds
        SoundEffect mouseClick;

        public Menu()
        {
        }
        
        public void Initialize()
        {
            // Initialize background
            background.Initialize(  Vector2.Zero, new Rectangle(0, 0, 798, 798),
                                    Color.White, Vector2.Zero, 
                                    new Vector2(0.5f), 1.0f);
            // Initialize cursor
            cursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50),
                                Color.White, Vector2.Zero,
                                new Vector2(0.5f), 0.0f);

            // Initialize buttons
            //gameButton.Initialize(  new Vector2(279/2, 200/2), new Rectangle(0, 0, 240, 100),
            //                        Vector2.Zero, new Vector2(0.5f),
            //                        0.5f, 10, 2, false);

            //tutorialButton.Initialize(  new Vector2(279/2, 349/2), new Rectangle(0, 0, 240, 100),
            //                            Vector2.Zero, new Vector2(0.5f),
            //                            0.5f, 10, 2, false);

            //quitButton.Initialize(  new Vector2(279/2, 500/2), new Rectangle(0, 0, 240, 100),
            //                        Vector2.Zero, new Vector2(0.5f),
            //                        0.5f, 10, 2, false);


            onePlayerJojamianButton.Initialize( new Vector2(92 / 2, 300 / 2 ), new Rectangle(0, 0, 280, 80),
                                    Vector2.Zero, new Vector2(0.5f),
                                    0.5f, 10, 2, false);
                                        

            twoPlayerJojamianButton.Initialize(new Vector2(92 / 2, 400 / 2), new Rectangle(0, 0, 280, 80),
                                    Vector2.Zero, new Vector2(0.5f),
                                    0.5f, 10, 2, false);

            rulesJojamianButton.Initialize(new Vector2(92 / 2, 500 / 2), new Rectangle(0, 0, 280, 80),
                                    Vector2.Zero, new Vector2(0.5f),
                                    0.5f, 10, 2, false);

            onePlayerOthelloButton.Initialize(new Vector2(442 / 2, 400 / 2), new Rectangle(0, 0, 280, 80),
                        Vector2.Zero, new Vector2(0.5f),
                        0.5f, 10, 2, false);
            
            endGameButton.Initialize(new Vector2(300 / 2, 625 / 2), new Rectangle(0, 0, 280, 80),
                                    Vector2.Zero, new Vector2(0.5f),
                                    0.5f, 10, 2, false);





            //End Initialize Buttons
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D menuBackground,
                                Texture2D cursorTex, Texture2D onePlayerJojamianButtonTexture, 
                                Texture2D twoPlayerJojamianButtonTexture, Texture2D rulesJojamianButtonTexture,
                                Texture2D endGameButtonTexture, Texture2D onePlayerOthelloButtonTexture)
        {
            // Pass content to the background image
            background.LoadContent(spriteBatch, ref menuBackground);
            // Pass content to the cursor
            cursor.LoadContent(spriteBatch, ref cursorTex);

            // Pass content to the buttons
            //gameButton.LoadContent(spriteBatch, ref gameBtnTex);
            //tutorialButton.LoadContent(spriteBatch, ref tutorialBtnTex);
            //quitButton.LoadContent(spriteBatch, ref quitBtnTex);

            onePlayerJojamianButton.LoadContent(spriteBatch, ref onePlayerJojamianButtonTexture);
            twoPlayerJojamianButton.LoadContent(spriteBatch, ref twoPlayerJojamianButtonTexture);
            rulesJojamianButton.LoadContent(spriteBatch, ref rulesJojamianButtonTexture);
            endGameButton.LoadContent(spriteBatch, ref endGameButtonTexture);
            onePlayerOthelloButton.LoadContent(spriteBatch, ref onePlayerOthelloButtonTexture);

        }

        public void LoadAudio(SoundEffect mouseClick)
        {
            this.mouseClick = mouseClick;
        }

        public void Update(GameTime gameTime, ref int state)
        {


#if WINDOWS
            // Get the mouse state
            MouseState mouseState = Mouse.GetState();
            // Get the keyboard state
            KeyboardState keyState = Keyboard.GetState();

            // Temporary keyboard shortcuts to get to the new state
            // Will be removed with the UI has these buttons
            if (keyState.IsKeyDown(Keys.A))
            {
                state = (int)State.AIGame;
                return;
            }
            else if (keyState.IsKeyDown(Keys.O))
            {
                state = (int)State.Othello;
                return;
            }
            //'Mouse'over
            //
            // Change one player jojamian button on mouse over
            if (mouseState.X > 92 / 2 && mouseState.X < 378 / 2 &&
                mouseState.Y > 300 / 2 && mouseState.Y < 380 / 2)
                onePlayerJojamianButton.LastFrame();
            else
                onePlayerJojamianButton.Reset();

            //change two player jojamian button
            if (mouseState.X > 92 / 2 && mouseState.X < 378 / 2 &&
                 mouseState.Y > 400 / 2 && mouseState.Y < 480 / 2)
                twoPlayerJojamianButton.LastFrame();
            else
                twoPlayerJojamianButton.Reset();

            //change rules button on mouseover
            if (mouseState.X > 92 / 2 && mouseState.X < 378 / 2 &&
                mouseState.Y > 500 / 2 && mouseState.Y < 580 / 2)
                rulesJojamianButton.LastFrame();
            else
                rulesJojamianButton.Reset();

            //change othello button on mouse over
            if (mouseState.X > 442 / 2 && mouseState.X < 722 / 2 &&
                mouseState.Y > 400 / 2 && mouseState.Y < 480 / 2)
                onePlayerOthelloButton.LastFrame();
            else
                onePlayerOthelloButton.Reset();

            //change end game button on mouseover
            if (mouseState.X > 300 / 2 && mouseState.X < 580 / 2 &&
                mouseState.Y > 625 / 2 && mouseState.Y < 705 / 2)
                endGameButton.LastFrame();
            else
                endGameButton.Reset();


#endif

#if XBOX
            /*
             * XBOX port code
             * 
             * 
             * 
             * Modified/Created by: Miles Aurbeck
             * May 06 2009
            */            

            //Mouseover Code
            //           
            // Change game button on mouse over
            if (Game.xbCursorX > 279 / 2 && Game.xbCursorX < 519 / 2 &&
                Game.xbCursorY > 200 / 2 && Game.xbCursorY < 300 / 2)
                gameButton.LastFrame();
            else
                gameButton.Reset();

            // Change tutorial button on mouse over
            if (Game.xbCursorX > 279 / 2 && Game.xbCursorX < 519 / 2 &&
                Game.xbCursorY > 349 / 2 && Game.xbCursorY < 449 / 2)
                tutorialButton.LastFrame();
            else
                tutorialButton.Reset();

            // Change quit button on mouse over
            if (Game.xbCursorX > 279 / 2 && Game.xbCursorX < 519 / 2 && 
                Game.xbCursorY > 500 / 2 && Game.xbCursorY < 600 / 2)
                quitButton.LastFrame();
            else
                quitButton.Reset();

#endif

            // Update Cursor position
            cursor.Update();

            // Update the buttons
            //gameButton.Update(gameTime);
            //tutorialButton.Update(gameTime);
            //quitButton.Update(gameTime);
            onePlayerJojamianButton.Update(gameTime);
            twoPlayerJojamianButton.Update(gameTime);
            rulesJojamianButton.Update(gameTime);
            onePlayerOthelloButton.Update(gameTime);
            endGameButton.Update(gameTime);
            
        }


        public void aButtClick(ref int state)
        {

        // Check if clicked on tutorial button
            if (Game.xbCursorX > 279 / 2 && Game.xbCursorX < 519 / 2 &&
                Game.xbCursorY > 349 / 2 && Game.xbCursorY < 449 / 2 &&
                Game.previousGamePadState.Buttons.A == ButtonState.Pressed &&
                Game.gamePadState.Buttons.A == ButtonState.Released )
            {
                // No tutorial state yet
                state = (int)State.Instructions;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            }

            // Check if clicked on game button
            if (Game.xbCursorX > 279 / 2 && Game.xbCursorX < 519 / 2 && 
                Game.xbCursorY > 200 / 2 && Game.xbCursorY < 300 / 2 &&
                Game.previousGamePadState.Buttons.A == ButtonState.Pressed &&
                Game.gamePadState.Buttons.A == ButtonState.Released )
            {
                // Set the state to the game state
                state = (int)State.Game;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            }

            // Check if clicked on quit button
            if (Game.xbCursorX > 279 / 2 && Game.xbCursorX < 519 / 2 &&
                Game.xbCursorY > 500 / 2 && Game.xbCursorY < 600 / 2 &&
                Game.previousGamePadState.Buttons.A == ButtonState.Pressed &&
                Game.gamePadState.Buttons.A == ButtonState.Released )
            {
                state = (int)State.Credits;
            }

}

        public void Click(MouseState mouseState, ref int state)
        {
            // Check if clicked on one player jojamain button
            if (mouseState.X > 92 / 2 && mouseState.X < 378 / 2 &&
                mouseState.Y > 300 / 2 && mouseState.Y < 380 / 2)
            {
                // No tutorial state yet
                state = (int)State.AIGame;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            }

            //click on two player jojamian button
            if (mouseState.X > 92 / 2 && mouseState.X < 378 / 2 &&
                 mouseState.Y > 400 / 2 && mouseState.Y < 480 / 2)
            {
                // No tutorial state yet
                state = (int)State.Game;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            }

            //click on rules button
            if (mouseState.X > 92 / 2 && mouseState.X < 378 / 2 &&
                mouseState.Y > 500 / 2 && mouseState.Y < 580 / 2)
            {
                // No tutorial state yet
                state = (int)State.Instructions;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            }

            //click on othello button
            if (mouseState.X > 442 / 2 && mouseState.X < 722 / 2 &&
                mouseState.Y > 400 / 2 && mouseState.Y < 480 / 2)
            {
                // No tutorial state yet
                state = (int)State.Othello;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);

            }
            //click on end game button
            if (mouseState.X > 300 / 2 && mouseState.X < 580 / 2 &&
                mouseState.Y > 625 / 2 && mouseState.Y < 705 / 2)
            {
                // No tutorial state yet
                state = (int)State.Credits;
                mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            }

            //// Check if clicked on game button
            //if (mouseState.X > 279 / 2 && mouseState.X < 519 / 2 && 
            //    mouseState.Y > 200 / 2 && mouseState.Y < 300 / 2)
            //{
            //    // Set the state to the game state
            //    state = (int)State.Game;
            //    mouseClick.Play(1.0f, 0.0f, 0.0f, false);
            //}

            //// Check if clicked on quit button
            //if (mouseState.X > 279 / 2 && mouseState.X < 519 / 2 &&
            //    mouseState.Y > 500 / 2 && mouseState.Y < 600 / 2)
            //{
            //    state = (int)State.Credits;
            //}
        }

        public void Draw()
        {
            // Draw background
            background.Draw();

            // Draw cursor
            cursor.Draw();

            // Draw buttons
            //gameButton.Draw();
            //tutorialButton.Draw();
            //quitButton.Draw();
            onePlayerJojamianButton.Draw();
            twoPlayerJojamianButton.Draw();
            rulesJojamianButton.Draw();
            onePlayerOthelloButton.Draw();
            endGameButton.Draw();
        }
    }
}

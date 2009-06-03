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
    /// 

    public class Game : Microsoft.Xna.Framework.Game
    {

        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Menu menu = new Menu();

        Instructions instructions = new Instructions();

        Board board = new Board();

        AIBoard aiBoard = new AIBoard();

        OthelloBoard othello = new OthelloBoard();

        Credits credits = new Credits();

        

        // Cursor textures
        Texture2D greenCursor;
        Texture2D blueCursor;
        Texture2D blackCursor;
        Texture2D whiteCursor;

        // Textures for the menu state
        Texture2D menuBgTex;
        Texture2D gameBtnTex;
        Texture2D tutorialBtnTex;
        Texture2D quitBtnTex;

        // Textures for instructions
        Texture2D instruction1BgTex;
        Texture2D instruction2BgTex;
        Texture2D instruction3BgTex;
        Texture2D mainBtnTex;
        Texture2D backBtnTex;
        Texture2D nextBtnTex;
        Texture2D moveAnim;
        Texture2D attAnim;
        Texture2D tipsAnim;
        Texture2D tipsAnimLeft;
        Texture2D tipsAnimRight;

        // Textures for the game state
        Texture2D boardTex;
        Texture2D blueTex;
        Texture2D greenTex;
        Texture2D blueSelectTex;
        Texture2D greenSelectTex;
        Texture2D blueToGreen;
        Texture2D greenToBlue;
        Texture2D transToPlr1;
        Texture2D transToPlr2;
        Texture2D blueBanner;
        Texture2D greenBanner;

        //Textures for Othello
        Texture2D othBoard;
        Texture2D blackTex;
        Texture2D whiteTex;
        Texture2D blackToWhite;
        Texture2D whiteToBlack;
        Texture2D blackBanner;
        Texture2D whiteBanner;
       

        // Textures for the credits state
        Texture2D creditBgTex;

        //Sounds
        SoundEffect mtInstructionScreen1;
        SoundEffect mtInstructionScreen2;
        SoundEffect playerOneToPlayerTwo;
        SoundEffect selectPiece;
        SoundEffect unavailableMove;
        SoundEffect availableMove;

        //Manual Cursor locations for 360 input
        public static int xbCursorX;
        public static int xbCursorY;

        //Initialize previous gamepadstate
        public static GamePadState previousGamePadState = 
            GamePad.GetState(PlayerIndex.One);
        public static GamePadState gamePadState = 
            GamePad.GetState(PlayerIndex.One);

        public static GamePadState previousGamePadState2 = 
            GamePad.GetState(PlayerIndex.Two);
        public static GamePadState gamePadState2 = 
            GamePad.GetState(PlayerIndex.Two);


        bool clickEnabled = true;


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

        int gameState;

        public Game()
        {
            //variables to store positions of cursor manually

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            
            // Set windows size
            graphics.PreferredBackBufferWidth = 798 / 2;
            graphics.PreferredBackBufferHeight = 798 / 2;
            graphics.ApplyChanges();

            //graphics.IsFullScreen = true;

            // Set mouse to visible
            //this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameState = (int)State.Menu;
            menu.Initialize();
            instructions.Initialize();
            board.Initialize();
            credits.Initialize();

            aiBoard.Initialize();
            othello.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load the content for the menu state
            menuBgTex = Content.Load<Texture2D>("MainScreenPieces");
            gameBtnTex = Content.Load<Texture2D>("StartBut");
            tutorialBtnTex = Content.Load<Texture2D>("RulesBut");
            quitBtnTex = Content.Load<Texture2D>("EndBut");
            // This is used as the normal cursor, will also be used in other states
            blueCursor = Content.Load<Texture2D>("blueCursor"); 

            menu.LoadContent(   spriteBatch, menuBgTex, blueCursor, 
                                gameBtnTex, tutorialBtnTex, quitBtnTex);

            // Load the content for the instructions state
            instruction1BgTex = Content.Load<Texture2D>("NewInstructions");
            instruction2BgTex = Content.Load<Texture2D>("NewInstructions1");
            instruction3BgTex = Content.Load<Texture2D>("NewInstructions2");
            mainBtnTex = Content.Load<Texture2D>("StartGameBut");
            backBtnTex = Content.Load<Texture2D>("BackButton");
            nextBtnTex = Content.Load<Texture2D>("NextButton");
            moveAnim = Content.Load<Texture2D>("NewMoveAnim");
            attAnim = Content.Load<Texture2D>("NewAttackAnim");
            tipsAnim = Content.Load<Texture2D>("TipsAnim");
            tipsAnimLeft = Content.Load<Texture2D>("TipsAnimLeft");
            tipsAnimRight = Content.Load<Texture2D>("TipsAnimRight");

            instructions.LoadContent(   spriteBatch, instruction1BgTex, 
                                        instruction2BgTex, instruction3BgTex,
                                        moveAnim, attAnim, 
                                        tipsAnim, tipsAnimLeft,
                                        tipsAnimRight, blueCursor,
                                        mainBtnTex, backBtnTex,
                                        nextBtnTex);

            // Load the content for the game state
            boardTex = Content.Load<Texture2D>("BoardTest");
            blueTex = Content.Load<Texture2D>("Blue");
            greenTex = Content.Load<Texture2D>("Green");
            blueSelectTex = Content.Load<Texture2D>("BlueSelection");
            greenSelectTex = Content.Load<Texture2D>("GreenSelection");
            blueToGreen = Content.Load<Texture2D>("BlueToGreen");
            greenToBlue = Content.Load<Texture2D>("GreenToBlue");
            transToPlr1 = Content.Load<Texture2D>("TransToPlr1");
            transToPlr2 = Content.Load<Texture2D>("TransToPlr2");
            blueBanner = Content.Load<Texture2D>("BlueBanner");
            greenBanner = Content.Load<Texture2D>("GreenBanner");

            //Load the content for the othello state
            othBoard = Content.Load<Texture2D>("Board8x8Green");
            blackTex = Content.Load<Texture2D>("Black");
            whiteTex = Content.Load<Texture2D>("White");
            blackToWhite = Content.Load<Texture2D>("blackToWhite");
            whiteToBlack = Content.Load<Texture2D>("whiteToBlack");
            blackBanner = Content.Load<Texture2D>("BlackBanner");
            whiteBanner = Content.Load<Texture2D>("WhiteBanner");
            blackCursor = Content.Load<Texture2D>("BlackCursor");
            whiteCursor = Content.Load<Texture2D>("WhiteCursor");

            // Load the content for the credit state
            creditBgTex = Content.Load<Texture2D>("Credits");
            

            greenCursor = Content.Load<Texture2D>("GreenCursor");

            credits.LoadContent(spriteBatch, creditBgTex, greenCursor);

            //Load the sounds
            mtInstructionScreen1 = Content.Load<SoundEffect>
                ("MouseToneInstructionScreen_8Bit(hi)");
            mtInstructionScreen2 = Content.Load<SoundEffect>
                ("MouseToneInstructionScreen_8Bit(low)");
            playerOneToPlayerTwo = Content.Load<SoundEffect>
                ("Transform_8Bit");
            selectPiece = Content.Load<SoundEffect>
                ("PieceSelectTone_8Bit");
            unavailableMove = Content.Load<SoundEffect>
                ("UnavailableMove_8Bit");
            availableMove = Content.Load<SoundEffect>
                ("AvailableMove_8Bit");

            
            board.LoadContent(  spriteBatch, boardTex, blueTex,
                                greenTex, greenSelectTex, blueSelectTex,
                                blueToGreen, greenToBlue, transToPlr1,
                                transToPlr2, blueCursor, greenCursor,
                                blueBanner, greenBanner);
            board.LoadAudio(selectPiece, unavailableMove, availableMove,
                            playerOneToPlayerTwo);
            menu.LoadAudio(mtInstructionScreen1);
            instructions.LoadAudio(mtInstructionScreen2);

            aiBoard.LoadContent(spriteBatch, boardTex, blueTex,
                                greenTex, greenSelectTex, blueSelectTex,
                                blueToGreen, greenToBlue, transToPlr1,
                                transToPlr2, blueCursor, greenCursor,
                                blueBanner, greenBanner);
            aiBoard.LoadAudio(  selectPiece, unavailableMove, availableMove,
                                playerOneToPlayerTwo);

            othello.LoadContent(spriteBatch, othBoard, blackTex,
                                whiteTex, blackToWhite, whiteToBlack,
                                blueBanner, greenBanner, blackCursor, whiteCursor,
                                transToPlr1, transToPlr2);
            othello.LoadAudio(  selectPiece, unavailableMove, availableMove,
                                playerOneToPlayerTwo);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            //Initialise gamePadState
            gamePadState = GamePad.GetState(PlayerIndex.One);
            gamePadState2 = GamePad.GetState(PlayerIndex.Two);

            //Update cursor for 360 controller input


            if (gamePadState.ThumbSticks.Left.X > 0.0f)
                xbCursorX += 5;

            if (gamePadState.ThumbSticks.Left.X < 0.0f)
                xbCursorX -= 5;

            if (gamePadState.ThumbSticks.Left.Y > 0.0f)
                xbCursorY -= 5;

            if (gamePadState.ThumbSticks.Left.Y < 0.0f)
                xbCursorY += 5;

            if (gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                //omg nothing
            }


            if (clickEnabled)
            {
#if WINDOWS
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    switch (gameState)
                    {
                        case (int)State.Menu:
                            menu.Click(mouseState, ref gameState);
                            break;
                        case (int)State.Instructions:
                            instructions.Click(mouseState, ref gameState);
                            break;
                        case (int)State.Game:
                            board.Click(mouseState);
                            break;
                        case (int)State.Credits:
                            credits.Click(ref gameState);
                            break;
                        case (int)State.AIGame:
                            aiBoard.Click(mouseState);
                            break;
                        case (int)State.Othello:
                            othello.Click(mouseState);
                            break;
                    }
                    clickEnabled = false;
                }
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
                if (Game.previousGamePadState.Buttons.A == ButtonState.Pressed && 
                    Game.gamePadState.Buttons.A == ButtonState.Released)
                {
                    switch (gameState)
                    {
                        case (int)State.Menu:
                            menu.aButtClick(ref gameState);
                            break;
                        case (int)State.Instructions:
                            instructions.aButtClick(ref gameState);
                            break;
                        case (int)State.Game:
                            board.aButtClick();
                            break;
                        case (int)State.Credits:
                            credits.Click(ref gameState);
                            break;
                        case (int)State.AIGame:
                            aiBoard.Click(mouseState);
                            break;
                        case (int)State.Othello:
                            othello.Click(mouseState);
                            break;
                    }
                    clickEnabled = false;
                }
#endif
            }
            else
            {
#if WINDOWS
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    clickEnabled = true;
                }
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
                if (Game.previousGamePadState.Buttons.A == ButtonState.Released)
                {
                    clickEnabled = true;
                }
#endif
            }

            // Find which update logic to perform
            switch (gameState)
            {
                case (int)State.Menu:
                    menu.Update(gameTime, ref gameState);
                    break;
                case (int)State.Instructions:
                    instructions.Update(gameTime, ref gameState);
                    break;
                case (int)State.Game:
                    board.Update(gameTime, ref gameState);
                    break;
                case (int)State.Credits:
                    credits.Update(gameTime, ref gameState);
                    break;
                case (int)State.Othello:
                    othello.Update(gameTime, ref gameState);
                    break;
                case (int)State.AIGame:
                    aiBoard.Update(gameTime, ref gameState);
                    break;
            }

            // Quit the game if told to do so
            if (gameState == (int)State.Quit)
            {
                this.Exit();
            }


            previousGamePadState = gamePadState;
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
                case (int)State.Instructions:
                    instructions.Draw();
                    break;
                case (int)State.Game:
                    board.Draw();
                    break;
                case (int)State.Credits:
                    credits.Draw();
                    break;
                case (int)State.AIGame:
                    aiBoard.Draw();
                    break;
                case (int)State.Othello:
                    othello.Draw();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}

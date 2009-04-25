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

        Instructions instructions = new Instructions();

        Board board = new Board();

        // Cursor textures
        Texture2D greenCursor;
        Texture2D redCursor;

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

        // Textures for the game state
        Texture2D boardTex;
        Texture2D redTex;
        Texture2D greenTex;
        Texture2D redSelectTex;
        Texture2D greenSelectTex;
        Texture2D redToGreen;
        Texture2D greenToRed;
        Texture2D transToPlr1;
        Texture2D transToPlr2;
        Texture2D blueBanner;
        Texture2D greenBanner;

        //Sounds
        SoundEffect mtInstructionScreen1;
        SoundEffect mtInstructionScreen2;
        SoundEffect mtInstructionScreen3;
        SoundEffect mtMainScreen1;
        SoundEffect mtMainScreen2;
        SoundEffect switchTurn;
        SoundEffect playerOneToPlayerTwo;
        SoundEffect playerTwoToPlayerOne;
        SoundEffect playerOneUnknown1;
        SoundEffect playerOneUnknown2;
        SoundEffect playerTwoUnknown1;
        SoundEffect playerTwoUnknown2;
        SoundEffect mouseClick;
        SoundEffect selectPiece;
        SoundEffect unavailableMove;
        SoundEffect availableMove;
        SoundEffect passTurn;

        bool clickEnabled = true;

        enum State
        {
            Menu = 1,
            Instructions = 2,
            Game = 3,
            Quit = 4
        }

        int gameState;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set windows size
            graphics.PreferredBackBufferWidth = 798/2;
            graphics.PreferredBackBufferHeight = 798/2;
            graphics.ApplyChanges();

            // Set mouse to visible
            //this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameState = (int)State.Menu;
            menu.Initialize();
            instructions.Initialize();
            board.Initialize();

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
            redCursor = Content.Load<Texture2D>("RedCursor"); // This is used as the normal cursor, will also be used in other states

            menu.LoadContent(spriteBatch, menuBgTex, redCursor, gameBtnTex, tutorialBtnTex, quitBtnTex);

            // Load the content for the instructions state
            instruction1BgTex = Content.Load<Texture2D>("Instructions");
            instruction2BgTex = Content.Load<Texture2D>("Instructions1");
            instruction3BgTex = Content.Load<Texture2D>("Instructions2");
            mainBtnTex = Content.Load<Texture2D>("ReturnBut");
            backBtnTex = Content.Load<Texture2D>("BackButton");
            nextBtnTex = Content.Load<Texture2D>("NextButton");
            moveAnim = Content.Load<Texture2D>("MoveAnim");
            attAnim = Content.Load<Texture2D>("AttackAnim");

            instructions.LoadContent(spriteBatch, instruction1BgTex, instruction2BgTex, instruction3BgTex, moveAnim, attAnim, redCursor, mainBtnTex, backBtnTex, nextBtnTex);

            // Load the content for the game state
            boardTex = Content.Load<Texture2D>("Board");
            redTex = Content.Load<Texture2D>("Red");
            greenTex = Content.Load<Texture2D>("Green");
            redSelectTex = Content.Load<Texture2D>("BlueSelection");
            greenSelectTex = Content.Load<Texture2D>("GreenSelection");
            redToGreen = Content.Load<Texture2D>("RedToGreen");
            greenToRed = Content.Load<Texture2D>("GreenToRed");
            transToPlr1 = Content.Load<Texture2D>("TransToPlr1");
            transToPlr2 = Content.Load<Texture2D>("TransToPlr2");
            blueBanner = Content.Load<Texture2D>("BlueBanner");
            greenBanner = Content.Load<Texture2D>("GreenBanner");
            

            greenCursor = Content.Load<Texture2D>("GreenCursor");

            //Load the sounds
            mtInstructionScreen1 = Content.Load<SoundEffect>("MouseToneInstructionScreen_8Bit(hi)");
            mtInstructionScreen2 = Content.Load<SoundEffect>("MouseToneInstructionScreen_8Bit(low)");
            mtInstructionScreen3 = Content.Load<SoundEffect>("MouseToneInstructionScreen_8Bit(mid)");
            mtMainScreen1 = Content.Load<SoundEffect>("MouseToneMainScreen_8Bit(hi1)");
            mtMainScreen2 = Content.Load<SoundEffect>("MouseToneMainScreen_8Bit(mid)");
            switchTurn = Content.Load<SoundEffect>("PlayerTurnSwitchTone");
            playerOneToPlayerTwo = Content.Load<SoundEffect>("BconvertsG");
            playerTwoToPlayerOne = Content.Load<SoundEffect>("GconvertsB");
            playerOneUnknown1 = Content.Load<SoundEffect>("BlueRebuttle1");
            playerOneUnknown2 = Content.Load<SoundEffect>("BlueRebuttle2");
            playerTwoUnknown1 = Content.Load<SoundEffect>("Grebuttle1");
            playerTwoUnknown2 = Content.Load<SoundEffect>("GRebuttle2");
            mouseClick = Content.Load<SoundEffect>("MouseClick");
            selectPiece = Content.Load<SoundEffect>("PieceSelectTone_8Bit");
            unavailableMove = Content.Load<SoundEffect>("UnavailableMove_8Bit");
            availableMove = Content.Load<SoundEffect>("AvailableMove_8Bit");
            passTurn = Content.Load<SoundEffect>("ForceTurn_8Bit");

            selectPiece.Play(1.0f, 0.0f, 0.0f, false);
            
            board.LoadContent(spriteBatch, boardTex, redTex, greenTex, greenSelectTex, redSelectTex, redToGreen, greenToRed, transToPlr1, transToPlr2, redCursor, greenCursor, blueBanner, greenBanner);
            board.LoadAudio(selectPiece, unavailableMove, availableMove, playerOneToPlayerTwo);
            menu.LoadAudio(mtInstructionScreen1);
            instructions.LoadAudio(mtInstructionScreen2);
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
                        case (int)State.Instructions:
                            instructions.Click(mouseState, ref gameState);
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
                case (int)State.Instructions:
                    instructions.Update(gameTime, ref gameState);
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
                case (int)State.Instructions:
                    instructions.Draw();
                    break;
                case (int)State.Game:
                    board.Draw();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}

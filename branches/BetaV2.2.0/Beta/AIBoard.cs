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
    class AIBoard
    {
        // Enumeration for player turns
        private enum PlayerTurn
        {
            Red = 1,
            Green = 2
        }

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

        // Enumeration for the AI states
        enum AIState
        {
            Calculations = 1,
            Select = 2,
            Move = 3
        }

        // Image of the board
        Image tempBoard = new Image();

        // Cursors
        Cursor redCursor = new Cursor();
        Cursor greenCursor = new Cursor();

        // Images for the selected piece
        Image redSelect = new Image();
        Image greenSelect = new Image();

        // Images for the banners
        Image blueBanner = new Image();
        Image greenBanner = new Image();

        // Array of pieces representing the board
        Piece[,] pieces = new Piece[7, 7];

        // Queue to handle the animations one at a time
        Queue<Vector2> animate = new Queue<Vector2>();
        // Time for animation delay
        int time;

        // Selected Piece
        Vector2 selectedPiece;

        // Player turn
        int playerTurn;

        // AI State
        int aiState;
        // Square for the AI to move into
        Vector2 aiMoveTo;
        // Piece to select
        Vector2 aiPieceToSelect;
        // Random
        Random aiRandom = new Random();

        // Board offset
        Vector2 offset;

        // Board full
        public static bool boardFull;

        // Piece count
        //int redCount;
        //int greenCount;

        // final scores for each player
        int plr1score;
        int plr2score;

        //limit of board dimensions
        const int X = 7;
        const int Y = 7;

        // To draw the banners or not
        // Will be combined with animate.Count == 0
        bool player1Win;
        bool player2Win;
        bool drawEndBanner;

        //Sounds
        SoundEffect selectPiece;
        SoundEffect unMove;
        SoundEffect aMove;
        SoundEffect convert;
        SoundEffectInstance seiConvert;
        SoundEffectInstance seiAvaMove;

        public AIBoard()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y] = new Piece();
                }
            }
        }

        public void Initialize()
        {
            // Set the offset
            offset = new Vector2(49 / 2, 49 / 2);
            // Clear the animation queue
            animate.Clear();
            // Initialize the cursors
            redCursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0.0f);
            greenCursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0.0f);

            // Initialize the selected peices
            redSelect.Initialize(Vector2.Zero, new Rectangle(0, 0, 100, 100), Color.White, Vector2.Zero, new Vector2(0.5f), 0f);
            greenSelect.Initialize(Vector2.Zero, new Rectangle(0, 0, 100, 100), Color.White, Vector2.Zero, new Vector2(0.5f), 0f);

            // Initialize the banners
            blueBanner.Initialize(new Vector2(0, 297 / 2), new Rectangle(0, 0, 798, 204), new Color(255, 255, 255, 0), Vector2.Zero, new Vector2(0.5f), 0f);
            greenBanner.Initialize(new Vector2(0, 297 / 2), new Rectangle(0, 0, 798, 204), new Color(255, 255, 255, 0), Vector2.Zero, new Vector2(0.5f), 0f);

            // Initialize the board image
            tempBoard.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 1f);

            // Initialize Pieces to be all blank
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].Initialize(new Vector2(((100 * x) / 2 + offset.X), ((100 * y) / 2 + offset.Y)), new Rectangle(0, 0, 100, 100), Vector2.Zero, new Vector2(0.5f), 0.5f);
                    pieces[x, y].SetState(0);
                }
            }

            // Set the initial corner pieces
            pieces[0, 0].SetState(1);
            pieces[6, 6].SetState(1);
            pieces[0, 6].SetState(2);
            pieces[6, 0].SetState(2);

            //Peices for quick end of game
            //pieces[0, 0].SetState(1);
            //pieces[1, 0].SetState(1);
            //pieces[2, 0].SetState(1);
            //pieces[3, 0].SetState(1);
            //pieces[4, 0].SetState(1);
            //pieces[5, 0].SetState(1);
            //pieces[6, 0].SetState(1);
            //pieces[0, 1].SetState(1);
            //pieces[1, 1].SetState(2);
            //pieces[2, 1].SetState(1);
            //pieces[3, 1].SetState(1);
            //pieces[4, 1].SetState(1);
            //pieces[5, 1].SetState(2);
            //pieces[6, 1].SetState(1);
            //pieces[0, 2].SetState(1);
            ////pieces[1, 2].SetState(2);
            ////pieces[2, 2].SetState(1);
            ////pieces[3, 2].SetState(1);
            //pieces[4, 2].SetState(1);
            //pieces[5, 2].SetState(2);
            //pieces[6, 2].SetState(1);
            //pieces[0, 3].SetState(1);
            //pieces[1, 3].SetState(1);
            //pieces[2, 3].SetState(2);
            //pieces[3, 3].SetState(2);
            //pieces[4, 3].SetState(2);
            //pieces[5, 3].SetState(2);
            //pieces[6, 3].SetState(1);
            //pieces[0, 4].SetState(2);
            //pieces[1, 4].SetState(2);
            //pieces[2, 4].SetState(2);
            //pieces[3, 4].SetState(2);
            //pieces[4, 4].SetState(1);
            //pieces[5, 4].SetState(1);
            //pieces[6, 4].SetState(2);
            //pieces[0, 5].SetState(1);
            //pieces[1, 5].SetState(1);
            //pieces[2, 5].SetState(2);
            //pieces[3, 5].SetState(2);
            //pieces[4, 5].SetState(2);
            //pieces[5, 5].SetState(1);
            //pieces[6, 5].SetState(1);
            //pieces[0, 6].SetState(2);
            //pieces[1, 6].SetState(1);
            //pieces[2, 6].SetState(1);
            //pieces[3, 6].SetState(1);
            //pieces[4, 6].SetState(1);
            //pieces[5, 6].SetState(1);
            //pieces[6, 6].SetState(1);

            // Fill board to test complete green elimination
            //pieces[0, 0].SetState(1);
            //pieces[1, 0].SetState(1);
            //pieces[0, 1].SetState(1);
            //pieces[2, 2].SetState(2);

            // Set playerTurn
            playerTurn = (int)PlayerTurn.Red;
            // Set aiState
            aiState = (int)AIState.Calculations;
            aiMoveTo = new Vector2();
            aiPieceToSelect = new Vector2();
            // Set selected piece
            selectedPiece = new Vector2(-1, -1);
            // Set time
            time = 0;
            // Board full
            boardFull = false;
            // Set counts
            //redCount = 0;
            //greenCount = 0;

            player1Win = false;
            player2Win = false;
            drawEndBanner = false;          
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D board, Texture2D red, Texture2D green, Texture2D redSelection, Texture2D greenSelection, Texture2D redGreen, Texture2D greenRed, Texture2D tPlr1, Texture2D tPlr2, Texture2D redCur, Texture2D greenCur, Texture2D blueBanner, Texture2D greenBanner)
        {
            // Load the content for the board image
            this.tempBoard.LoadContent(spriteBatch, ref board);

            // Load the content for the selected piece images
            this.redSelect.LoadContent(spriteBatch, ref redSelection);
            this.greenSelect.LoadContent(spriteBatch, ref greenSelection);

            // Load the banners
            this.blueBanner.LoadContent(spriteBatch, ref blueBanner);
            this.greenBanner.LoadContent(spriteBatch, ref greenBanner);

            // Load the content for the pieces
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].LoadContent(spriteBatch, red, green, redGreen, greenRed,tPlr1,tPlr2);
                }
            }

            redCursor.LoadContent(spriteBatch, ref redCur);
            greenCursor.LoadContent(spriteBatch, ref greenCur);
        }

        public void LoadAudio(SoundEffect selectPiece, SoundEffect unMove, SoundEffect aMove, SoundEffect convert)
        {
            this.selectPiece = selectPiece;
            this.unMove = unMove;
            this.aMove = aMove;
            this.convert = convert;

            seiConvert = convert.Play(1.0f, 0.0f, 0.0f, false);
            seiConvert.Stop();
            seiAvaMove = aMove.Play(1.0f, 0.0f, 0.0f, false);
            seiAvaMove.Stop();
        }

        public void Update(GameTime gameTime, ref int state)
        {
          
            // Get the keyboard state
            KeyboardState keyState = Keyboard.GetState();

            // If esc was pressed, set the state to the main menu
            // Then reset this board
            // Then return so nothing gets processed
            if (keyState.IsKeyDown(Keys.Escape) == true)
            {
                state = (int)State.Menu;
                this.Initialize();
                return;
            }

            // If 'r' was pressed
            // Reset the board
            if (keyState.IsKeyDown(Keys.R) == true)
            {
                this.Initialize();
            }

            // Update both cursors
            redCursor.Update();
            greenCursor.Update();

            #region Handle animations one at a time

            if (time < 1050)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (time >= 1015)
            {
                if (animate.Count > 0)
                {
                    seiConvert.Stop();
                    pieces[(int)animate.Peek().X, (int)animate.Peek().Y].Animate();
                    animate.Dequeue();
                    time = 0;
                    seiConvert.Play();
                }
            }
            #endregion

            #region Update all the pieces on the board
            // Update all pieces on the board
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].Update(gameTime);
                }
            }
            #endregion

            #region Do the AI Logic
            if(playerTurn == (int)PlayerTurn.Green && !drawEndBanner && !boardFull)
            {
                if(time > 1000)
                {
                    switch (aiState)
                    {
                        case (int)AIState.Calculations:
                            if (animate.Count == 0)
                            {
                                FindBestMove(GenerateBoard(), ref aiPieceToSelect, ref aiMoveTo);
                                aiState = (int)AIState.Select;
                                time = 0;
                            }
                            break;
                        case (int)AIState.Select:
                            selectedPiece = aiPieceToSelect;
                            selectPiece.Play(1.0f, 0.0f, 0.0f, false);
                            aiState = (int)AIState.Move;
                            time = 0;
                            break;
                        case (int)AIState.Move:
                                MovePiece(selectedPiece, (int)aiMoveTo.X, (int)aiMoveTo.Y, (int)PlayerTurn.Green);
                                selectedPiece = new Vector2(-1, -1);
                                if (AnyMoves((int)PlayerTurn.Red))
                                {
                                    seiAvaMove.Stop();
                                    playerTurn = (int)PlayerTurn.Red;
                                    seiAvaMove.Play();
                                }
                                else if (!AnyPieces((int)PlayerTurn.Red))
                                {
                                    FillBoard((int)PlayerTurn.Green);
                                    player2Win = true;
                                }
                                else
                                {
                                    if (BoardFull())
                                        boardFull = true;
                                }
                                aiState = (int)AIState.Calculations;
                            break;
                    }
                }
            }
            #endregion


            if (boardFull == true && animate.Count == 0 && time > 1000)
            {
                ClearBoard();
                PlopEndPieces();
            }

            if (drawEndBanner == true && animate.Count == 0 && time > 1050)
            {
                if (player1Win)
                {
                    if (blueBanner.color.A <= 253)
                        blueBanner.color.A += 2;
                }
                if (player2Win)
                {
                    if (greenBanner.color.A <= 253)
                        greenBanner.color.A += 2;
                }
            }
        
        }

        public void Click(MouseState mouseState)
        {
            // Find the grid coordinates of the mouse click
            int mouseX = (int)Math.Floor((double)(mouseState.X - offset.X) / 50);
            int mouseY = (int)Math.Floor((double)(mouseState.Y - offset.Y) / 50);

            if (animate.Count == 0 && time >= 500 && !drawEndBanner)
            {
                switch (playerTurn)
                {
                    #region Red turn logic if mouse is clicked
                    case 1: // Red turn
                        #region No piece selected
                        if (selectedPiece.X < 0 || selectedPiece.Y < 0) // If selectedPiece is negative a.k.a. no piece selected
                        {
                            // Only perform click method if the click was on the board
                            if (mouseX >= 0 && mouseX < 7 && mouseY >= 0 && mouseY < 7)
                            {
                                // Perform logic depending on what piece is in the space
                                switch (pieces[mouseX, mouseY].Value())
                                {
                                    case 0:     // If blank space clicked
                                        // Do nothing for now
                                        // Will play error noise in the future
                                        break;
                                    case 1:     // If clicked on red piece
                                        // Select the red piece
                                        selectedPiece = new Vector2(mouseX, mouseY);
                                        selectPiece.Play(1.0f, 0.0f, 0.0f, false);
                                        break;
                                    case 2:     // If clicked on green piece
                                        // Do nothing for now
                                        // Will play error noise in the future
                                        break;
                                    default:    // Error
                                        // Error
                                        break;
                                }
                            }
                        }
                        #endregion
                        #region Piece on board selected
                        else if (selectedPiece.X >= 0 && selectedPiece.X < 7 && selectedPiece.Y >= 0 && selectedPiece.Y < 7) // If piece on the board is selected
                        {
                            // Only perform click method if the click was on the board
                            if (mouseX >= 0 && mouseX < 7 && mouseY >= 0 && mouseY < 7)
                            {
                                // Perform logic depending on what piece is in the space
                                switch (pieces[mouseX, mouseY].Value())
                                {
                                    case 0:     // If blank space clicked
                                        // If PerformMove is true
                                        if (MovePiece(selectedPiece, mouseX, mouseY, playerTurn))
                                        {
                                            // Prepare for next player
                                            if (AnyMoves(2))
                                            {
                                                seiAvaMove.Stop();
                                                playerTurn = 2;
                                                seiAvaMove.Play();
                                            }
                                            else if (!AnyPieces(2))
                                            {
                                                FillBoard(1);
                                                player1Win = true;
                                            }
                                            else
                                                if (BoardFull())
                                                    boardFull = true;

                                            selectedPiece = new Vector2(-1, -1);
                                        }
                                        else
                                        {
                                            // Deselect piece
                                            selectedPiece = new Vector2(-1, -1);
                                            unMove.Play(1.0f, 0.0f, 0.0f, false);
                                        }
                                        break;
                                    case 1:     // If clicked on red piece
                                        // Select piece
                                        selectedPiece = new Vector2(mouseX, mouseY);
                                        selectPiece.Play(1.0f, 0.0f, 0.0f, false);
                                        break;
                                    case 2:     // If clicked on green piece
                                        // Deselect piece
                                        selectedPiece = new Vector2(-1, -1);
                                        unMove.Play(1.0f, 0.0f, 0.0f, false);
                                        break;
                                    default:    // Error
                                        // Error
                                        break;
                                }
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region Green turn logic if mouse is clicked
                    //case 2: // Green turn
                    //    #region No piece selected
                    //    if (selectedPiece.X < 0 || selectedPiece.Y < 0) // If selectedPiece is negative a.k.a. no piece selected
                    //    {
                    //        // Only perform click method if the click was on the board
                    //        if (mouseX >= 0 && mouseX < 7 && mouseY >= 0 && mouseY < 7)
                    //        {
                    //            // Perform logic depending on what piece is in the space
                    //            switch (pieces[mouseX, mouseY].Value())
                    //            {
                    //                case 0:     // If blank space clicked
                    //                    // Do nothing for now
                    //                    // Will play error noise in the future
                    //                    break;
                    //                case 1:     // If clicked on red piece
                    //                    // Do nothing for now
                    //                    // Will play error noise in the future
                    //                    break;
                    //                case 2:     // If clicked on green piece
                    //                    // Select the green piece
                    //                    selectedPiece = new Vector2(mouseX, mouseY);
                    //                    selectPiece.Play(1.0f, 0.0f, 0.0f, false);
                    //                    break;
                    //                default:    // Error
                    //                    // Error
                    //                    break;
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //    #region Piece on board selected
                    //    else if (selectedPiece.X >= 0 && selectedPiece.X < 7 && selectedPiece.Y >= 0 && selectedPiece.Y < 7) // If piece on the board is selected
                    //    {
                    //        // Only perform click method if the click was on the board
                    //        if (mouseX >= 0 && mouseX < 7 && mouseY >= 0 && mouseY < 7)
                    //        {
                    //            // Perform logic depending on what piece is in the space
                    //            switch (pieces[mouseX, mouseY].Value())
                    //            {
                    //                case 0:     // If blank space clicked
                    //                    // If PerformMove is false (invalid move)
                    //                    if (MovePiece(selectedPiece, mouseX, mouseY, playerTurn))
                    //                    {
                    //                        // Prepare for next player
                    //                        if (AnyMoves(1))
                    //                        {
                    //                            seiAvaMove.Stop();
                    //                            playerTurn = 1;
                    //                            seiAvaMove.Play();
                    //                        }
                    //                        else if (!AnyPieces(1))
                    //                        {
                    //                            FillBoard(2);
                    //                            drawGreenBanner = true;
                    //                        }
                    //                        else
                    //                            if (BoardFull())
                    //                                boardFull = true;

                    //                        selectedPiece = new Vector2(-1, -1);
                    //                    }
                    //                    else
                    //                    {
                    //                        // Deselect piece
                    //                        selectedPiece = new Vector2(-1, -1);
                    //                        unMove.Play(1.0f, 0.0f, 0.0f, false);
                    //                    }
                    //                    break;
                    //                case 1:     // If clicked on red piece
                    //                    // Deselect piece
                    //                    selectedPiece = new Vector2(-1, -1);
                    //                    unMove.Play(1.0f, 0.0f, 0.0f, false);
                    //                    break;
                    //                case 2:     // If clicked on green piece
                    //                    // Deselect piece
                    //                    selectedPiece = new Vector2(mouseX, mouseY);
                    //                    selectPiece.Play(1.0f, 0.0f, 0.0f, false);
                    //                    break;
                    //                default:    // Error
                    //                    // Error
                    //                    break;
                    //            }
                    //        }
                    //    }
                    //    break;
                    //    #endregion
                    #endregion
                    #region If playerTurn is not set to a valid player
                    default: // Error
                        // If playerTurn is not 0 or 1
                        // this would be due to a logical
                        // error somewhere else in code
                        break;
                    #endregion
                }
            }
        }

        public void Draw()
        {
            // Draw the board
            tempBoard.Draw();

            // Loop through and draw all of the pieces
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].Draw();
                }
            }

            // Draw the selected peices if needed
            if (selectedPiece.X != -1 || selectedPiece.Y != -1)
            {
                switch (playerTurn)
                {
                    case 1:
                        redSelect.SetPosition(new Vector2(((100 * selectedPiece.X) / 2 + offset.X), ((100 * selectedPiece.Y) / 2 + offset.Y)));
                        redSelect.Draw();
                        break;

                    case 2:
                        greenSelect.SetPosition(new Vector2(((100 * selectedPiece.X) / 2 + offset.X), ((100 * selectedPiece.Y) / 2 + offset.Y)));
                        greenSelect.Draw();
                        break;
                }
            }

            // Draw the banners
            if (player1Win == true && animate.Count == 0 && time >= 1000)
            {
                blueBanner.Draw();
            }
            if (player2Win == true && animate.Count == 0 && time >= 1000)
            {
                greenBanner.Draw();
            }

            if (!boardFull)
            {
                // Draw the correct cursor
                if (playerTurn == (int)PlayerTurn.Red)
                {
                    redCursor.Draw();
                }
                else if (playerTurn == (int)PlayerTurn.Green)
                {
                    greenCursor.Draw();
                }
            }
        }

        private bool MovePiece(Vector2 selected, int targetX, int targetY, int player)
        {
            //if a valid move is found this will be set to true
            bool validMove = false;
            #region Handle Movement
            for (int moveNum = 1; moveNum < 3; moveNum++)
            {
                //If target is moveNum square NorthWest
                if ((targetX + moveNum) == selected.X && (targetY + moveNum) == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square North
                if (targetX == selected.X && (targetY + moveNum) == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square NorthEast
                if ((targetX - moveNum) == selected.X && (targetY + moveNum) == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square East
                if ((targetX - moveNum) == selected.X && targetY == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square SouthEast
                if ((targetX - moveNum) == selected.X && (targetY - moveNum) == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square South
                if (targetX == selected.X && (targetY - moveNum) == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square SouthWest
                if ((targetX + moveNum) == selected.X && (targetY - moveNum) == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
                //If target is moveNum square West
                if ((targetX + moveNum) == selected.X && targetY == selected.Y)
                {
                    pieces[targetX, targetY].SetState(player);
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.X), (int)Math.Floor(selected.Y)].SetState(0);
                    validMove = true;
                }
            }
            #endregion

            // Call Mutate
            if (validMove)
            {
                Mutate(targetX, targetY, player);
            }

            return validMove;
        }

        private void Mutate(int targetX, int targetY, int player)
        {

            if (targetY > 0 && targetX > 0)//1
            {
                // animation bug here, was checking if pieces[targetX - 1, targetX - 1].Value() != player
                if (pieces[targetX - 1, targetY - 1].Value() > 0 && pieces[targetX - 1, targetY - 1].Value() != player)
                {
                    pieces[targetX - 1, targetY - 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX - 1, targetY - 1));
                }
            }
            if (targetY > 0)//2
            {
                if (pieces[targetX, targetY - 1].Value() > 0 && pieces[targetX, targetY - 1].Value() != player)
                {
                    pieces[targetX, targetY - 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX, targetY - 1));
                }
            }
            if (targetY > 0 && targetX < 6)//3
            {
                if (pieces[targetX + 1, targetY - 1].Value() > 0 && pieces[targetX + 1, targetY - 1].Value() != player)
                {
                    pieces[targetX + 1, targetY - 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX + 1, targetY - 1));
                }
            }
            if (targetX < 6)//5
            {
                if (pieces[targetX + 1, targetY].Value() > 0 && pieces[targetX + 1, targetY].Value() != player)
                {
                    pieces[targetX + 1, targetY].Mutate(player);
                    animate.Enqueue(new Vector2(targetX + 1, targetY));
                }
            }
            if (targetY < 6 && targetX < 6)//8
            {
                if (pieces[targetX + 1, targetY + 1].Value() > 0 && pieces[targetX + 1, targetY + 1].Value() != player)
                {
                    pieces[targetX + 1, targetY + 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX + 1, targetY + 1));
                }
            }
            if (targetY < 6)//7
            {
                if (pieces[targetX, targetY + 1].Value() > 0 && pieces[targetX, targetY + 1].Value() != player)
                {
                    pieces[targetX, targetY + 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX, targetY + 1));
                }
            }
            if (targetY < 6 && targetX > 0)//6
            {
                if (pieces[targetX - 1, targetY + 1].Value() > 0 && pieces[targetX - 1, targetY + 1].Value() != player)
                {
                    pieces[targetX - 1, targetY + 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX - 1, targetY + 1));
                }

            }
            if (targetX > 0)//4
            {
                if (pieces[targetX - 1, targetY].Value() > 0 && pieces[targetX - 1, targetY].Value() != player)
                {
                    pieces[targetX - 1, targetY].Mutate(player);
                    animate.Enqueue(new Vector2(targetX - 1, targetY));
                }
            }
        }

        public bool AnyMoves(int player)
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (pieces[x, y].Value() == player)
                    {
                        for (int moveNum = 1; moveNum < 3; moveNum++)
                        {
                            // Northwest
                            if (y > (0 + (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (pieces[(x - moveNum), (y - moveNum)].Value() == 0)
                                    return true;
                            }
                            // North
                            if (y > (0 + (moveNum - 1)))
                            {
                                if (pieces[(x), (y - moveNum)].Value() == 0)
                                    return true;
                            }
                            // Northeast
                            // crashing bug was here, was checking if y < (7 - (moveNum - 1)) and x > (0 + (moveNum - 1))
                            if (y > (0 + (moveNum - 1)) && x < (6 - (moveNum - 1)))
                            {
                                if (pieces[(x + moveNum), (y - moveNum)].Value() == 0)
                                    return true;
                            }
                            // East
                            if (x < (6 - (moveNum - 1)))
                            {
                                if (pieces[(x + moveNum), (y)].Value() == 0)
                                    return true;
                            }
                            // Southeast
                            if (y < (6 - (moveNum - 1)) && x < (6 - (moveNum - 1)))
                            {
                                if (pieces[(x + moveNum), (y + moveNum)].Value() == 0)
                                    return true;
                            }
                            // South
                            if (y < (6 - (moveNum - 1)))
                            {
                                if (pieces[(x), (y + moveNum)].Value() == 0)
                                    return true;
                            }
                            // Southwest
                            if (y < (6 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (pieces[(x - moveNum), (y + moveNum)].Value() == 0)
                                    return true;
                            }
                            // West
                            if (x > (0 + (moveNum - 1)))
                            {
                                if (pieces[(x - moveNum), (y)].Value() == 0)
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool AnyPieces(int player)
        {
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (pieces[x, y].Value() == player)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void FillBoard(int player)
        {
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (pieces[x, y].Value() == 0)
                    {
                        pieces[x, y].SetState(player + 4);
                        animate.Enqueue(new Vector2(x, y));
                    }
                }
            }
            drawEndBanner = true;
        }

        public bool BoardFull()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (pieces[x, y].Value() == 0)
                        return false;
                }
            }
            return true;
        }

        public void ClearBoard()
        {
            //save the score before board is erased
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    switch (pieces[x, y].Value())
                    {
                        case 1:
                            plr1score++;
                            break;
                        case 2:
                            plr2score++;
                            break;
                    }
                }

            }

            //clear pieces
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].SetState(0);
                }
            }
            return;
        }

        public void PlopEndPieces()
        {
            if (plr1score > plr2score)
                player1Win = true;
            else
                player2Win = true;

            blueBanner.color.A = 100;
            greenBanner.color.A = 100;


            for (int y = 0; y < 7; y++)
            {
                for(int x = 0; x < 7; x++)
                {
                    if (plr1score > 0)
                    {
                        pieces[x,y].SetState(5);
                        animate.Enqueue(new Vector2(x,y));
                        plr1score--;
                    }
                    if (plr2score > 0)
                    {
                        pieces[6-x,6-y].SetState(6);
                        animate.Enqueue(new Vector2(6-x,6-y));
                        plr2score--;
                    }
                }
            }
            boardFull = false;
            drawEndBanner = true;


            return;
        }

        
 /*******************************************************************
 * A.I.
 * ****************************************************************/ 
        private void FindBestMove(byte[,] board, ref Vector2 moveFrom, ref Vector2 moveTo)
        {
            byte gainOne = 0;
            byte gainTwo = 0;

            byte[,] tempBoard = new byte[7, 7];

            List<Move> moves = new List<Move>();
            List<byte> rebuttle = new List<byte>();

            #region Find all available moves on the board and put them in a list
            for (byte x = 0; x < 7; x++)
            {
                for (byte y = 0; y < 7; y++)
                {
                    // If its player 2
                    if (board[x, y] == 2)
                    {
                        for (int moveNum = 1; moveNum < 3; moveNum++)
                        {
                            Move tempMove;
                            // Northwest
                            if (y > (0 + (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (board[x - moveNum, y - moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x - moveNum, y - moveNum);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // North
                            if (y > (0 + (moveNum - 1)))
                            {
                                if (board[x, y - moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x, y - moveNum);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // Northeast
                            if (y > (0 + (moveNum - 1)) && x < (6 - (moveNum - 1)))
                            {
                                if (board[x + moveNum, y - moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x + moveNum, y - moveNum);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // East
                            if (x < (6 - (moveNum - 1)))
                            {
                                if (board[x + moveNum, y] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x + moveNum, y);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // Southeast
                            if (y < (6 - (moveNum - 1)) && x < (6 - (moveNum - 1)))
                            {
                                if (board[x + moveNum, y + moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x + moveNum, y + moveNum);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // South
                            if (y < (6 - (moveNum - 1)))
                            {
                                if (board[x, y + moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x, y + moveNum);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // Southwest
                            if (y < (6 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (board[x - moveNum, y + moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x - moveNum, y + moveNum);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                            // West
                            if (x > (0 + (moveNum - 1)))
                            {
                                if (board[x - moveNum, y] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x - moveNum, y);
                                    tempMove.Gain = FindGain(board, tempMove.From, tempMove.To);
                                    if (moveNum == 1 || (moveNum == 2 && tempMove.Gain > 4))
                                        moves.Add(tempMove);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Find the top 2 gains in the list
            foreach (Move move in moves)
            {
                if (move.Gain > gainOne)
                {
                    gainTwo = gainOne;
                    gainOne = move.Gain;
                }
            }
            if (gainTwo == 0)
                gainTwo = gainOne;
            #endregion

            #region Remove Everything that is not the top two gains
            List<int> toRemove = new List<int>();
            int count = 0;
            foreach (Move move in moves)
            {
                if (move.Gain < gainTwo)
                    toRemove.Add(count);
                count++;
            }
            count = 0;
            foreach (int move in toRemove)
            {
                moves.RemoveAt(move - count);
                count++;
            }
            toRemove.Clear();
            #endregion

            #region Take the players possible moves into account
            foreach (Move move in moves)
            {
                rebuttle.Add(FindRebuttalValue(tempBoard));
            }
            #endregion

            #region Find the new top gain
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Gain - rebuttle[i] > gainOne)
                    gainOne = (byte)(moves[i].Gain - rebuttle[i]);
            }
            #endregion

            #region Remove all but the top gains
            count = 0;
            foreach (Move move in moves)
            {
                if (move.Gain < gainOne)
                    toRemove.Add(count);
                count++;
            }
            count = 0;
            foreach (int move in toRemove)
            {
                moves.RemoveAt(move - count);
                count++;
            }
            #endregion

            #region Return the move
            int randMove = aiRandom.Next(0, moves.Count);
            moveFrom = moves[randMove].From;
            moveTo = moves[randMove].To;
            moves.Clear();
            rebuttle.Clear();
            return;
            #endregion
        }

        private byte FindGain(byte[,] tempBoard, Vector2 fromPos, Vector2 toPos)
        {
            byte player = tempBoard[(int)fromPos.X, (int)fromPos.Y];
            bool jump = true;
            byte count = 1;

            if (toPos.Y > 0 && toPos.X > 0)//1
            {
                if (tempBoard[(int)toPos.X - 1, (int)toPos.Y - 1] != 0 && tempBoard[(int)toPos.X - 1, (int)toPos.Y - 1] != player)
                {
                    count++;
                    if (toPos.X - 1 == fromPos.X && toPos.Y - 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y > 0)//2
            {
                if (tempBoard[(int)toPos.X, (int)toPos.Y - 1] != 0 && tempBoard[(int)toPos.X, (int)toPos.Y - 1] != player)
                {
                    count++;
                    if (toPos.X == fromPos.X && toPos.Y - 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y > 0 && toPos.X < 6)//3
            {
                if (tempBoard[(int)toPos.X + 1, (int)toPos.Y - 1] != 0 && tempBoard[(int)toPos.X + 1, (int)toPos.Y - 1] != player)
                {
                    count++;
                    if (toPos.X + 1 == fromPos.X && toPos.Y - 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.X < 6)//5
            {
                if (tempBoard[(int)toPos.X + 1, (int)toPos.Y] != 0 && tempBoard[(int)toPos.X + 1, (int)toPos.Y] != player)
                {
                    count++;
                    if (toPos.X + 1 == fromPos.X && toPos.Y == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y < 6 && toPos.X < 6)//8
            {
                if (tempBoard[(int)toPos.X + 1, (int)toPos.Y + 1] != 0 && tempBoard[(int)toPos.X + 1, (int)toPos.Y + 1] != player)
                {
                    count++;
                    if (toPos.X + 1 == fromPos.X && toPos.Y + 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y < 6)//7
            {
                if (tempBoard[(int)toPos.X, (int)toPos.Y + 1] != 0 && tempBoard[(int)toPos.X, (int)toPos.Y + 1] != player)
                {
                    count++;
                    if (toPos.X == fromPos.X && toPos.Y + 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y < 6 && toPos.X > 0)//6
            {
                if (tempBoard[(int)toPos.X - 1, (int)toPos.Y + 1] != 0 && tempBoard[(int)toPos.X - 1, (int)toPos.Y + 1] != player)
                {
                    count++;
                    if (toPos.X - 1 == fromPos.X && toPos.Y + 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.X > 0)//4
            {
                if (tempBoard[(int)toPos.X - 1, (int)toPos.Y] != 0 && tempBoard[(int)toPos.X - 1, (int)toPos.Y] != player)
                {
                    count++;
                    if (toPos.X - 1 == fromPos.X && toPos.Y == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (jump == true)
            {
                count--;
            }

            return count;
        }

        private byte[,] TestMove(byte[,] tempBoard, Vector2 fromPos, Vector2 toPos)
        {
            //if (pieces[(int)toPos.X, (int)toPos.Y].Value() != 0)
            //{
            //    return NULL;
            //}

            //if (pieces[(int)fromPos.X, (int)fromPos.Y].Value() == 0)
            //{
            //    return NULL;
            //}

            byte player = tempBoard[(int)fromPos.X, (int)fromPos.Y];
            bool jump = true;

            if (toPos.Y > 0 && toPos.X > 0)//1
            {
                if (tempBoard[(int)toPos.X - 1, (int)toPos.Y - 1] != 0 && tempBoard[(int)toPos.X - 1, (int)toPos.Y - 1] != player)
                {
                    tempBoard[(int)toPos.X - 1, (int)toPos.Y - 1] = player;
                    if (toPos.X - 1 == fromPos.X && toPos.Y - 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y > 0)//2
            {
                if (tempBoard[(int)toPos.X, (int)toPos.Y - 1] != 0 && tempBoard[(int)toPos.X, (int)toPos.Y - 1] != player)
                {
                    tempBoard[(int)toPos.X, (int)toPos.Y - 1] = player;
                    if (toPos.X == fromPos.X && toPos.Y - 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y > 0 && toPos.X < 6)//3
            {
                if (tempBoard[(int)toPos.X + 1, (int)toPos.Y - 1] != 0 && tempBoard[(int)toPos.X + 1, (int)toPos.Y - 1] != player)
                {
                    tempBoard[(int)toPos.X + 1, (int)toPos.Y - 1] = player;
                    if (toPos.X + 1 == fromPos.X && toPos.Y - 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.X < 6)//5
            {
                if (tempBoard[(int)toPos.X + 1, (int)toPos.Y] != 0 && tempBoard[(int)toPos.X + 1, (int)toPos.Y] != player)
                {
                    tempBoard[(int)toPos.X + 1, (int)toPos.Y] = player;
                    if (toPos.X + 1 == fromPos.X && toPos.Y == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y < 6 && toPos.X < 6)//8
            {
                if (tempBoard[(int)toPos.X + 1, (int)toPos.Y + 1] != 0 && tempBoard[(int)toPos.X + 1, (int)toPos.Y + 1] != player)
                {
                    tempBoard[(int)toPos.X + 1, (int)toPos.Y + 1] = player;
                    if (toPos.X + 1 == fromPos.X && toPos.Y + 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y < 6)//7
            {
                if (tempBoard[(int)toPos.X, (int)toPos.Y + 1] != 0 && tempBoard[(int)toPos.X, (int)toPos.Y + 1] != player)
                {
                    tempBoard[(int)toPos.X, (int)toPos.Y + 1] = player;
                    if (toPos.X == fromPos.X && toPos.Y + 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.Y < 6 && toPos.X > 0)//6
            {
                if (tempBoard[(int)toPos.X - 1, (int)toPos.Y + 1] != 0 && tempBoard[(int)toPos.X - 1, (int)toPos.Y + 1] != player)
                {
                    tempBoard[(int)toPos.X - 1, (int)toPos.Y + 1] = player;
                    if (toPos.X - 1 == fromPos.X && toPos.Y + 1 == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (toPos.X > 0)//4
            {
                if (tempBoard[(int)toPos.X - 1, (int)toPos.Y] != 0 && tempBoard[(int)toPos.X - 1, (int)toPos.Y] != player)
                {
                    tempBoard[(int)toPos.X - 1, (int)toPos.Y] = player;
                    if (toPos.X - 1 == fromPos.X && toPos.Y == fromPos.Y)
                    {
                        jump = false;
                    }
                }
            }

            if (jump == true)
            {
                tempBoard[(int)fromPos.X, (int)fromPos.Y] = 0;
            }

            return tempBoard;
        }

        private byte FindRebuttalValue(byte[,] tempBoard)
        {
            List<Move> moves = new List<Move>();
            byte gainOne = 0;
            byte gainTwo = 0;
            int gainAverage = 0;

            #region Find all available moves on the board and put them in a list
            for (byte x = 0; x < 7; x++)
            {
                for (byte y = 0; y < 7; y++)
                {
                    // If its player 2
                    if (tempBoard[x, y] == 1)
                    {
                        for (int moveNum = 1; moveNum < 3; moveNum++)
                        {
                            Move tempMove;
                            // Northwest
                            if (y > (0 + (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (tempBoard[x - moveNum, y - moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x - moveNum, y - moveNum);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // North
                            if (y > (0 + (moveNum - 1)))
                            {
                                if (tempBoard[x, y - moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x, y - moveNum);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // Northeast
                            if (y > (0 + (moveNum - 1)) && x < (6 - (moveNum - 1)))
                            {
                                if (tempBoard[x + moveNum, y - moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x + moveNum, y - moveNum);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // East
                            if (x < (6 - (moveNum - 1)))
                            {
                                if (tempBoard[x + moveNum, y] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x + moveNum, y);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // Southeast
                            if (y < (6 - (moveNum - 1)) && x < (6 - (moveNum - 1)))
                            {
                                if (tempBoard[x + moveNum, y + moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x + moveNum, y + moveNum);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // South
                            if (y < (6 - (moveNum - 1)))
                            {
                                if (tempBoard[x, y + moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x, y + moveNum);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // Southwest
                            if (y < (6 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (tempBoard[x - moveNum, y + moveNum] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x - moveNum, y + moveNum);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                            // West
                            if (x > (0 + (moveNum - 1)))
                            {
                                if (tempBoard[x - moveNum, y] == 0)
                                {
                                    tempMove.From = new Vector2(x, y);
                                    tempMove.To = new Vector2(x - moveNum, y);
                                    tempMove.Gain = FindGain(tempBoard, tempMove.From, tempMove.To);
                                    moves.Add(tempMove);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Find the top 2 gains in the list
            foreach (Move move in moves)
            {
                if (move.Gain > gainOne)
                {
                    gainTwo = gainOne;
                    gainOne = move.Gain;
                }
            }
            if (gainTwo == 0)
                gainTwo = gainOne;
            #endregion

            #region Remove Everything that is not the top two gains
            List<int> toRemove = new List<int>();
            int count = 0;
            foreach (Move move in moves)
            {
                if (move.Gain < gainTwo)
                    toRemove.Add(count);
                count++;
            }
            count = 0;
            foreach (int move in toRemove)
            {
                moves.RemoveAt(move - count);
                count++;
            }
            toRemove.Clear();
            #endregion

            foreach (Move move in moves)
            {
                gainAverage += move.Gain;
            }

            if (moves.Count > 0)
                gainAverage /= moves.Count;

            return (byte)gainAverage;
        }

        private byte[,] GenerateBoard()
        {
            byte[,] tempBoard = new byte[7, 7];

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (pieces[x, y].Value() == 0)
                    {
                        tempBoard[x, y] = 0;
                    }

                    if (pieces[x, y].Value() == 1 || pieces[x, y].Value() == 3)
                    {
                        tempBoard[x, y] = 1;
                    }

                    if (pieces[x, y].Value() == 2 || pieces[x, y].Value() == 4)
                    {
                        tempBoard[x, y] = 2;
                    }
                }
            }

            return tempBoard;
        }

        private struct Move
        {
            public byte Gain;
            public Vector2 From;
            public Vector2 To;
        }

        //private Vector2 PieceToSelect(Vector2 moveTo, bool jump)
        //{
        //    List<Vector2> pieceList = new List<Vector2>();
        //    float x = moveTo.X;
        //    float y = moveTo.Y;

        //    float moveNum;

        //    if (!jump)
        //    {
        //        moveNum = 1;
        //    }
        //    else
        //    {
        //        moveNum = 2;
        //    }

        //    // Northwest
        //    if (y > (0 + (moveNum - 1)) && x > (0 + (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x - moveNum), (int)(y - moveNum)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x - moveNum, y - moveNum));
        //        }
        //    }
        //    // North
        //    if (y > (0 + (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x), (int)(y - moveNum)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x, y - moveNum));
        //        }
        //    }
        //    // Northeast
        //    if (y > (0 + (moveNum - 1)) && x < (6 - (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x + moveNum), (int)(y - moveNum)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x + moveNum, y - moveNum));
        //        }
        //    }
        //    // East
        //    if (x < (6 - (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x + moveNum), (int)(y)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x + moveNum, y));
        //        }
        //    }
        //    // Southeast
        //    if (y < (6 - (moveNum - 1)) && x < (6 - (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x + moveNum), (int)(y + moveNum)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x + moveNum, y + moveNum));
        //        }
        //    }
        //    // South
        //    if (y < (6 - (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x), (int)(y + moveNum)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x, y + moveNum));
        //        }
        //    }
        //    // Southwest
        //    if (y < (6 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x - moveNum), (int)(y + moveNum)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x - moveNum, y + moveNum));
        //        }
        //    }
        //    // West
        //    if (x > (0 + (moveNum - 1)))
        //    {
        //        if (pieces[(int)(x - moveNum), (int)(y)].Value() == (int)PlayerTurn.Green)
        //        {
        //            pieceList.Add(new Vector2(x - moveNum, y));
        //        }
        //    }

        //    Vector2 piece = new Vector2();
        //    if (pieceList.Count == 0)
        //    {
        //        piece = PieceToSelect(moveTo, !jump);
        //    }
        //    else
        //    {
        //        int pieceNum = AIRandom.Next(0, pieceList.Count - 1);
        //        piece = pieceList[pieceNum];
        //    }
        //    return piece; 
        //}


        //private bool IsValidMove(int targetX, int targetY, bool jump)
        //{
        //    int moveNum;

        //    if (!jump)
        //    {
        //        moveNum = 1;
        //    }
        //    else
        //    {
        //        moveNum = 2;
        //    }

        //    if (targetX < ( 6 - (moveNum - 1)))
        //    {
        //        if (pieces[targetX + moveNum, targetY].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetX > ( 0 + (moveNum - 1)))
        //    {
        //        if (pieces[targetX - moveNum, targetY].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetY < ( 6 - (moveNum - 1)))
        //    {
        //        if (pieces[targetX, targetY + moveNum].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetY > ( 0 + (moveNum - 1)))
        //    {
        //        if (pieces[targetX, targetY - moveNum].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetX < ( 6 - (moveNum - 1)) && targetY < ( 6 - (moveNum - 1)))
        //    {
        //        if (pieces[targetX + moveNum, targetY + moveNum].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetX > ( 0 + (moveNum - 1)) && targetY < ( 6 - (moveNum - 1)))
        //    {
        //        if (pieces[targetX - moveNum, targetY + moveNum].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetX > ( 0 + (moveNum - 1)) && targetY > ( 0 + (moveNum - 1)))
        //    {
        //        if (pieces[targetX - moveNum, targetY - moveNum].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    if (targetX < ( 6 - (moveNum - 1)) && targetY > ( 0 + (moveNum - 1)))
        //    {
        //        if (pieces[targetX + moveNum, targetY - moveNum].Value() == (int)PlayerTurn.Green)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }

}

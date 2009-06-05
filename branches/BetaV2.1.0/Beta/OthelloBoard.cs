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
    class OthelloBoard
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

        // Image of the board
        Image board = new Image();

        // Cursors
        Cursor blackCursor = new Cursor();
        Cursor whiteCursor = new Cursor();

        // Images for the banners
        Image blackBanner = new Image();
        Image whiteBanner = new Image();

        // Array of pieces representing the board
        Piece[,] pieces = new Piece[LIMIT, LIMIT];

        // Queue to handle the animations one at a time
        Queue<Vector2> animate = new Queue<Vector2>();
        // Time for animation delay
        int time;

        // Selected Piece
        Vector2 selectedPiece;

        // Player turn
        int playerTurn;

        // Board offset
        Vector2 offset;

        // Board full
        public static bool boardFull;

        // final scores for each player
        int plr1score;
        int plr2score;

        // To draw the banners or not
        // Will be combined with animate.Count == 0
        bool blackWin;
        bool whiteWin;
        bool drawEndBanner;

        //Sounds
        SoundEffect selectPiece;
        SoundEffect unMove;
        SoundEffect aMove;
        SoundEffect convert;
        SoundEffectInstance seiConvert;
        SoundEffectInstance seiAvaMove;

        //Define the min and max board positions
        const int MIN = 0;
        const int MAX = 7;

        //Define the pieces array size
        const int LIMIT = 8;

        public OthelloBoard()
        {
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
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
            blackCursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0.0f);
            whiteCursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, new Vector2(0.5f), 0.0f);

            // Initialize the banners
            blackBanner.Initialize(new Vector2(0, 297 / 2), new Rectangle(0, 0, 798, 204), new Color(255, 255, 255, 0), Vector2.Zero, new Vector2(0.5f), 0f);
            whiteBanner.Initialize(new Vector2(0, 297 / 2), new Rectangle(0, 0, 798, 204), new Color(255, 255, 255, 0), Vector2.Zero, new Vector2(0.5f), 0f);

            // Initialize the board image
            board.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 1f);

            // Initialize Pieces to be all blank
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
                {
                    pieces[x, y].Initialize(new Vector2(((100 * x) / 2), ((100 * y) / 2)), new Rectangle(0, 0, 100, 100), Vector2.Zero, new Vector2(0.5f), 0.5f);
                    pieces[x, y].SetState(0);
                }
            }

            //Set the initial pieces for normal othello game
            //pieces[3, 3].SetState(1);
            //pieces[4, 4].SetState(1);
            //pieces[4, 3].SetState(2);
            //pieces[3, 4].SetState(2);


            //Quick end Othello game
            pieces[3, 3].SetState(1);
            pieces[4, 4].SetState(1);
            pieces[4, 3].SetState(1);
            pieces[3, 4].SetState(2);

            // Fill board to test complete green elimination
            //pieces[0, 0].SetState(1);
            //pieces[1, 0].SetState(1);
            //pieces[0, 1].SetState(1);
            //pieces[2, 2].SetState(2);

            // Set playerTurn
            playerTurn = (int)PlayerTurn.Red;

            // Set selected piece
            selectedPiece = new Vector2(-1, -1);

            // Set time
            time = 0;

            // Board full
            boardFull = false;

            blackWin = false;
            whiteWin = false;

        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D board, Texture2D red, Texture2D green, Texture2D redGreen, Texture2D greenRed, Texture2D blueBanner, Texture2D greenBanner, Texture2D redCur, Texture2D greenCur, Texture2D tPlr1, Texture2D tPlr2)
        {
            // Load the content for the board image
            this.board.LoadContent(spriteBatch, ref board);

            // Load the banners
            this.blackBanner.LoadContent(spriteBatch, ref blueBanner);
            this.whiteBanner.LoadContent(spriteBatch, ref greenBanner);

            // Load the content for the pieces
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
                {
                    pieces[x, y].LoadContent(spriteBatch, red, green, redGreen, greenRed, tPlr1, tPlr2);
                }
            }

            blackCursor.LoadContent(spriteBatch, ref redCur);
            whiteCursor.LoadContent(spriteBatch, ref greenCur);
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
            blackCursor.Update();
            whiteCursor.Update();

            #region Handle animations one at a time
            /* Old version: allowed for movement before the last piece was animated
            if (animate.Count > 0 || time < 500)
            {
                time += Math.Max(1, gameTime.ElapsedRealTime.Milliseconds);
                if (time > 500)
                {
                    pieces[(int)animate.Peek().X, (int)animate.Peek().Y].Animate();
                    animate.Dequeue();
                    time = 0;
                }
            }*/

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
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
                {
                    pieces[x, y].Update(gameTime);
                }
            }
            #endregion

            if (boardFull == true && animate.Count == 0 && time > 1000)
            {
                ClearBoard();
                PlopEndPieces();
            }


            if (drawEndBanner == true)
            {
                blackBanner.color.A = 0;

                blackBanner.color.A += (byte)(2 * gameTime.ElapsedGameTime.TotalSeconds);

                if (blackBanner.color.A == 255)
                    drawEndBanner = false;
            }
        }

        public void Click(MouseState mouseState)
        {
            // Find the grid coordinates of the mouse click
            int mouseX = (int)Math.Floor((double)(mouseState.X) / 50);
            int mouseY = (int)Math.Floor((double)(mouseState.Y) / 50);

            if (animate.Count == 0 && time >= 500 && !blackWin && !whiteWin)
            {


                if (selectedPiece.X < 0 || selectedPiece.Y < 0) // If selectedPiece is negative a.k.a. no piece selected
                {
                    // Only perform click method if the click was on the board
                    if (mouseX >= 0 && mouseX < LIMIT && mouseY >= MIN && mouseY < LIMIT)
                    {
                        // Perform logic depending on what piece is in the space
                        if (pieces[mouseX, mouseY].Value() == 0)
                        {
                            //Save out the mouce positions
                            int sX = mouseX;
                            int sY = mouseY;

                            //Create temporary mouse positions
                            int tempX;
                            int tempY;

                            //Flags for finding if you have a piece capping off a line
                            bool oppositePlayerObstruction = false;
                            bool samePlayerEndOfLine = false;

                            //Is true if you have an opponents piece with a friendly piece after it
                            bool validMove = false;

                            //Set up a variable to hold who's turn it is
                            //Used so I don't ahve to copy and paste a hugh chunk of code and
                            //change 1's to 2's and vise versa.
                            int player = playerTurn;

                            //Ready to store the value of the opposite player's number
                            //Must initialize to 0
                            int oppositePlayer = 0;


                            //Sets up the opposite player's number
                            if (player == 1)
                                oppositePlayer = 2;
                            if (player == 2)
                                oppositePlayer = 1;

                            //Vert Up

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;
                            if (!validMove)
                                CheckUp(sX, tempY, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            //Vert Down

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckDown(sX, tempY, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            //Horo Left

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckLeft(sY, tempX, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            //Horo Right

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckRight(sY, tempX, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            // Diag Top Left

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckTopLeft(tempX, tempY, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            // Diag Bottom Right

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckBottRight(tempX, tempY, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            // Diag Top Right

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckTopRight(tempX, tempY, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            // Diag Bottom Left

                            //Save out mouse positions
                            tempX = sX;
                            tempY = sY;

                            if (!validMove)
                                CheckBottLeft(tempX, tempY, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);


                            //if any of the above Checks pass, at least one mutation needs to occur
                            if (validMove == true)
                            {
                                pieces[mouseX, mouseY].SetState(player);
                                Mutate(mouseX, mouseY, player);

                                if (AnyMoves(oppositePlayer))
                                    playerTurn = oppositePlayer;
                                else
                                {
                                    boardFull = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckBottLeft(int tempX, int tempY, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempY < MAX && tempX > MIN)
                {
                    if (pieces[tempX - 1, tempY + 1].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempY < MAX - 1 && tempX > MIN + 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[tempX - 2, tempY + 2].Value() != player)
                            //{
                            //    break;
                            //}
                            //Check to see if you have a piece capping a line off 
                            if (pieces[tempX - 2, tempY + 2].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempX--;
                        tempY++;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Diag Bottom Left

            }
        }

        private void CheckTopRight(int tempX, int tempY, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempY > MIN && tempX < MAX)
                {
                    if (pieces[tempX + 1, tempY - 1].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempY > MIN + 1 && tempX < MAX - 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[tempX + 2, tempY - 2].Value() != player)
                            //{
                            //    break;
                            //}
                            //Check to see if you have a piece capping a line off 
                            if (pieces[tempX + 2, tempY - 2].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempX++;
                        tempY--;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Diag Top Right

            }
        }

        private void CheckBottRight(int tempX, int tempY, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempY < MAX && tempX < MAX)
                {
                    if (pieces[tempX + 1, tempY + 1].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempY < MAX - 1 && tempX < MAX - 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[tempX + 2, tempY + 2].Value() != player)
                            //{
                            //    break;
                            //}
                            //Check to see if you have a piece capping a line off 
                            if (pieces[tempX + 2, tempY + 2].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempX++;
                        tempY++;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Diag Bottom Right

            }
        }

        private void CheckTopLeft(int tempX, int tempY, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempY > MIN && tempX > MIN)
                {
                    if (pieces[tempX - 1, tempY - 1].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempY > MIN + 1 && tempX > MIN + 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[tempX - 2, tempY - 2].Value() != player)
                            //{
                            //    break;
                            //}
                            //Check to see if you have a piece capping a line off 
                            if (pieces[tempX - 2, tempY - 2].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempX--;
                        tempY--;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Diag Top Left
            }
        }

        private void CheckRight(int sY, int tempX, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempX < MAX)
                {
                    if (pieces[tempX + 1, sY].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempX < MAX - 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[tempX + 2, sY].Value() != player)
                            //{
                            //    break;
                            //}
                            //Check to see if you have a piece capping a line off 
                            if (pieces[tempX + 2, sY].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempX++;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Horo Right

            }
        }

        private void CheckLeft(int sY, int tempX, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempX > MIN)
                {
                    if (pieces[tempX - 1, sY].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempX > MIN + 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[tempX - 2, sY].Value() != player)
                            //{
                            //    break;
                            //}

                            //Check to see if you have a piece capping a line off 
                            if (pieces[tempX - 2, sY].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempX--;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Horo Left
            }
        }

        private void CheckDown(int sX, int tempY, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempY < MAX)
                {
                    if (pieces[sX, tempY + 1].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempY < MAX - 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[sX, tempY + 2].Value() != player)
                            //{
                            //    break;
                            //}

                            //Check to see if you have a piece capping a line off 
                            if (pieces[sX, tempY + 2].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }
                        }
                        tempY++;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Vert Down   
            }
        }

        private void CheckUp(int sX, int tempY, ref bool oppositePlayerObstruction, ref bool samePlayerEndOfLine, ref bool validMove, int player, int oppositePlayer)
        {
            //Reset
            oppositePlayerObstruction = false;
            samePlayerEndOfLine = false;

            if (validMove == false)
            {

                //Check for at least one opponent's piece
                if (tempY > MIN)
                {
                    if (pieces[sX, tempY - 1].Value() == oppositePlayer)
                    {
                        oppositePlayerObstruction = true;
                    }
                }

                if (oppositePlayerObstruction == true)
                {
                    while (tempY > MIN + 1 && samePlayerEndOfLine != true)
                    {
                        {
                            ////Make sure there are no blank spaces in between
                            //if (pieces[sX, tempY - 2].Value() != player)
                            //{
                            //    break;
                            //}

                            //Check to see if you have a piece capping a line off 
                            if (pieces[sX, tempY - 2].Value() == player)
                            {
                                samePlayerEndOfLine = true;
                            }

                        }
                        tempY--;
                    }
                }

                if (oppositePlayerObstruction == true && samePlayerEndOfLine == true)
                {
                    validMove = true;
                    oppositePlayerObstruction = false;
                    samePlayerEndOfLine = false;
                }
                //
                //
                //end Vert Up   
            }
        }



        public void Draw()
        {
            // Draw the board
            board.Draw();

            // Loop through and draw all of the pieces
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
                {
                    pieces[x, y].Draw();
                }
            }

            // Draw the banners
            if (whiteWin == true && animate.Count == 0 && time >= 1000)
            {
                whiteBanner.Draw();
            }
            if (blackWin == true && animate.Count == 0 && time >= 1000)
            {
                blackBanner.Draw();
            }

            if (!boardFull)
            {
                // Draw the correct cursor
                if (playerTurn == (int)PlayerTurn.Red)
                {
                    blackCursor.Draw();
                }
                else if (playerTurn == (int)PlayerTurn.Green)
                {
                    whiteCursor.Draw();
                }
            }
        }

        private bool MovePiece(Vector2 selected, int targetX, int targetY, int player)
        {
            //if a valid move is found this will be set to true
            bool validMove = true;
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

            int sX = targetX;
            int sY = targetY;
            int tempX;
            int tempY;
            bool validMove = false;

            //Draw pices from clicked all the way vertically upwards
            #region Up
            tempY = sY;
            while (tempY > MIN)
            {
                {
                    if (pieces[sX, tempY - 1].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[sX, tempY - 1].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempY--;
            }

            if (validMove == true)
            {
                tempY = sY;

                while (tempY > MIN)
                {
                    if (pieces[sX, tempY - 1].Value() == player || pieces[sX, tempY - 1].Value() == 0)
                    {
                        break;
                    }
                    tempY = DrawVertUp(player, sX, tempY);
                    tempY--;
                }
            }
            validMove = false;
            #endregion

            //Draw pices from clicked vertically downwards
            #region Down

            tempY = sY;

            while (tempY < MAX)
            {
                {
                    if (pieces[sX, tempY + 1].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[sX, tempY + 1].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempY++;
            }

            if (validMove == true)
            {
                tempY = sY;

                while (tempY < MAX)
                {
                    if (pieces[sX, tempY + 1].Value() == player || pieces[sX, tempY + 1].Value() == 0)
                    {
                        break;
                    }
                    tempY = DrawVertDown(player, sX, tempY);
                }

                validMove = false;
            }
            #endregion

            //Draw pieces from clicked horozontally to the left
            #region Left

            tempX = sX;

            while (tempX > MIN)
            {
                {
                    if (pieces[tempX - 1, sY].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[tempX - 1, sY].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempX--;
            }

            if (validMove == true)
            {
                tempX = sX;

                while (tempX > MIN)
                {
                    if (pieces[tempX - 1, sY].Value() == player || pieces[tempX - 1, sY].Value() == 0)
                    {
                        break;
                    }
                    tempX = DrawHoroLeft(player, sY, tempX);
                }
                validMove = false;
            }
            #endregion

            //Draw pieces from clicked horozontally to the right
            #region Right

            tempX = sX;

            while (tempX < MAX)
            {
                {
                    if (pieces[tempX + 1, sY].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[tempX + 1, sY].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempX++;
            }

            if (validMove == true)
            {
                tempX = sX;
                while (tempX < MAX)
                {
                    if (pieces[tempX + 1, sY].Value() == player || pieces[tempX + 1, sY].Value() == 0)
                    {
                        break;
                    }
                    tempX = DrawHoroRight(player, sY, tempX);
                }
                validMove = false;
            }
            #endregion

            //Draw pieces diagonally to top left
            #region Top Left

            tempX = sX;
            tempY = sY;

            while (tempY > MIN && tempX > MIN)
            {
                {
                    if (pieces[tempX - 1, tempY - 1].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[tempX - 1, tempY - 1].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempX--;
                tempY--;
            }

            if (validMove == true)
            {
                tempX = sX;
                tempY = sY;

                while (tempY > MIN && tempX > MIN)
                {
                    if (pieces[tempX - 1, tempY - 1].Value() == player ||
                        pieces[tempX - 1, tempY - 1].Value() == 0)
                    {
                        break;
                    }
                    DrawDiagTopLeft(player, ref tempX, ref tempY);
                }
                validMove = false;
            }
            #endregion

            //Draw pieces diagonally to bottom right
            #region Bottom Right
            tempX = sX;
            tempY = sY;

            while (tempY < MAX && tempX < MAX)
            {
                {
                    if (pieces[tempX + 1, tempY + 1].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[tempX + 1, tempY + 1].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempX++;
                tempY++;
            }

            if (validMove == true)
            {
                tempX = sX;
                tempY = sY;

                while (tempY < MAX && tempX < MAX)
                {
                    if (pieces[tempX + 1, tempY + 1].Value() == player ||
                        pieces[tempX + 1, tempY + 1].Value() == 0)
                    {
                        break;
                    }
                    DrawDiagBottRight(player, ref tempX, ref tempY);
                }
                validMove = false;
            }
            #endregion

            //Draw pieces diagonally to top right
            #region Top Right

            tempX = sX;
            tempY = sY;

            while (tempY > MIN && tempX < MAX)
            {
                {
                    if (pieces[tempX + 1, tempY - 1].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[tempX + 1, tempY - 1].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempX++;
                tempY--;
            }

            if (validMove == true)
            {
                tempX = sX;
                tempY = sY;

                while (tempY > MIN && tempX < MAX)
                {
                    if (pieces[tempX + 1, tempY - 1].Value() == player ||
                        pieces[tempX + 1, tempY - 1].Value() == 0)
                    {
                        break;
                    }
                    DrawDiagTopRight(player, ref tempX, ref tempY);
                }
                validMove = false;
            }
            #endregion

            //Draw pieces diagonally to bottom left
            #region Bottom Left

            tempX = sX;
            tempY = sY;

            while (tempY < MAX && tempX > MIN)
            {
                {
                    if (pieces[tempX - 1, tempY + 1].Value() == 0 && validMove == false)
                    {
                        break;
                    }
                    if (pieces[tempX - 1, tempY + 1].Value() == player)
                    {
                        validMove = true;
                    }
                }
                tempX--;
                tempY++;
            }

            if (validMove == true)
            {
                tempX = sX;
                tempY = sY;

                while (tempY < MAX && tempX > MIN)
                {
                    if (pieces[tempX - 1, tempY + 1].Value() == player ||
                        pieces[tempX - 1, tempY + 1].Value() == 0)
                    {
                        break;
                    }
                    DrawDiagBottLeft(player, ref tempX, ref tempY);
                }
                validMove = false;
            }
            #endregion
        }

        private void DrawDiagBottLeft(int player, ref int tempX, ref int tempY)
        {
            // if (pieces[tempX - 1, tempY + 1].Value() != player)
            pieces[tempX - 1, tempY + 1].Mutate(player);
            animate.Enqueue(new Vector2(tempX - 1, tempY + 1));
            tempX--;
            tempY++;
        }

        private void DrawDiagTopRight(int player, ref int tempX, ref int tempY)
        {
            pieces[tempX + 1, tempY - 1].Mutate(player);
            animate.Enqueue(new Vector2(tempX + 1, tempY - 1));
            tempX++;
            tempY--;
        }

        private void DrawDiagBottRight(int player, ref int tempX, ref int tempY)
        {
            pieces[tempX + 1, tempY + 1].Mutate(player);
            animate.Enqueue(new Vector2(tempX + 1, tempY + 1));
            tempX++;
            tempY++;
        }

        private void DrawDiagTopLeft(int player, ref int tempX, ref int tempY)
        {
            pieces[tempX - 1, tempY - 1].Mutate(player);
            animate.Enqueue(new Vector2(tempX - 1, tempY - 1));
            tempX--;
            tempY--;
        }

        private int DrawHoroRight(int player, int sY, int tempX)
        {
            pieces[tempX + 1, sY].Mutate(player);
            animate.Enqueue(new Vector2(tempX + 1, sY));
            tempX++;
            return tempX;
        }

        private int DrawHoroLeft(int player, int sY, int tempX)
        {
            pieces[tempX - 1, sY].Mutate(player);
            animate.Enqueue(new Vector2(tempX - 1, sY));
            tempX--;
            return tempX;
        }

        private int DrawVertDown(int player, int sX, int tempY)
        {
            pieces[sX, tempY + 1].Mutate(player);
            animate.Enqueue(new Vector2(sX, tempY + 1));
            tempY++;
            return tempY;
        }

        private int DrawVertUp(int player, int sX, int tempY)
        {
            pieces[sX, tempY - 1].Mutate(player);
            animate.Enqueue(new Vector2(sX, tempY - 1));

            return tempY;
        }

        public bool AnyMoves(int player)
        {
            //Initialize variables

            //Flags for finding if you have a piece capping off a line
            bool oppositePlayerObstruction = false;
            bool samePlayerEndOfLine = false;

            //Is true if you have an opponents piece with a friendly piece after it
            bool validMove = false;

            //Set up a variable to hold who's turn it is
            //Used so I don't ahve to copy and paste a hugh chunk of code and
            //change 1's to 2's and vise versa.
            //int player = playerTurn;

            //Ready to store the value of the opposite player's number
            //Must initialize to 0
            int oppositePlayer = 0;


            //Sets up the opposite player's number
            if (player == 1)
                oppositePlayer = 2;
            if (player == 2)
                oppositePlayer = 1;


            //Simulates mouse click
            //Iterates thru pieces to see if any have a move
            for (int y = 0; y < LIMIT; y++)
            {
                for (int x = 0; x < LIMIT; x++)
                {
                    if (validMove == false)
                    {
                        if (pieces[x, y].Value() == 0)
                        {
                            CheckUp(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckDown(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckLeft(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckRight(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckTopLeft(x, x, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckTopRight(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckBottLeft(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);
                            CheckBottRight(x, y, ref oppositePlayerObstruction, ref samePlayerEndOfLine, ref validMove, player, oppositePlayer);

                            if (validMove)
                            {
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
            for (int y = 0; y < LIMIT; y++)
            {
                for (int x = 0; x < LIMIT; x++)
                {
                    if (pieces[x, y].Value() == player)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public bool BoardFull()
        {
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
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
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
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
            for (int x = 0; x < LIMIT; x++)
            {
                for (int y = 0; y < LIMIT; y++)
                {
                    pieces[x, y].SetState(0);
                }
            }
            return;
        }


        public void PlopEndPieces()
        {
            if (plr1score > plr2score)
                blackWin = true;
            else
                whiteWin = true;

            blackBanner.color.A = 100;


            for (int y = 0; y < LIMIT; y++)
            {
                for (int x = 0; x < LIMIT; x++)
                {
                    if (plr1score > 0)
                    {
                        pieces[x, y].SetState(5);
                        animate.Enqueue(new Vector2(x, y));
                        plr1score--;
                    }
                    if (plr2score > 0)
                    {
                        pieces[MAX - x, MAX - y].SetState(6);
                        animate.Enqueue(new Vector2(MAX - x, MAX - y));
                        plr2score--;
                    }
                }
            }
            boardFull = false;
            drawEndBanner = true;

            return;
        }
    }
}

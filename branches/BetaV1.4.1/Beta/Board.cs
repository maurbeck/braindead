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
    class Board
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
            Quit = 4
        }

        // Image of the board
        Image board = new Image();

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
        bool drawBlueBanner;
        bool drawGreenBanner;

        //make counters for plopendpieces looping
        //int x1;
        //int y1;
        //int x2;
        //int y2;
        //int count1;
        //int count2;

        //Sounds
        SoundEffect selectPiece;
        SoundEffect unMove;
        SoundEffect aMove;
        SoundEffect convert;

        public Board()
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
            blueBanner.Initialize(new Vector2(0, 297/2), new Rectangle(0, 0, 798, 204), Color.White, Vector2.Zero, new Vector2(0.5f), 0f);
            greenBanner.Initialize(new Vector2(0, 297/2), new Rectangle(0, 0, 798, 204), Color.White, Vector2.Zero, new Vector2(0.5f), 0f);

            // Initialize the board image
            board.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, new Vector2(0.5f), 1f);

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
            //pieces[0, 0].SetState(1);
            //pieces[6, 6].SetState(1);
            //pieces[0, 6].SetState(2);
            //pieces[6, 0].SetState(2);

            //Peices for quick end of game

            pieces[0, 0].SetState(1);
            pieces[1, 0].SetState(1);
            pieces[2, 0].SetState(1);
            pieces[3, 0].SetState(1);
            pieces[4, 0].SetState(1);
            pieces[5, 0].SetState(1);
            pieces[6, 0].SetState(1);
            pieces[0, 1].SetState(1);
            pieces[1, 1].SetState(2);
            pieces[2, 1].SetState(1);
            pieces[3, 1].SetState(1);
            pieces[4, 1].SetState(1);
            pieces[5, 1].SetState(2);
            pieces[6, 1].SetState(1);
            pieces[0, 2].SetState(1);
            //pieces[1, 2].SetState(2);
            //pieces[2, 2].SetState(1);
            //pieces[3, 2].SetState(1);
            pieces[4, 2].SetState(1);
            pieces[5, 2].SetState(2);
            pieces[6, 2].SetState(1);
            pieces[0, 3].SetState(1);
            pieces[1, 3].SetState(1);
            pieces[2, 3].SetState(2);
            pieces[3, 3].SetState(2);
            pieces[4, 3].SetState(2);
            pieces[5, 3].SetState(2);
            pieces[6, 3].SetState(1);
            pieces[0, 4].SetState(2);
            pieces[1, 4].SetState(2);
            pieces[2, 4].SetState(2);
            pieces[3, 4].SetState(2);
            pieces[4, 4].SetState(1);
            pieces[5, 4].SetState(1);
            pieces[6, 4].SetState(2);
            pieces[0, 5].SetState(1);
            pieces[1, 5].SetState(1);
            pieces[2, 5].SetState(2);
            pieces[3, 5].SetState(2);
            pieces[4, 5].SetState(2);
            pieces[5, 5].SetState(1);
            pieces[6, 5].SetState(1);
            pieces[0, 6].SetState(2);
            pieces[1, 6].SetState(1);
            pieces[2, 6].SetState(1);
            pieces[3, 6].SetState(1);
            pieces[4, 6].SetState(1);
            pieces[5, 6].SetState(1);
            pieces[6, 6].SetState(1);



            // Set playerTurn
            playerTurn = (int)PlayerTurn.Red;
            // Set selected piece
            selectedPiece = new Vector2(-1, -1);
            // Set time
            time = 0;
            // Board full
            boardFull = false;
            // Set counts
            //redCount = 0;
            //greenCount = 0;

            drawBlueBanner = false;
            drawGreenBanner = false;

            //x1 = 0;
            //y1 = 0;
            //x2 = 6;
            //y2 = 6;
            
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D board, Texture2D red, Texture2D green, Texture2D redSelection, Texture2D greenSelection, Texture2D redGreen, Texture2D greenRed, Texture2D tPlr1, Texture2D tPlr2, Texture2D redCur, Texture2D greenCur, Texture2D blueBanner, Texture2D greenBanner)
        {
            // Load the content for the board image
            this.board.LoadContent(spriteBatch, board);

            // Load the content for the selected piece images
            this.redSelect.LoadContent(spriteBatch, redSelection);
            this.greenSelect.LoadContent(spriteBatch, greenSelection);

            // Load the banners
            this.blueBanner.LoadContent(spriteBatch, blueBanner);
            this.greenBanner.LoadContent(spriteBatch, greenBanner);

            // Load the content for the pieces
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].LoadContent(spriteBatch, red, green, redGreen, greenRed,tPlr1,tPlr2);
                }
            }

            redCursor.LoadContent(spriteBatch, redCur);
            greenCursor.LoadContent(spriteBatch, greenCur);
        }

        public void LoadAudio(SoundEffect selectPiece, SoundEffect unMove, SoundEffect aMove, SoundEffect convert)
        {
            this.selectPiece = selectPiece;
            this.unMove = unMove;
            this.aMove = aMove;
            this.convert = convert;
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

            if (time < 1000)
            {
                time += Math.Max(1, gameTime.ElapsedRealTime.Milliseconds);
            }
            if (time >= 750)
            {
                if (animate.Count > 0)
                {
                    pieces[(int)animate.Peek().X, (int)animate.Peek().Y].Animate();
                    animate.Dequeue();
                    time = 0;
                    convert.Play(1.0f, 0.0f, 0.0f, false);
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

            #region Keep running total of pieces on board
            // Update count of pieces if all animations are finished
            // and the total of the last count is less than 49
            /*
            if (animate.Count == 0 && ((redCount + greenCount) < 49))
            {
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        switch (pieces[x, y].Value())
                        {
                            case 1:
                                redCount++;
                                break;
                            case 2:
                                greenCount++;
                                break;
                        }
                    }
                }
            }
            */


            if (boardFull == true && animate.Count == 0 && time > 1000)
            {
                ClearBoard();
                PlopEndPieces();
            }
            #endregion
        }

        public void Click(MouseState mouseState)
        {
            // Find the grid coordinates of the mouse click
            int mouseX = (int)Math.Floor((double)(mouseState.X - offset.X) / 50);
            int mouseY = (int)Math.Floor((double)(mouseState.Y - offset.Y) / 50);

            if (animate.Count == 0 && time >= 500 && !drawBlueBanner && !drawGreenBanner)
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
                                                playerTurn = 2;
                                                aMove.Play(1.0f, 0.0f, 0.0f, false);
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
                    case 2: // Green turn
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
                                        // Do nothing for now
                                        // Will play error noise in the future
                                        break;
                                    case 2:     // If clicked on green piece
                                        // Select the green piece
                                        selectedPiece = new Vector2(mouseX, mouseY);
                                        selectPiece.Play(1.0f, 0.0f, 0.0f, false);
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
                                        // If PerformMove is false (invalid move)
                                        if (MovePiece(selectedPiece, mouseX, mouseY, playerTurn))
                                        {
                                            // Prepare for next player
                                            if (AnyMoves(1))
                                            {
                                                playerTurn = 1;
                                                aMove.Play(1.0f, 0.0f, 0.0f, false);
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
                                        // Deselect piece
                                        selectedPiece = new Vector2(-1, -1);
                                        unMove.Play(1.0f, 0.0f, 0.0f, false);
                                        break;
                                    case 2:     // If clicked on green piece
                                        // Deselect piece
                                        selectedPiece = new Vector2(mouseX, mouseY);
                                        selectPiece.Play(1.0f, 0.0f, 0.0f, false);
                                        break;
                                    default:    // Error
                                        // Error
                                        break;
                                }
                            }
                        }
                        break;
                        #endregion
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
            board.Draw();

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
                    case 2:
                        redSelect.SetPosition(new Vector2(((100 * selectedPiece.X) / 2 + offset.X), ((100 * selectedPiece.Y) / 2 + offset.Y)));
                        redSelect.Draw();
                        break;

                    case 1:
                        greenSelect.SetPosition(new Vector2(((100 * selectedPiece.X) / 2 + offset.X), ((100 * selectedPiece.Y) / 2 + offset.Y)));
                        greenSelect.Draw();
                        break;
                }
            }

            // Draw the banners
            if (drawGreenBanner == true && animate.Count == 0 && time >= 1000)
            {
                greenBanner.Draw();
            }
            if (drawBlueBanner == true && animate.Count == 0 && time >= 1000)
            {
                blueBanner.Draw();
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
            if (targetY > 0 && targetX > 0)
            {
                // animation bug here, was checking if pieces[targetX - 1, targetX - 1].Value() != player
                if (pieces[targetX - 1, targetY - 1].Value() > 0 && pieces[targetX - 1, targetY - 1].Value() != player)
                {
                    pieces[targetX - 1, targetY - 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX - 1, targetY - 1));
                }
            }
            if (targetY > 0)
            {
                if (pieces[targetX, targetY - 1].Value() > 0 && pieces[targetX, targetY - 1].Value() != player)
                {
                    pieces[targetX, targetY - 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX, targetY - 1));
                }
            }
            if (targetY > 0 && targetX < 6)
            {
                if (pieces[targetX + 1, targetY - 1].Value() > 0 && pieces[targetX + 1, targetY - 1].Value() != player)
                {
                    pieces[targetX + 1, targetY - 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX + 1, targetY - 1));
                }
            }
            if (targetX > 0)
            {
                if (pieces[targetX - 1, targetY].Value() > 0 && pieces[targetX - 1, targetY].Value() != player)
                {
                    pieces[targetX - 1, targetY].Mutate(player);
                    animate.Enqueue(new Vector2(targetX - 1, targetY));
                }
            }
            if (targetX < 6)
            {
                if (pieces[targetX + 1, targetY].Value() > 0 && pieces[targetX + 1, targetY].Value() != player)
                {
                    pieces[targetX + 1, targetY].Mutate(player);
                    animate.Enqueue(new Vector2(targetX + 1, targetY));
                }
            }
            if (targetY < 6 && targetX > 0)
            {
                if (pieces[targetX - 1, targetY + 1].Value() > 0 && pieces[targetX - 1, targetY + 1].Value() != player)
                {
                    pieces[targetX - 1, targetY + 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX - 1, targetY + 1));
                }

            }
            if (targetY < 6)
            {
                if (pieces[targetX, targetY + 1].Value() > 0 && pieces[targetX, targetY + 1].Value() != player)
                {
                    pieces[targetX, targetY + 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX, targetY + 1));
                }
            }
            if (targetY < 6 && targetX < 6)
            {
                if (pieces[targetX + 1, targetY + 1].Value() > 0 && pieces[targetX + 1, targetY + 1].Value() != player)
                {
                    pieces[targetX + 1, targetY + 1].Mutate(player);
                    animate.Enqueue(new Vector2(targetX + 1, targetY + 1));
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


        /*OLD PlOP END PIECES/
        public void PlopEndPieces()
        {

            for (int y = 0; y < 7 && plr1score > 0; y++)
            {
                for (int x = 0; x < 7 && plr1score > 0; x++)
                {
                    pieces[x, y].SetState(1);
                    plr1score--;
                }
            }


            for (int y = 6; y > -1 && plr2score > 0; y--)
            {
                for (int x = 6; x > -1 && plr2score > 0; x--)
                {
                    pieces[x, y].SetState(2);
                    plr2score--;
                }
            }

            //    //bool plr1working = true;

            //    //#region

            //    //while (plr1working == true)
            //    //{

            //    //    int y1 = 0;

            //    //    for (int x1 = 0; x1 <= 7; x1++)
            //    //    {
            //    //        if (plr1score > 0)
            //    //        {
            //    //            if (x1 == 7)
            //    //            {
            //    //                x1 = 0;
            //    //                y1++;

            //    //            }
            //    //            if (y1 == 7)
            //    //            {
            //    //                break;
            //    //            }

            //    //            else
            //    //            {
            //    //                pieces[x1, y1].SetState(1);
            //    //            }
            //    //            plr1score--;
            //    //        }
            //    //        else
            //    //        {
            //    //            plr1working = false;
            //    //            break;
            //    //        }
            //    //    }
            //    //}

            //    //while (plr1working == false)
            //    //{
            //    //    int yIterations = 0;
            //    //    int y2 = 6;

            //    //    for (int x2 = 6; x2 >= -1; x2--)
            //    //    {
            //    //        if (plr2score > 0)
            //    //        {
            //    //            if (x2 == -1)
            //    //            {
            //    //                x2 = 6;
            //    //                y2--;

            //    //            }
            //    //            if (y2 == -1)
            //    //            {
            //    //                y2 = 6;
            //    //                yIterations++;
            //    //            }

            //    //            else
            //    //            {
            //    //                pieces[x2, y2].SetState(2);
            //    //                //plr1working = false;
            //    //            }
            //    //            plr2score--;
            //    //        }
            //    //        else
            //    //        {
            //    //            return;
            //    //        }
            //    //    }
            //    //}
            //    //#endregion
        }*/

        public void PlopEndPieces()
        {
            //count1 = plr1score;
            //count2 = plr2score;
            //bool status1 = true;
            //bool status2 = true;

            if (plr1score > plr2score)
                drawBlueBanner = true;
            else
                drawGreenBanner = true;


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

            /*
            while ((status1 || status2) == true)
            {
                status1 = DispPlr1();
                status2 = DispPlr2();
            }
            boardFull = false;

            status1 = true;
            status2 = true;
             * */


            return;
        }
        /*
        public bool DispPlr1()
        {

            if (count1 == 0)
                return false;
            pieces[x1, y1].SetState(5);
            animate.Enqueue(new Vector2(x1, y1));
            count1--;
            y1++;
            if (y1 == Y)
            {
                y1 = 0;
                x1++;
            }
            return true;
        }

        public bool DispPlr2()
        {

            if (count2 == 0)
                return false;
            pieces[x2, y2].SetState(6);
            animate.Enqueue(new Vector2(x2, y2));
            count2--;
            y2--;
            if (y2 == -1)
            {
                y2 = 6;
                x2--;
            }
            return true;
        }
        */
    }
}

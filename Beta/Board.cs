using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
        bool boardFull;

        // Piece count
        int redCount;
        int greenCount;

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
            offset = new Vector2(49, 49);
            // Clear the animation queue
            animate.Clear();
            // Initialize the cursors
            redCursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, Vector2.Zero, Vector2.One, 0.0f);
            greenCursor.Initialize(Vector2.Zero, new Rectangle(0, 0, 50, 50), Color.White, new Vector2(50,50), Vector2.One, 0.0f);

            // Initialize the board image
            board.Initialize(Vector2.Zero, new Rectangle(0, 0, 798, 798), Color.White, Vector2.Zero, Vector2.One, 1f);

            // Initialize Pieces to be all blank
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].Initialize(new Vector2(((100*x)+offset.X), ((100*y)+offset.Y)), new Rectangle(0, 0, 100, 100), Vector2.Zero, 0.5f);
                    pieces[x, y].SetState(0);
                }
            }

            // Set the initial corner pieces
            pieces[0, 0].SetState(1);
            pieces[6, 6].SetState(1);
            pieces[0, 6].SetState(2);
            pieces[6, 0].SetState(2);

            // Set playerTurn
            playerTurn = (int)PlayerTurn.Red;
            // Set selected piece
            selectedPiece = new Vector2(-1, -1);
            // Set time
            time = 0;
            // Board full
            boardFull = false;
            // Set counts
            redCount = 0;
            greenCount = 0;
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D board, Texture2D red, Texture2D green, Texture2D redGreen, Texture2D greenRed, Texture2D redCur, Texture2D greenCur)
        {
            // Load the content for the board image
            this.board.LoadContent(spriteBatch, board);

            // Load the content for the pieces
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].LoadContent(spriteBatch, red, green, redGreen, greenRed);
                }
            }

            redCursor.LoadContent(spriteBatch, redCur);
            greenCursor.LoadContent(spriteBatch, greenCur);
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

            
            if (animate.Count > 0)
            {
                time += Math.Max(1, gameTime.ElapsedRealTime.Milliseconds);
                if (time > 500)
                {
                    pieces[(int)animate.Peek().X, (int)animate.Peek().Y].Animate();
                    animate.Dequeue();
                    time = 0;
                }
            }
            // Update all pieces on the board
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[x, y].Update(gameTime);
                }
            }

            // Update count of pieces if all animations are finished
            // and the total of the last count is less than 49
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
        }

        public void Click(MouseState mouseState)
        {
            // Find the grid coordinates of the mouse click
            int mouseX = (int)Math.Floor((double)(mouseState.X - offset.X) / 100);
            int mouseY = (int)Math.Floor((double)(mouseState.Y - offset.Y) / 100);

            if (animate.Count == 0)
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
                                                playerTurn = 2;
                                            else
                                                if (BoardFull())
                                                    boardFull = true;

                                            selectedPiece = new Vector2(-1, -1);
                                        }
                                        else
                                        {
                                            // Deselect piece
                                            selectedPiece = new Vector2(-1, -1);
                                        }
                                        break;
                                    case 1:     // If clicked on red piece
                                        // Select piece
                                        selectedPiece = new Vector2(mouseX, mouseY);
                                        break;
                                    case 2:     // If clicked on green piece
                                        // Deselect piece
                                        selectedPiece = new Vector2(-1, -1);
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
                                                playerTurn = 1;
                                            else
                                                if (BoardFull())
                                                    boardFull = true;

                                            selectedPiece = new Vector2(-1, -1);
                                        }
                                        else
                                        {
                                            // Deselect piece
                                            selectedPiece = new Vector2(-1, -1);
                                        }
                                        break;
                                    case 1:     // If clicked on red piece
                                        // Deselect piece
                                        selectedPiece = new Vector2(-1, -1);
                                        break;
                                    case 2:     // If clicked on green piece
                                        // Deselect piece
                                        selectedPiece = new Vector2(mouseX, mouseY);
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
                            if (y > (0 + (moveNum - 1)) && x < (7 - (moveNum - 1)))
                            {
                                if (pieces[(x + moveNum), (y - moveNum)].Value() == 0)
                                    return true;
                            }
                            // East
                            if (x < (7 - (moveNum - 1)))
                            {
                                if (pieces[(x + moveNum), (y)].Value() == 0)
                                    return true;
                            }
                            // Southeast
                            if (y < (7 - (moveNum - 1)) && x < (7 - (moveNum - 1)))
                            {
                                if (pieces[(x + moveNum), (y + moveNum)].Value() == 0)
                                    return true;
                            }
                            // South
                            if (y < (7 - (moveNum - 1)))
                            {
                                if (pieces[(x), (y + moveNum)].Value() == 0)
                                    return true;
                            }
                            // Southwest
                            if (y < (7 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
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
    }
}

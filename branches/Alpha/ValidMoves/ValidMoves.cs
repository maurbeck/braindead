using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ValidMoves
{
    public class ValidMoves : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D greenTex;
        Texture2D redTex;
        Texture2D boardTex;

        MouseState mouseState;

        int[,] pieces = new int[7, 7];
        bool[,] validMoves = new bool[7, 7];

        Vector2 selectedPiece;

        int playerTurn;

        bool clickEnabled = true;
        int mouseX;
        int mouseY;

        public ValidMoves()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            playerTurn = 1;

            selectedPiece = new Vector2(-1, -1);

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    pieces[y, x] = 0;
                    validMoves[y, x] = false;
                }
            }

            pieces[0, 0] = 1;
            pieces[0, 6] = 2;
            pieces[6, 0] = 2;
            pieces[6, 6] = 1;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            redTex = Content.Load<Texture2D>("Red");
            greenTex = Content.Load<Texture2D>("Green");
            boardTex = Content.Load<Texture2D>("Board");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Reset the entire board
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                this.Initialize();

            

            // Get the mouse state
            mouseState = Mouse.GetState();

            #region If click is enabled
            if (clickEnabled)
            {
                #region If LMB is pressed and click is enabled
                // If left mouse is down do the click logic
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    // Find the grid coordinates of the mouse click
                    mouseX = (int)Math.Floor((double)mouseState.X / 100);
                    mouseY = (int)Math.Floor((double)mouseState.Y / 100);

                    #region Select player logic to do for click
                    // Select which player logic to do
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
                                    switch (pieces[mouseY, mouseX])
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
                                            // If the current square clicked on does
                                            this.Exit();
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
                                    switch (pieces[mouseY, mouseX])
                                    {
                                        case 0:     // If blank space clicked
                                            // If PerformMove is true
                                            if (PerformMove(selectedPiece, mouseX, mouseY, playerTurn))
                                            {
                                                // Prepare for next player
                                                if(AnyMoves(2))
                                                    playerTurn = 2;

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
                                            selectedPiece = new Vector2(mouseX, mouseY);
                                            break;
                                        case 2:     // If clicked on green piece
                                            // Deselect piece
                                            selectedPiece = new Vector2(-1, -1);
                                            break;
                                        default:    // Error
                                            // If the current square clicked on does
                                            this.Exit();
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
                                    switch (pieces[mouseY, mouseX])
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
                                            // Select the red piece
                                            selectedPiece = new Vector2(mouseX, mouseY);
                                            break;
                                        default:    // Error
                                            // If the current square clicked on does
                                            this.Exit();
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
                                    switch (pieces[mouseY, mouseX])
                                    {
                                        case 0:     // If blank space clicked
                                            // If PerformMove is false (invalid move)
                                            if (PerformMove(selectedPiece, mouseX, mouseY, playerTurn))
                                            {
                                                // Prepare for next player
                                                if(AnyMoves(1))
                                                    playerTurn = 1;
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
                                            // If the current square clicked on does
                                            this.Exit();
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
                            this.Exit();
                            break;
                        #endregion
                    }
                    #endregion
                    // Disable clicking until mouse is released
                    clickEnabled = false;
                }
                #endregion
            }
            #endregion
            #region If click is disabled
            else // If user has already clicked
            {
                //If left mouse is released enable click
                if (mouseState.LeftButton == ButtonState.Released)
                    // Enable clicking again
                    clickEnabled = true;
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Draw the board background
            spriteBatch.Begin();
            spriteBatch.Draw(boardTex, new Rectangle(0, 0, 700, 700), Color.White);
            spriteBatch.End();

            // Loop through the board and draw the pieces
            spriteBatch.Begin();
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (pieces[y, x] == 1)
                    {
                        spriteBatch.Draw(redTex, new Rectangle(x * 100, y * 100, 100, 100), Color.White);
                    }
                    if (pieces[y, x] == 2)
                    {
                        spriteBatch.Draw(greenTex, new Rectangle(x * 100, y * 100, 100, 100), Color.White);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected bool PerformMove(Vector2 selected, int targetX, int targetY, int player)
        {
            //if a valid move is found this will be set to true
            bool validMove = false;
            #region Handle Movement
            for (int moveNum = 1; moveNum < 3; moveNum++)
            {
                //If target is moveNum square NorthWest
                if ((targetX + moveNum) == selected.X && (targetY + moveNum) == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square North
                if (targetX == selected.X && (targetY + moveNum) == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square NorthEast
                if ((targetX - moveNum) == selected.X && (targetY + moveNum) == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square East
                if ((targetX - moveNum) == selected.X && targetY == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square SouthEast
                if ((targetX - moveNum) == selected.X && (targetY - moveNum) == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square South
                if (targetX == selected.X && (targetY - moveNum) == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square SouthWest
                if ((targetX + moveNum) == selected.X && (targetY - moveNum) == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
                //If target is moveNum square West
                if ((targetX + moveNum) == selected.X && targetY == selected.Y)
                {
                    pieces[targetY, targetX] = player;
                    if (moveNum == 2)
                        pieces[(int)Math.Floor(selected.Y), (int)Math.Floor(selected.X)] = 0;
                    validMove = true;
                }
            }
            #endregion

            //If the move was allowed to be made
            #region Do Mutations
            if (validMove)
            {
                pieces[targetY, targetX] = playerTurn;
                if (targetY > 0 && targetX > 0)
                {
                    if (pieces[targetY - 1, targetX - 1] > 0 && pieces[targetY - 1, targetX - 1] != playerTurn)
                        pieces[targetY - 1, targetX - 1] = playerTurn;
                }
                if (targetY > 0)
                {
                    if (pieces[targetY - 1, targetX] > 0 && pieces[targetY - 1, targetX] != playerTurn)
                        pieces[targetY - 1, targetX] = playerTurn;
                }
                if (targetY > 0 && targetX < 6)
                {
                    if (pieces[targetY - 1, targetX + 1] > 0 && pieces[targetY - 1, targetX + 1] != playerTurn)
                        pieces[targetY - 1, targetX + 1] = playerTurn;
                }
                if (targetX > 0)
                {
                    if (pieces[targetY, targetX - 1] > 0 && pieces[targetY, targetX - 1] != playerTurn)
                        pieces[targetY, targetX - 1] = playerTurn;
                }
                if (targetX < 6)
                {
                    if (pieces[targetY, targetX + 1] > 0 && pieces[targetY, targetX + 1] != playerTurn)
                        pieces[targetY, targetX + 1] = playerTurn;
                }
                if (targetY < 6 && targetX > 0)
                {
                    if (pieces[targetY + 1, targetX - 1] > 0 && pieces[targetY + 1, targetX - 1] != playerTurn)
                        pieces[targetY + 1, targetX - 1] = playerTurn;

                }
                if (targetY < 6)
                {
                    if (pieces[targetY + 1, targetX] > 0 && pieces[targetY + 1, targetX] != playerTurn)
                        pieces[targetY + 1, targetX] = playerTurn;
                }
                if (targetY < 6 && targetX < 6)
                {
                    if (pieces[targetY + 1, targetX + 1] > 0 && pieces[targetY + 1, targetX + 1] != playerTurn)
                        pieces[targetY + 1, targetX + 1] = playerTurn;
                }
            }
            #endregion

            // If the for loop did not find a valid move
            return validMove;
        }

        protected bool AnyMoves(int player)
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (pieces[y, x] == player)
                    {
                        for (int moveNum = 1; moveNum < 3; moveNum++)
                        {
                            // Northwest
                            if (y > 0 && x > 0)
                            {
                                if (pieces[(y - moveNum), (x - moveNum)] == 0)
                                    return true;
                            }
                            // North
                            if (y > (0 + (moveNum - 1)))
                            {
                                if (pieces[(y - moveNum), (x)] == 0)
                                    return true;
                            }
                            // Northeast
                            if (y < (7 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (pieces[(y + moveNum), (x + moveNum)] == 0)
                                    return true;
                            }
                            // East
                            if (x < (7 - (moveNum - 1)))
                            {
                                if (pieces[(y), (x + moveNum)] == 0)
                                    return true;
                            }
                            // Southeast
                            if (y < (7 - (moveNum - 1)) && x < (7 - (moveNum - 1)))
                            {
                                if (pieces[(y + moveNum), (x + moveNum)] == 0)
                                    return true;
                            }
                            // South
                            if (y < (7 - (moveNum - 1)))
                            {
                                if (pieces[(y + moveNum), (x)] == 0)
                                    return true;
                            }
                            // Southwest
                            if (y < (7 - (moveNum - 1)) && x > (0 + (moveNum - 1)))
                            {
                                if (pieces[(y + moveNum), (x - moveNum)] == 0)
                                    return true;
                            }
                            // West
                            if (x > (0 + (moveNum - 1)))
                            {
                                if (pieces[(y), (x - moveNum)] == 0)
                                    return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}

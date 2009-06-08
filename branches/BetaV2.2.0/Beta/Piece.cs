using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beta
{
    class Piece : Animation
    {
        // Derives all variables from Animation
        // Animation derives all variables from Image
        // Piece specific variables
        Texture2D player1Piece;
        Texture2D player2Piece;
        Texture2D player1TransformToPlayer2;
        Texture2D player2TransformToPlayer1;
        Texture2D transparentToPlayer1;
        Texture2D transparentToPlayer2;
        int state;


        private enum State
        {
            Blank = 0,
            Player1 = 1,
            Player2 = 2,
            Player1ToPlayer2 = 3,
            Player2ToPlayer1 = 4,
            TransToPlr1 = 5,
            TransToPlr2 = 6
        }

        // Constructor
        public Piece()
        {
            // Do nothing
        }

        // Initialize all required variables
        public void Initialize( Vector2 pos, Rectangle src, 
                                Vector2 origin, Vector2 scale,
                                Single depth)
        {
            // Initialize user defined variables
            this.position = pos;
            this.source = src;
            this.origin = origin;
            this.depth = depth;
            this.scale = scale;

            // Initialize constant variables
            //this.state = (int)State.Blank;
            this.fps = 25;
            if (Board.boardFull == true)
                CalculateDelay2();
            else
                CalculateDelay();
            this.loop = false;
            this.started = false;
            this.curFrame = 1;
            this.numFrame = 10;
            this.color = Color.White;
            this.rotation = 0;
            this.effects = SpriteEffects.None;
        }

        // Load all required graphics content
        public void LoadContent(SpriteBatch spriteBatch, Texture2D player1,
                                Texture2D player2, Texture2D player1Player2,
                                Texture2D player2Player1, Texture2D transparentPlayer1,
                                Texture2D transparentPlayer2)
        {
            this.spriteBatch = spriteBatch;
            this.player1Piece = player1;
            this.player2Piece = player2;
            this.player1TransformToPlayer2 = player1Player2;
            this.player2TransformToPlayer1 = player2Player1;
            this.transparentToPlayer1 = transparentPlayer1;
            this.transparentToPlayer2 = transparentPlayer2;
        }

        // Update the animations
        new public void Update(GameTime gameTime)
        {
            if (started)
            {
                time += Math.Max(1, gameTime.ElapsedGameTime.Milliseconds);
                if (time >= delay)
                {
                    curFrame++;
                    if (curFrame >= numFrame)
                    {
                        // Animation.Reset()
                        Reset();
                        // Set the state to the correct static state
                        if (state == (int)State.Player1ToPlayer2)
                        {
                            state = 2;
                        }
                        if (state == (int)State.Player2ToPlayer1)
                        {
                            state = 1;
                        }
                        if (state == (int)State.TransToPlr1)
                        {
                            state = 1;
                        }
                        if (state == (int)State.TransToPlr2)
                        {
                            state = 2;
                        }
                    }
                    CalculateSource();
                    time = 0;
                }
            }
        }

        // Called by the animation handler to start the animation
        // This is just an alias for Start()
        public void Animate()
        {
            // Simply call start()
            Start();
        }

        // Will set this to animate from the current state to toState when
        // the animation handler starts the animation
        public void Mutate(int toState)
        {
            // If toState is player1
            if (toState == (int)State.Player1)
            {
                state = (int)State.Player2ToPlayer1;
            }
            // If toState is player2
            if (toState == (int)State.Player2)
            {
                state = (int)State.Player1ToPlayer2;
            }
        }

        // Set the state directly with no animations
        public void SetState(int toState)
        {
            Reset();
            CalculateSource();
            this.state = toState;
        }

        // Return the value of the current state
        public int Value()
        {
            return state;
        }
        // Draw to the screen
        new public void Draw()
        {
            switch(state)
            {
                case 0:
                    // Dont draw anything (blank)
                    break;
                case 1:
                    spriteBatch.Begin();
                    spriteBatch.Draw(   this.player1Piece, this.position, this.source,
                                        this.color, this.rotation, this.origin,
                                        this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 2:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.player2Piece, this.position, this.source,
                                     this.color, this.rotation, this.origin,
                                     this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 3:
                    spriteBatch.Begin();
                    spriteBatch.Draw(   this.player1TransformToPlayer2, this.position, this.source,
                                        this.color, this.rotation, this.origin,
                                        this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 4:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.player2TransformToPlayer1, this.position, this.source,
                                     this.color, this.rotation, this.origin,
                                     this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 5:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.transparentToPlayer1, this.position, this.source,
                                     this.color, this.rotation, this.origin, 
                                     this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 6:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.transparentToPlayer2, this.position, this.source, 
                                     this.color, this.rotation, this.origin,
                                     this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
            }
        }
    }
}

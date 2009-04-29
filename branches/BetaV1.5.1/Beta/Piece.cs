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
        Texture2D redTex;
        Texture2D greenTex;
        Texture2D redToGreen;
        Texture2D greenToRed;
        Texture2D transToPlr1;
        Texture2D transToPlr2;
        int state;


        private enum State
        {
            Blank = 0,
            Red = 1,
            Green = 2,
            RedToGreen = 3,
            GreenToRed = 4,
            TransToPlr1 = 5,
            TransToPlr2 = 6
        }

        // Constructor
        public Piece()
        {
            // Do nothing
        }

        // Initialize all required variables
        public void Initialize(Vector2 pos, Rectangle src, Vector2 origin, Vector2 scale, Single depth)
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
        public void LoadContent(SpriteBatch spriteBatch, Texture2D red, Texture2D green, Texture2D redGreen, Texture2D greenRed, Texture2D tPlr1, Texture2D tPlr2)
        {
            this.spriteBatch = spriteBatch;
            this.redTex = red;
            this.greenTex = green;
            this.redToGreen = redGreen;
            this.greenToRed = greenRed;
            this.transToPlr1 = tPlr1;
            this.transToPlr2 = tPlr2;
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
                        if (state == (int)State.RedToGreen)
                        {
                            state = 2;
                        }
                        if (state == (int)State.GreenToRed)
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
            // If toState is red
            if (toState == (int)State.Red)
            {
                state = (int)State.GreenToRed;
            }
            // If toState is green
            if (toState == (int)State.Green)
            {
                state = (int)State.RedToGreen;
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
                    spriteBatch.Draw(this.redTex, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 2:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.greenTex, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 3:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.redToGreen, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 4:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.greenToRed, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 5:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.transToPlr1, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
                case 6:
                    spriteBatch.Begin();
                    spriteBatch.Draw(this.transToPlr2, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
                    spriteBatch.End();
                    break;
            }
        }
    }
}

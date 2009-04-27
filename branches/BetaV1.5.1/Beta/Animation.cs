﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Beta
{
    class Animation : Image
    {
        // Derives all variables from Image
        // Animation specific variables
        protected float fps;      // Frames per second of the animation
        protected float delay;    // Delay in milliseconds per frame of the animation
        protected bool loop;    // To loop or not to loop
        protected bool started; // Is the animation started
        protected int curFrame; // What frame number it is on
        protected int numFrame; // How many frames in the animation
        protected int time;     // Time elapsed

        // Constructor
        public Animation()
        {
            // Do nothing
        }

        // Initialize all variables
        public void Initialize(Vector2 pos, Rectangle src, Vector2 origin, Vector2 scale, Single depth, float fps, int numFrame, bool loop)
        {
            // Initialize user defined variables
            this.position = pos;
            this.source = src;
            this.origin = origin;
            this.scale = scale;
            this.depth = depth;
            this.fps = fps;
            this.numFrame = numFrame;
            this.loop = loop;

            // initialize other variables
            this.time = 0;
            this.curFrame = 1;
            this.color = Color.White;
            this.rotation = 0;
            this.effects = SpriteEffects.None;
            this.started = true;
            CalculateSource();
            CalculateDelay();

        }

        // Update the animation with the time
        public void Update(GameTime gameTime)
        {
            if (started)
            {
                time += Math.Max(1,gameTime.ElapsedRealTime.Milliseconds);
                if (time >= delay)
                {
                    curFrame++;
                    if (curFrame > numFrame && loop == true)
                    {
                        curFrame = 1;
                    }
                    else if (curFrame >= numFrame && loop == false)
                    {
                        curFrame = numFrame;
                        Pause();
                    }
                    CalculateSource();
                    time = 0;
                }
            }
        }

        // Start the animation
        public void Start()
        {
            started = true;
        }

        // Pause the animation
        public void Pause()
        {
            started = false;
        }

        // Reset the animation
        public void Reset()
        {
            started = false;
            time = 0;
            curFrame = 1;
            CalculateSource();
        }

        // Change animation to the last frame
        public void LastFrame()
        {
            started = false;
            time = 0;
            curFrame = numFrame;
            CalculateSource();
        }

        // Calculate what the delay is by the fps specified
        protected void CalculateDelay()
        {
            delay = 1000 / fps;
        }

        // Calculate what the delay is by the fps specified
        protected void CalculateDelay2()
        {
            delay = 300;
        }

        // Calculat which source rectangle to draw
        protected void CalculateSource()
        {
            source.X = (source.Width * curFrame) - source.Width;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beta
{
    class Image
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle source;
        protected Color color;
        protected Single rotation;
        protected Vector2 origin;
        protected Vector2 scale;
        protected SpriteEffects effects;
        protected Single depth;
        

        // Constructor
        public Image()
        {
            
            // Do nothing
        }

        // Initialization
        public void Initialize(Vector2 pos, Rectangle src, Color color, Vector2 origin, Vector2 scale, Single depth)
        {
            // Initialize user inputed variables
            this.position = pos;
            this.source = src;
            this.color = color;
            this.origin = origin;
            this.scale = scale;
            this.depth = depth;

            // Initialize constant variables
            this.rotation = 0;
            this.effects = SpriteEffects.None;

        }

       public Matrix SpriteScale;

       public void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            float screenscale = 1 / 2f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
        }

        // Load all content used
        public void LoadContent(SpriteBatch spriteBatch, Texture2D texture)
        {
            // Load user inputed content
            this.spriteBatch = spriteBatch;
            this.texture = texture;

            // Default resolution is 800x600; scale sprites up or down based on
            // current viewport
            float screenscale = 1/2f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
            /// UnloadContent will be called once per game and is the place to unload
            /// all content.
        }

        // Draw to the screen
        public void Draw()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.None, SpriteScale);
            spriteBatch.Draw(this.texture, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.depth);
            spriteBatch.End();
        }
    }
}

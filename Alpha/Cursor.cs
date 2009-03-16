using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Alpha
{
    class Cursor : Image
    {
        // Derives all variables from Image
        // Cursor specific variables
        protected MouseState mouseState;

        // Update the mouse position
        public void Update()
        {
            mouseState = Mouse.GetState();
            this.position = new Vector2(mouseState.X, mouseState.Y);
        }
    }
}

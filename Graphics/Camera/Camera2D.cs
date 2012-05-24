using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Omega
{
    public class Camera2D
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 ScreenDimensions;
        public Matrix Transform
        { 
            get { return Matrix.CreateTranslation(new Vector3(Position * -1 + ScreenDimensions / 2, 0.0f)); } 
        }

        public Camera2D(Vector2 screenDimensions)
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            ScreenDimensions = screenDimensions;
        }

        public void Update()
        {
            
        }
    }
}

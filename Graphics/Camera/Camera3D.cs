using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Omega
{
    public class Camera3D
    {
        public Vector3 Position = Vector3.Backward;
        public Vector3 Target = Vector3.Zero;
        public Vector3 Up = Vector3.Up;
        public Rectangle ScreenDimensions;

        public Matrix World = Matrix.Identity;
        
        public Matrix View 
        { 
            get { return Matrix.CreateLookAt(Position, Target, Up); }
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)ScreenDimensions.Width / (float)ScreenDimensions.Height, 1.0f, 1000.0f); }
        }

        public Camera3D(Rectangle screenDimensions)
        {
            ScreenDimensions = screenDimensions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public abstract class Actor
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public float Rotation;
        public float Radius;

        public abstract void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager);
        public abstract void Update();
        public abstract void Draw(Camera3D camera);
    }
}

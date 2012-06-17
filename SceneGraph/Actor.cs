using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public abstract class Actor
    {
        public bool Alive;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public float Rotation;
        public float Radius;

        public SpriteBatch SpriteBatch;
        public LineBatch3D LineBatch;
        public PointBatch3D PointBatch;
        public PrimitiveBatch2D PrimitiveBatch;
        public SpriteFont SpriteFont;

        public Actor Parent;
        public List<Actor> Children;

        public Actor(Actor parent)
        {
            Parent = parent;

            Children = new List<Actor>();
        }

        public void AddChild(Actor child)
        {
            Children.Add(child);
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (Actor a in Children)
            {
                a.SpriteBatch = SpriteBatch;
                a.LineBatch = LineBatch;
                a.PointBatch = PointBatch;
                a.PrimitiveBatch = PrimitiveBatch;
                a.SpriteFont = SpriteFont;

                a.LoadContent(contentManager);
            }
        }

        public virtual void Update()
        {
            foreach (Actor a in Children)
                a.Update();
        }

        public virtual void Draw(Camera3D camera)
        {
            foreach (Actor a in Children)
                a.Draw(camera);
        }
    }
}

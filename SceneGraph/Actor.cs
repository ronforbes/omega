using System.Collections.Generic;
using System.Runtime.Serialization;
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
        public Scene Scene;
        public Actor Parent;
        public List<Actor> Children;

        public Actor()
        {
            Children = new List<Actor>();
        }

        public void AddChild(Actor child)
        {
            child.Parent = this;
            child.Scene = Scene;
            Children.Add(child);
        }

        public void RemoveChild(Actor child)
        {
            Children.Remove(child);
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            SpriteBatch = Scene.SpriteBatch;
            LineBatch = Scene.LineBatch;
            PointBatch = Scene.PointBatch;
            PrimitiveBatch = Scene.PrimitiveBatch;
            SpriteFont = Scene.SpriteFont;

            foreach (Actor a in Children)
            {
                a.LoadContent(contentManager);
            }
        }

        public virtual void Update(GameTimerEventArgs e)
        {
            foreach (Actor a in Children)
                a.Update(e);
        }

        public virtual void Draw(Camera3D camera)
        {
            foreach (Actor a in Children)
                a.Draw(camera);
        }
    }
}

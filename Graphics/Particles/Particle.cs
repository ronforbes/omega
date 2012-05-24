using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public class Particle
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Color Color;
        public bool Active;
        public int Life;

        public Particle()
        {
            Active = false;
        }

        public void Die()
        {
            Active = false;
        }

        public void Update()
        {
            Velocity *= 0.9f;
            Position += Velocity;
            Color = new Color(Color.R * 0.8f, Color.G * 0.8f, Color.B * 0.8f, Color.A * 0.8f);

            Life--;
            if (Life <= 0)
                Die();
        }

        public void Draw(LineBatch3D lineBatch)
        {
            lineBatch.Draw(Position, Position - Velocity * 5);
        }
    }
}

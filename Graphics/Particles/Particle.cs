using Microsoft.Xna.Framework;

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

        public void Update(GameTimerEventArgs e)
        {
            if (Active)
            {
                Velocity *= 0.9f;
                Position += Velocity;
                Color = new Color(Color.R - 3, Color.G - 3, Color.B - 3, Color.A - 1);

                Life--;
                if (Life <= 0)
                    Die();
            }
        }

        public void Draw(LineBatch3D lineBatch, PointBatch3D pointBatch)
        {
            if (Active)
            {
                Vector3 delta = Velocity;
                delta.Normalize();
                lineBatch.Draw(Position, Position - delta * 15.0f, 5.0f, Color);
                pointBatch.Draw(Position - delta * 7.5f, 15.0f, new Color(Color.R / 2, Color.G / 2, Color.B / 2, Color.A / 2));
            }
        }
    }
}

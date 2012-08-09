using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Omega
{
    public class ParticleManager : Actor
    {
        List<Particle> particles;
        
        static Random random = new Random();

        public ParticleManager(int particleCapacity) : base()
        {
            particles = new List<Particle>(particleCapacity);
        }

        public void CreateParticles()
        {
            for (int i = 0; i < particles.Capacity; i++)
            {
                particles.Add(new Particle());
            }
        }

        public void CreateParticleEffect(int particleCount, Vector3 position, int radius, Color color)
        {
            for(int i = 0; i < particles.Capacity; i++)
            {
                if(!particles[i].Active)
                {
                    particles[i].Position = position;
                    float angle = MathHelper.ToRadians(random.Next(0, 360));
                    int speed = random.Next(radius);
                    particles[i].Velocity = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0.0f) * speed;
                    int intensity = random.Next(128);
                    particles[i].Color = new Color(color.R - intensity, color.G - intensity, color.B - intensity, color.A - intensity);
                    particles[i].Active = true;
                    particles[i].Life = 100;

                    particleCount--;
                    if(particleCount == 0)
                        return;
                }
            }
        }

        public override void Update(GameTimerEventArgs e)
        {
            foreach (Particle p in particles)
            {
                if (p.Active)
                    p.Update(e);
            }
        }

        public override void Draw(Camera3D camera)
        {
            Matrix translation = Matrix.CreateTranslation(0.0f, 0.0f, -500.0f);

            LineBatch.Begin(translation, camera);
            PointBatch.Begin(translation, camera);

            foreach (Particle p in particles)
            {
                if (p.Active)
                    p.Draw(LineBatch, PointBatch);
            }

            PointBatch.End();
            LineBatch.End();
        }
    }
}

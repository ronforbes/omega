using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public class ParticleManager : Actor
    {
        List<Particle> particles;
        LineBatch3D lineBatch;

        const int particleCapacity = 10000;
        
        static Random random = new Random();

        public ParticleManager()
        {
            particles = new List<Particle>(particleCapacity);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            for (int i = 0; i < particleCapacity; i++)
            {
                particles.Add(new Particle());
            }

            lineBatch = new LineBatch3D(graphicsDevice, contentManager);
        }

        public void CreateParticleEffect(int particleCount, Vector3 position, int radius, Color color)
        {
            for(int i = 0; i < particleCapacity; i++)
            {
                if(!particles[i].Active)
                {
                    particles[i].Position = position;
                    float angle = MathHelper.ToRadians(random.Next(0, 360));
                    int speed = random.Next(radius);
                    particles[i].Velocity = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0.0f) * speed;
                    float intensity = (float)random.NextDouble();
                    particles[i].Color = new Color(color.R * intensity, color.G * intensity, color.B * intensity, color.A * intensity);
                    particles[i].Active = true;
                    particles[i].Life = 100;

                    particleCount--;
                    if(particleCount == 0)
                        return;
                }
            }
        }

        public override void Update()
        {
            foreach (Particle p in particles)
            {
                if (p.Active)
                    p.Update();
            }
        }

        public override void Draw(Camera3D camera)
        {
            Matrix translation = Matrix.CreateTranslation(0.0f, 0.0f, -500.0f);

            lineBatch.Begin(translation, camera, new Vector3(0.8f, 0.8f, 0.8f));

            foreach (Particle p in particles)
            {
                if (p.Active)
                    p.Draw(lineBatch);
            }

            lineBatch.End();
        }
    }
}

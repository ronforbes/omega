using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public class PrimitiveBatch2D
    {
        SpriteBatch spriteBatch;
        Texture2D texture;

        public PrimitiveBatch2D(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
        }

        public void Begin(BlendState blendState, Camera2D camera, Matrix transform)
        {
            Matrix cameraTransform;
            if (camera != null)
                cameraTransform = camera.Transform;
            else
                cameraTransform = Matrix.Identity;

            spriteBatch.Begin(SpriteSortMode.Deferred, blendState, null, null, null, null, Matrix.Multiply(transform, cameraTransform));
        }

        public void DrawPoint(Vector2 position, int radius, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X - radius / 2, (int)position.Y - radius / 2, radius * 2, radius * 2), color);
        }

        public void DrawLine(Vector2 startPosition, Vector2 endPosition, int width, Color color)
        {
            Vector2 delta = endPosition - startPosition;
            float rotation = (float)Math.Atan2(delta.Y, delta.X);

            spriteBatch.Draw(texture, startPosition, null, color, rotation, Vector2.Zero, new Vector2(delta.Length() / texture.Width, width / texture.Height), SpriteEffects.None, 0);
        }

        public void End()
        {
            spriteBatch.End();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public class MotionBlur : Postprocess
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;

        RenderTarget2D downsample;
        RenderTarget2D intermediate;

        const float blurIntensity = 0.25f;

        public MotionBlur(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            PresentationParameters pp = graphicsDevice.PresentationParameters;

            downsample = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth / 2, pp.BackBufferHeight / 2, false, pp.BackBufferFormat, DepthFormat.None);
            intermediate = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public override void Begin(RenderTarget2D scene)
        {
            graphicsDevice.SetRenderTarget(intermediate);
            graphicsDevice.Clear(Color.Black);

            DrawSprite(scene, BlendState.Opaque, Color.White);
            DrawSprite(downsample, BlendState.Additive, new Color(1.0f, 1.0f, 1.0f, blurIntensity));

            graphicsDevice.SetRenderTarget(downsample);
            graphicsDevice.Clear(Color.Black);

            DrawSprite(intermediate, BlendState.Opaque, Color.White);
        }

        void DrawSprite(Texture2D source, BlendState blendState, Color color)
        {
            spriteBatch.Begin(0, blendState);
            spriteBatch.Draw(source, graphicsDevice.Viewport.Bounds, color);
            spriteBatch.End();
        }

        public override void End(RenderTarget2D scene)
        {
            DrawSprite(intermediate, BlendState.Additive, Color.White);
        }
    }
}

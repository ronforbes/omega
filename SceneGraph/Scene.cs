using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public class Scene
    {
        GraphicsDevice graphicsDevice;
        LineBatch3D lineBatch;
        PointBatch3D pointBatch;
        SpriteBatch spriteBatch;
        PrimitiveBatch2D primitiveBatch;
        SpriteFont spriteFont;
        RenderTarget2D renderTarget;

        public Color ClearColor;
        public Camera3D Camera;

        List<Actor> actors;
        List<Postprocess> postprocesses;

        public Scene(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            ClearColor = Color.Black;

            Camera = new Camera3D(new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));

            actors = new List<Actor>();
            postprocesses = new List<Postprocess>();

            PresentationParameters pp = graphicsDevice.PresentationParameters;

            // Setup scene render target
            renderTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public void AddActor(Actor actor)
        {
            actors.Add(actor);
        }

        public void AddPostprocess(Postprocess postprocess)
        {
            postprocesses.Add(postprocess);
        }

        public void LoadContent(ContentManager contentManager)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            lineBatch = new LineBatch3D(graphicsDevice, contentManager);
            pointBatch = new PointBatch3D(graphicsDevice, contentManager);
            primitiveBatch = new PrimitiveBatch2D(graphicsDevice);
            spriteFont = contentManager.Load<SpriteFont>("SpriteFont");

            foreach (Actor a in actors)
            {
                a.SpriteBatch = spriteBatch;
                a.LineBatch = lineBatch;
                a.PointBatch = pointBatch;
                a.PrimitiveBatch = primitiveBatch;
                a.SpriteFont = spriteFont;

                a.LoadContent(contentManager);
            }
        }

        public void Update()
        {
            foreach (Actor actor in actors)
                actor.Update();

            Camera.Update();
        }

        public void Draw()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(ClearOptions.Target, ClearColor, 0, 0);

            foreach (Actor actor in actors)
                actor.Draw(Camera);

            foreach (Postprocess pp in postprocesses)
                pp.Begin(renderTarget);

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(ClearOptions.Target, ClearColor, 0, 0);

            foreach (Postprocess pp in postprocesses)
                pp.End(renderTarget);
        }
    }
}

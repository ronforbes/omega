using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public class Scene
    {
        GraphicsDevice graphicsDevice;
        
        public LineBatch3D LineBatch;
        public PointBatch3D PointBatch;
        public SpriteBatch SpriteBatch;
        public PrimitiveBatch2D PrimitiveBatch;
        public SpriteFont SpriteFont;
        public ContentManager ContentManager;
        public Color ClearColor;
        public Camera3D Camera;
        
        List<Actor> actors;
        List<Postprocess> postprocesses;
        RenderTarget2D renderTarget;

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
            actor.Scene = this;
            actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
        }

        public void AddPostprocess(Postprocess postprocess)
        {
            postprocesses.Add(postprocess);
        }

        public void LoadContent(ContentManager contentManager)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            LineBatch = new LineBatch3D(graphicsDevice, contentManager);
            PointBatch = new PointBatch3D(graphicsDevice, contentManager);
            PrimitiveBatch = new PrimitiveBatch2D(graphicsDevice);
            SpriteFont = contentManager.Load<SpriteFont>("SpriteFont");
            ContentManager = contentManager;

            foreach (Actor a in actors)
            {
                a.LoadContent(contentManager);
            }
        }

        public virtual void Update(GameTimerEventArgs e)
        {
            foreach (Actor actor in actors)
                actor.Update(e);

            Camera.Update(e);
        }

        public virtual void Draw()
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

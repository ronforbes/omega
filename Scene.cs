using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Omega
{
    public class Scene
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
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

            PresentationParameters pp = graphicsDevice.PresentationParameters;

            // Setup scene render target
            renderTarget = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            foreach (Actor actor in actors)
                actor.LoadContent(graphicsDevice, contentManager);
        }

        public void Update()
        {
            foreach (Actor actor in actors)
                actor.Update();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Omega
{
    public abstract class Postprocess
    {
        public abstract void Begin(RenderTarget2D scene);
        public abstract void End(RenderTarget2D scene);
    }
}

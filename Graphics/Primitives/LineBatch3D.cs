#region File Description
//-----------------------------------------------------------------------------
// LineBatch.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Omega
{
    // LineBatch is a class that handles efficient rendering automatically for its
    // users, in a similar way to SpriteBatch.
    public class LineBatch3D : IDisposable
    {
        #region Constants and Fields

        // this constant controls how large the vertices buffer is. Larger buffers will
        // require flushing less often, which can increase performance. However, having
        // buffer that is unnecessarily large will waste memory.
        const int VertexBufferSize = 400;
        const int IndexBufferSize = 600;

        const float lineWidth = 5.0f;

        // a block of vertices that calling Add will fill. Flush will draw using
        // this array, and will determine how many primitives to draw from
        // positionInBuffer.
        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[VertexBufferSize];

        short[] indices = new short[IndexBufferSize];

        // keeps track of how many vertices have been added. this value increases until
        // we run out of space in the buffer, at which time Flush is automatically
        // called.
        int vertexBufferPosition = 0;

        int indexBufferPosition = 0;

        // a basic effect, which contains the shaders that we will use to draw our
        // lines.
        BasicEffect basicEffect;

        // a texture that will be drawn on the lines
        Texture2D texture;

        // the device that we will issue draw calls to.
        GraphicsDevice graphicsDevice;

        // a line is composed of 6 vertices specified as a triangle list.
        const int verticesPerLine = 4;
        const int indicesPerLine = 6;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        bool hasBegun = false;

        bool isDisposed = false;

        #endregion

        // the constructor creates a new LineBatch and sets up all of the internals
        // that LineBatch will need.
        public LineBatch3D(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            this.graphicsDevice = graphicsDevice;

            // set up a texture
            //texture = new Texture2D(graphicsDevice, 1, 1);
            //texture.SetData(new Color[] { Color.White });
            texture = contentManager.Load<Texture2D>("LineTexture");

            // set up a new basic effect, and enable lighting and texture mapping.
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.EnableDefaultLighting();
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = texture;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                if (basicEffect != null)
                    basicEffect.Dispose();

                isDisposed = true;
            }
        }

        // Begin is called to tell the LineBatch what camera to draw from, 
        // and to prepare the graphics card to render those primitives.
        public void Begin(Matrix world, Camera3D camera, Vector3 color)
        {
            if (hasBegun)
            {
                throw new InvalidOperationException
                    ("End must be called before Begin can be called again.");
            }

            basicEffect.World = world;
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;

            basicEffect.DiffuseColor = color;
            basicEffect.EmissiveColor = color;
            basicEffect.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f);
            //basicEffect.SpecularPower = 1.0f;

            //tell our basic effect to begin.
            basicEffect.CurrentTechnique.Passes[0].Apply();

            // flip the error checking boolean. It's now ok to call Add, Flush,
            // and End.
            hasBegun = true;
        }

        // Draw is called to add another line to be rendered.
        // this function can only be called once begin has been called.
        // if there is not enough room in the vertices buffer, Flush is called
        // automatically.
        public void Draw(Vector3 startPosition, Vector3 endPosition)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before Add can be called.");
            }

            // if there's no more room to add new vertices, flush the buffer first
            if (vertexBufferPosition + verticesPerLine >= vertices.Length || indexBufferPosition + indicesPerLine >= indices.Length)
                Flush();

            // once we know there's enough room, set the vertex in the buffer,
            // and increase position.
            Vector3 delta = endPosition - startPosition;
            float angle = (float)Math.Atan2(delta.Y, delta.X);

            vertices[vertexBufferPosition + 0].Position = new Vector3(startPosition.X + (float)Math.Cos(angle + MathHelper.PiOver2) * lineWidth / 2, 
                                                                  startPosition.Y + (float)Math.Sin(angle + MathHelper.PiOver2) * lineWidth / 2, 
                                                                  startPosition.Z);
            vertices[vertexBufferPosition + 0].Normal = Vector3.Backward;
            vertices[vertexBufferPosition + 0].TextureCoordinate = Vector2.Zero;

            vertices[vertexBufferPosition + 1].Position = new Vector3(startPosition.X - (float)Math.Cos(angle + MathHelper.PiOver2) * lineWidth / 2,
                                                                  startPosition.Y - (float)Math.Sin(angle + MathHelper.PiOver2) * lineWidth / 2,
                                                                  startPosition.Z);
            vertices[vertexBufferPosition + 1].Normal = Vector3.Backward;
            vertices[vertexBufferPosition + 1].TextureCoordinate = Vector2.UnitY;

            vertices[vertexBufferPosition + 2].Position = new Vector3(endPosition.X + (float)Math.Cos(angle + MathHelper.PiOver2) * lineWidth / 2,
                                                                  endPosition.Y + (float)Math.Sin(angle + MathHelper.PiOver2) * lineWidth / 2,
                                                                  endPosition.Z);
            vertices[vertexBufferPosition + 2].Normal = Vector3.Backward;
            vertices[vertexBufferPosition + 2].TextureCoordinate = Vector2.UnitX;

            vertices[vertexBufferPosition + 3].Position = new Vector3(endPosition.X - (float)Math.Cos(angle + MathHelper.PiOver2) * lineWidth / 2,
                                                                  endPosition.Y - (float)Math.Sin(angle + MathHelper.PiOver2) * lineWidth / 2,
                                                                  endPosition.Z);
            vertices[vertexBufferPosition + 3].Normal = Vector3.Backward;
            vertices[vertexBufferPosition + 3].TextureCoordinate = Vector2.One;

            indices[indexBufferPosition + 0] = (short)(vertexBufferPosition + 0);
            indices[indexBufferPosition + 1] = (short)(vertexBufferPosition + 2);
            indices[indexBufferPosition + 2] = (short)(vertexBufferPosition + 1);
            indices[indexBufferPosition + 3] = (short)(vertexBufferPosition + 2);
            indices[indexBufferPosition + 4] = (short)(vertexBufferPosition + 3);
            indices[indexBufferPosition + 5] = (short)(vertexBufferPosition + 1);

            vertexBufferPosition += verticesPerLine;
            indexBufferPosition += indicesPerLine;
        }

        // End is called once all the lines have been drawn using Add.
        // it will call Flush to actually submit the draw call to the graphics card, and
        // then tell the basic effect to end.
        public void End()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before End can be called.");
            }

            // Draw whatever the user wanted us to draw
            Flush();

            hasBegun = false;
        }

        // Flush is called to issue the draw call to the graphics card. Once the draw
        // call is made, positionInBuffer is reset, so that Add can start over
        // at the beginning. End will call this to draw the primitives that the user
        // requested, and Add will call this if there is not enough room in the
        // buffer.
        private void Flush()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before Flush can be called.");
            }

            // no work to do
            if (vertexBufferPosition == 0 || indexBufferPosition == 0)
                return;

            // how many primitives will we draw?
            int primitiveCount = 2 * vertexBufferPosition / verticesPerLine;

            // submit the draw call to the graphics card
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, vertexBufferPosition, indices, 0, primitiveCount);

            // now that we've drawn, it's ok to reset positionInBuffer back to zero,
            // and write over any vertices that may have been set previously.
            vertexBufferPosition = 0;
            indexBufferPosition = 0;
        }
    }
}

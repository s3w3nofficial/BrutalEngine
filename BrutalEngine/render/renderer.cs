using System;
using System.Collections.Generic;
using System.Text;
using BrutalEngine.model;
using OpenTK.Graphics.OpenGL;

namespace BrutalEngine.render
{
    class Renderer
    {
        public void prepare()
        {
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthClamp);
        }

        public void render(RawModel model, int textureID)
        {
            GL.BindVertexArray(model.VaoId);
            GL.EnableVertexAttribArray(0);

            GL.EnableVertexAttribArray(1);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.DrawElements(PrimitiveType.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(model.VaoId);
        }
    }
}

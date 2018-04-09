using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using BrutalEngine.model;
using Bitmap = System.Drawing.Bitmap;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;

namespace BrutalEngine.loader
{
    public class Loader
    {
        private List<int> _vaos = new List<int>();
        private List<int> _vbos = new List<int>();

        private RawModel _model;

        public RawModel LoadToVao(float[] positions, int[] indicies, float[] uvs)
        {
            int vaoId = CreateVao();
            BindIndiciesBuffer(indicies);
            StoreDataInAttributeArray(0, positions, 3);
            StoreDataInAttributeArray(1, uvs, 2);
            UnbindVao();
            _model = new RawModel(vaoId, indicies.Length, uvs.Length / 2);
            return _model;
        }

        public void CleanUp()
        {
            foreach (int vao in _vaos)
            {
                GL.DeleteVertexArray(vao);
            }

            foreach (int vbo in _vbos)
            {
                GL.DeleteBuffer(vbo);
            }
        }

        private int CreateVao()
        {
            int vaoId = GL.GenVertexArray();
            _vaos.Add(vaoId);
            GL.BindVertexArray(vaoId);
            return vaoId;
        }

        private void StoreDataInAttributeArray(int attributeNumber, float[] data, int coordSize)
        {
            int vboId = GL.GenBuffer();
            _vbos.Add(vboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(attributeNumber, coordSize, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void BindIndiciesBuffer(int[] indicies)
        {
            int vboId = GL.GenBuffer();
            _vbos.Add(vboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indicies.Length, indicies, BufferUsageHint.DynamicDraw);
        }

        private void UnbindVao()
        {
            GL.BindVertexArray(0);
        }

        public int LoadTexture(string file)
        {
            var img = (Bitmap)Bitmap.FromFile(file);

            using (img)
            {
                var rect = new Rectangle(new Point(), img.Size);
                var data = img.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

                int texId = GL.GenTexture(); //generate a new texture

                GL.BindTexture(TextureTarget.Texture2D, texId); // bind the texture (to start working withthe texture)

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    img.Size.Width,
                    img.Size.Height,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);

                img.UnlockBits(data);

                //set filters - this is important
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                return texId;
            }
        }

    }
}

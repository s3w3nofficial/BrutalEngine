using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalEngine.model
{
    public class RawModel
    {
        public int VaoId { get; set; }
        public int VertexCount { get; set; }
        public int UvsCount { get; set; }

        public RawModel(int vaoId, int vertexCount, int uvsCount)
        {
            VaoId = vaoId;
            VertexCount = vertexCount;
            UvsCount = uvsCount;
        }
    }
}

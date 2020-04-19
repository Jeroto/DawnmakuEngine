using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;

namespace DawnmakuEngine.Data
{
    class Mesh
    {
        public float[] vertices;
        protected int vertexArrayHandle;
        protected int vertexBufferHandle;

        public int VertexArrayHandle { get { return vertexArrayHandle; } }
        public int VertexBufferHandle { get { return vertexBufferHandle; } }

        public enum Primitives { SqrPlane, TallPlane, Cube, Pyramid }

        public void Use()
        {
            GL.BindVertexArray(VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
        }

        public void SetUp(BufferUsageHint bufferUsage)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, bufferUsage);
        }

        public Mesh()
        {
            vertexArrayHandle = GL.GenVertexArray();
            vertexBufferHandle = GL.GenBuffer();
        }

        public Mesh(Primitives primitiveBase) : this()
        {
            vertices = GetPrimitiveVertices(primitiveBase);
        }

        public Mesh(float[] vertices_) : this()
        {
            vertices = vertices_;
        }

        public void SetUVs(float[] uvs)
        {
            int count = uvs.Length;
            for (int i = 0; i < count; i += 2)
            {
                vertices[(i / 2) * 5 + 3] = uvs[i];
                vertices[(i / 2) * 5 + 4] = uvs[i + 1];
            }
        }

        public static float[] GetPrimitiveVertices(Primitives primitiveType)
        {
            switch (primitiveType)
            {
                case Primitives.SqrPlane:
                    return new float[] {
                        -.5f, 0.5f, 0, 0, 1,
                        0.5f, 0.5f, 0, 1, 1,
                        0.5f, -.5f, 0, 1, 0,

                        -.5f, 0.5f, 0, 0, 1,
                        0.5f, -.5f, 0, 1, 0,
                        -.5f, -.5f, 0, 0, 0,
                    };

                case Primitives.TallPlane:
                    return new float[] {
                        -.25f, 0.5f, 0, 0, 1,
                        0.25f, 0.5f, 0, 1, 1,
                        0.25f, -.5f, 0, 1, 0,

                        -.25f, 0.5f, 0, 0, 1,
                        0.25f, -.5f, 0, 1, 0,
                        -.25f, -.5f, 0, 0, 0,
                    };


                case Primitives.Cube:
                    return new float[] {                     //Tex Coords
                        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                         0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

                        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

                        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                         0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                         0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
                         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

                        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
                        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f};
                case Primitives.Pyramid:
                    return new float[] {                     //Tex Coords
                        0f,    0.5f,  0.0f,  0.5f, 1f,
                       -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,

                        0f,    0.5f,  0.0f,  0.5f, 1f,
                       -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                       -0.5f, -0.5f,  0.5f,  1.0f, 0.0f,

                        0f,    0.5f,  0.0f,  0.5f, 1f,
                        0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,

                        0f,    0.5f,  0.0f,  0.5f, 1f,
                       -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                    };

                default:
                    return null;
            }
        }

    }
}

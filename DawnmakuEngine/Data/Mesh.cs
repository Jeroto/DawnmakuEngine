using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace DawnmakuEngine.Data
{
    public class Mesh
    {
        public float[] vertices;
        public uint[] triangleData;
        protected int vertexArrayHandle, vertexBufferHandle, elementBufferHandle;

        public int VertexArrayHandle { get { return vertexArrayHandle; } }
        public int VertexBufferHandle { get { return vertexBufferHandle; } }

        public enum Primitives { SqrPlaneWTriangles, SqrPlane, TallPlane, Cube, Pyramid }

        public void Use()
        {
            GL.BindVertexArray(VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
        }

        public void SetUp(BufferUsageHint bufferUsage)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, bufferUsage);
            if(triangleData != null)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
                GL.BufferData(BufferTarget.ElementArrayBuffer, triangleData.Length * sizeof(uint), triangleData, bufferUsage);
            }
        }

        public Vector3 GetVertex(int index)
        {
            int startingVert = index * 5;
            return new Vector3(vertices[startingVert], vertices[startingVert + 1], vertices[startingVert + 2]);
        }
        public void SetVertex(int index, Vector3 newVert)
        {
            int startingVert = index * 5;
            vertices[startingVert] = newVert.X;
            vertices[startingVert + 1] = newVert.Y;
            vertices[startingVert + 2] = newVert.Z;
        }
        public Vector2 GetUV(int index)
        {
            int startingVert = index * 5 + 3;
            return new Vector2(vertices[startingVert], vertices[startingVert + 1]);
        }
        public void SetUV(int index, Vector2 newUV)
        {
            int startingVert = index * 5 + 3;
            vertices[startingVert] = newUV.X;
            vertices[startingVert + 1] = newUV.Y;
        }

        public Mesh()
        {
            vertexArrayHandle = GL.GenVertexArray();
            vertexBufferHandle = GL.GenBuffer();
            elementBufferHandle = GL.GenBuffer();
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

        public static Mesh CreatePrimitiveMesh(Primitives primitiveType)
        {
            Mesh created = new Mesh(primitiveType);
            if (primitiveType == Primitives.SqrPlaneWTriangles)
                created.triangleData = new uint[] {0, 1, 2,   0, 2, 3};
            return created;
        }

        public static float[] GetPrimitiveVertices(Primitives primitiveType)
        {
            switch (primitiveType)
            {
                case Primitives.SqrPlaneWTriangles:
                    return new float[] {
                        -.5f, 0.5f, 0, 0, 1, //Top left
                        0.5f, 0.5f, 0, 1, 1, //Top Right
                        0.5f, -.5f, 0, 1, 0, //Bottom Right
                        -.5f, -.5f, 0, 0, 0, //Bottom Left
                    };

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

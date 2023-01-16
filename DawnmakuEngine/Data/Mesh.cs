using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class Mesh
    {
        public float[] vertices;
        public uint[] triangleData;
        public float[] vertNormals,
            faceNormals;
        protected int vertexArrayHandle, vertexBufferHandle, elementBufferHandle;

        public int VertexArrayHandle { get { return vertexArrayHandle; } }
        public int VertexBufferHandle { get { return vertexBufferHandle; } }

        public enum Primitives { SqrPlaneWTriangles, SqrPlane, TallPlane, Cube, Pyramid }

        public void Use()
        {
           /* GL.BindVertexArray(VertexArrayHandle);*/
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
            GameMaster.lastBoundMesh = this;
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
            //CalculateNormals();
        }

        public int GetVertexCount { get { return vertices.Length / /*8*/5; } }
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
        public Vector3 GetVertexNormal(int index)
        {
            int startingVert = index * 3;
            return new Vector3(vertNormals[startingVert], vertNormals[startingVert + 1], vertNormals[startingVert + 2]);
        }
        public void SetVertexNormal(int index, Vector3 newVert)
        {
            int startingVert = index * 3;
            vertNormals[startingVert] = newVert.X;
            vertNormals[startingVert + 1] = newVert.Y;
            vertNormals[startingVert + 2] = newVert.Z;
        }
        public Vector3 GetFaceNormal(int index)
        {
            int startingVert = index * 3;
            return new Vector3(faceNormals[startingVert], faceNormals[startingVert + 1], faceNormals[startingVert + 2]);
        }
        public void SetFaceNormal(int index, Vector3 newVert)
        {
            int startingVert = index * 3;
            faceNormals[startingVert] = newVert.X;
            faceNormals[startingVert + 1] = newVert.Y;
            faceNormals[startingVert + 2] = newVert.Z;
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
        public void SetUV(int index, float newUV1, float newUV2)
        {
            int startingVert = index * 5 + 3;
            vertices[startingVert] = newUV1;
            vertices[startingVert + 1] = newUV2;
        }
        public void SetUVs(float[] uvs)
        {
            /*int count = uvs.Length;
            for (int i = 0; i < count; i += 2)
            {
                vertices[(i / 2) * 5 + 3] = uvs[i];
                vertices[(i / 2) * 5 + 4] = uvs[i + 1];
            }*/
            int count = Math.Min(GetVertexCount, uvs.Length / 2);
            for (int i = 0; i < count; i++)
                SetUV(i, uvs[i * 2], uvs[i * 2 + 1]);
        }
        public void SetUVs(int startIndex, float[] uvs)
        {
            int count = Math.Min(GetVertexCount, uvs.Length);
            for (int i = startIndex; i < count; i++)
                SetUV(i, uvs[i * 2], uvs[i * 2 + 1]);
        }
        public void CalculateNormals()
        {
            Vector3 v1, v2;
            float[] newVertexArray = new float[DawnMath.Round((vertices.Length / 5f) * 8)];
            int i, t;
            List<int> indexes = new List<int>();
            faceNormals = new float[triangleData.Length];
            vertNormals = new float[(vertices.Length / 5) * 3];
            for (i = 0; i < faceNormals.Length / 3; i++)
            {
                v1 = GetVertex((int)triangleData[i * 3 + 2]) - GetVertex((int)triangleData[i * 3 + 1]);
                v2 = GetVertex((int)triangleData[i * 3]) - GetVertex((int)triangleData[i * 3 + 1]);
                SetFaceNormal(i, Vector3.Cross(v1, v2));
            }
            for (i = 0; i < vertices.Length / 5; i++)
            {
                v1 = Vector3.Zero;
                indexes.Clear();
                for (t = 0; t < triangleData.Length; t++)
                    if (triangleData[t] == i)
                        indexes.Add(t);
                for (t = 0; t < indexes.Count; t++)
                    v1 += GetFaceNormal(DawnMath.Floor(t / 3f));
                SetVertexNormal(i, (v1 / indexes.Count).Normalized());
            }
            for (i = 0; i < faceNormals.Length / 3; i++)
                SetFaceNormal(i, GetFaceNormal(i).Normalized());

            for (i = 0; i < newVertexArray.Length / 8; i++)
            {
                newVertexArray[i * 8] = vertices[i * 5];
                newVertexArray[i * 8 + 1] = vertices[i * 5 + 1];
                newVertexArray[i * 8 + 2] = vertices[i * 5 + 2];
                newVertexArray[i * 8 + 3] = vertices[i * 5 + 3];
                newVertexArray[i * 8 + 4] = vertices[i * 5 + 4];

                newVertexArray[i * 8 + 5] = vertNormals[i * 3];
                newVertexArray[i * 8 + 6] = vertNormals[i * 3 + 1];
                newVertexArray[i * 8 + 7] = vertNormals[i * 3 + 2];
            }
            vertices = newVertexArray;
        }
        public Mesh()
        {
            //vertexArrayHandle = GL.GenVertexArray();
            vertexArrayHandle = GL.GenVertexArray();
            vertexBufferHandle = GL.GenBuffer();
            elementBufferHandle = GL.GenBuffer();
        }

        public Mesh(Primitives primitiveBase) : this()
        {
            vertices = GetPrimitiveVertices(primitiveBase);
            switch(primitiveBase)
            {
                case Primitives.SqrPlaneWTriangles:
                    triangleData = new uint[] { 0, 1, 2, 0, 2, 3 };
                    break;
            }
        }

        public Mesh(float[] vertices_) : this()
        {
            vertices = vertices_;
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

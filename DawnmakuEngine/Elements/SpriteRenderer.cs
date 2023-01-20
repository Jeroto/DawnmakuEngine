using DawnmakuEngine.Data;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK.Graphics.ES30;

namespace DawnmakuEngine.Elements
{
    public class SpriteRenderer : Element
    {
        public SpriteSet.Sprite curSprite;
        public MeshRenderer meshRend;
        public Mesh mesh;

        public Vector4 ColorByte
        {
            get { return meshRend.ColorByte; }
            set { meshRend.ColorByte = value; }
        }
        public Vector4 ColorFloat
        {
            get { return meshRend.ColorFloat; }
            set { meshRend.ColorFloat = value; }
        }

        public SpriteSet.Sprite Sprite
        {
            get { return curSprite; }
            set
            {
                curSprite = value;
                UpdateGraphic();
            }
        }


        public virtual void UpdateGraphic()
        {
            if(curSprite == null)
            {
                mesh.vertices = new float[4 * 5];
                mesh.SetUp(BufferUsageHint.DynamicDraw);
            }

            meshRend.tex = curSprite.tex;

            float halfWidth = MathHelper.Abs(curSprite.left - curSprite.right) / 2f * curSprite.tex.Width;
            float halfHeight = MathHelper.Abs(curSprite.bottom - curSprite.top) / 2f * curSprite.tex.Height;

            mesh.vertices = new float[4 * 5];

            mesh.vertices[0 + 0] = -halfWidth;
            mesh.vertices[1 + 0] = halfHeight;
            mesh.vertices[2 + 0] = 0;
            mesh.vertices[3 + 0] = curSprite.left;
            mesh.vertices[4 + 0] = curSprite.top;

            mesh.vertices[0 + 5] = halfWidth;
            mesh.vertices[1 + 5] = halfHeight;
            mesh.vertices[2 + 5] = 0;
            mesh.vertices[3 + 5] = curSprite.right;
            mesh.vertices[4 + 5] = curSprite.top;

            mesh.vertices[0 + 10] = halfWidth;
            mesh.vertices[1 + 10] = -halfHeight;
            mesh.vertices[2 + 10] = 0;
            mesh.vertices[3 + 10] = curSprite.right;
            mesh.vertices[4 + 10] = curSprite.bottom;

            mesh.vertices[0 + 15] = -halfWidth;
            mesh.vertices[1 + 15] = -halfHeight;
            mesh.vertices[2 + 15] = 0;
            mesh.vertices[3 + 15] = curSprite.left;
            mesh.vertices[4 + 15] = curSprite.bottom;

            mesh.SetUp(BufferUsageHint.DynamicDraw);
        }

        public SpriteRenderer(MeshRenderer newMeshRend)
        {
            mesh = new Mesh(Mesh.Primitives.SqrPlaneWTriangles);
            newMeshRend.mesh = mesh;
            meshRend = newMeshRend;
        }
    }
}

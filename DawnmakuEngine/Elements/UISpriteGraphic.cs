using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;
using System.Diagnostics;
using DawnmakuEngine.Data;
using System.Drawing;

namespace DawnmakuEngine.Elements
{
    public class UISpriteGraphic : SpriteRenderer
    {
        public override void UpdateGraphic()
        {
            if (curSprite == null)
            {
                mesh.vertices = new float[4 * 5];
                mesh.SetUp(BufferUsageHint.StaticDraw);
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

            mesh.SetUp(BufferUsageHint.StaticDraw);
        }


        public UISpriteGraphic(MeshRenderer meshRend) : base (meshRend)
        {

        }

        const string UI_SPRITE_NAME = "uisprite";
        public static UISpriteGraphic Create(Vector3 position, Vector3 rotation, Shader shader, SpriteSet.Sprite graphic, string name = UI_SPRITE_NAME)
        {
            Entity newEntity = new Entity(name, position, rotation, Vector3.One);
            MeshRenderer newMeshRend = new MeshRenderer();

            UISpriteGraphic newUISprite = new UISpriteGraphic(newMeshRend);

            newEntity.AddElement(newUISprite);
            newEntity.AddElement(newMeshRend);

            newUISprite.Sprite = graphic;

            newMeshRend.shader = shader;
            newMeshRend.LayerName = "borderui";

            return newUISprite;
        }
        public static UISpriteGraphic Create(Vector3 position, Vector3 rotation, Shader shader, SpriteSet.Sprite graphic, Vector4 color, bool colorIsByte, string name = UI_SPRITE_NAME)
        {
            UISpriteGraphic newUISprite = Create(position, rotation, shader, graphic, name);

            if (colorIsByte)
                newUISprite.ColorByte = color;
            else
                newUISprite.ColorFloat = color;

            return newUISprite;
        }
    }
}

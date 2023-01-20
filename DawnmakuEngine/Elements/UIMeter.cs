using DawnmakuEngine.Data;
using DawnmakuEngine.Data.Resources;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;

namespace DawnmakuEngine.Elements
{
    public class UIMeter : SpriteRenderer
    {
        public ResourceGroup resources;
        public ResourceGroup.CalculationType calcType;
        
        public float value;
        public float prevValue;
        public float maxWidth, maxHeight;

        public override void OnUpdate()
        {
            value = resources.GetFloatCalc(calcType);
            if(value != prevValue)
            {
                UpdateGraphic();

                prevValue = value;
            }
        }

        public override void UpdateGraphic()
        {
            float clampedValue = MathHelper.Clamp(value, 0, 1);
            float width = maxWidth * clampedValue;

            mesh.vertices = new float[4 * 5];

            mesh.vertices[0 + 0] = 0;
            mesh.vertices[1 + 0] = maxHeight;
            mesh.vertices[2 + 0] = 0;
            mesh.vertices[3 + 0] = curSprite.left;
            mesh.vertices[4 + 0] = curSprite.top;

            mesh.vertices[0 + 5] = width;
            mesh.vertices[1 + 5] = maxHeight;
            mesh.vertices[2 + 5] = 0;
            mesh.vertices[3 + 5] = curSprite.right * clampedValue;
            mesh.vertices[4 + 5] = curSprite.top;

            mesh.vertices[0 + 10] = width;
            mesh.vertices[1 + 10] = 0;
            mesh.vertices[2 + 10] = 0;
            mesh.vertices[3 + 10] = curSprite.right * clampedValue;
            mesh.vertices[4 + 10] = curSprite.bottom;

            mesh.vertices[0 + 15] = 0;
            mesh.vertices[1 + 15] = 0;
            mesh.vertices[2 + 15] = 0;
            mesh.vertices[3 + 15] = curSprite.left;
            mesh.vertices[4 + 15] = curSprite.bottom;

            mesh.SetUp(BufferUsageHint.StaticDraw);
        }

        public UIMeter(MeshRenderer meshRend) : base(meshRend)
        {
        }

        public static UIMeter CreateUIMeter(Vector3 position, Vector3 rotation, SpriteSet.Sprite graphic, Vector2 size, Shader shader)
        {
            const string METER_NAME = "uimeter";
            Entity newEntity = new Entity(METER_NAME, position, rotation, Vector3.One);
            MeshRenderer newMeshRend = new MeshRenderer();

            UIMeter newMeter = new UIMeter(newMeshRend);

            newEntity.AddElement(newMeter);
            newEntity.AddElement(newMeshRend);

            newMeter.meshRend = newMeshRend;
            newMeshRend.mesh = newMeter.mesh;

            newMeshRend.tex = graphic.tex;
            newMeter.curSprite = graphic;

            newMeter.maxWidth = size.X;
            newMeter.maxHeight = size.Y;

            newMeshRend.shader = shader;
            newMeshRend.LayerName = "borderui";

            return newMeter;
        }

        public static UIMeter Create(Vector3 position, Vector3 rotation, SpriteSet.Sprite graphic, Vector2 size, Shader shader,
            ResourceGroup resources, ResourceGroup.CalculationType calcType)
        {
            UIMeter newMeter = CreateUIMeter(position, rotation, graphic, size, shader);
            newMeter.resources = resources;
            newMeter.calcType = calcType;

            newMeter.value = resources.GetFloatCalc(calcType);
            newMeter.UpdateGraphic();

            return newMeter;
        }
    }
}

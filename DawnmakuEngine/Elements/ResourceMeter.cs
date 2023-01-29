using DawnmakuEngine.Data;
using DawnmakuEngine.Data.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    public class ResourceMeter : UIMeter
    {
        public ResourceGroup resources;
        public ResourceGroup.CalculationType calcType;

        public override void OnUpdate()
        {
            value = resources.GetFloatCalc(calcType);

            base.OnUpdate();
        }

        public ResourceMeter(MeshRenderer meshRend) : base(meshRend)
        {
        }

        const string METER_NAME = "resourcemeter";
        new public static ResourceMeter Create(Vector3 position, Vector3 rotation, SpriteSet.Sprite graphic, Vector2 size, Shader shader, string name = METER_NAME)
        {
            Entity newEntity = new Entity(name, position, rotation, Vector3.One);
            MeshRenderer newMeshRend = new MeshRenderer();

            ResourceMeter newMeter = new ResourceMeter(newMeshRend);

            newEntity.AddElement(newMeter);
            newEntity.AddElement(newMeshRend);

            newMeter.maxWidth = size.X;
            newMeter.maxHeight = size.Y;

            newMeter.Sprite = graphic;

            newMeshRend.shader = shader;
            newMeshRend.LayerName = "borderui";

            return newMeter;
        }

        public static ResourceMeter Create(Vector3 position, Vector3 rotation, SpriteSet.Sprite graphic, Vector2 size, Shader shader,
            ResourceGroup resources, ResourceGroup.CalculationType calcType, string name = METER_NAME)
        {
            ResourceMeter newMeter = Create(position, rotation, graphic, size, shader, name);
            newMeter.resources = resources;
            newMeter.calcType = calcType;

            newMeter.value = resources.GetFloatCalc(calcType);
            newMeter.UpdateGraphic();

            return newMeter;
        }
    }
}

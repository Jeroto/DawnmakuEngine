using DawnmakuEngine.Data;
using DawnmakuEngine.Data.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    public class ResourceSpriteGraphic : Element
    {
        public UISpriteGraphic uiSprite;
        public ResourceGroup resources;
        public ResourceGroup.CalculationType calcType;

        public List<SpriteSet.Sprite> sprites;
        public int index, prevIndex;

        public object[][] calcMods = new object[1][] { new object[1] { 0 } };
        public int minIndex, maxIndex;
        public int defaultIndex = 0;

        public override void OnUpdate()
        {
            index = resources.GetIntCalc(calcType, calcMods);
            if (index != prevIndex)
            {
                prevIndex = index;

                if (index < minIndex)
                    index = minIndex;
                else if (index > maxIndex)
                    index = maxIndex;

                uiSprite.Sprite = sprites[MathHelper.Clamp(index, 0, sprites.Count - 1)];
            }
        }

        public ResourceSpriteGraphic() : base() { }

        const string RESOURCE_SPRITE_NAME = "uiresourcegraphic";
        public static ResourceSpriteGraphic Create(Vector3 position, Vector3 rotation, Vector3 scale, Shader shader, List<SpriteSet.Sprite> sprites,
            string name = RESOURCE_SPRITE_NAME)
        {
            UISpriteGraphic spriteRend = UISpriteGraphic.Create(position, rotation, shader, sprites[0], name);
            Entity newEntity = spriteRend.EntityAttachedTo;
            ResourceSpriteGraphic newResGraphic = new ResourceSpriteGraphic();

            newResGraphic.uiSprite = spriteRend;

            newEntity.AddElement(newResGraphic);

            newResGraphic.sprites = sprites;

            spriteRend.Sprite = sprites[0];

            spriteRend.meshRend.shader = shader;
            spriteRend.meshRend.LayerName = "borderui";

            return newResGraphic;
        }

        public static ResourceSpriteGraphic Create(Vector3 position, Vector3 rotation, Vector3 scale, Shader shader, List<SpriteSet.Sprite> sprites,
            Vector4 color, bool colorIsByte, ResourceGroup resources, ResourceGroup.CalculationType calcType, int minIndex = 0, int maxIndex = 9999, object[][] calcMods = null, string name = RESOURCE_SPRITE_NAME)
        {
            ResourceSpriteGraphic newResGraphic = Create(position, rotation, scale, shader, sprites, name);
            newResGraphic.resources = resources;
            newResGraphic.calcType = calcType;


            newResGraphic.minIndex = minIndex;
            newResGraphic.maxIndex = maxIndex;

            if (colorIsByte)
                newResGraphic.uiSprite.ColorByte = color;
            else
                newResGraphic.uiSprite.ColorFloat = color;

            if(calcMods != null)
                newResGraphic.calcMods = calcMods;
            
            newResGraphic.index = resources.GetIntCalc(calcType, newResGraphic.calcMods);

            if (newResGraphic.index < newResGraphic.minIndex)
                newResGraphic.index = newResGraphic.minIndex;
            else if (newResGraphic.index > newResGraphic.maxIndex)
                newResGraphic.index = newResGraphic.maxIndex;

            newResGraphic.uiSprite.Sprite = newResGraphic.sprites[MathHelper.Clamp(newResGraphic.index, 0, newResGraphic.sprites.Count - 1)];

            return newResGraphic;
        }
    }
}

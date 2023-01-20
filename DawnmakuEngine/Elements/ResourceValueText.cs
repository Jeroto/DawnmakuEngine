using DawnmakuEngine.Data.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using DawnmakuEngine.Data;
using SixLabors.Fonts;

namespace DawnmakuEngine.Elements
{
    public class ResourceValueText : Element
    {
        public TextRenderer textRend;
        public BaseResource resource;
        public int characterLimit = 10;


        public override void OnUpdate()
        {
            string text = resource.OutputString();
            if (text.Length > characterLimit)
                text = text.Remove(characterLimit);
            textRend.Text = text;
        }

        public ResourceValueText() : base()
        {

        }

        const string RESOURCE_TEXT_NAME = "resourcevaluetext";
        public static ResourceValueText Create(Vector3 position, Vector3 rotation, string font, Shader shader, BaseResource resource, int characterLimit = 10, string name = RESOURCE_TEXT_NAME)
        {
            TextRenderer textRend = TextRenderer.Create(position, rotation, true, font, shader, name);

            ResourceValueText newValText = new ResourceValueText();
            newValText.textRend = textRend;
            newValText.resource = resource;
            newValText.characterLimit = characterLimit;
            textRend.EntityAttachedTo.AddElement(newValText);

            return newValText;
        }
        public static ResourceValueText Create(Vector3 position, Vector3 rotation, string font, Shader shader,
            Vector4 color, float textSize, HorizontalAlignment horiAlign, VerticalAlignment vertAlign, float monospaceWidth, BaseResource resource, int characterLimit = 10, string name = RESOURCE_TEXT_NAME)
        {
            TextRenderer textRend = TextRenderer.Create(position, rotation, true, font, shader, color, textSize, horiAlign, vertAlign, "", monospaceWidth, name);

            ResourceValueText newValText = new ResourceValueText();
            newValText.textRend = textRend;
            newValText.resource = resource;
            newValText.characterLimit = characterLimit;
            textRend.EntityAttachedTo.AddElement(newValText);

            return newValText;
        }
    }
}

using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DawnmakuEngine.Elements
{
    class LightElement : Element
    {
        public static List<LightElement> allLights = new List<LightElement>();
        public float r, g, b, intensity, distance;


        public static void PassLights(Shader targetShader)
        {
            int lightcount = allLights.Count, i;
            targetShader.SetInt("pointLightCount", lightcount);
            /*for (i = 0; i < lightcount; i++)
                targetShader.SetVector3(*/
        }

        public static Vector3[] GetAllLightPos()
        {
            Vector3[] array = new Vector3[allLights.Count];
            for (int i = 0; i < allLights.Count; i++)
                array[i] = allLights[i].entityAttachedTo.WorldPosition;
            return array;
        }

        public static Vector4[] GetAllColors()
        {
            Vector4[] array = new Vector4[allLights.Count];
            for (int i = 0; i < allLights.Count; i++)
            {
                array[i] = new Vector4(allLights[i].r, allLights[i].g, allLights[i].b, 0) * allLights[i].intensity;
                array[i].W = 1;
            }
            return array;
        }

        public LightElement(string _format, float _valOne, float _valTwo, float _valThree, float _intensity, float _distance) : 
            this(_format, new Vector3(_valOne, _valTwo, _valThree), _intensity, _distance) { }
        public LightElement(string _format, Vector3 _colorVal, float _intensity, float _distance) : base()
        {
            _colorVal = DawnMath.ConvertColorToRgb(_format, _colorVal, false);
            r = _colorVal.X;
            g = _colorVal.Y;
            b = _colorVal.Z;
            intensity = _intensity;
            distance = _distance;
        }

        protected override void OnEnableAndCreate()
        {
            allLights.Add(this);
            base.OnEnableAndCreate();
        }
        protected override void OnDisableAndDestroy()
        {
            allLights.Remove(this);
            base.OnDisableAndDestroy();
        }
        public override Element Clone()
        {
            return new LightElement("rgb", new Vector3(r, g, b), intensity, distance);
        }
    }
}

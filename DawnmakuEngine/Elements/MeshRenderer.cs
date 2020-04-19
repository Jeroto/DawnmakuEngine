using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace DawnmakuEngine.Elements
{
    class MeshRenderer : Element
    {
        public static List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        public Mesh mesh;
        public Shader shader;
        public Texture tex, tex2;
        public BufferUsageHint bufferUsageType = BufferUsageHint.DynamicDraw;
        public byte colorR = 255, colorG = 255, colorB = 255, colorA = 255;
        public Vector4 ColorFloat
        {
            get { return new Vector4(colorR / 255f, colorG / 255f, colorB / 255f, colorA / 255f); }
            set
            {
                colorR = (byte)Math.Floor(value.X * 255);
                colorG = (byte)Math.Floor(value.Y * 255);
                colorB = (byte)Math.Floor(value.Z * 255);
                colorA = (byte)Math.Floor(value.W * 255);
            }
        }
        public Vector4 ColorByte
        {
            get { return new Vector4(colorR, colorG, colorB, colorA); }
            set
            {
                colorR = (byte)Math.Clamp(Math.Floor(value.X), 0, 255);
                colorG = (byte)Math.Clamp(Math.Floor(value.Y), 0, 255);
                colorB = (byte)Math.Clamp(Math.Floor(value.Z), 0, 255);
                colorA = (byte)Math.Clamp(Math.Floor(value.W), 0, 255);
            }
        }


        public void BindRendering()
        {
            if (mesh == null)
            {
                Console.WriteLine("There is no mesh on " + entityAttachedTo.Name + "'s Mesh Renderer Element!");
                return;
            }
            shader.Use();
            mesh.Use();


            int positionLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);
            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            
            if (tex != null)
                tex.Use();
            if (tex2 != null)
                tex2.Use(TextureUnit.Texture1);
            Quaternion modelRotation = entityAttachedTo.LocalRotation;
            Matrix4 model = Matrix4.Identity * Matrix4.CreateFromQuaternion(entityAttachedTo.LocalRotation) * 
                Matrix4.CreateTranslation(entityAttachedTo.WorldPosition) *
                Matrix4.CreateScale(entityAttachedTo.LocalScale);
            shader.SetMatrix4("model", model);
            shader.SetVector4("colorModInput", ColorFloat);
        }

        public MeshRenderer() : base()
        {

        }

        protected override void OnEnableAndCreate()
        {
            meshRenderers.Add(this);
        }

        protected override void OnDisableAndDestroy()
        {
            meshRenderers.Remove(this);
        }

        public override void Remove()
        {
            meshRenderers.Remove(this);
        }

    }
}

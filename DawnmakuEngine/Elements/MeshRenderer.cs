﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    public class MeshRenderer : Element
    {
        public static List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        public static List<List<MeshRenderer>> renderLayers = new List<List<MeshRenderer>>();

        public Mesh mesh;
        public Shader shader;
        public Texture tex, tex2;
        public float modelScale = 1,
            modelScaleX = 1, modelScaleY = 1, modelScaleZ = 1;
        protected int layer = -1;

        public int Layer
        {
            set
            {
                if (layer >= 0)
                    renderLayers[layer].Remove(this);
                layer = value;
                renderLayers[layer].Add(this);
            }
            get { return layer; }
        }
        
        public string LayerName
        {
            set { Layer = GameMaster.layerIndexes[value.ToLower()]; }
        }

        public BufferUsageHint bufferUsageType = BufferUsageHint.DynamicDraw;
        public float colorR = 1, colorG = 1, colorB = 1, colorA = 1;


        public Vector4 ColorFloat
        {
            get { return new Vector4(colorR, colorG, colorB, colorA); }
            set
            {
                colorR = value.X;
                colorG = value.Y;
                colorB = value.Z;
                colorA = value.W;
            }
        }
        public Vector4 ColorByte
        {
            get { return new Vector4((int)Math.Floor(colorR * 255),
                (int)Math.Floor(colorG * 255),
                (int)Math.Floor(colorB * 255),
                (int)Math.Floor(colorA * 255)); }
            set
            {
                colorR = value.X / 255;
                colorG = value.Y / 255;
                colorB = value.Z / 255;
                colorA = value.W / 255;
            }
        }

        public void ClampColor()
        {
            if (colorR < 0)
                colorR = 0;
            else if (colorR > 255)
                colorR = 255;
        }

        public void BindRendering()
        {
            if (mesh == null)
            {
                GameMaster.LogError("There is no mesh on " + entityAttachedTo.Name + "'s Mesh Renderer Element!");
                return;
            }

            if(shader != null && shader != GameMaster.lastBoundShader)
                shader.Use();
            if(mesh != null && mesh != GameMaster.lastBoundMesh)
                mesh.Use();

            int positionLocation = shader.GetAttribLocation("position");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            int texCoordLocation = shader.GetAttribLocation("texCoordAttrib");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            /*int positionLocation = shader.GetAttribLocation("position");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            int texCoordLocation = shader.GetAttribLocation("texCoordAttrib");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            int normalLocation = shader.GetAttribLocation("normalAttrib");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));*/

            if (tex != null && tex != GameMaster.lastBoundTexture)
                tex.Use();
            if (tex2 != null)
                tex2.Use(TextureUnit.Texture1);
            Quaternion modelRotation = entityAttachedTo.LocalRotation;
            Matrix4 model = Matrix4.Identity *
                Matrix4.CreateScale(entityAttachedTo.WorldScale * modelScale * new Vector3(modelScaleX, modelScaleY, modelScaleZ))
                * Matrix4.CreateFromQuaternion(entityAttachedTo.WorldRotation) * Matrix4.CreateTranslation(entityAttachedTo.WorldPosition);
            shader.SetMatrix4("model", model);
            shader.SetMatrix3("normalMatrix", Matrix3.Transpose(Matrix3.Invert(new Matrix3(model))));
            shader.SetVector4("colorMod", ColorFloat);
        }

        /*public void SetShaderPositionAndView(Matrix4 view, Matrix4 proj)
        {
            int viewLoc = shader.GetAttribLocation("view");
            GL.EnableVertexAttribArray(viewLoc);
            GL.VertexAttribPointer(viewLoc, 3, VertexAttribPointerType.Float, false, 4 * 4 * sizeof(float), 0);
            int projLoc = shader.GetAttribLocation("projection");
            GL.EnableVertexAttribArray(projLoc);
            GL.VertexAttribPointer(projLoc, 3, VertexAttribPointerType.Float, false, 4 * 4 * sizeof(float), 0);
        }*/
        
        public void LoadModel(TexturedModel model)
        {
            mesh = model.modelMesh;
            tex = model.modelTex;
        }
        public void LoadModel(string modelName, Dictionary<string, TexturedModel> dic)
        {
            TexturedModel model = dic[modelName];
            mesh = model.modelMesh;
            tex = model.modelTex;
        }
        public void LoadBackgroundModel(string modelName)
        {
            TexturedModel model = GameMaster.gameMaster.backgroundModels[modelName];
            mesh = model.modelMesh;
            tex = model.modelTex;
        }

        public MeshRenderer() : base(false, false)
        {

        }
        public MeshRenderer(Mesh newMesh, BufferUsageHint bufferUsage, float scale = 1) : this()
        {
            mesh = newMesh;
            modelScale = scale;
            mesh.SetUp(bufferUsage);
        }
        public MeshRenderer(Mesh newMesh, int layer_, BufferUsageHint bufferUsage, float scale = 1) : this(newMesh, bufferUsage, scale)
        {
            Layer = layer_;
        }
        public MeshRenderer(Mesh newMesh, string layer_, BufferUsageHint bufferUsage, float scale = 1) : this(newMesh, bufferUsage, scale)
        {
            LayerName = layer_;
        }
        public MeshRenderer(Mesh newMesh, BufferUsageHint bufferUsage, Texture texture, float scale = 1, Texture texture2 = null) : this(newMesh, bufferUsage, scale)
        {
            tex = texture;
            tex2 = texture2;
        }
        public MeshRenderer(Mesh newMesh, int layer_, BufferUsageHint bufferUsage, Texture texture,
            float scale = 1, Texture texture2 = null) : this(newMesh, bufferUsage, texture, scale, texture2)
        {
            Layer = layer_;
        }
        public MeshRenderer(Mesh newMesh, string layer_, BufferUsageHint bufferUsage, Texture texture,
            float scale = 1, Texture texture2 = null) : this(newMesh, bufferUsage, texture, scale, texture2)
        {
            LayerName = layer_;
        }
        public MeshRenderer(Mesh newMesh, BufferUsageHint bufferUsage, Shader newShader, Texture texture, float scale = 1,
            Texture texture2 = null) : this(newMesh, bufferUsage, texture, scale, texture2)
        {
            shader = newShader;
        }
        public MeshRenderer(Mesh newMesh, int layer_, BufferUsageHint bufferUsage, Shader newShader, Texture texture, float scale = 1,
            Texture texture2 = null) : this(newMesh, bufferUsage, newShader, texture, scale, texture2)
        {
            Layer = layer_;
        }
        public MeshRenderer(Mesh newMesh, string layer_, BufferUsageHint bufferUsage, Shader newShader, Texture texture, float scale = 1,
            Texture texture2 = null) : this(newMesh, bufferUsage, newShader, texture, scale, texture2)
        {
            LayerName = layer_;
        }
        public MeshRenderer(TexturedModel newMesh, BufferUsageHint bufferUsage, Shader newShader, Texture texture2 = null) : this()
        {
            mesh = newMesh.modelMesh;
            tex = newMesh.modelTex;
            modelScale = newMesh.scale;
            mesh.SetUp(bufferUsage);
            shader = newShader;
        }
        public MeshRenderer(TexturedModel newMesh, int layer_, BufferUsageHint bufferUsage,
            Shader newShader, Texture texture2 = null) : this(newMesh, bufferUsage, newShader, texture2)
        {
            Layer = layer_;
        }
        public MeshRenderer(TexturedModel newMesh, string layer_, BufferUsageHint bufferUsage,
            Shader newShader, Texture texture2 = null) : this(newMesh, bufferUsage, newShader, texture2)
        {
            LayerName = layer_;
        }

        protected override void OnEnableAndCreate()
        {
            meshRenderers.Add(this);
            base.OnEnableAndCreate();
        }

        protected override void OnDisableAndDestroy()
        {
            meshRenderers.Remove(this);
            base.OnDisableAndDestroy();
        }

        public override void Remove()
        {
            meshRenderers.Remove(this);
        }

        public override void AttemptDelete()
        {
            base.AttemptDelete();
            meshRenderers.Remove(this);
        }
    }
}

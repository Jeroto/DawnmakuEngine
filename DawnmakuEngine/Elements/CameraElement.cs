using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Elements
{
    class CameraElement : Element
    {
        public static List<CameraElement> cameras = new List<CameraElement>();

        public bool perspective = false;

        protected Matrix4 viewMatrix = Matrix4.Identity, projectionMatrix = Matrix4.Identity;
        public float nearDist = 0.1f, farDist = 10000;
        public List<int> renderableLayers = new List<int>();

        GameMaster gameMaster = GameMaster.gameMaster;


        public Matrix4 ViewMatrix { get { return viewMatrix; } }
        public Matrix4 ProjectionMatrix { get { return projectionMatrix; } }


        public bool Perspective
        {
            set
            {
                perspective = value;
                CreateProjectionMatrix();
            }
            get { return perspective; }
        }
        /// <summary> The closest an object can come to the camera before it starts being cut off (larger number = further away) </summary>
        public float NearPlane
        {
            set
            {
                nearDist = value;
                CreateProjectionMatrix();
            }
            get { return nearDist; }
        }
        /// <summary> The furthest an object can go from the camera before it starts being cut off (larger number = further away) </summary>
        public float FarPlane
        {
            set
            {
                farDist = value;
                CreateProjectionMatrix();
            }
            get { return farDist; }
        }

        //Orthographic settings
        protected float orthoScale = 1;
        /// <summary> Orthographic camera only. The scale of the camera's view (larger numbers = zoom out, smaller numbers = zoom in) </summary>
        public float OrthoScale
        {
            set
            {
                orthoScale = value;
                CreateProjectionMatrix();
            }
            get { return orthoScale; }
        }

        protected float orthoScaleX = 1;
        /// <summary> Orthographic camera only. Used to modify scale of the camera's view on the X axis only (larger numbers = zoom out, smaller numbers = zoom in) </summary>
        public float OrthoScaleX
        {
            set
            {
                orthoScaleX = value;
                CreateProjectionMatrix();
            }
            get { return orthoScaleX; }
        }

        protected float orthoScaleY = 1;
        /// <summary> Orthographic camera only. Used to modify scale of the camera's view on the Y axis only (larger numbers = zoom out, smaller numbers = zoom in) </summary>
        public float OrthoScaleY
        {
            set
            {
                orthoScaleY = value;
                CreateProjectionMatrix();
            }
            get { return orthoScaleY; }
        }

        //Perspective settings
        protected float fov = MathHelper.PiOver2;
        /// <summary> Perspective camera only. The camera's field of view in radians </summary>
        public float FOV
        {
            set
            {
                fov = value;
                CreateProjectionMatrix();
            }
            get { return fov; }
        }


        public CameraElement() : base(true, false, true)
        {
        }

        public CameraElement(bool perspectiveCam) : this()
        {
            perspective = perspectiveCam;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleOrFov">If perspective mode, is treated as FOV. If orthographic, is treated as Ortho scale </param>
        public CameraElement(bool perspectiveCam, float scaleOrFov) : this()
        {
            perspective = perspectiveCam;
            if (perspective)
                fov = scaleOrFov;
            else
                orthoScale = scaleOrFov;
        }
        /// <summary> This will force the camera to orthographic mode </summary>
        public CameraElement(float orthoScale_, float orthoScaleX_, float orthoScaleY_) : this(false)
        {
            orthoScale = orthoScale_;
            orthoScaleX = orthoScaleX_;
            orthoScaleY = orthoScaleY_;
        }
        /// <summary> This will force the camera to perspective mode </summary>
        public CameraElement(float fov_) : this(true)
        {
            fov = fov_;
        }

        public override void PostCreate()
        {
            CreateProjectionMatrix();
            base.PostCreate();
        }

        public override void PreRender()
        {
            viewMatrix = Matrix4.CreateTranslation(-entityAttachedTo.WorldPosition) * Matrix4.CreateFromQuaternion(entityAttachedTo.WorldRotation);
            base.PreRender();
        }

        public void CreateProjectionMatrix()
        {
            if (perspective)
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, gameMaster.windowWidth / (float)gameMaster.windowHeight, nearDist, farDist);
            else
                projectionMatrix = Matrix4.CreateOrthographic(gameMaster.windowWidth / 2f * orthoScale * orthoScaleX,
                    gameMaster.windowHeight / 2f * orthoScale * orthoScaleY, nearDist, farDist);
        }

        public void SetLayers(int[] layers)
        {
            renderableLayers = new List<int>();
            for (int i = 0; i < layers.Length; i++)
            {
                renderableLayers.Add(layers[i]);
            }
            renderableLayers.Sort();
        }
        public void AddLayer(int layer)
        {
            if (!renderableLayers.Contains(layer))
            {
                renderableLayers.Add(layer);
                renderableLayers.Sort();
            }
        }
        public void RemoveLayer(int layer)
        {
            renderableLayers.Remove(layer);
        }
        public void SetAllLayersRenderable()
        {
            renderableLayers = new List<int>();
            for (int i = 0; i < GameMaster.layerIndexes.Count; i++)
            {
                renderableLayers.Add(i);
            }
        }
        public void SetNoLayersRenderable()
        {
            renderableLayers = new List<int>();
        }

        protected override void OnEnableAndCreate()
        {
            cameras.Add(this);
            base.OnEnableAndCreate();
        }

        protected override void OnDisableAndDestroy()
        {
            cameras.Remove(this);
            base.OnDisableAndDestroy();
        }

        public override void Remove()
        {
            cameras.Remove(this);
        }

        public override void AttemptDelete()
        {
            base.AttemptDelete();
            cameras.Remove(this);
        }
    }
}

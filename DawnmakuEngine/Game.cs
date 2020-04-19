using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Input;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;

namespace DawnmakuEngine
{
    class Game : GameWindow
    {
        //int vertexBufferObject, vertexArrayObject, elementBufferObject;
        bool ortho = true;
        //Texture texture1, texture2;
        float time;
        /*float[] vertices =
        {                     //Tex Coords
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };
        uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };
        Shader shader;*/

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (Focused)
            {
                if (input.IsKeyDown(Key.Escape))
                {
                    Exit();
                }
                if (input.IsKeyDown(Key.O))
                {
                    ortho = true;
                }
                if (input.IsKeyDown(Key.P))
                {
                    ortho = false;
                }
                if (input.IsKeyDown(Key.F))
                    TargetUpdateFrequency = 240;
                if (input.IsKeyDown(Key.L))
                    TargetUpdateFrequency = 60;

                if (input.IsKeyDown(Key.Plus))
                {
                    Vector4 color = MeshRenderer.meshRenderers[0].ColorByte;
                    if (input.IsKeyDown(Key.R))
                        color.X += 2;
                    if (input.IsKeyDown(Key.G))
                        color.Y += 2;
                    if (input.IsKeyDown(Key.B))
                        color.Z += 2;
                    if (input.IsKeyDown(Key.A))
                        color.W += 2;
                    MeshRenderer.meshRenderers[0].ColorByte = color;
                }
                else if (input.IsKeyDown(Key.Minus))
                {
                    Vector4 color = MeshRenderer.meshRenderers[0].ColorByte;
                    if (input.IsKeyDown(Key.R))
                        color.X -= 2;
                    if (input.IsKeyDown(Key.G))
                        color.Y -= 2;
                    if (input.IsKeyDown(Key.B))
                        color.Z -= 2;
                    if (input.IsKeyDown(Key.A))
                        color.W -= 2;
                    MeshRenderer.meshRenderers[0].ColorByte = color;
                }

                InputScript.GetInput();
            }
            else
                InputScript.ClearInput();

            //Entity.FindEntity("FPSCounter").GetElement<TextRenderer>().textToWrite = "FPS:" + UpdateFrequency.ToString("0.00");

            int elementCount = Element.allElements.Count;
            Element currentElement;
            for (int i = 0; i < elementCount; i++)
            {
                currentElement = Element.allElements[i];
                if (!currentElement.enabled || !currentElement.EntityAttachedTo.enabled)
                    continue;
                if (!currentElement.PostCreateRan)
                    currentElement.PostCreate();
                currentElement.OnUpdate();
            }

            /*if (input.IsKeyDown(Key.D))
                Entity.allEntities[0].Rotate(new Vector3(0, .02f, 0));
            if (input.IsKeyDown(Key.A))
                Entity.allEntities[0].Rotate(new Vector3(0, -.02f, 0));
            if (input.IsKeyDown(Key.S))
                Entity.allEntities[0].Rotate(new Vector3(0.02f, 0, 0));
            if (input.IsKeyDown(Key.W))
                Entity.allEntities[0].Rotate(new Vector3(-0.02f, 0, 0));
            if (input.IsKeyDown(Key.Q))
                Entity.allEntities[0].Rotate(new Vector3(0, 0, 0.02f));
            if (input.IsKeyDown(Key.E))
                Entity.allEntities[0].Rotate(new Vector3(0, 0, -0.02f));*/

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            DataLoader loader = new DataLoader("1");
            //loader.EnemyLoader();
            loader.EnemyPatternLoader();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            /*vertexBufferObject = GL.GenBuffer();
            elementBufferObject = GL.GenBuffer();
            vertexArrayObject = GL.GenVertexArray();

            GL.BindVertexArray(vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);*/

            /*shader = new Shader("Shaders/Shader.vert", "Shaders/TransparentShader.frag");

            texture1 = new Texture("Wood.jpg", false);
            texture2 = new Texture("PlsRember.png", false);
            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);*/


            /*int positionLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);
            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));*/


            Texture reimuSprite1 = new Texture("Reimu1.png", false), reimuSprite2 = new Texture("Reimu2.png", false),
                reimuSprite3 = new Texture("Reimu3.png", false), reimuSprite4 = new Texture("Reimu4.png", false);

            MeshRenderer newRenderer1 = new MeshRenderer(), newRenderer2 = new MeshRenderer();
            newRenderer1.mesh = new Mesh(Mesh.Primitives.TallPlane);
            newRenderer1.mesh.SetUp(BufferUsageHint.DynamicDraw);
            newRenderer1.shader = GameMaster.gameMaster.spriteShader;
            //newRenderer1.tex = new Texture("Wood.jpg", true);
            newRenderer1.tex = reimuSprite1;
            newRenderer1.shader.SetInt("texture0", 0);
            newRenderer1.shader.SetInt("texture1", 1);

            newRenderer2.mesh = new Mesh(Mesh.Primitives.Pyramid);
            newRenderer2.mesh.SetUp(BufferUsageHint.DynamicDraw);
            newRenderer2.shader = new Shader("Shaders/Shader.vert", "Shaders/Shader.frag");
            newRenderer2.tex = new Texture("Fire.png", true);
            newRenderer2.shader.SetInt("texture0", 0);
            newRenderer2.shader.SetInt("texture1", 1);

            Entity debugEntity = new Entity("Player"), debugChild = new Entity("Pyramid", debugEntity, new Vector3(500f, 0, 0), Vector3.Zero, Vector3.One * 0.5f);
            debugEntity.AddElement(newRenderer1);
            debugChild.AddElement(newRenderer2);

            PlayerController debugPlayerControl = new PlayerController();
            debugPlayerControl.MoveSpeed = 1.75f;
            debugPlayerControl.FocusSpeedPercent = 65;
            debugEntity.AddElement(debugPlayerControl);

            TextureAnimator.AnimationState[] animStates = new TextureAnimator.AnimationState[3];
            animStates[0] = new TextureAnimator.AnimationState();
            animStates[1] = new TextureAnimator.AnimationState();
            animStates[2] = new TextureAnimator.AnimationState();

            /*animStates[0].animFrames = new TextureAnimator.AnimationFrame[4];
            animStates[0].animFrames[0] = TextureAnimator.CreateAnimFrames(1, reimuSprite1, new float[] { 8f },
                new float[] { 0, 1, 1, 1, 1, 0, 0, 0 })[0];
            animStates[0].animFrames[1] = TextureAnimator.CreateAnimFrames(1, reimuSprite2, new float[] { 8f },
                new float[] { 0, 1, 1, 1, 1, 0, 0, 0 })[0];
            animStates[0].animFrames[2] = TextureAnimator.CreateAnimFrames(1, reimuSprite3, new float[] { 8f },
                new float[] { 0, 1, 1, 1, 1, 0, 0, 0 })[0];
            animStates[0].animFrames[3] = TextureAnimator.CreateAnimFrames(1, reimuSprite4, new float[] { 8f },
                new float[] { 0, 1, 1, 1, 1, 0, 0, 0 })[0];

            animStates[1].animFrames = TextureAnimator.CreateAnimFrames(1, reimuSprite3, new float[] { 8f },
                new float[] { 0, 1, 1, 1, 1, 0, 0, 0 });
            animStates[2].animFrames = TextureAnimator.CreateAnimFrames(1, reimuSprite4, new float[] { 8f },
                new float[] { 0, 1, 1, 1, 1, 0, 0, 0 });*/

            Texture reimuSprite = new Texture("Reimu.png", false);
            animStates[0].animFrames = TextureAnimator.CreateAnimFrames(4, reimuSprite, new float[]{ 8f, 8f, 8f, 8f},
                new float[] {0,1 ,0.25f,1, 0.25f,0,  0,1, 0.25f,0, 0,0,
                    0.25f,1, 0.5f,1, 0.5f,0,  0.25f,1, 0.5f,0, 0.25f,0,
                    0.5f,1, 0.75f,1, 0.75f,0,  0.5f,1, 0.75f,0, 0.5f,0,
                    0.75f,1, 1,1, 1,0,  0.75f,1, 1,0, 0.75f,0});
            animStates[1].animFrames = TextureAnimator.CreateAnimFrames(1, reimuSprite, new float[] { 8f },
                new float[] {0.5f,1, 0.75f,1, 0.75f,0,  0.5f,1, 0.75f,0, 0.5f,0});
            animStates[2].animFrames = TextureAnimator.CreateAnimFrames(1, reimuSprite, new float[] { 8f },
                new float[] {0.75f,1, 1,1, 1,0,  0.75f,1, 1,0, 0.75f,0 });
            TextureAnimator debugPlayerAnim = new TextureAnimator(animStates, debugEntity.GetElement<MeshRenderer>(), true);
            debugEntity.AddElement(debugPlayerAnim);

            Entity text = new Entity("FPSCounter", new Vector3(.5f, -.5f, 0), Vector3.Zero, Vector3.One);

            /*TextRenderer textRend = new TextRenderer();
            textRend.SetDrawingColor(255, 0, 0, 255);
            textRend.textToWrite = "FPS:";
            textRend.font = new QuickFont.QFont("Fonts/WitchingHour.ttf", 72, new QuickFont.Configuration.QFontBuilderConfiguration());
            textRend.SetUp();
            text.AddElement(textRend);*/
            Console.WriteLine(debugEntity.ToString());

            Entity newBullet = new Entity("Bullet");

            MeshRenderer renderer = new MeshRenderer();
            renderer.tex = GameMaster.gameMaster.bulletSheet;
            renderer.shader = GameMaster.gameMaster.spriteShader;
            renderer.mesh = new Mesh(Mesh.Primitives.SqrPlane);
            renderer.mesh.SetUp(OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);

            newBullet.AddElement(renderer);
            newBullet.AddElement(new TextureAnimator(BulletElement.GetBulletAnim(BulletElement.BulletType.Butterfly, BulletElement.BulletColor.Red, GameMaster.gameMaster.bulletSheet), renderer, true));

            newBullet.AddElement(new RotateElement(180, true, true));



            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            /*GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Equal, GameMaster.gameMaster.spriteShader.Handle, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilMask(0xFF);*/
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            time += 32 * (float)e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            //GL.BindVertexArray(vertexArrayObject);

            /*texture1.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);*/

            Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationX(10) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time)),
                view = Matrix4.CreateTranslation(0f, 0f, -3f),
                projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100f),
                orthoProjection = Matrix4.CreateOrthographic(Width / 2, Height / 2, 0.1f, 100f);
            int index, count;
            count = Element.allElements.Count;
            for (index = 0; index < count; index++)
            {
                Element.allElements[index].PreRender();
            }

            count = MeshRenderer.meshRenderers.Count;
            for (index = 0; index < count; index++)
            {
                MeshRenderer.meshRenderers[index].BindRendering();
                MeshRenderer.meshRenderers[index].shader.SetMatrix4("view", view);

                if (!ortho)
                    MeshRenderer.meshRenderers[index].shader.SetMatrix4("projection", projection);
                else
                    MeshRenderer.meshRenderers[index].shader.SetMatrix4("projection", orthoProjection);


                GL.DrawArrays(PrimitiveType.Triangles, 0, MeshRenderer.meshRenderers[index].mesh.vertices.Length);
            }
            /*count = TextRenderer.textRenderers.Count;
            for (index = 0; index < count; index++)
            {
                if (!ortho) 
                    TextRenderer.textRenderers[index].Draw(projection);
                else
                    TextRenderer.textRenderers[index].Draw(orthoProjection);

            }*/

            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);


            for (int i = 0; i < MeshRenderer.meshRenderers.Count; i++)
            {
                if (MeshRenderer.meshRenderers[i].mesh != null)
                {
                    GL.DeleteBuffer(MeshRenderer.meshRenderers[i].mesh.VertexBufferHandle);
                    GL.DeleteVertexArray(MeshRenderer.meshRenderers[i].mesh.VertexArrayHandle);
                }

                if (MeshRenderer.meshRenderers[i].shader != null)
                    GL.DeleteProgram(MeshRenderer.meshRenderers[i].shader.Handle);
                if (MeshRenderer.meshRenderers[i].tex != null)
                    GL.DeleteTexture(MeshRenderer.meshRenderers[i].tex.Handle);
                if (MeshRenderer.meshRenderers[i].tex2 != null)
                    GL.DeleteTexture(MeshRenderer.meshRenderers[i].tex2.Handle);

                if (MeshRenderer.meshRenderers[i].shader != null)
                    MeshRenderer.meshRenderers[i].shader.Dispose();
            }
            base.OnUnload(e);
        }
    }
}

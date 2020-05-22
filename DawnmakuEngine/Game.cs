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


        protected override void OnLoad(EventArgs e)
        {
            DataLoader loader = new DataLoader("1");
            loader.LoadBullets();
            loader.LoadPlayerOrbs();
            loader.LoadPlayers();
            loader.LoadEnemies();

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


            MeshRenderer debugPlayerRenderer = new MeshRenderer();
            debugPlayerRenderer.mesh = Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles);
            debugPlayerRenderer.mesh.SetUp(BufferUsageHint.DynamicDraw);
            debugPlayerRenderer.shader = GameMaster.gameMaster.spriteShader;
            //newRenderer1.tex = new Texture("Wood.jpg", true);
            debugPlayerRenderer.shader.SetInt("texture0", 0);
            debugPlayerRenderer.shader.SetInt("texture1", 1);

            Entity debugPlayer = new Entity("Player");
            debugPlayer.AddElement(debugPlayerRenderer);

            PlayerController debugPlayerControl = new PlayerController();
            debugPlayerControl.playerData = GameMaster.gameMaster.loadedPlayerChars["reimu"];
            debugPlayer.AddElement(debugPlayerControl);

            TextureAnimator debugPlayerAnim = new TextureAnimator(debugPlayer.GetElement<MeshRenderer>(), true);
            debugPlayer.AddElement(debugPlayerAnim);

            GameMaster.gameMaster.playerEntity = debugPlayer;

            Entity text = new Entity("FPSCounter", new Vector3(.5f, -.5f, 0), Vector3.Zero, Vector3.One);

            /*TextRenderer textRend = new TextRenderer();
            textRend.SetDrawingColor(255, 0, 0, 255);
            textRend.textToWrite = "FPS:";
            textRend.font = new QuickFont.QFont("Fonts/WitchingHour.ttf", 72, new QuickFont.Configuration.QFontBuilderConfiguration());
            textRend.SetUp();
            text.AddElement(textRend);*/
            Console.WriteLine(debugPlayer.ToString());

            /*Entity newBullet = new Entity("Bullet", debugPlayer, new Vector3(20, 0, 0), Vector3.Zero, Vector3.One);

            MeshRenderer renderer = new MeshRenderer();
            renderer.tex = GameMaster.gameMaster.bulletSprites["butterfly"].sprites[0].tex;
            renderer.shader = GameMaster.gameMaster.spriteShader;
            renderer.mesh = Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles);
            renderer.mesh.SetUp(OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);

            newBullet.AddElement(renderer);
            newBullet.AddElement(new TextureAnimator(BulletElement.GetBulletAnim("butterfly", (int)BulletElement.BulletColor.Red), renderer, true));

            newBullet.AddElement(new RotateElement(180, true, true));*/


            EnemyElement.SpawnEnemy(GameMaster.gameMaster.loadedEnemyData["blueenemy1"], Vector3.Zero);


            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            /*GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Equal, GameMaster.gameMaster.spriteShader.Handle, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilMask(0xFF);*/
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            GameMaster gameMaster = GameMaster.gameMaster;

            gameMaster.GameMasterUpdate();

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

                if(input.IsKeyDown(Key.Y))
                {
                    BulletElement.BulletStage[] stage = new BulletElement.BulletStage[] { new BulletElement.BulletStage() };
                    stage[0].spriteType = gameMaster.bulletTypes[BulletElement.Random(0, gameMaster.bulletTypes.Count - 1)];
                    stage[0].bulletColor = BulletElement.Random(0, gameMaster.bulletSprites[stage[0].spriteType].sprites.Count - 1);
                    stage[0].movementDirection = DawnMath.RandomCircle();
                    stage[0].startingSpeed = 100;
                    stage[0].endingSpeed = 50;
                    stage[0].framesToChangeSpeed = 60;
                    BulletElement.SpawnBullet(stage, gameMaster.playerEntity.WorldPosition, BulletElement.ShouldSpin(stage[0].spriteType));
                }

                if (input.IsKeyDown(Key.H))
                {
                    gameMaster.timeScaleUpdate = 0.5f;
                }
                else if(input.IsKeyDown(Key.R))
                {
                    gameMaster.timeScaleUpdate = 1f;
                }

                if (InputScript.specialDown)
                {
                    if (InputScript.Focus)
                        gameMaster.currentPowerLevel--;
                    else
                        gameMaster.currentPowerLevel++;
                }

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

            gameMaster.ElementUpdate();
            

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

            GameMaster.gameMaster.ElementPreRender();

            MeshRenderer thisRenderer;
            count = MeshRenderer.meshRenderers.Count;
            for (index = 0; index < count; index++)
            {
                thisRenderer = MeshRenderer.meshRenderers[index];
                thisRenderer.BindRendering();
                thisRenderer.shader.SetMatrix4("view", view);

                if (!ortho)
                    thisRenderer.shader.SetMatrix4("projection", projection);
                else
                    thisRenderer.shader.SetMatrix4("projection", orthoProjection);

                if(thisRenderer.mesh.triangleData == null || thisRenderer.mesh.triangleData.Length == 0)
                    GL.DrawArrays(PrimitiveType.Triangles, 0, thisRenderer.mesh.vertices.Length);
                else
                    GL.DrawElements(PrimitiveType.Triangles, thisRenderer.mesh.triangleData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
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

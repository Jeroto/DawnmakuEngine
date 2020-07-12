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
        bool ortho = true;
        float time;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            GameMaster.gameMaster.windowWidth = Width;
            GameMaster.gameMaster.windowHeight = Height;
        }


        protected override void OnLoad(EventArgs e)
        {
            DataLoader loader = new DataLoader("1");
            loader.InitializeGame();
            loader.InitializeStage();

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


            GameMaster.gameMaster.curCharName = "reimu";
            PlayerController.SpawnPlayer();

            Entity text = new Entity("FPSCounter", new Vector3(.5f, -.5f, 0), Vector3.Zero, Vector3.One);

            Entity gameBorder = new Entity("GameBorder", new Vector3(96.5f,225.5f, 0));
            gameBorder.AddElement(new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "gameborder", BufferUsageHint.StaticDraw, true));
            gameBorder.GetElement<MeshRenderer>().shader = GameMaster.gameMaster.shaders["spriteshader"];
            TextureAnimator.AnimationState newState = new TextureAnimator.AnimationState();
            newState.animFrames = new TextureAnimator.AnimationFrame[1] { new TextureAnimator.AnimationFrame() };
            newState.animFrames[0].frameDuration = 8;
            newState.animFrames[0].sprite = new SpriteSet.Sprite(0, 0, 1, 1, GameMaster.gameMaster.UITextures["gameborder"], false);
            newState.autoTransition = -1;
            gameBorder.AddElement(new TextureAnimator(new TextureAnimator.AnimationState[1] { newState }, gameBorder.GetElement<MeshRenderer>()));

            /*TextRenderer textRend = new TextRenderer();
            textRend.SetDrawingColor(255, 0, 0, 255);
            textRend.textToWrite = "FPS:";
            textRend.font = new QuickFont.QFont("Fonts/WitchingHour.ttf", 72, new QuickFont.Configuration.QFontBuilderConfiguration());
            textRend.SetUp();
            text.AddElement(textRend);*/

            Entity debugCam = new Entity("BackgroundCamera", new Vector3(0, 25, 100)), debugCam2 = new Entity("Camera", new Vector3(96.5f,225.5f,3));
            debugCam.AddElement(new CameraElement(true));
            debugCam.GetElement<CameraElement>().AddLayer(0);

            debugCam2.AddElement(new CameraElement(false));
            debugCam2.GetElement<CameraElement>().SetAllLayersRenderable();
            debugCam2.GetElement<CameraElement>().RemoveLayer(0);

            Entity stageController = new Entity("StageControl");
            stageController.AddElement(new StageElement(loader.stageData));

            /*BulletElement.BulletStage[] stages = new BulletElement.BulletStage[1];
            stages[0] = new BulletElement.BulletStage();
            stages[0].spriteType = "round";
            stages[0].bulletColor = 5;
            stages[0].movementDirection = new Vector2(0, -1);
            stages[0].endingSpeed = 25;

            BulletElement.SpawnBullet(stages, new Vector3(0, GameMaster.gameMaster.playerBoundsY.Y, 0));*/
            
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.DepthFunc(DepthFunction.Less);

            Entity debugText = new Entity("DebugText", new Vector3(300, 450, 0));
            TextRenderer textRenderer = new TextRenderer();
            debugText.AddElement(textRenderer);
            textRenderer.WriteFontFamilyName = "Arial";
            //textRenderer.WriteFontFamilyName = "Witching Hour";
            textRenderer.currentShader = GameMaster.gameMaster.shaders["textshader"];
            textRenderer.TextSize = 50;
            textRenderer.Text = "Normal";
            /*MeshRenderer textMesh = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "borderui", BufferUsageHint.StaticDraw, 
                GameMaster.gameMaster.generalTextShader, null, true);
            textMesh.colorR = 150;
            textMesh.colorG = 150;
            debugText.AddElement(textMesh);*/

            debugText = new Entity("ScoreLabel", new Vector3(300, 400, 0));
            textRenderer = new TextRenderer();
            debugText.AddElement(textRenderer);
            textRenderer.WriteFontFamilyName = "Arial";
            //textRenderer.WriteFontFamilyName = "Witching Hour";
            textRenderer.currentShader = GameMaster.gameMaster.shaders["textshader"];
            textRenderer.TextSize = 50;
            textRenderer.Text = "Score";
            textRenderer.HoriAlign = SixLabors.Fonts.HorizontalAlignment.Right;
            /*textMesh = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "borderui", BufferUsageHint.StaticDraw,
                GameMaster.gameMaster.generalTextShader, null, true);
            textMesh.colorR = 150;
            textMesh.colorG = 150;
            debugText.AddElement(textMesh);*/

            debugText = new Entity("ScoreVal", new Vector3(300, 400, 0));
            textRenderer = new TextRenderer();
            debugText.AddElement(textRenderer);
            textRenderer.WriteFontFamilyName = "Arial";
            //textRenderer.WriteFontFamilyName = "Witching Hour";
            textRenderer.currentShader = GameMaster.gameMaster.shaders["textshader"];
            textRenderer.TextSize = 35;
            textRenderer.Text = "000000000";
            textRenderer.HoriAlign = SixLabors.Fonts.HorizontalAlignment.Left;
            /*textMesh = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "borderui", BufferUsageHint.StaticDraw,
                GameMaster.gameMaster.generalTextShader, null, true);
            debugText.AddElement(textMesh);*/

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
                if (InputScript.pauseDown)
                {
                    gameMaster.paused = !gameMaster.paused;
                    //Exit();
                }

                if (input.IsKeyDown(Key.F))
                    TargetUpdateFrequency = 240;
                if (input.IsKeyDown(Key.L))
                    TargetUpdateFrequency = 60;

                if(input.IsKeyDown(Key.B))
                {
                    BulletElement.BulletStage[] stage = new BulletElement.BulletStage[] { new BulletElement.BulletStage() };
                    stage[0].spriteType = gameMaster.bulletTypes[BulletElement.Random(0, gameMaster.bulletTypes.Count - 1)];
                    stage[0].bulletColor = BulletElement.Random(0, gameMaster.bulletSprites[stage[0].spriteType].sprites.Count - 1);
                    stage[0].movementDirection = DawnMath.RandomCircle();
                    stage[0].startingSpeed = 20;
                    stage[0].endingSpeed = 10;
                    stage[0].framesToChangeSpeed = 60;
                    stage[0].rotate = BulletElement.ShouldTurn(stage[0].spriteType);
                    BulletElement.SpawnBullet(stage, gameMaster.playerEntity.WorldPosition + new Vector3(stage[0].movementDirection * 20),
                        BulletElement.ShouldSpin(stage[0].spriteType));
                }
                if(input.IsKeyDown(Key.I))
                {
                    ItemElement.SpawnItem(gameMaster.itemData[gameMaster.itemTypes[ItemElement.Random(0, gameMaster.itemData.Count)]],
                        gameMaster.playerEntity.WorldPosition + new Vector3(0, 16, 0));
                }


                uint prevScore = gameMaster.score;
                if (input.IsKeyDown(Key.W))
                {
                    gameMaster.score++;
                }
                if (input.IsKeyDown(Key.S))
                {
                    gameMaster.score--;
                }

                if(gameMaster.score != prevScore)
                    Entity.FindEntity("ScoreVal").GetElement<TextRenderer>().Text = gameMaster.score.ToString("000000000");




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

            GameMaster.gameMaster.windowWidth = Width;
            GameMaster.gameMaster.windowHeight = Height;

            //GL.BindVertexArray(vertexArrayObject);

            /*texture1.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);*/

            GameMaster.gameMaster.ElementPreRender();

            Render();

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected void Render()
        {
            int camCount = CameraElement.cameras.Count, layersCount, meshCount,
                c, r, m;
            CameraElement thisCam;
            MeshRenderer thisRenderer;
            for (c = 0; c < camCount; c++)
            {
                thisCam = CameraElement.cameras[c];
                if (!thisCam.enabled)
                    continue;
                layersCount = thisCam.renderableLayers.Count;
                for (r = 0; r < layersCount; r++)
                {
                    SetLayerSettings(GameMaster.renderLayerSettings[thisCam.renderableLayers[r]]);
                    meshCount = MeshRenderer.renderLayers[thisCam.renderableLayers[r]].Count;
                    for (m = 0; m < meshCount; m++)
                    {
                        thisRenderer = MeshRenderer.renderLayers[thisCam.renderableLayers[r]][m];

                        if (!thisRenderer.IsEnabled)
                            continue;

                        thisRenderer.BindRendering();

                        thisRenderer.shader.SetMatrix4("view", thisCam.ViewMatrix);
                        thisRenderer.shader.SetMatrix4("projection", thisCam.ProjectionMatrix);

                        if (thisRenderer.mesh.triangleData == null || thisRenderer.mesh.triangleData.Length == 0)
                            GL.DrawArrays(PrimitiveType.Triangles, 0, thisRenderer.mesh.vertices.Length);
                        else
                        {
                            GL.DrawElements(PrimitiveType.Triangles, thisRenderer.mesh.triangleData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                        }
                    }
                }
            }
        }

        protected void SetLayerSettings(GameMaster.RenderLayer layerSettings)
        {
            if(layerSettings.hasDepth)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Height / (float)Width < 960 / 1280f)
                Height = DawnMath.Round(Width * (960 / 1280f));
            else if (Width / (float)Height < 1280f / 960)
                Width = DawnMath.Round(Height * (1280f / 960));

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

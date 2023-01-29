using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Desktop;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;
using Microsoft.VisualBasic.CompilerServices;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

using SixLabors.ImageSharp;
using DawnmakuEngine.Data.Resources;

namespace DawnmakuEngine
{
    public class Game : GameWindow
    {
        bool ortho = true;
        float time;

        public Game(GameWindowSettings gameWindow, NativeWindowSettings nativeWindow) : base(gameWindow, nativeWindow)
        {
            GameMaster.gameMaster.currentSize = nativeWindow.Size;
            GameMaster.window = this;
        }


        protected override void OnLoad()
        {
            DataLoader loader = new DataLoader("1");

            GL.ClearColor(0f, 0f, 0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            Context.SwapBuffers();

            loader.InitializeGame();

            ResizeViewport(Size);

            loader.InitializeStage();

            Context.SwapBuffers();
            GL.ClearColor(0f, 0f, 0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.Enable(EnableCap.ScissorTest);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GameMaster.gameMaster.curCharName = "reimu";
            PlayerController.SpawnPlayer();

            Entity text = new Entity("FPSCounter", new Vector3(.5f, -.5f, 0), Vector3.Zero, Vector3.One);

            Entity gameBorder = new Entity("GameBorder", new Vector3(96.5f,225.5f, 0));
            gameBorder.AddElement(new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "gameborder", BufferUsageHint.StaticDraw));
            gameBorder.GetElement<MeshRenderer>().shader = GameMaster.gameMaster.shaders["spriteshader"];
            SpriteRenderer borderSpriteRend = new SpriteRenderer(gameBorder.GetElement<MeshRenderer>());
            borderSpriteRend.Sprite = GameMaster.gameMaster.uiSprites["gameborder"].sprites[0];

            gameBorder.AddElement(borderSpriteRend);

            

            Entity debugCam = new Entity("BackgroundCamera", new Vector3(0, 25, 100)), debugCam2 = new Entity("Camera", new Vector3(96.5f,225.5f,3));
            debugCam.AddElement(new CameraElement(true));
            debugCam.GetElement<CameraElement>().AddLayer(0);

            debugCam2.AddElement(new CameraElement(false));
            debugCam2.GetElement<CameraElement>().SetAllLayersRenderable();
            debugCam2.GetElement<CameraElement>().RemoveLayer(0);

            Entity stageController = new Entity("StageControl");
            stageController.AddElement(new StageElement(loader.stageData));

            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.DepthFunc(DepthFunction.Less);


            TextRenderer.Create(new Vector3(320, 400, 0), Vector3.Zero, true, "Arial", GameMaster.gameMaster.shaders["spriteshader"],
                            new Vector4(255, 255, 255, 255), 35, SixLabors.Fonts.HorizontalAlignment.Right, SixLabors.Fonts.VerticalAlignment.Center, "Score", 16, "ScoreLabel");

            TextRenderer.Create(new Vector3(320, 350, 0), Vector3.Zero, true, "Arial", GameMaster.gameMaster.shaders["spriteshader"],
                            new Vector4(255, 255, 255, 255), 35, SixLabors.Fonts.HorizontalAlignment.Center, SixLabors.Fonts.VerticalAlignment.Center, "000000000", 16, "ScoreVal");

            ResourceGroup meterResources = new ResourceGroup(new List<BaseResource>() { GameMaster.gameMaster.resources["power"] });
            ResourceMeter.Create(new Vector3(280, 250, 0), Vector3.Zero, GameMaster.gameMaster.uiSprites["powermeter"].sprites[0],
                                new Vector2(120, 24), GameMaster.gameMaster.shaders["spriteshader"], meterResources, ResourceGroup.CalculationType.Add);

            TextRenderer.Create(new Vector3(285, 262, 0), Vector3.Zero, true, "Arial", GameMaster.gameMaster.shaders["spriteshader"],
                            new Vector4(173, 13, 40, 255), 35, SixLabors.Fonts.HorizontalAlignment.Right, SixLabors.Fonts.VerticalAlignment.Bottom, "Power", 16, "meterlabel");

            ResourceValueText.Create(new Vector3(350, 262, 0), Vector3.Zero, "Arial", GameMaster.gameMaster.shaders["spriteshader"],
                    new Vector4(255, 170, 170, 255), 25, SixLabors.Fonts.HorizontalAlignment.Center, SixLabors.Fonts.VerticalAlignment.Center, 15,
                    GameMaster.gameMaster.resources["power"], 6, "metervalue");


            TextRenderer.Create(new Vector3(205, 300, 0), Vector3.Zero, true, "Arial", GameMaster.gameMaster.shaders["spriteshader"],
                            new Vector4(239, 67, 182, 255), 2, SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment.Bottom, "Lives", 11, "liveslabel");

            List<SpriteSet.Sprite> graphicSprites = new List<SpriteSet.Sprite>();
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["emptylifeheart"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["fulllifeheart"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["onethirdslifeheart"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["halflifeheart"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["twothirdslifeheart"].sprites[0]);

            ResourceGroup graphicResGroup = new ResourceGroup(new List<BaseResource>() { GameMaster.gameMaster.resources["lives"] });

            ResourceSpriteGraphic.Create(new Vector3(263, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { 0 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(280, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -1 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(297, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -2 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(314, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -3 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(331, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -4 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(348, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -5 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(365, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -6 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(382, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -7 } }, "lifeheart");

            ResourceSpriteGraphic.Create(new Vector3(399, 300, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 1, new object[1][] { new object[1] { -8 } }, "lifeheart");


            graphicSprites = new List<SpriteSet.Sprite>();
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["emptyufo"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["redufo"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["greenufo"].sprites[0]);
            graphicSprites.Add(GameMaster.gameMaster.uiSprites["blueufo"].sprites[0]);

            graphicResGroup = new ResourceGroup(new List<BaseResource>() { GameMaster.gameMaster.resources["ufos"] });

            for (int i = 0; i < 21; i++)
            {
                ResourceSpriteGraphic.Create(new Vector3(-180 + (18 * i), 27, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 3, new object[1][] { new object[1] { i } }, "ufo" + i.ToString());
            }

            for (int i = 0; i < 21; i++)
            {
                ResourceSpriteGraphic.Create(new Vector3(-180 + (18 * i), 9, 0), Vector3.Zero, Vector3.One, GameMaster.gameMaster.shaders["spriteshader"], graphicSprites,
                new Vector4(255, 170, 170, 255), true, graphicResGroup, ResourceGroup.CalculationType.Add, 0, 3, new object[1][] { new object[1] { i + 21 } }, "ufo" + (i + 21).ToString());
            }

            //GameMaster.gameMaster.fontImages["Arial"].SaveAsPng("D:/Damio/source/repos/DawnmakuEngine/DawnmakuEngine/bin/Debug/netcoreapp3.1/Data/General/Fonts/arial.png");
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = KeyboardState;
            GameMaster gameMaster = GameMaster.gameMaster;

            gameMaster.GameMasterUpdate();

            if (IsFocused)
            {
                if (InputScript.pauseDown)
                {
                    gameMaster.paused = !gameMaster.paused;
                    //Exit();
                }

                if(input.IsKeyDown(Keys.LeftAlt) && input.IsKeyPressed(Keys.Enter))
                {
                    GameMaster.playerSettings.fullscreen = !GameMaster.playerSettings.fullscreen;
                    FullScreen(GameMaster.playerSettings.fullscreen);
                }

                DebugKeys(input);

                InputScript.GetInput();
            }
            else
                InputScript.ClearInput();

            //Entity.FindEntity("FPSCounter").GetElement<TextRenderer>().textToWrite = "FPS:" + UpdateFrequency.ToString("0.00");

            gameMaster.ElementUpdate();


            if(toggleUFO && ItemElement.Random(0, 60) == 0)
            {
                FloatAroundItem.SpawnItem(new List<ItemData>() { gameMaster.itemData["redufo"], gameMaster.itemData["greenufo"], gameMaster.itemData["blueufo"] },
                    new Vector3(ItemElement.Random(-100f, 100f), 350, 0), ItemElement.Random(0, 3));
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

        bool toggleUFO;
        protected void DebugKeys(KeyboardState input)
        {
            GameMaster gameMaster = GameMaster.gameMaster;

            if (input.IsKeyDown(Keys.F))
                UpdateFrequency = 240;
            if (input.IsKeyDown(Keys.L))
                UpdateFrequency = 60;

            if (input.IsKeyPressed(Keys.U))
                toggleUFO = !toggleUFO;

            if (input.IsKeyDown(Keys.B))
            {
                BulletElement.BulletStage[] stage = new BulletElement.BulletStage[] { new BulletElement.BulletStage() };
                stage[0].bulletType = gameMaster.bulletTypes[BulletElement.Random(0, gameMaster.bulletTypes.Count)];
                stage[0].bulletColor = BulletElement.Random(0, gameMaster.bulletSprites[stage[0].bulletType].sprites.Count - 1);
                stage[0].movementDirection = DawnMath.RandomCircle();
                stage[0].startingSpeed = 20;
                stage[0].endingSpeed = 10;
                stage[0].framesToChangeSpeed = 60;
                stage[0].rotate = BulletElement.ShouldTurn(stage[0].bulletType);
                BulletElement.SpawnBullet(stage, gameMaster.playerEntity.WorldPosition + new Vector3(stage[0].movementDirection * 20),
                    BulletElement.ShouldSpin(stage[0].bulletType));
            }
            if(input.IsKeyDown(Keys.I))
            {
                ItemElement.SpawnItem(gameMaster.itemData["poweritem"],
                    gameMaster.playerEntity.WorldPosition + new Vector3(0, 100, 0));
            }
            if (input.IsKeyDown(Keys.Apostrophe))
            {
                ItemElement.SpawnItem(gameMaster.itemData["lifeitem"],
                    gameMaster.playerEntity.WorldPosition + new Vector3(0, 100, 0));
            }


            if (input.IsKeyPressed(Keys.Semicolon))
            {
                ItemElement.SpawnItem(gameMaster.itemData["bombitem"],
                    gameMaster.playerEntity.WorldPosition + new Vector3(0, 100, 0));
            }
            if (input.IsKeyPressed(Keys.Slash))
            {
                ItemElement.SpawnItem(gameMaster.itemData["pointitem"],
                    gameMaster.playerEntity.WorldPosition + new Vector3(0, 100, 0));
            }


            uint prevScore = gameMaster.score;
            if (input.IsKeyDown(Keys.W))
            {
                gameMaster.resources["power"].ModifyValue(1);
                //gameMaster.score++;
            }
            if (input.IsKeyDown(Keys.S))
            {
                //gameMaster.score--;
                gameMaster.resources["power"].ModifyValue(-1);
            }

            if (input.IsKeyPressed(Keys.W))
            {
                gameMaster.resources["lives"].ModifyValue(1);
            }
            if (input.IsKeyPressed(Keys.S))
            {
                gameMaster.resources["lives"].ModifyValue(-1);
            }


            if (input.IsKeyPressed(Keys.D1))
            {
                gameMaster.resources["ufos"].ModifyValue(0, 0);
            }
            if (input.IsKeyPressed(Keys.D2))
            {
                gameMaster.resources["ufos"].ModifyValue(0, 1);
            }
            if (input.IsKeyPressed(Keys.D3))
            {
                gameMaster.resources["ufos"].ModifyValue(0, 2);
            }
            if (input.IsKeyPressed(Keys.D4))
            {
                gameMaster.resources["ufos"].ModifyValue(0, 3);
            }

            if (input.IsKeyPressed(Keys.D5))
            {
                gameMaster.resources["ufos"].ModifyValue(1, 0);
            }
            if (input.IsKeyPressed(Keys.D6))
            {
                gameMaster.resources["ufos"].ModifyValue(1, 1);
            }
            if (input.IsKeyPressed(Keys.D7))
            {
                gameMaster.resources["ufos"].ModifyValue(1, 2);
            }
            if (input.IsKeyPressed(Keys.D8))
            {
                gameMaster.resources["ufos"].ModifyValue(1, 3);
            }

            if (input.IsKeyPressed(Keys.D9))
            {
                gameMaster.resources["ufos"].ModifyValue(2, 0);
            }
            if (input.IsKeyPressed(Keys.D0))
            {
                gameMaster.resources["ufos"].ModifyValue(2, 1);
            }
            if (input.IsKeyPressed(Keys.Minus))
            {
                gameMaster.resources["ufos"].ModifyValue(2, 2);
            }
            if (input.IsKeyPressed(Keys.Equal))
            {
                gameMaster.resources["ufos"].ModifyValue(2, 3);
            }


            if (gameMaster.score != prevScore)
                Entity.FindEntity("ScoreVal").GetElement<TextRenderer>().Text = gameMaster.score.ToString("000000000");


            if (InputScript.bombDown)
                gameMaster.audioManager.PlaySound(gameMaster.sfx["playerdeath"], AudioController.AudioCategory.Player);
            else if (InputScript.bombUp)
                gameMaster.audioManager.PlaySound(gameMaster.sfx["playerdeath"], AudioController.AudioCategory.Player, 0.5f);


            if (input.IsKeyDown(Keys.H))
            {
                gameMaster.timeScaleUpdate = 0.5f;
            }
            else if (input.IsKeyDown(Keys.R))
            {
                gameMaster.timeScaleUpdate = 1f;
            }

            if (input.IsKeyPressed(Keys.Space))
            {
                switch (Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().HoriAlign)
                {
                    case SixLabors.Fonts.HorizontalAlignment.Left:
                        Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().HoriAlign = SixLabors.Fonts.HorizontalAlignment.Center;
                        Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().UpdateText();
                        break;
                    case SixLabors.Fonts.HorizontalAlignment.Center:
                        Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().HoriAlign = SixLabors.Fonts.HorizontalAlignment.Right;
                        Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().UpdateText();
                        break;
                    case SixLabors.Fonts.HorizontalAlignment.Right:
                        Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().HoriAlign = SixLabors.Fonts.HorizontalAlignment.Left;
                        Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().UpdateText();
                        break;
                }
            }

            if (input.IsKeyPressed(Keys.K))
            {
                Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().Kerning = !Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().Kerning;
                if (Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().Kerning)
                    Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().Text = "Kerning: On";
                else
                    Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>().Text = "Kerning: Off";
            }

            if (InputScript.specialDown)
            {
                if (InputScript.Focus)
                    gameMaster.currentPowerLevel--;
                else
                    gameMaster.currentPowerLevel++;
            }

            if (input.IsKeyDown(Keys.Equal))
            {
                TextRenderer textRend = Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>();
                Vector4 color = textRend.Color;
                if (input.IsKeyDown(Keys.R))
                    color.X += 2;
                if (input.IsKeyDown(Keys.G))
                    color.Y += 2;
                if (input.IsKeyDown(Keys.B))
                    color.Z += 2;
                if (input.IsKeyDown(Keys.A))
                    color.W += 2;
                textRend.Color = color;
            }
            else if (input.IsKeyDown(Keys.Minus))
            {
                TextRenderer textRend = Entity.FindEntity("ScoreLabel").GetElement<TextRenderer>();
                Vector4 color = textRend.Color;
                if (input.IsKeyDown(Keys.R))
                    color.X -= 2;
                if (input.IsKeyDown(Keys.G))
                    color.Y -= 2;
                if (input.IsKeyDown(Keys.B))
                    color.Z -= 2;
                if (input.IsKeyDown(Keys.A))
                    color.W -= 2;
                textRend.Color = color;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            time += 32 * (float)e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GameMaster.gameMaster.currentSize = Size;

            //GL.BindVertexArray(vertexArrayObject);

            /*texture1.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);*/

            GameMaster.StartTimer();
            GameMaster.gameMaster.ElementPreRender();
            GameMaster.LogTimeMilliseconds("all pre-renders");

            GameMaster.gameMaster.enemyBulletSpawnSoundPlayed = false;
            GameMaster.gameMaster.enemyBulletStageSoundPlayed = false;
            GameMaster.gameMaster.playerBulletSpawnSoundPlayed = false;
            GameMaster.gameMaster.playerBulletStageSoundPlayed = false;

            Render();

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected void Render()
        {
            int camCount = CameraElement.cameras.Count, layersCount, meshCount,
                c, r, m;
            double totalMilliseconds = 0;
            long totalTicks = 0;
            CameraElement thisCam;
            MeshRenderer thisRenderer;
            Vector4 ambientLight;
            if (GameMaster.gameMaster.overrideAmbient)
                ambientLight = new Vector4(GameMaster.gameMaster.overrideAmbR, GameMaster.gameMaster.overrideAmbG, GameMaster.gameMaster.overrideAmbB, 0) * GameMaster.gameMaster.overrideAmbIntensity;
            else
                ambientLight = new Vector4(GameMaster.gameMaster.ambientR, GameMaster.gameMaster.ambientG, GameMaster.gameMaster.ambientB, 0) * GameMaster.gameMaster.ambientIntensity;
            ambientLight.W = 1;
            for (c = 0; c < camCount; c++)
            {
                thisCam = CameraElement.cameras[c];
                if (!thisCam.enabled)
                    continue;
                layersCount = thisCam.renderableLayers.Count;
                for (r = 0; r < layersCount; r++)
                {
                    GameMaster.StartTimer();
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
                        thisRenderer.shader.SetVector4("ambientLight", ambientLight);


                        GL.DrawElements(PrimitiveType.Triangles, thisRenderer.mesh.triangleData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                    }
                    
                    GameMaster.LogTimeMilliseconds("rendering layer " + thisCam.renderableLayers[r].ToString() + " for " + thisCam.EntityAttachedTo.Name);
                    totalMilliseconds += GameMaster.timeLogger.Elapsed.TotalMilliseconds; 
                    totalTicks += GameMaster.timeLogger.ElapsedTicks;
                }
            }
            GameMaster.LogTimeCustMillisecondsAndTicks("rendering", totalMilliseconds, totalTicks, "Total time taken for ");
        }

        protected void SetLayerSettings(GameMaster.RenderLayer layerSettings)
        {
            if(layerSettings.hasDepth)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
        }
        
        public void FullScreen(bool fullscreen)
        {
            if(fullscreen)
                WindowState = WindowState.Fullscreen;
            else
                WindowState = WindowState.Normal;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            ResizeViewport(e.Size);
        }

        protected void ResizeViewport(Vector2 size)
        {
            Vector2 aspectRatio = GameMaster.gameMaster.aspectRatio;

            //Set ratio to smaller between X and Y
            float ratioX = size.X / aspectRatio.X;
            float ratioY = size.Y / aspectRatio.Y;
            float ratio = ratioX < ratioY ? ratioX : ratioY;

            //Calculate the viewport width and height
            int viewWidth = (int)(aspectRatio.X * ratio);
            int viewHeight = (int)(aspectRatio.Y * ratio);

            //Calculate the viewport offset to keep it centered
            int viewX = (int)((size.X - aspectRatio.X * ratio) / 2);
            int viewY = (int)((size.Y - aspectRatio.Y * ratio) / 2);

            //Set viewport position + size and GL Scissor position + size
            GL.Scissor(viewX, viewY, viewWidth, viewHeight);
            GL.Viewport(viewX, viewY, viewWidth, viewHeight);
        }

        protected override void OnUnload()
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
            base.OnUnload();
        }
    }
}

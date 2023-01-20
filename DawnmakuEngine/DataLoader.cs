using System;
using System.Collections.Generic;
using System.IO;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;
using static DawnmakuEngine.DawnMath;
using OpenTK;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Typography.OpenFont;
using OpenTK.Graphics.ES30;

using Newtonsoft.Json;

//using System.Drawing;
using OpenTK.Mathematics;
//using System.Drawing.Text;
using SixLabors.ImageSharp.ColorSpaces;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;
using System.Runtime.InteropServices;
using DawnmakuEngine.Data.Resources;
using DawnmakuEngine.Data.JSONFormats;
using System.Threading;
//using System.Windows.Media;

namespace DawnmakuEngine
{
    class DataLoader
    {
        GameMaster gameMaster;
        public string directory, containingFolder, generalDir, bulletDataDir, playerDataDir, itemDataDir,
            generalTexDir, bulletTexDir, playerTexDir, playerOrbTexDir, playerFxTexDir, itemTexDir, enemyTexDir, UITexDir, loadTexDir,
            generalAnimDir, bulletAnimDir, playerAnimDir, playerOrbAnimDir, playerFxAnimDir, itemAnimDir, enemyAnimDir, texAnimDir,
            playerPatternDir, playerOrbDir, playerShotTypeDir, playerShotTypePartsDir, playerCharactersDir,
            soundEffectDir,
            shaderDir, fontDir,
            stageDir, curStageDir, enemyDir, enemyPatternDir, enemyMovementDir, backgroundDir, backgroundModelDir, backgroundTexDir,
            backgroundSecDir;

        public StageData stageData;

        public DataLoader(string stageName)
        {
            gameMaster = GameMaster.gameMaster;
            directory = new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).AbsolutePath;
            //Console.WriteLine(directory);
            /*while(directory[directory.Length - 1] != '/')
            {
                directory = directory.Remove(directory.Length - 1, 1);
            }*/
            DirectoryInfo dir = Directory.GetParent(directory);
            containingFolder = dir.FullName;

            //Console.WriteLine(directory);
            DirectoryInfo[] dirs = dir.GetDirectories();
            int i = 0;
            for (i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == "Data")
                {
                    directory = dirs[i].FullName;
                    dirs = dirs[i].GetDirectories();
                    break;
                }
            }
            for (i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == "Stages")
                {
                    stageDir = dirs[i].FullName;
                }
                else if (dirs[i].Name == "General")
                {
                    generalDir = dirs[i].FullName;
                }
            }
            dirs = new DirectoryInfo(generalDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "Textures":
                        generalTexDir = dirs[i].FullName;
                        break;
                    case "Anims":
                        generalAnimDir = dirs[i].FullName;
                        break;
                    case "BulletData":
                        bulletDataDir = dirs[i].FullName;
                        break;
                    case "PlayerData":
                        playerDataDir = dirs[i].FullName;
                        break;
                    case "ItemData":
                        itemDataDir = dirs[i].FullName;
                        break;
                    case "Shaders":
                        shaderDir = dirs[i].FullName;
                        break;
                    case "Fonts":
                        fontDir = dirs[i].FullName;
                        break;
                    case "Sfx":
                        soundEffectDir = dirs[i].FullName;
                        break;
                }
            }
            dirs = new DirectoryInfo(generalTexDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "Bullets":
                        bulletTexDir = dirs[i].FullName;
                        break;
                    case "Enemies":
                        enemyTexDir = dirs[i].FullName;
                        break;
                    case "UI":
                        UITexDir = dirs[i].FullName;
                        break;
                    case "Player":
                        playerTexDir = dirs[i].FullName;
                        break;
                    case "PlayerOrbs":
                        playerOrbTexDir = dirs[i].FullName;
                        break;
                    case "PlayerEffects":
                        playerFxTexDir = dirs[i].FullName;
                        break;
                    case "Items":
                        itemTexDir = dirs[i].FullName;
                        break;
                    case "Loading":
                        loadTexDir = dirs[i].FullName;
                        break;
                }
            }
            dirs = new DirectoryInfo(playerDataDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "Orbs":
                        playerOrbDir = dirs[i].FullName;
                        break;
                    case "ShotTypes":
                        playerShotTypeDir = dirs[i].FullName;
                        break;
                    case "ShotTypeParts":
                        playerShotTypePartsDir = dirs[i].FullName;
                        break;
                    case "Characters":
                        playerCharactersDir = dirs[i].FullName;
                        break;
                    case "Patterns":
                        playerPatternDir = dirs[i].FullName;
                        break;
                }
            }

            dirs = new DirectoryInfo(generalAnimDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "EnemyAnims":
                        enemyAnimDir = dirs[i].FullName;
                        break;
                    case "TextureAnims":
                        texAnimDir = dirs[i].FullName;
                        break;
                    case "BulletAnims":
                        bulletAnimDir = dirs[i].FullName;
                        break;
                    case "PlayerAnims":
                        playerAnimDir = dirs[i].FullName;
                        break;
                    case "PlayerOrbAnims":
                        playerOrbAnimDir = dirs[i].FullName;
                        break;
                    case "PlayerEffectAnims":
                        playerFxAnimDir = dirs[i].FullName;
                        break;
                    case "ItemAnims":
                        itemAnimDir = dirs[i].FullName;
                        break;
                }
            }

            dirs = new DirectoryInfo(stageDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == stageName)
                {
                    curStageDir = dirs[i].FullName;
                    dirs = dirs[i].GetDirectories();
                    break;
                }
            }
            for (i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == "Enemies")
                {
                    enemyDir = dirs[i].FullName;
                }
                if (dirs[i].Name == "Background")
                {
                    backgroundDir = dirs[i].FullName;
                }
            }
            dirs = new DirectoryInfo(enemyDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "Patterns":
                        enemyPatternDir = dirs[i].FullName;
                        break;
                    case "Movement":
                        enemyMovementDir = dirs[i].FullName;
                        break;
                }
            }

            dirs = new DirectoryInfo(backgroundDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "Models":
                        backgroundModelDir = dirs[i].FullName;
                        break;
                    case "Textures":
                        backgroundTexDir = dirs[i].FullName;
                        break;
                    case "Sections":
                        backgroundSecDir = dirs[i].FullName;
                        break;
                }
            }

            dirs = dirs[0].Parent.Parent.GetDirectories();
        }

        public string GetFileNameOnly(string path)
        {
            string[] filename = path.Split('\\', StringSplitOptions.RemoveEmptyEntries);
            return filename[filename.Length - 1].Split('.', StringSplitOptions.RemoveEmptyEntries)[0].ToLower().Replace(" ", "");
        }

        #region Load Callers
        /// <summary>
        /// Load consistent game data only once
        /// </summary>
        public void InitializeGame()
        {
            GameMaster.StartTimer();
            ShaderLoader();

            {
                if (!File.Exists(containingFolder + "\\settings.dwnsettings"))
                    GeneratePlayerSettings();
                else
                    ReadPlayerSettings(containingFolder + "\\settings.dwnsettings");
            }

            RenderLoadBackdrop();

            LoadGame();
            LoadAudio();
            LoadBullets();
            LoadPlayerOrbs();
            LoadPlayerEffects();
            LoadPlayers();
            LoadItems();
            LoadUI();
            GameMaster.LogTimeMilliseconds("game init");
        }

        public void LoadGame()
        {
            GameSettingsLoader();
            LoadResources();
        }

        /// <summary>
        /// Load stage-specific data
        /// </summary>
        public void InitializeStage()
        {
            GameMaster.StartTimer();
            LoadEnemies();
            LoadBackground();
            LoadStage(GetStageBGMPaths());
            GameMaster.LogTimeMilliseconds("stage init");
        }

        public void LoadBullets()
        {
            BulletTextureLoader();
            BulletSpriteLoader();
            BulletAnimLoader();
            BulletDataLoader();
        }
        public void LoadPlayerOrbs()
        {
            PlayerOrbTextureLoader();
            PlayerOrbSpriteLoader();
            PlayerOrbAnimLoader();
            PlayerPatternLoader();
            PlayerOrbDataLoader();
        }
        public void LoadPlayers()
        {
            PlayerTextureLoader();
            PlayerSpriteLoader();
            PlayerAnimLoader();
            PlayerShotLoader();
            PlayerTypeLoader();
            PlayerCharLoader();
        }
        public void LoadPlayerEffects()
        {
            PlayerFxTextureLoader();
            PlayerFxSpriteLoader();
            PlayerFxAnimLoader();
        }
        public void LoadItems()
        {
            ItemTextureLoader();
            ItemSpriteLoader();
            ItemAnimLoader();
            ItemDataLoader();
        }

        public void LoadAudio()
        {
            SoundEffectLoader();
        }
        public void LoadEnemies()
        {
            EnemyPatternLoader();
            EnemyMovementLoader();
            EnemyTextureLoader();
            EnemySpriteLoader();
            EnemyAnimLoader();
            EnemyDataLoader();
        }

        public void LoadBackground()
        {
            BackgroundObjLoader();
            BackgroundTextureLoader();
            BackgroundModelDataLoader();
            BackgroundSectionDataLoader();
        }

        public void LoadUI()
        {
            UITextureLoader();
            UISpriteLoader();

            FontLoader();
            BitmapFontLoader();
        }

        public void LoadStage(string[] bgmPaths)
        {
            StageDataLoader(bgmPaths);
            //Stage-specific data
        }
        #endregion

        #region Load Screen
        void RenderLoadBackdrop()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            Shader shader = gameMaster.shaders["spriteshader"];
            shader.Use();

            int vertexBufferHandle = GL.GenBuffer();
            int elementBufferHandle = GL.GenBuffer();

            float[] vertices = new float[] {
                -1, -1, 0,  0, 0,
                 1, -1, 0,  1, 0,
                 1,1, 0,  1, 1,
                -1,1, 0,  0, 1
            };

            uint[] triangleData = new uint[6]
            {
                0, 1, 2,
                0, 2, 3
            };

            Texture tex = new Texture(loadTexDir + "\\back.png", false);
            tex.Use();

            GameMaster.LogPositiveNotice("Load image backdrop loaded");

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int positionLocation = shader.GetAttribLocation("position");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            int texCoordLocation = shader.GetAttribLocation("texCoordAttrib");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, triangleData.Length * sizeof(uint), triangleData, BufferUsageHint.StaticDraw);


            shader.SetVector4("colorMod", new Vector4(1, 1, 1, 1));
            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetMatrix4("view", Matrix4.Identity);
            shader.SetMatrix4("projection", Matrix4.Identity);
            shader.SetVector4("ambientLight", new Vector4(1, 1, 1, 1));

            GL.DrawElements(PrimitiveType.Triangles, triangleData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GameMaster.window.Context.SwapBuffers();
            GameMaster.LogPositiveNotice("Load image backdrop rendered");
        }
        #endregion

        #region File Iterators

        #region Game Settings
        public void GameSettingsLoader()
        {
            string file = Directory.GetFiles(generalDir, "*.json")[0];

            GameMaster.LogPositiveNotice("Loading game settings...\n");
            GameSettings settings = ReadGameSettings(file);
            GameMaster.debugMode = settings.debugMode;
            GameMaster.ShowConsole(GameMaster.debugMode);

            gameMaster.logAllFontChars = settings.logAllFontChars && GameMaster.debugMode;
            gameMaster.canToggleInvincible = settings.canToggleInvincible && GameMaster.debugMode;
            gameMaster.logTimers = settings.logTimers && GameMaster.debugMode;


            gameMaster.maxPower = settings.maxPower;
            gameMaster.powerLostOnDeath = settings.powerLostOnDeath;
            gameMaster.powerTotalDroppedOnDeath = settings.powerTotalDroppedOnDeath;
            gameMaster.powerLevelSplits = settings.powerLevelSplits;
            gameMaster.fullPowerPOC = settings.fullPowerCollection;
            gameMaster.shiftForPOC = settings.shiftForCollection;

            gameMaster.playerBoundsX = settings.playerBoundsX;
            gameMaster.playerBoundsY = settings.playerBoundsY;
            gameMaster.grazeDistance = settings.grazeDistance;

            gameMaster.bulletBoundsX = settings.bulletBoundsX;
            gameMaster.bulletBoundsY = settings.bulletBoundsY;

            gameMaster.enemyBoundsX = settings.enemyBoundsX;
            gameMaster.enemyBoundsY = settings.enemyBoundsY;


            gameMaster.maxItemCount = settings.maxItemCount;
            gameMaster.pocHeight = settings.collectionHeight;
            gameMaster.itemDisableHeight = settings.itemDisableHeight;

            gameMaster.itemRandXRange = settings.itemRandXRange;
            gameMaster.itemRandYRange = settings.itemRandYRange;
            gameMaster.itemMaxFallSpeed = settings.itemMaxFallSpeed;
            gameMaster.itemGravAccel = settings.itemGravAccel;
            gameMaster.itemXDecel = settings.itemXDecel;
            gameMaster.itemMagnetDist = settings.itemMagnetDist;
            gameMaster.itemMagnetSpeed = settings.itemMagnetSpeed;
            gameMaster.itemDrawSpeed = settings.itemDrawSpeed;
            gameMaster.itemCollectDist = settings.itemCollectDist;

            gameMaster.generalTextShader = settings.generalTextShader;
            gameMaster.dialogueTextShader = settings.dialogueTextShader;

            gameMaster.mainStageNames = settings.mainStageFolderNames;
            gameMaster.exStageNames = settings.exStageFolderNames;
            gameMaster.languages = settings.languages;

            GameMaster.layerIndexes = settings.renderLayers;
            GameMaster.renderLayerSettings = settings.renderLayerSettings;
            for (int i = 0; i < GameMaster.layerIndexes.Count; i++)
            {
                MeshRenderer.renderLayers.Add(new List<MeshRenderer>());
            }
        }

        public void LoadResources()
        {
            try
            {
                GameMaster.LogPositiveNotice("\nResources:");
                if (!File.Exists(generalDir + "/Resources.json"))
                    throw new Exception("No 'Resources.json' file found.");
                string fileText = File.ReadAllText(generalDir + "/Resources.json");
                JsonSerializerSettings settings = new JsonSerializerSettings();
                JSONResource[] resources = JsonConvert.DeserializeObject<JSONResource[]>(fileText, settings);
                BaseResource newResource;
                Type resourceType;

                for (int i = 0; i < resources.Length; i++)
                {
                    GameMaster.Log(resources[i].name);

                    resourceType = Type.GetType("DawnmakuEngine.Data.Resources." + resources[i].type + "Resource");

                    newResource = (BaseResource)resourceType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { resources[i].name.ToLower() });

                    newResource.min = resources[i].min;
                    newResource.max = resources[i].max;

                    newResource.InitValue(resources[i].value);
                }

            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading the resource data!", e.Message);
            }
        }

        public void GeneratePlayerSettings()
        {
            GameMaster.LogWarningMessage("settings.dwnsettings not found", "creating settings file");
            File.Create(containingFolder + "\\settings.dwnsettings").Close();
            List<string> lines = new List<string>();
            lines.Add("windowsizeindex=0;");
            lines.Add("framecap=60;");
            lines.Add("fullscreen=false;");
            lines.Add("mastervolume=1.0;");
            lines.Add("bgmvolume=1.0;");
            lines.Add("sfxvolume=0.25;");
            lines.Add("bulletspawnvolume=0.02;");
            lines.Add("bulletstagevolume=0.1;");
            lines.Add("playershootvolume=0.75;");
            lines.Add("playerbulletvolume=0.2;");
            lines.Add("playerdeathvolume=1.0;");
            lines.Add("enemydeathvolume=0.5;");
            File.WriteAllLines(containingFolder + "\\settings.dwnsettings", lines.ToArray());
        }

        public void ReadPlayerSettings(string file)
        {
            string[] allLines = File.ReadAllLines(file);
            try
            {
                GameMaster.playerSettings.windowSizeIndex = GetParseRoundedNum(allLines, "windowsizeindex=");
                GameMaster.playerSettings.frameCap = GetParseRoundedNum(allLines, "framecap=");
                GameMaster.playerSettings.fullscreen = GetParseBool(allLines, "fullscreen=");
                GameMaster.playerSettings.masterVolume = GetParseRoundedNum(allLines, "mastervolume=");
                GameMaster.playerSettings.bgmVolume = GetParseRoundedNum(allLines, "bgmvolume=");
                GameMaster.playerSettings.sfxVolume = GetParseRoundedNum(allLines, "sfxvolume=");
                GameMaster.playerSettings.bulletSpawnVolume = GetParseRoundedNum(allLines, "bulletspawnvolume=");
                GameMaster.playerSettings.bulletStageVolume = GetParseRoundedNum(allLines, "bulletstagevolume=");
                GameMaster.playerSettings.playerShootVolume = GetParseRoundedNum(allLines, "playershootvolume=");
                GameMaster.playerSettings.playerBulletVolume = GetParseRoundedNum(allLines, "playerbulletvolume=");
                GameMaster.playerSettings.playerDeathVolume = GetParseRoundedNum(allLines, "playerdeathvolume=");
                GameMaster.playerSettings.enemyDeathVolume = GetParseRoundedNum(allLines, "enemydeathvolume=");
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading the player settings!", e.Message);
            }
        }
        #endregion

        #region Shaders
        public void ShaderLoader()
        {
            string[] files = Directory.GetFiles(shaderDir, "*.json");

            GameMaster.LogPositiveNotice("\nShaders:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.shaders.Add(GetFileNameOnly(files[i]), ReadShaderData(files[i]));
            }
        }
        #endregion

        #region Bullets
        public void BulletTextureLoader()
        {
            string[] files = Directory.GetFiles(bulletTexDir, "*.png");

            GameMaster.LogPositiveNotice("\nBullet Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.bulletTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }

        public void BulletSpriteLoader()
        {
            string[] files = Directory.GetFiles(bulletTexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nBullet Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                spritePairs = ReadSpriteData(files[i], gameMaster.bulletTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.bulletSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                    gameMaster.bulletTypes.Add(spritePairs[e].Key);
                }
            }
        }
        public void BulletAnimLoader()
        {
            string[] files = Directory.GetFiles(bulletAnimDir, "*.json");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            GameMaster.LogPositiveNotice("\nBullet Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                animPairs = ReadSpriteAnimData(files[i], gameMaster.bulletSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    GameMaster.LogSecondary(animPairs[p].Key);
                    gameMaster.bulletAnimStates[animPairs[p].Key] = animPairs[p].Value;
                }
            }
        }

        public void BulletDataLoader()
        {
            string[] files = Directory.GetFiles(bulletDataDir, "*.json");
            GameMaster.LogPositiveNotice("\nBullet Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.bulletData.Add(GetFileNameOnly(files[i]), ReadBulletData(files[i]));
            }
        }
        #endregion

        #region Player Orbs
        public void PlayerOrbTextureLoader()
        {
            string[] files = Directory.GetFiles(playerOrbTexDir, "*.png");

            GameMaster.LogPositiveNotice("\nPlayer Orb Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerOrbTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }

        public void PlayerOrbSpriteLoader()
        {
            string[] files = Directory.GetFiles(playerOrbTexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nPlayer Orb Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                spritePairs = ReadSpriteData(files[i], gameMaster.playerOrbTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.playerOrbSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }
        public void PlayerOrbAnimLoader()
        {
            string[] files = Directory.GetFiles(playerOrbAnimDir, "*.json");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            GameMaster.LogPositiveNotice("\nPlayer Orb Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                animPairs = ReadSpriteAnimData(files[i], gameMaster.playerOrbSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    GameMaster.LogSecondary(animPairs[p].Key);
                    gameMaster.playerOrbAnimStates[animPairs[p].Key] = animPairs[p].Value;
                }
            }
        }
        public void PlayerOrbDataLoader()
        {
            string[] files = Directory.GetFiles(playerOrbDir, "*.json");

            GameMaster.LogPositiveNotice("\nPlayer Orb Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerOrbData.Add(GetFileNameOnly(files[i]), ReadOrbData(files[i]));
            }
        }
        #endregion

        #region Player Characters
        public void PlayerTextureLoader()
        {
            string[] files = Directory.GetFiles(playerTexDir, "*.png");

            GameMaster.LogPositiveNotice("\nPlayer Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }
        public void PlayerSpriteLoader()
        {
            string[] files = Directory.GetFiles(playerTexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nPlayer Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                spritePairs = ReadSpriteData(files[i], gameMaster.playerTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.playerSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }
        public void PlayerAnimLoader()
        {
            string[] files = Directory.GetFiles(playerAnimDir, "*.json");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            GameMaster.LogPositiveNotice("\nPlayer Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                animPairs = ReadSpriteAnimData(files[i], gameMaster.playerSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    GameMaster.LogSecondary(animPairs[p].Key);
                    gameMaster.playerAnimStates[animPairs[p].Key] = animPairs[p].Value;
                }
            }
        }
        #endregion

        #region Player FX
        public void PlayerFxTextureLoader()
        {
            string[] files = Directory.GetFiles(playerFxTexDir, "*.png");

            GameMaster.LogPositiveNotice("\nPlayer Effect Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerEffectTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }
        public void PlayerFxSpriteLoader()
        {
            string[] files = Directory.GetFiles(playerFxTexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nPlayer Effect Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                spritePairs = ReadSpriteData(files[i], gameMaster.playerEffectTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.playerEffectSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }
        public void PlayerFxAnimLoader()
        {
            string[] files = Directory.GetFiles(playerFxAnimDir, "*.json");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            GameMaster.LogPositiveNotice("\nPlayer Effect Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                animPairs = ReadSpriteAnimData(files[i], gameMaster.playerEffectSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    GameMaster.LogSecondary(animPairs[p].Key);
                    gameMaster.playerEffectAnimStates[animPairs[p].Key] = animPairs[p].Value;
                }
            }
        }
        #endregion

        #region Player Shots
        public void PlayerShotLoader()
        {
            string[] files = Directory.GetFiles(playerShotTypePartsDir, "*.json");

            GameMaster.LogPositiveNotice("\nPlayer Shots:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerShot.Add(GetFileNameOnly(files[i]), ReadShotData(files[i]));
            }
        }
        public void PlayerTypeLoader()
        {
            string[] files = Directory.GetFiles(playerShotTypeDir, "*.json");

            GameMaster.LogPositiveNotice("\nPlayer Shot Types:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.playerTypes.Add(GetFileNameOnly(files[i]), ReadTypeData(files[i]));
            }
        }
        public void PlayerCharLoader()
        {
            string[] files = Directory.GetFiles(playerCharactersDir, "*.json");

            GameMaster.LogPositiveNotice("\nPlayer Characters:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.playerChars.Add(GetFileNameOnly(files[i]), ReadPlayerCharData(files[i]));
            }
        }
        public void PlayerPatternLoader()
        {
            string[] files = Directory.GetFiles(playerPatternDir + "/bullet", "*.json");


            GameMaster.LogPositiveNotice("\nPlayer Patterns(bullet):");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(files[i]));
            }

            /*files = Directory.GetFiles(playerPatternDir + "/laser", "*.json");

            GameMaster.LogPositiveNotice("\nPlayer Patterns(laser):");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerPatterns.Add(GetFileNameOnly(files[i]), ReadPatternDataJSON(files[i]));
            }*/
        }
        #endregion

        #region Items
        public void ItemTextureLoader()
        {
            string[] files = Directory.GetFiles(itemTexDir, "*.png");

            GameMaster.Log("\nItem Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.itemTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }
        public void ItemSpriteLoader()
        {
            string[] files = Directory.GetFiles(itemTexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nItem Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                spritePairs = ReadSpriteData(files[i], gameMaster.itemTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.itemSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }
        public void ItemAnimLoader()
        {
            string[] files = Directory.GetFiles(itemAnimDir, "*.json");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            GameMaster.LogPositiveNotice("\nItem Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                animPairs = ReadSpriteAnimData(files[i], gameMaster.itemSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    GameMaster.LogSecondary(animPairs[p].Key);
                    gameMaster.itemAnimStates[animPairs[p].Key] = animPairs[p].Value;
                }
            }
        }
        public void ItemDataLoader()
        {
            string[] files = Directory.GetFiles(itemDataDir, "*.json");

            GameMaster.LogPositiveNotice("\nItem Datas:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.itemData.Add(GetFileNameOnly(files[i]), ReadItemData(files[i]));
                gameMaster.itemTypes.Add(GetFileNameOnly(files[i]));
            }
        }
        #endregion

        #region Audio
        public void SoundEffectLoader()
        {
            string[] directories = Directory.GetDirectories(soundEffectDir),
                files;
            int f, i;
            for (i = 0; i < directories.Length; i++)
            {
                files = Directory.GetFiles(directories[i], "*.mp3");
                for (f = 0; f < files.Length; f++)
                    gameMaster.sfx.Add(GetFileNameOnly(files[f]), new AudioData(files[f]));
                files = Directory.GetFiles(directories[i], "*.wav");
                for (f = 0; f < files.Length; f++)
                    gameMaster.sfx.Add(GetFileNameOnly(files[f]), new AudioData(files[f]));
            }
        }

        public string[] GetStageBGMPaths()
        {
            List<string> paths = new List<string>();
            string[] files = Directory.GetFiles(curStageDir, "*.mp3");
            int i;
            for (i = 0; i < files.Length; i++)
                paths.Add(files[i]);
            files = Directory.GetFiles(curStageDir, "*.wav");
            for (i = 0; i < files.Length; i++)
                paths.Add(files[i]);
            return paths.ToArray();
        }
        #endregion

        #region Enemies
        public void EnemyPatternLoader()
        {
            string[] files = Directory.GetFiles(enemyPatternDir + "/bullet", "*.json");

            GameMaster.LogPositiveNotice("\nEnemy Patterns(bullet):");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.enemyPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(files[i]));
            }

            /*files = Directory.GetFiles(enemyPatternDir + "/laser", "*.json");

            GameMaster.LogPositiveNotice("\nEnemy Patterns(laser):");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.enemyPatterns.Add(GetFileNameOnly(files[i]), ReadPatternDataJSON(files[i]));
            }*/
        }
        public void EnemyMovementLoader()
        {
            string[] files = Directory.GetFiles(enemyMovementDir, "*.json");

            GameMaster.LogPositiveNotice("\nEnemy Movement:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.enemyMovementPaths.Add(GetFileNameOnly(files[i]), ReadBezierData(files[i]));
            }
        }
        public void EnemyTextureLoader()
        {
            string[] files = Directory.GetFiles(enemyTexDir, "*.png");

            GameMaster.LogPositiveNotice("\nEnemy Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.enemyTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }
        public void EnemySpriteLoader()
        {
            string[] files = Directory.GetFiles(enemyTexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nEnemy Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                spritePairs = ReadSpriteData(files[i], gameMaster.enemyTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.enemySprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }

        public void EnemyAnimLoader()
        {
            string[] files = Directory.GetFiles(enemyAnimDir, "*.json");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            GameMaster.LogPositiveNotice("\nEnemy Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                animPairs = ReadSpriteAnimData(files[i], gameMaster.enemySprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    GameMaster.LogSecondary(animPairs[p].Key);
                    gameMaster.enemyAnimStates[animPairs[p].Key] = animPairs[p].Value;
                }
            }
        }

        public void EnemyDataLoader()
        {
            string[] files = Directory.GetFiles(enemyDir + "/enemies", "*.json");

            GameMaster.LogPositiveNotice("\nEnemy Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.enemyData.Add(GetFileNameOnly(files[i]), ReadEnemyData(files[i]));
            }
        }
        #endregion

        #region Stage Backgrounds
        public void BackgroundObjLoader()
        {
            string[] files = Directory.GetFiles(backgroundModelDir, "*.obj");

            GameMaster.LogPositiveNotice("\nBackground Objects:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.backgroundMeshes.Add(GetFileNameOnly(files[i]), ReadObjData(files[i]));
            }
        }
        public void BackgroundTextureLoader()
        {
            string[] files = Directory.GetFiles(backgroundTexDir, "*.png");

            GameMaster.LogPositiveNotice("\nBackground Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.backgroundTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }
        public void BackgroundModelDataLoader()
        {
            string[] files = Directory.GetFiles(backgroundModelDir, "*.json");
            KeyValuePair<string, TexturedModel>[] modelData;
            int m;
            GameMaster.LogPositiveNotice("\nBackground Model Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                modelData = ReadModelData(files[i], gameMaster.backgroundMeshes, gameMaster.backgroundTextures);
                for (m = 0; m < modelData.Length; m++)
                {
                    GameMaster.LogSecondary(modelData[m].Key);
                    gameMaster.backgroundModels.Add(modelData[m].Key, modelData[m].Value);
                }
            }
        }
        public void BackgroundSectionDataLoader()
        {
            string[] files = Directory.GetFiles(backgroundSecDir, "*.json");
            GameMaster.LogPositiveNotice("\nBackground Section:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.backgroundSections.Add(GetFileNameOnly(files[i]), ReadBackgroundSectionData(files[i]));
            }
        }
        #endregion

        #region UI
        public void UITextureLoader()
        {
            string[] files = Directory.GetFiles(UITexDir, "*.png");

            GameMaster.LogPositiveNotice("\nUI Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.uiTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }

        public void UISpriteLoader()
        {
            string[] files = Directory.GetFiles(UITexDir, "*.json");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            GameMaster.LogPositiveNotice("\nUI Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                spritePairs = ReadSpriteData(files[i], gameMaster.uiTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    GameMaster.LogSecondary(spritePairs[e].Key);
                    gameMaster.uiSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }
        #endregion

        #region Font
        public void FontLoader()
        {
            GameMaster.LogPositiveNotice("\nFonts:");
            string[] files = null;
            int i = 0;
            try
            {
                files = Directory.GetFiles(fontDir, "*.ttf");
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    InstallAndRenderFont(files[i]);
                }
                files = Directory.GetFiles(fontDir, "*.otf");
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    InstallAndRenderFont(files[i]);
                }
                files = Directory.GetFiles(fontDir, "*.woff");
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    InstallAndRenderFont(files[i]);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("Failed to load font file: " + files[i], e.Message);
            }
        }
        public void BitmapFontLoader()
        {
            GameMaster.LogPositiveNotice("\nBitmap Fonts:");
            string[] files = null;
            int i = 0, c = 0;
            try
            {
                files = Directory.GetFiles(fontDir, "*.png");
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    gameMaster.fontSheets.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
                }
                files = Directory.GetFiles(fontDir, "*.jpg");
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    gameMaster.fontSheets.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
                }
                files = Directory.GetFiles(fontDir, "*.json");
                KeyValuePair<string, SpriteSet>[] spritePairs;
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    spritePairs = ReadSpriteData(files[i], gameMaster.fontSheets[GetFileNameOnly(files[i])]);
                    for (c = 0; c < spritePairs.Length; c++)
                    {
                        GameMaster.LogSecondary(spritePairs[c].Key);
                        gameMaster.fontGlyphSprites.Add(spritePairs[c].Key, spritePairs[c].Value);
                    }
                }
                files = Directory.GetFiles(fontDir, "*.dwnfnt");
                for (i = 0; i < files.Length; i++)
                {
                    GameMaster.Log(GetFileNameOnly(files[i]));
                    gameMaster.dawnFonts.Add(GetFileNameOnly(files[i]), ReadDawnFont(files[i]));
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("Failed to load file: " + files[i], e.Message);
            }
        }
        #endregion

        #region Stage
        public void StageDataLoader(string[] bgmPaths)
        {
            string file = Directory.GetFiles(curStageDir, "*.json")[0];

            GameMaster.LogPositiveNotice("\nLoading Stage");
            try
            {
                ReadStageData(file, bgmPaths);
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("Failed to load stage file: " + file, e.Message);
            }
        }
        #endregion

        #endregion

        #region Loading Assistance
        public string SimplifyText(string data, bool lowercase = true)
        {
            if (lowercase)
                return data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            return data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "");
        }

        public int[] FindIndexes(string text, string opener, int startIndex = 0, string closer = ";")
        {
            text = SimplifyText(text);
            if (!text.Contains(opener))
                return new int[] { text.Length - 1, text.Length };
            int indexStart = text.IndexOf(opener, startIndex) + opener.Length;
            if (startIndex < indexStart)
                startIndex = indexStart;

            int indexEnd = text.IndexOf(closer, indexStart);
            if (indexEnd == -1)
                indexEnd = text.Length;
            return new int[] { indexStart, indexEnd };
        }

        public string GetInputSubstring(int[] indexes, string data, bool simplify = false)
        {
            if (indexes[0] == -1)
                return "";
            if (simplify)
                return SimplifyText(data.Substring(indexes[0], indexes[1] - indexes[0]));
            return data.Substring(indexes[0], indexes[1] - indexes[0]);
        }
        public string GetInputSubstring(int index, string data)
        {
            return data.Substring(index, data.Length - index);
        }
        public string GetInputSubstring(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return GetInputSubstring(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public string GetInputSubstring(string data, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            if (simplify)
                data = SimplifyText(data);
            return GetInputSubstring(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public bool TryGetInputSubstring(int[] indexes, string data, out string substring)
        {
            substring = "";
            try
            {
                substring = data.Substring(indexes[0], indexes[1] - indexes[0]);
                if (substring == ";")
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool TryGetInputSubstring(int index, string data, out string substring)
        {
            substring = "";
            try
            {
                substring = data.Substring(index, data.Length - index);
                if (substring == ";")
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string GetData(string[] data, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string finalString = "", currentString, simpleString;
            bool foundOpener = false;
            int i;
            for (i = lineOffset; i < data.Length; i++)
            {
                currentString = data[i];
                simpleString = SimplifyText(currentString);

                if (simplify)
                    currentString = simpleString;
                if (!foundOpener)
                    if (simpleString.Contains(opener))
                        foundOpener = true;

                if (foundOpener)
                {
                    finalString += currentString;
                    if (simpleString.Contains(closer))
                        break;
                }
            }
            lineOffset = i;
            return finalString;
        }
        public string GetData(string[] data, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string finalString = "", currentString, simpleString;
            bool foundOpener = false;
            int i;
            for (i = lineOffset; i < data.Length; i++)
            {
                currentString = data[i];
                simpleString = SimplifyText(currentString);

                if (simplify)
                    currentString = simpleString;
                if (!foundOpener)
                    if (simpleString.Contains(opener))
                        foundOpener = true;

                if (foundOpener)
                {
                    finalString += currentString;
                    if (simpleString.Contains(closer))
                        break;
                }
            }
            lineOffset = i;
            return finalString;
        }
        public string GetData(string data, string opener, bool simplify = true, string closer = ";")
        {
            int firstInd, secondInd;
            string simpleString = SimplifyText(data);

            if (simplify)
                data = simpleString;

            if (simpleString.Contains(opener))
                firstInd = simpleString.IndexOf(opener);
            else
                firstInd = -1;

            if (firstInd > -1)
            {
                if (simpleString.Substring(firstInd, data.Length - firstInd).Contains(closer))
                    secondInd = simpleString.IndexOf(closer, firstInd) + 1;
                else
                    secondInd = -1;

                if (secondInd > -1)
                    return GetInputSubstring(new int[] { firstInd, secondInd }, data);
                return GetInputSubstring(new int[] { firstInd, data.Length - 1 }, data);
            }
            return "";
        }
        public bool TryGetData(string[] data, string opener, out string finalString, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            finalString = "";
            string currentString;
            bool foundOpener = false;
            int i;
            for (i = lineOffset; i < data.Length; i++)
            {
                currentString = data[i];

                if (simplify)
                    currentString = SimplifyText(currentString);
                if (!foundOpener)
                    if (currentString.Contains(opener))
                        foundOpener = true;

                if (foundOpener)
                {
                    finalString += currentString;
                    if (currentString.Contains(closer))
                        break;
                }
            }
            lineOffset = i;
            return foundOpener;
        }
        public bool TryGetData(string[] data, string opener, out string finalString, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            finalString = "";
            string currentString;
            bool foundOpener = false;
            int i;
            for (i = lineOffset; i < data.Length; i++)
            {
                currentString = data[i];

                if (simplify)
                    currentString = SimplifyText(currentString);
                if (!foundOpener)
                    if (currentString.Contains(opener))
                        foundOpener = true;

                if (foundOpener)
                {
                    finalString += currentString;
                    if (currentString.Contains(closer))
                        break;
                }
            }
            lineOffset = i;
            return foundOpener;
        }
        public bool TryGetData(string data, string opener, out string finalString, bool simplify = true, string closer = ";")
        {
            int firstInd, secondInd;
            finalString = "";

            if (simplify)
                data = SimplifyText(data);

            if (data.Contains(opener))
                firstInd = data.IndexOf(opener);
            else
                firstInd = -1;

            if (data.Contains(closer))
                secondInd = data.IndexOf(closer);
            else
                secondInd = -1;

            if (firstInd > -1)
            {
                if (secondInd > -1)
                    finalString = GetInputSubstring(new int[] { firstInd, secondInd }, data);
                finalString = GetInputSubstring(new int[] { firstInd, data.Length - 1 }, data);
                return true;
            }
            return false;
        }
        public bool CheckHasData(string[] data, string contains, int lineOffset = 0)
        {
            for (int i = lineOffset; i < data.Length; i++)
                if (data[i].Contains(contains))
                    return true;
            return false;
        }
        public bool CheckHasData(string[] data, string contains, ref int lineOffset)
        {
            for (int i = lineOffset; i < data.Length; i++)
                if (data[i].Contains(contains))
                    return true;
            return false;
        }
        public bool CheckHasData(string data, string contains)
        {
            if (data.Contains(contains))
                return true;
            return false;
        }

        public float ParseFloat(int[] indexes, string data)
        {
            float numVal = 0;
            float.TryParse(GetInputSubstring(indexes, data, true), out numVal);
            return numVal;
        }
        public float ParseFloat(string data)
        {
            float numVal = 0;
            float.TryParse(data, out numVal);
            return numVal;
        }
        public float GetParseFloat(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseFloat(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public float GetParseFloat(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseFloat(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public float GetParseFloat(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseFloat(GetInputSubstring(data, opener, 0, simplify, closer));
        }
        public bool TryParseFloat(int[] indexes, string data, out float numVal)
        {
            if (indexes[0] == -1 || indexes[1] == -1)
            {
                numVal = 0;
                return false;
            }
            return float.TryParse(GetInputSubstring(indexes, data), out numVal);
        }
        public bool TryParseFloat(string data, out float numVal)
        {
            return float.TryParse(data, out numVal);
        }
        public Vector2 ParseVector2(int[] indexes, string data)
        {
            Vector2 vec2 = new Vector2();
            string[] tempStrings = GetInputSubstring(indexes, data, true).Split(',', StringSplitOptions.RemoveEmptyEntries);
            vec2.X = ParseFloat(tempStrings[0]);
            vec2.Y = ParseFloat(tempStrings[1]);
            return vec2;
        }
        public Vector2 ParseVector2(string data)
        {
            Vector2 vec2 = new Vector2();
            string[] tempStrings = data.Split(',', StringSplitOptions.RemoveEmptyEntries);
            vec2.X = ParseFloat(tempStrings[0]);
            vec2.Y = ParseFloat(tempStrings[1]);
            return vec2;
        }
        public Vector2 GetParseVector2(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseVector2(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Vector2 GetParseVector2(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseVector2(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Vector2 GetParseVector2(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseVector2(GetInputSubstring(data, opener, 0, simplify, closer));
        }
        public Vector3 ParseVector3(int[] indexes, string data)
        {
            Vector3 vec3 = new Vector3();
            string[] tempStrings = GetInputSubstring(indexes, data, true).Split(',', StringSplitOptions.RemoveEmptyEntries);
            vec3.X = ParseFloat(tempStrings[0]);
            vec3.Y = ParseFloat(tempStrings[1]);
            vec3.Z = ParseFloat(tempStrings[2]);
            return vec3;
        }
        public Vector3 ParseVector3(string data)
        {
            Vector3 vec3 = new Vector3();
            string[] tempStrings = data.Split(',', StringSplitOptions.RemoveEmptyEntries);
            vec3.X = ParseFloat(tempStrings[0]);
            vec3.Y = ParseFloat(tempStrings[1]);
            vec3.Z = ParseFloat(tempStrings[2]);
            return vec3;
        }
        public Vector3 GetParseVector3(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseVector3(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Vector3 GetParseVector3(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseVector3(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Vector3 GetParseVector3(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseVector3(GetInputSubstring(data, opener, 0, simplify, closer));
        }
        public Vector4 ParseVector4(int[] indexes, string data)
        {
            Vector4 vec4 = new Vector4();
            string[] tempStrings = GetInputSubstring(indexes, data, true).Split(',', StringSplitOptions.RemoveEmptyEntries);
            vec4.X = ParseFloat(tempStrings[0]);
            vec4.Y = ParseFloat(tempStrings[1]);
            vec4.Z = ParseFloat(tempStrings[2]);
            vec4.Z = ParseFloat(tempStrings[3]);
            return vec4;
        }
        public Vector4 ParseVector4(string data)
        {
            Vector4 vec4 = new Vector4();
            string[] tempStrings = data.Split(',', StringSplitOptions.RemoveEmptyEntries);
            vec4.X = ParseFloat(tempStrings[0]);
            vec4.Y = ParseFloat(tempStrings[1]);
            vec4.Z = ParseFloat(tempStrings[2]);
            vec4.Z = ParseFloat(tempStrings[3]);
            return vec4;
        }
        public Vector4 GetParseVector4(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseVector4(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Vector4 GetParseVector4(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseVector4(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Vector4 GetParseVector4(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseVector4(GetInputSubstring(data, opener, 0, simplify, closer));
        }


        public Matrix3 ParseMat3(int[] indexes, string data)
        {
            Matrix3 mat3 = new Matrix3();
            string[] tempStrings = GetInputSubstring(indexes, data, true).Split(',', StringSplitOptions.RemoveEmptyEntries);
            mat3.M11 = ParseFloat(tempStrings[0]);
            mat3.M12 = ParseFloat(tempStrings[1]);
            mat3.M13 = ParseFloat(tempStrings[2]);
            mat3.M21 = ParseFloat(tempStrings[3]);
            mat3.M22 = ParseFloat(tempStrings[4]);
            mat3.M23 = ParseFloat(tempStrings[5]);
            mat3.M31 = ParseFloat(tempStrings[6]);
            mat3.M32 = ParseFloat(tempStrings[7]);
            mat3.M33 = ParseFloat(tempStrings[8]);
            return mat3;
        }
        public Matrix3 ParseMat3(string data)
        {
            Matrix3 mat3 = new Matrix3();
            string[] tempStrings = data.Split(',', StringSplitOptions.RemoveEmptyEntries);
            mat3.M11 = ParseFloat(tempStrings[0]);
            mat3.M12 = ParseFloat(tempStrings[1]);
            mat3.M13 = ParseFloat(tempStrings[2]);
            mat3.M21 = ParseFloat(tempStrings[3]);
            mat3.M22 = ParseFloat(tempStrings[4]);
            mat3.M23 = ParseFloat(tempStrings[5]);
            mat3.M31 = ParseFloat(tempStrings[6]);
            mat3.M32 = ParseFloat(tempStrings[7]);
            mat3.M33 = ParseFloat(tempStrings[8]);
            return mat3;
        }
        public Matrix3 GetParseMat3(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseMat3(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Matrix3 GetParseMat3(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseMat3(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Matrix3 GetParseMat3(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseMat3(GetInputSubstring(data, opener, 0, simplify, closer));
        }

        public Matrix4 ParseMat4(int[] indexes, string data)
        {
            Matrix4 mat4 = new Matrix4();
            string[] tempStrings = GetInputSubstring(indexes, data, true).Split(',', StringSplitOptions.RemoveEmptyEntries);
            mat4.M11 = ParseFloat(tempStrings[0]);
            mat4.M12 = ParseFloat(tempStrings[1]);
            mat4.M13 = ParseFloat(tempStrings[2]);
            mat4.M14 = ParseFloat(tempStrings[3]);
            mat4.M21 = ParseFloat(tempStrings[4]);
            mat4.M22 = ParseFloat(tempStrings[5]);
            mat4.M23 = ParseFloat(tempStrings[6]);
            mat4.M24 = ParseFloat(tempStrings[7]);
            mat4.M31 = ParseFloat(tempStrings[8]);
            mat4.M32 = ParseFloat(tempStrings[9]);
            mat4.M33 = ParseFloat(tempStrings[10]);
            mat4.M34 = ParseFloat(tempStrings[11]);
            mat4.M41 = ParseFloat(tempStrings[12]);
            mat4.M42 = ParseFloat(tempStrings[13]);
            mat4.M43 = ParseFloat(tempStrings[14]);
            mat4.M44 = ParseFloat(tempStrings[15]);
            return mat4;
        }
        public Matrix4 ParseMat4(string data)
        {
            Matrix4 mat4 = new Matrix4();
            string[] tempStrings = data.Split(',', StringSplitOptions.RemoveEmptyEntries);
            mat4.M11 = ParseFloat(tempStrings[0]);
            mat4.M12 = ParseFloat(tempStrings[1]);
            mat4.M13 = ParseFloat(tempStrings[2]);
            mat4.M14 = ParseFloat(tempStrings[3]);
            mat4.M21 = ParseFloat(tempStrings[4]);
            mat4.M22 = ParseFloat(tempStrings[5]);
            mat4.M23 = ParseFloat(tempStrings[6]);
            mat4.M24 = ParseFloat(tempStrings[7]);
            mat4.M31 = ParseFloat(tempStrings[8]);
            mat4.M32 = ParseFloat(tempStrings[9]);
            mat4.M33 = ParseFloat(tempStrings[10]);
            mat4.M34 = ParseFloat(tempStrings[11]);
            mat4.M41 = ParseFloat(tempStrings[12]);
            mat4.M42 = ParseFloat(tempStrings[13]);
            mat4.M43 = ParseFloat(tempStrings[14]);
            mat4.M44 = ParseFloat(tempStrings[15]);
            return mat4;
        }
        public Matrix4 GetParseMat4(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseMat4(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Matrix4 GetParseMat4(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseMat4(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public Matrix4 GetParseMat4(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseMat4(GetInputSubstring(data, opener, 0, simplify, closer));
        }

        public int ParseRoundedNum(int[] indexes, string data)
        {
            return Round(ParseFloat(indexes, data));
        }
        public int ParseRoundedNum(string data)
        {
            return Round(ParseFloat(data));
        }
        public int GetParseRoundedNum(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseRoundedNum(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public int GetParseRoundedNum(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseRoundedNum(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public int GetParseRoundedNum(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseRoundedNum(GetInputSubstring(data, opener, 0, simplify, closer));
        }
        public bool TryParseRoundedNum(string data, out int numVal)
        {
            float floatVal = 0;
            bool boolVal = float.TryParse(data, out floatVal);
            numVal = Round(floatVal);
            return boolVal;
        }
        public bool TryParseRoundedNum(int[] indexes, string data, out int numVal)
        {
            if (indexes[0] == -1 || indexes[1] == -1)
            {
                numVal = 0;
                return false;
            }
            float floatVal = 0;
            bool boolVal = float.TryParse(GetInputSubstring(indexes, data), out floatVal);
            numVal = Round(floatVal);
            return boolVal;
        }
        public bool ParseBool(int[] indexes, string data)
        {
            bool boolVal = false;
            bool.TryParse(GetInputSubstring(indexes, data), out boolVal);
            return boolVal;
        }
        public bool ParseBool(string data)
        {
            bool boolVal = false;
            bool.TryParse(data, out boolVal);
            return boolVal;
        }
        public bool GetParseBool(string[] allLines, string opener, int lineOffset = 0, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, lineOffset, simplify, closer);
            return ParseBool(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public bool GetParseBool(string[] allLines, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string data = GetData(allLines, opener, ref lineOffset, simplify, closer);
            return ParseBool(FindIndexes(data, opener, lineOffset, closer), data);
        }
        public bool GetParseBool(string data, string opener, bool simplify = true, string closer = ";")
        {
            return ParseBool(GetInputSubstring(data, opener, 0, simplify, closer));
        }
        public bool TryParseBool(int[] indexes, string data, out bool boolVal)
        {
            if (indexes[0] == -1 || indexes[1] == -1)
            {
                boolVal = false;
                return false;
            }
            bool parsed = bool.TryParse(GetInputSubstring(indexes, data), out boolVal);
            return parsed;
        }
        public bool TryParseBool(string data, out bool boolVal)
        {
            bool parsed = bool.TryParse(data, out boolVal);
            return parsed;
        }
        #endregion

        #region File Readers

        #region Shaders
        public Shader ReadShaderData(string file)
        {
            Shader outputShader;

            string text = File.ReadAllText(file);
            JSONShaderData shaderData = JsonConvert.DeserializeObject<JSONShaderData>(text);

            outputShader = new Shader(shaderDir + "/" + shaderData.vert, shaderDir + "/" + shaderData.frag);
            string simplifiedText;
            string[] splitText;
            float[] floatValues;

            float floatVal = 0;
            int intVal = 0;

            int i, k;

            try
            {
                for (i = 0; i < shaderData.constants.Count; i++)
                {
                    switch (shaderData.constants[i].type.ToLower())
                    {
                        case "float":
                        case "single":
                            if (float.TryParse(shaderData.constants[i].value, out floatVal))
                                outputShader.SetFloat(shaderData.constants[i].name, floatVal);
                            break;
                        case "int":
                        case "integer":
                            if (int.TryParse(shaderData.constants[i].value, out intVal))
                                outputShader.SetInt(shaderData.constants[i].name, intVal);
                            break;
                        case "vec2":
                        case "vector2":
                            simplifiedText = SimplifyText(shaderData.constants[i].value, true);
                            splitText = simplifiedText.Split(',');
                            floatValues = new float[splitText.Length];

                            if(floatValues.Length < 2)
                                throw new Exception("Not enough values for a Vector 2 for " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));


                            for (k = 0; k < splitText.Length; k++)
                            {
                                if (!float.TryParse(splitText[k], out floatValues[k]))
                                    throw new Exception("Could not parse constant value " + k + " of " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));
                            }

                            outputShader.SetVector2(shaderData.constants[i].name, new Vector2(floatValues[0], floatValues[1]));
                            break;
                        case "vec3":
                        case "vector3":
                            simplifiedText = SimplifyText(shaderData.constants[i].value, true);
                            splitText = simplifiedText.Split(',');
                            floatValues = new float[splitText.Length];

                            if (floatValues.Length < 3)
                                throw new Exception("Not enough values for a Vector 3 for " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));


                            for (k = 0; k < splitText.Length; k++)
                            {
                                if (!float.TryParse(splitText[k], out floatValues[k]))
                                    throw new Exception("Could not parse constant value " + k + " of " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));
                            }

                            outputShader.SetVector3(shaderData.constants[i].name, new Vector3(floatValues[0], floatValues[1], floatValues[2]));
                            break;
                        case "vec4":
                        case "vector4":
                            simplifiedText = SimplifyText(shaderData.constants[i].value, true);
                            splitText = simplifiedText.Split(',');
                            floatValues = new float[splitText.Length];

                            if (floatValues.Length < 4)
                                throw new Exception("Not enough values for a Vector 3 for " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));


                            for (k = 0; k < splitText.Length; k++)
                            {
                                if (!float.TryParse(splitText[k], out floatValues[k]))
                                    throw new Exception("Could not parse constant value " + k + " of " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));
                            }

                            outputShader.SetVector4(shaderData.constants[i].name, new Vector4(floatValues[0], floatValues[1], floatValues[2], floatValues[3]));
                            break;
                        case "mat3":
                        case "mat3x3":
                        case "matrix3":
                        case "matrix3x3":
                            simplifiedText = SimplifyText(shaderData.constants[i].value, true);
                            splitText = simplifiedText.Split(',');
                            floatValues = new float[splitText.Length];

                            if (floatValues.Length < 9)
                                throw new Exception("Not enough values for a 3x3 Matrix for " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));


                            for (k = 0; k < splitText.Length; k++)
                            {
                                if (!float.TryParse(splitText[k], out floatValues[k]))
                                    throw new Exception("Could not parse constant value " + k + " of " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));
                            }

                            outputShader.SetMatrix3(shaderData.constants[i].name, 
                                new Matrix3(floatValues[0], floatValues[1], floatValues[2], 
                                            floatValues[3], floatValues[4], floatValues[5],
                                            floatValues[6], floatValues[7], floatValues[8]));
                            break;
                        case "mat4":
                        case "mat4x4":
                        case "matrix4":
                        case "matrix4x4":

                            simplifiedText = SimplifyText(shaderData.constants[i].value, true);
                            splitText = simplifiedText.Split(',');
                            floatValues = new float[splitText.Length];

                            if (floatValues.Length < 16)
                                throw new Exception("Not enough values for a 4x4 Matrix for " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));


                            for (k = 0; k < splitText.Length; k++)
                            {
                                if (!float.TryParse(splitText[k], out floatValues[k]))
                                    throw new Exception("Could not parse constant value " + k + " of " + shaderData.constants[i].name + " in " + GetFileNameOnly(file));
                            }

                            outputShader.SetMatrix4(shaderData.constants[i].name,
                                new Matrix4(floatValues[0],  floatValues[1],  floatValues[2],  floatValues[3],
                                            floatValues[4],  floatValues[5],  floatValues[6],  floatValues[7],
                                            floatValues[8],  floatValues[9],  floatValues[10], floatValues[11],
                                            floatValues[12], floatValues[13], floatValues[14], floatValues[15]));
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this shader!", e.Message);
            }

            return outputShader;
        }
        #endregion

        #region Fonts

        //https://stackoverflow.com/a/45427497

        /*[DllImport("freetype.dll")]
        public static extern ulong FT_Get_First_Char();

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct FT_FaceRec_
        {
            public long num_faces;
            public long face_index;

            public long face_flags;
            public long style_flags;

            public long num_glyphs;

            public char* family_name;
            public char* style_name;

            public int num_fixed_sizes;
            public FT_Bitmap_size_* available_sizes;

            public int num_charmaps;
            public FT_CharMapRec_* charmaps;

            public FT_Generic_ generic;

            public FT_BBox_ bbox;

            public ushort units_per_EM;
            public short ascender;
            public short descender;
            public short height;

            public short max_advance_width;
            public short max_advance_height;

            public short underline_position;
            public short underline_thickness;


        }
        unsafe struct FT_Generic_
        {
            public void* data;
            public void* FT_Generic_Finalizer;
        }
        struct FT_BBox_
        {
            public long xMin, yMin;
            public long xMax, yMax;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct FT_Bitmap_size_
        {
            public short height;
            public short width;

            public long size;

            public long x_ppem;
            public long y_ppem;
        }


        [StructLayout(LayoutKind.Sequential)]
        struct FT_CharMapRec_
        {
            public FT_FaceRec_ face;
            public FT_Encoding_ encoding;
            public ushort platform_id;
            public ushort encoding_id;
        }

        enum FT_Encoding_
        {
            FT_ENCODING_NONE = 0,

            FT_ENCODING_MS_SYMBOL = (int)(((uint)'s' << 24) | ((uint)'y' << 16) | ((uint)'m' << 8) | ((uint)'b')),
            FT_ENCODING_UNICODE = (int)(((uint)'u' << 24) | ((uint)'n' << 16) | ((uint)'i' << 8) | ((uint)'c')),

            FT_ENCODING_SJIS = (int)(((uint)'s' << 24) | ((uint)'j' << 16) | ((uint)'i' << 8) | ((uint)'s')),
            FT_ENCODING_PRC = (int)(((uint)'g' << 24) | ((uint)'b' << 16) | ((uint)' ' << 8) | ((uint)' ')),
            FT_ENCODING_BIG5 = (int)(((uint)'b' << 24) | ((uint)'i' << 16) | ((uint)'g' << 8) | ((uint)'5')),
            FT_ENCODING_WANSUNG = (int)(((uint)'w' << 24) | ((uint)'a' << 16) | ((uint)'n' << 8) | ((uint)'s')),
            FT_ENCODING_JOHAB = (int)(((uint)'j' << 24) | ((uint)'o' << 16) | ((uint)'h' << 8) | ((uint)'a')),

            FT_ENCODING_GB2312 = FT_ENCODING_PRC,
            FT_ENCODING_MS_SJIS = FT_ENCODING_SJIS,
            FT_ENCODING_MS_GB2312 = FT_ENCODING_PRC,
            FT_ENCODING_MS_BIG5 = FT_ENCODING_BIG5,
            FT_ENCODING_MS_WANSUNG = FT_ENCODING_WANSUNG,
            FT_ENCODING_MS_JOHAB = FT_ENCODING_JOHAB,

            FT_ENCODING_ADOBE_STANDARD = (int)(((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'O' << 8) | ((uint)'B')),
            FT_ENCODING_ADOBE_EXPERT = (int)(((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'B' << 8) | ((uint)'E')),
            FT_ENCODING_ADOBE_CUSTOM = (int)(((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'B' << 8) | ((uint)'C')),
            FT_ENCODING_ADOBE_LATIN_1 = (int)(((uint)'l' << 24) | ((uint)'a' << 16) | ((uint)'t' << 8) | ((uint)'1')),

            FT_ENCODING_ADOBE_OLD_LATIN_2 = (int)(((uint)'l' << 24) | ((uint)'a' << 16) | ((uint)'t' << 8) | ((uint)'2')),

            FT_ENCODING_APPLE_ROMAN = (int)(((uint)'a' << 24) | ((uint)'r' << 16) | ((uint)'m' << 8) | ((uint)'n')),
        }

        enum FT_Glyph_Format_
        {
            FT_GLYPH_FORMAT_NONE = 0,

            FT_GLYPH_FORMAT_COMPOSITE  = (int)(((uint)'c' << 24) | ((uint)'o' << 16) | ((uint)'m' << 8) | ((uint)'p')),
            FT_GLYPH_FORMAT_BITMAP = (int)(((uint)'b' << 24) | ((uint)'i' << 16) | ((uint)'t' << 8) | ((uint)'s')),
            FT_GLYPH_FORMAT_OUTLINE = (int)(((uint)'o' << 24) | ((uint)'u' << 16) | ((uint)'t' << 8) | ((uint)'l')),
            FT_GLYPH_FORMAT_PLOTTER = (int)(((uint)'p' << 24) | ((uint)'l' << 16) | ((uint)'o' << 8) | ((uint)'t')),
            FT_GLYPH_FORMAT_SVG = (int)(((uint)'S' << 24) | ((uint)'V' << 16) | ((uint)'G' << 8) | ((uint)' ')),
        }


        unsafe struct FT_GlyphSlotRec_
        {

        }

        unsafe struct FT_LibraryRec_
        {
            public FT_MemoryRec_ memory;

            public int version_major;
            public int version_minor;
            public int version_patch;

            public uint num_modules;
            FT_ModuleRec_[] modules; // size of 32

            public FT_ListRec_ renderers;
            public FT_RendererRec_* cur_renderer;
            public  FT_ModuleRec_* auto_hinter;
        }

        unsafe struct FT_MemoryRec_
        {
            public void* user;
            public void* alloc;
            public void* free;
            public void* realloc;
        }

        unsafe struct FT_ModuleRec_
        {
            public void* clazz;
            public FT_LibraryRec_ library;
            public FT_MemoryRec_ memory;
        }

        unsafe struct FT_ListRec_
        {
            FT_ListNodeRec_* head;
            FT_ListNodeRec_* tail;
        }

        unsafe struct FT_ListNodeRec_
        {
            public FT_ListNodeRec_* prev;
            public FT_ListNodeRec_* next;
            public void* data;
        }

        unsafe struct FT_RendererRec_
        {
            public FT_ModuleRec_ root;
            public void* clazz;
            public FT_Glyph_Format_ glyph_format;
            public FT_Glyph_Class_ glyph_class;

            public void* raster;
            public void* raster_render;
            public void* render;
        }

        unsafe struct FT_Glyph_Class_
        {
            public long glyph_size;
            public FT_Glyph_Format_ glyph_format;

            public void* glyph_init;
            public void* glyph_done;
            public void* glyph_copy;
            public void* glyph_transform;
            public void* glyph_bbox;
            public void* glyph_prepare;
        }*/


        public FontCharList CreateCharList(string file)
        {
            FontCharList charList = new FontCharList();
            char newChar;
            IDictionary<int, ushort> tempDict = new Dictionary<int, ushort>();
            FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);

            Typeface typeFace = new OpenFontReader().Read(stream);

            int t;

            for (t = 0; t < typeFace.GlyphCount; t++)
            {
                newChar = Convert.ToChar(typeFace.Glyphs[t].GlyphIndex);
                charList.everyCharInFont += newChar;
                charList.charList.Add(newChar);
                charList.charListHash.Add(newChar);
                if (!charList.indexes.ContainsKey((ushort)newChar))
                    charList.indexes.Add((ushort)newChar, t);
            }
            if (gameMaster.logAllFontChars)
                GameMaster.LogNeutralNotice(charList.everyCharInFont);

            stream.Close();

            /*for (t = 0; t < typefaces.Length; t++)
            {
                typefaces[t].TryGetGlyphTypeface(out glyphTypeface);
                tempDict = glyphTypeface.CharacterToGlyphMap;

                foreach (KeyValuePair<int, ushort> glyph in tempDict)
                {
                    charList.fontChars.Add(glyph.Key, glyph.Value);
                    charList.everyCharInFont += (char)glyph.Value;
                    charList.charList.Add((char)glyph.Value);
                }
            }*/


            return charList;
        }

        public Image<Rgba32> RenderFont(string fontName, string file, out List<SpriteSet.Sprite> sprites)
        {
            sprites = new List<SpriteSet.Sprite>();
            try
            {
                int i;
                const int size = 25;
                //Image<A8> textImage;
                string allCharacters = gameMaster.fontCharList[fontName].everyCharInFont;
                int width = 0, height = 0;


                char[] charArray = allCharacters.ToCharArray();

                allCharacters = String.Join(' ', charArray);

                FontFamily family = gameMaster.fonts.Get(fontName);
                Font font = family.CreateFont(size);

                TextOptions options = new TextOptions(font)
                {
                    KerningMode = KerningMode.None,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    LineSpacing = 1.5f,
                    
                };


                FontRectangle[] rects = new FontRectangle[allCharacters.Length];

                const int charLimit = 192;
                int curWidth = 0, curHeight = 0, charsUnderLimit = 0;

                const int widthLimit = 3500;

                bool firstSpace = true;

                for (i = 0; i < allCharacters.Length; i++)
                {
                    rects[i] = TextMeasurer.Measure(allCharacters[i].ToString(), options);
                    curWidth += Ceil(rects[i].Width);
                    charsUnderLimit++;

                    if (rects[i].Height > curHeight)
                        curHeight = Ceil(rects[i].Height);

                    if (curWidth > widthLimit)
                    {
                        curWidth -= Ceil(rects[i].Width);
                        if (curWidth > width)
                            width = curWidth;
                        curWidth = 0;
                        height += curHeight;
                        curHeight = 0;
                    }
                }

                if (curWidth > width)
                    width = curWidth;
                height += curHeight;

                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                Image<Rgba32> image = new Image<Rgba32>(width < widthLimit ? width : widthLimit, height, new Rgba32(0, 0, 0, 0));
                PointF point = new PointF();
                int xDrawn = 0, yDrawn = 0;
                float imageCenter = height * 0.5f;
                Color color = new Color(new Rgba32(255, 255, 255, 255));
                curWidth = 0;
                curHeight = 0;

                for (i = 0; i < allCharacters.Length; i++)
                {
                    try
                    {
                        point.X = xDrawn;
                        point.Y = yDrawn;
                        image.Mutate(x => x.DrawText(allCharacters[i].ToString(), font, color, point));
                    }
                    catch (Exception e)
                    {
                        GameMaster.LogErrorMessage("There was an error rendering this font for character" + i + ": " + allCharacters[i], e.Message);
                    }


                    if (allCharacters[i] == ' ' && firstSpace)
                    {
                        firstSpace = false;
                        sprites.Add(new SpriteSet.Sprite(xDrawn, yDrawn, xDrawn + rects[i].Width, yDrawn + rects[i].Height, height, width, true));
                    }
                    else if (allCharacters[i] != ' ')
                        sprites.Add(new SpriteSet.Sprite(xDrawn, yDrawn, xDrawn + rects[i].Width, yDrawn + rects[i].Height, height, width, true));

                    xDrawn += Ceil(rects[i].Width);

                    charsUnderLimit++;

                    if (rects[i].Height > curHeight)
                        curHeight = Ceil(rects[i].Height);

                    if (i < allCharacters.Length - 1 && xDrawn + Ceil(rects[i + 1].Width) > widthLimit)
                    {
                        charsUnderLimit = 0;
                        xDrawn = 0;
                        yDrawn += curHeight;
                        curHeight = 0;
                    }
                }
                GameMaster.LogTimeCustMilliseconds("rendering font", timer.Elapsed.TotalMilliseconds);

                //SixLabors.Fonts.FontFamily family = gameMaster.fonts.Find(fontName);
                //Font font = family.CreateFont(size);

                /*Bitmap bm = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                Graphics graphic = Graphics.FromImage(bm);

                System.Drawing.SizeF[] textRects = new System.Drawing.SizeF[allCharacters.Length];
                PrivateFontCollection fontCollection = new PrivateFontCollection();
                fontCollection.AddFontFile(file);
                System.Drawing.Font thisFont = new System.Drawing.Font(fontCollection.Families[0], size);

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                for (i = 0; i < allCharacters.Length; i++)
                {
                    textRects[i] = graphic.MeasureString(allCharacters[i].ToString(), thisFont, 1000, format);
                    width += Ceil(textRects[i].Width);
                    if (textRects[i].Height > height)
                        height = Ceil(textRects[i].Height);
                }

                bm.Dispose();
                graphic.Dispose();

                bm = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                graphic = Graphics.FromImage(bm);
                graphic.Clear(System.Drawing.Color.Transparent);
                graphic.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Brush textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

                int distanceDrawn = 0;
                float imageCenter = bm.Height / 2f;
                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                for (i = 0; i < allCharacters.Length; i++)
                {
                    try
                    {
                        graphic.DrawString(allCharacters[i].ToString(), thisFont, textBrush,
                            new System.Drawing.PointF(distanceDrawn + (textRects[i].Width / 2), imageCenter), format);
                    }
                    catch (Exception e)
                    {
                        GameMaster.LogErrorMessage("There was an error rendering this font for character" + i + ": " + allCharacters[i], e.Message);
                    }
                    sprites.Add(new SpriteSet.Sprite(distanceDrawn, bm.Height, distanceDrawn + textRects[i].Width, 0, bm.Height, bm.Width, true));
                    distanceDrawn += Ceil(textRects[i].Width);
                }
                GameMaster.LogTimeCustMilliseconds("rendering font", timer.Elapsed.TotalMilliseconds);

                textBrush.Dispose();
                graphic.Dispose();
                format.Dispose();
                fontCollection.Dispose();

                /*string outputFileName = "D:/Damio/source/repos/DawnmakuEngine/DawnmakuEngine/bin/Debug/netcoreapp3.1/Data/General/Fonts/font.bmp";
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        bm.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }*/

                //image.SaveAsPng("D:/Damio/source/repos/DawnmakuEngine/DawnmakuEngine/bin/Debug/netcoreapp3.1/Data/General/Fonts/" + fontName + ".png");

                return image;
            } catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error setting up font rendering for this font: " + fontName, e.Message);
            }

            /*TextGraphicsOptions finalRendOpt = new TextGraphicsOptions()
            {
                TextOptions = new TextOptions()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    ApplyKerning = false
                },
                GraphicsOptions = new GraphicsOptions()
                {
                    Antialias = true
                }
            };

            RendererOptions textRendOptions = new RendererOptions(font)
            {
                ApplyKerning = false,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            sprites = new List<SpriteSet.Sprite>();
            FontRectangle[] rects = new FontRectangle[allCharacters.Length];

            FontRectangle fullRect = TextMeasurer.Measure(allCharacters, textRendOptions);
            textImage = new Image<A8>(Ceil(fullRect.Width), Ceil(fullRect.Height));
            int distanceDrawn = 0;

            for (i = 0; i < allCharacters.Length; i++)
            {
                rects[i] = TextMeasurer.Measure(allCharacters[i].ToString(), textRendOptions);
                try
                {
                    textImage.Mutate(x => x.DrawText(finalRendOpt, allCharacters[i].ToString(), font, SixLabors.ImageSharp.Color.White,
                        new PointF(distanceDrawn + rects[i].Width / 2, textImage.Height / 2f)));
                } catch(Exception e)
                {
                    Console.WriteLine("  There was an error rendering this font: {0}\n    for character {1}: {2}", fontName, i, allCharacters[i]);
                    Console.WriteLine(e.Message);
                }
                sprites.Add(new SpriteSet.Sprite(distanceDrawn, textImage.Height, distanceDrawn + rects[i].Width, 0, textImage.Height, textImage.Width, true));
                distanceDrawn += Ceil(rects[i].Width);
            }*/

            GameMaster.LogError("Something went wrong with rendering a font, returning a 1x1 purple texture: " + fontName);
            return new Image<Rgba32>(1, 1, new Rgba32(255, 0, 255, 255));
            //return new Bitmap(1,1, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
        }

        public void InstallAndRenderFont(string file)
        {
            List<SpriteSet.Sprite> tempSprites;
            gameMaster.fonts.Add(file);
            string fontName = gameMaster.fonts.Families.Last().Name;
            gameMaster.fontNames.Add(fontName);
            gameMaster.fontCharList.Add(fontName, CreateCharList(file));
            gameMaster.fontImages.Add(fontName, RenderFont(fontName, file, out tempSprites));
            Texture tempTex = new Texture(gameMaster.fontImages[fontName], false);
            gameMaster.fontSheets.Add(fontName, tempTex);
            gameMaster.fontGlyphSprites.Add(fontName, new SpriteSet(tempSprites, tempTex));
        }

        public DawnFont ReadDawnFont(string file)
        {
            DawnFont thisData = new DawnFont();
            DawnFont.CharSpecs spec;
            int i, s;
            string[] specs, kernings, kernVals, allLines = File.ReadAllLines(file);

            try
            {
                thisData.supportedChars = GetInputSubstring(allLines, "supportedcharacters=\"", 0, false, "\";").Replace(" ", "").Replace("\n", "").Replace("\r", "");
                thisData.sprites = gameMaster.fontGlyphSprites[GetInputSubstring(allLines, "sprites=")];
                thisData.defaultSize = GetParseFloat(allLines, "defaultsize=");
                thisData.defaultAdvance = GetParseFloat(allLines, "defaultadvance=");
                thisData.defaultHeight = GetParseFloat(allLines, "defaultheightoffset=");
                specs = GetData(allLines, "charspecs=", 0, false, "EndOfFile").Split(':', StringSplitOptions.RemoveEmptyEntries);
                for (i = 0; i < specs.Length; i++)
                {
                    spec = new DawnFont.CharSpecs();
                    spec.advance = GetParseFloat(specs[i], "advance=");
                    spec.heightOffset = GetParseFloat(specs[i], "heightoffset=");
                    kernings = GetInputSubstring(specs[i], "kerning=", 0, false).Split('/', StringSplitOptions.RemoveEmptyEntries);
                    for (s = 0; s < kernings.Length; s++)
                    {
                        kernVals = kernings[s].Split(',');
                        spec.kerning.Add(kernVals[0][0], ParseFloat(kernVals[1]));
                    }
                    thisData.charSpecs.Add(GetInputSubstring(FindIndexes(specs[i], "char="), specs[i])[0], spec);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this Dawn Font!", e.Message);
            }

            return thisData;
        }
        #endregion

        #region Sprites and Animation
        public KeyValuePair<string, SpriteSet>[] ReadSpriteData(string file, Texture tex)
        {
            List<KeyValuePair<string, SpriteSet>> spritePairs = new List<KeyValuePair<string, SpriteSet>>();
            SpriteSet value = null;

            string text = File.ReadAllText(file);
            JSONSpriteList[] data = JsonConvert.DeserializeObject<JSONSpriteList[]>(text);

            int i, k;
            int coord;

            try
            {
                for (i = 0; i < data.Length; i++)
                {
                    value = new SpriteSet();

                    for (k = 0; k < data[i].sprites.Count; k++)
                    {
                        value.sprites.Add(new SpriteSet.Sprite());

                        value.sprites[k].left = data[i].sprites[k][0] / (float)tex.Width;
                        value.sprites[k].top = 1 - (data[i].sprites[k][1] / (float)tex.Height);
                        value.sprites[k].right = data[i].sprites[k][2] / (float)tex.Width;
                        value.sprites[k].bottom = 1 - (data[i].sprites[k][3] / (float)tex.Height);

                        value.sprites[k].tex = tex;
                    }
                    spritePairs.Add(new KeyValuePair<string, SpriteSet>(data[i].name.ToLower(), value));
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this sprite data!", e.Message);
            }

            return spritePairs.ToArray();

        }

        public KeyValuePair<string, TextureAnimator.AnimationState>[] ReadSpriteAnimData(string file, Dictionary<string, SpriteSet> sprites)
        {
            string text = File.ReadAllText(file);
            JSONSpriteAnim[] anims = JsonConvert.DeserializeObject<JSONSpriteAnim[]>(text);

            SpriteSet set;
            TextureAnimator.AnimationState thisState;
            List<KeyValuePair<string, TextureAnimator.AnimationState>> animPairs = new List<KeyValuePair<string, TextureAnimator.AnimationState>>();

            int i, f;
            int spriteIndex;
            float duration;

            try
            {
                for (i = 0; i < anims.Length; i++)
                {
                    thisState = new TextureAnimator.AnimationState();
                    thisState.autoTransition = int.Parse(anims[i].autotransition);
                    thisState.loop = anims[i].loop == "true";
                    thisState.animFrames = new TextureAnimator.AnimationFrame[anims[i].frames.Count];

                    for (f = 0; f < anims[i].frames.Count; f++)
                    {
                        thisState.animFrames[f] = new TextureAnimator.AnimationFrame();

                        if (!float.TryParse(anims[i].frames[f].duration, out duration))
                            throw new Exception("Cannot parse " + anims[i].name + " frame " + f + " duration.");
                        thisState.animFrames[f].frameDuration = duration;

                        if (!sprites.TryGetValue(anims[i].frames[f].spriteset.ToLower(), out set))
                            throw new Exception("Spriteset " + anims[i].frames[f].spriteset + " has not been defined for " + anims[i].name + " frame " + f + ".");

                        if (!int.TryParse(anims[i].frames[f].sprite, out spriteIndex))
                            throw new Exception("Cannot parse " + anims[i].name + " frame " + f + " sprite index.");
                        thisState.animFrames[f].sprite = set.sprites[spriteIndex];
                    }

                    animPairs.Add(new KeyValuePair<string, TextureAnimator.AnimationState>(anims[i].name.ToLower(), thisState));
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this sprite anim!", e.Message);
            }

            return animPairs.ToArray();
        }
        #endregion

        #region Bullets
        public BulletData ReadBulletData(string file)
        {
            BulletData outputData = new BulletData();
            TextureAnimator.AnimationState state;

            string text = File.ReadAllText(file);
            JSONBulletData bulletData = JsonConvert.DeserializeObject<JSONBulletData>(text);

            int i, k;

            try
            {
                if (!gameMaster.shaders.TryGetValue(bulletData.shader.ToLower(), out outputData.shader))
                    throw new Exception("Can't find shader " + "\"" + bulletData.shader + "\" in dictionary");

                outputData.isAnimated = bulletData.isAnimated;
                outputData.spinSpeed = bulletData.spinSpeed;
                outputData.shouldTurn = bulletData.shouldTurn;
                outputData.spriteColors = (ushort)bulletData.spriteColorCount;
                outputData.randomizeSprite = bulletData.randomizeSprite;

                outputData.colliderSize = new Vector2[bulletData.colliders.Count];
                outputData.colliderOffset = new Vector2[bulletData.colliders.Count];

                for (i = 0; i < bulletData.colliders.Count; i++)
                {
                    outputData.colliderSize[i] = new Vector2(bulletData.colliders[i].sizeX, bulletData.colliders[i].sizeY);
                    outputData.colliderOffset[i] = new Vector2(bulletData.colliders[i].offsetX, bulletData.colliders[i].offsetY);
                }

                if (bulletData.boundsExitDist != -1)
                {
                    outputData.boundsExitDist = bulletData.boundsExitDist;
                }
                else
                {
                    float dist;
                    for (i = 0; i < outputData.colliderSize.Length; i++)
                    {
                        dist = outputData.colliderSize[i].X + Math.Abs(outputData.colliderOffset[i].X);
                        if (dist > outputData.boundsExitDist)
                            outputData.boundsExitDist = dist;

                        dist = outputData.colliderSize[i].Y + Math.Abs(outputData.colliderOffset[i].Y);
                        if (dist > outputData.boundsExitDist)
                            outputData.boundsExitDist = dist;
                    }
                }

                if (outputData.isAnimated)
                {
                    outputData.animStates = new TextureAnimator.AnimationState[bulletData.animStates.Count][];
                    for (i = 0; i < bulletData.animStates.Count; i++)
                    {
                        outputData.animStates[i] = new TextureAnimator.AnimationState[bulletData.animStates[i].Count];

                        for (k = 0; k < bulletData.animStates[i].Count; k++)
                        {
                            if (!gameMaster.bulletAnimStates.TryGetValue(bulletData.animStates[i][k], out outputData.animStates[i][k]))
                                throw new Exception("Can't find animation state " + "\"" + bulletData.animStates[i][k] + "\" in dictionary");
                        }
                    }
                }
                else
                    outputData.animStates = new TextureAnimator.AnimationState[0][];
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this bullet data!", e.Message);
            }

            return outputData;
        }
        #endregion

        #region Player Character
        public PlayerOrbData ReadOrbData(string file)
        {
            PlayerOrbData outputData = new PlayerOrbData();

            string text = File.ReadAllText(file);
            JSONOrbData orbData = JsonConvert.DeserializeObject<JSONOrbData>(text);

            int i;

            try
            {
                if (!gameMaster.shaders.TryGetValue(orbData.shader.ToLower(), out outputData.shader))
                    throw new Exception("Could not find shader \"" + orbData.shader.ToLower() + "\" in dictionary.");

                outputData.activePowerLevels = orbData.activePowerLevels.ToArray();
                outputData.unfocusPosition = new Vector2(orbData.unfocusPos.x, orbData.unfocusPos.y);
                outputData.focusPosition = new Vector2(orbData.focusPos.x, orbData.focusPos.y);

                outputData.framesToMove = orbData.framesToMove;
                outputData.rotateSpeed = orbData.rotateSpeed;

                outputData.animStates = new TextureAnimator.AnimationState[orbData.animStates.Count];
                for (i = 0; i < orbData.animStates.Count; i++)
                {
                    if (!gameMaster.playerOrbAnimStates.TryGetValue(orbData.animStates[i].ToLower(), out outputData.animStates[i]))
                        throw new Exception("Could not find animation \"" + orbData.animStates[i].ToLower() + "\" in dictionary.");
                }

                outputData.startAnimFrame = orbData.startAnimFrame;
                switch (orbData.leaveBehind)
                {
                    case "focus":
                    case "focused":
                    case "shift":
                    case "shifting":
                    case "holdingshift":
                        outputData.leaveBehindWhileFocused = true;
                        break;
                    case "unfocus":
                    case "unfocused":
                    case "unshift":
                    case "unshifting":
                    case "notshift":
                    case "notshifting":
                    case "notholdingshift":
                        outputData.leaveBehindWhileUnfocused = true;
                        break;
                }

                outputData.followPrevious = orbData.followPrevious;
                outputData.followDist = orbData.followDist;
                outputData.followDistSq = orbData.followDist * orbData.followDist;

                outputData.unfocusedPatterns = new Pattern[orbData.unfocusPatternsByPower.Count];
                for (i = 0; i < orbData.unfocusPatternsByPower.Count; i++)
                {
                    if (!gameMaster.playerPatterns.TryGetValue(orbData.unfocusPatternsByPower[i].ToLower(), out outputData.unfocusedPatterns[i]))
                        throw new Exception("Could not find player pattern \"" + orbData.unfocusPatternsByPower[i].ToLower() + "\" in dictionary.");
                }

                outputData.focusedPatterns = new Pattern[orbData.focusPatternsByPower.Count];
                for (i = 0; i < orbData.focusPatternsByPower.Count; i++)
                {
                    if (!gameMaster.playerPatterns.TryGetValue(orbData.focusPatternsByPower[i].ToLower(), out outputData.focusedPatterns[i]))
                        throw new Exception("Could not find player pattern \"" + orbData.focusPatternsByPower[i].ToLower() + "\" in dictionary.");
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player orb data!", e.Message);
            }

            return outputData;
        }

        public PlayerShotData ReadShotData(string file)
        {
            PlayerShotData outputData = new PlayerShotData();

            string text = File.ReadAllText(file);
            JSONShotData shotData = JsonConvert.DeserializeObject<JSONShotData>(text);

            int i;

            try
            {
                outputData.bombName = shotData.bombName.ToArray();

                outputData.moveSpeed = shotData.moveSpeed;
                outputData.focusModifier = shotData.focusSpeedPercent;
                outputData.colliderSize = shotData.colliderSize;
                outputData.colliderOffset = new Vector2(shotData.colliderOffsetX, shotData.colliderOffsetY);

                outputData.orbData = new PlayerOrbData[shotData.orbs.Count];
                for (i = 0; i < shotData.orbs.Count; i++)
                {
                    if (!gameMaster.playerOrbData.TryGetValue(shotData.orbs[i].ToLower(), out outputData.orbData[i]))
                        throw new Exception("Could not find orb data \"" + shotData.orbs[i].ToLower() + "\" in dictionary.");
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player shot data!", e.Message);
            }

            return outputData;
        }

        public PlayerTypeData ReadTypeData(string file)
        {
            PlayerTypeData outputData = new PlayerTypeData();

            string text = File.ReadAllText(file);
            JSONShotType typeData = JsonConvert.DeserializeObject<JSONShotType>(text);

            int i;

            try
            {
                outputData.name = typeData.name.ToArray();
                outputData.desc = typeData.desc.ToArray();

                outputData.shotData = new PlayerShotData[typeData.shotData.Count];
                for (i = 0; i < typeData.shotData.Count; i++)
                {
                    if (!gameMaster.playerShot.TryGetValue(typeData.shotData[i].ToLower(), out outputData.shotData[i]))
                        throw new Exception("Could not find shot data \"" + typeData.shotData[i].ToLower() + "\" in dictionary.");
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player type data!", e.Message);
            }

            return outputData;
        }

        public PlayerCharData ReadPlayerCharData(string file)
        {
            PlayerCharData outputData = new PlayerCharData();

            string text = File.ReadAllText(file);
            JSONCharacter charData = JsonConvert.DeserializeObject<JSONCharacter>(text);

            int i, k;

            try
            {
                if (!gameMaster.shaders.TryGetValue(charData.characterShader.ToLower(), out outputData.charShader))
                    throw new Exception("Could not find shader \"" + charData.characterShader.ToLower() + "\" in dictionary.");
                if (!gameMaster.shaders.TryGetValue(charData.hitboxShader.ToLower(), out outputData.hitboxShader))
                    throw new Exception("Could not find shader \"" + charData.hitboxShader.ToLower() + "\" in dictionary.");
                if (!gameMaster.shaders.TryGetValue(charData.focusEffectShader.ToLower(), out outputData.focusEffectShader))
                    throw new Exception("Could not find shader \"" + charData.focusEffectShader.ToLower() + "\" in dictionary.");

                outputData.name = charData.name.ToArray();

                outputData.moveSpeed = charData.moveSpeed;
                outputData.focusModifier = charData.focusSpeedPercent;
                outputData.colliderSize = charData.colliderSize;
                outputData.colliderOffset = new Vector2(charData.colliderOffsetX, charData.colliderOffsetY);

                if(!gameMaster.sfx.TryGetValue(charData.hitSound.ToLower(), out outputData.hitSound))
                    throw new Exception("Could not find sound \"" + charData.hitSound.ToLower() + "\" in dictionary.");
                if (!gameMaster.sfx.TryGetValue(charData.focusSound.ToLower(), out outputData.focusSound))
                    throw new Exception("Could not find sound \"" + charData.focusSound.ToLower() + "\" in dictionary.");
                if (!gameMaster.sfx.TryGetValue(charData.grazeSound.ToLower(), out outputData.grazeSound))
                    throw new Exception("Could not find sound \"" + charData.grazeSound.ToLower() + "\" in dictionary.");

                if (!gameMaster.playerEffectAnimStates.TryGetValue(charData.hitboxAnim.ToLower(), out outputData.hitboxAnim))
                    throw new Exception("Could not find animation \"" + charData.hitboxAnim.ToLower() + "\" in dictionary.");
                if (!gameMaster.playerEffectAnimStates.TryGetValue(charData.focusEffectAnim.ToLower(), out outputData.focusEffectAnim))
                    throw new Exception("Could not find animation \"" + charData.focusEffectAnim.ToLower() + "\" in dictionary.");

                outputData.hitboxInsetAmount = charData.hitboxPixelInset;
                outputData.focusEffectRotSpeed = charData.focusEffectRotSpeed;

                outputData.animStates = new TextureAnimator.AnimationState[charData.animStates.Count];
                for (i = 0; i < charData.animStates.Count; i++)
                {
                    if (!gameMaster.playerAnimStates.TryGetValue(charData.animStates[i].ToLower(), out outputData.animStates[i]))
                        throw new Exception("Could not find animation \"" + charData.animStates[i].ToLower() + "\" in dictionary.");
                }

                outputData.types = new PlayerTypeData[charData.shotTypes.Count];
                for (i = 0; i < charData.shotTypes.Count; i++)
                {
                    if (!gameMaster.playerTypes.TryGetValue(charData.shotTypes[i].ToLower(), out outputData.types[i]))
                        throw new Exception("Could not find shot type \"" + charData.shotTypes[i].ToLower() + "\" in dictionary.");

                    for (k = 0; k < outputData.types[i].shotData.Length; k++)
                    {
                        if (outputData.types[i].shotData[k].moveSpeed == -1)
                            outputData.types[i].shotData[k].moveSpeed = outputData.moveSpeed;
                        if (outputData.types[i].shotData[k].focusModifier == -1)
                            outputData.types[i].shotData[k].focusModifier = outputData.focusModifier;
                    }
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player character data!", e.Message);
            }

            return outputData;
        }
        #endregion

        #region Enemies

        public Bezier ReadBezierData(string file)
        {
            Bezier outputData = new Bezier();

            string text = File.ReadAllText(file);
            JSONBezier bezierData = JsonConvert.DeserializeObject<JSONBezier>(text);
            Bezier.Point point;

            int i;
            try
            {
                outputData.scale = bezierData.scale;
                outputData.autoSetPoints = bezierData.autoSetPoints;

                for (i = 0; i < bezierData.pointsList.Count; i++)
                {
                    point = new Bezier.Point(new Vector2(bezierData.pointsList[i].pos.x, bezierData.pointsList[i].pos.y), bezierData.pointsList[i].time, bezierData.pointsList[i].waitTime);
                    outputData.points.Add(point);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this bezier data!", e.Message);
            }

            return outputData;
        }
        public EnemyData ReadEnemyData(string file)
        {
            EnemyData outputData = new EnemyData();

            string text = File.ReadAllText(file);
            JSONEnemyData enemyData = JsonConvert.DeserializeObject<JSONEnemyData>(text);

            int i, k;

            try
            {
                outputData.enemyName = GetFileNameOnly(file);

                if (!gameMaster.shaders.TryGetValue(enemyData.shader.ToLower(), out outputData.shader))
                    throw new Exception("Can't find shader " + "\"" + enemyData.shader.ToLower() + "\" in dictionary");

                outputData.health = enemyData.health;
                outputData.spawnIFrames = enemyData.spawnIFrames;
                outputData.deathScoreValue = enemyData.deathScore;

                outputData.hurtColliderSize = enemyData.hurtColliderSize;
                outputData.hurtColliderOffset = new Vector2(enemyData.hurtColliderOffset.x, enemyData.hurtColliderOffset.y);
                outputData.killColliderSize = new Vector2(enemyData.playerKillcolliderSize.x, enemyData.playerKillcolliderSize.y);
                outputData.hurtColliderOffset = new Vector2(enemyData.playerKillColliderOffset.x, enemyData.playerKillColliderOffset.y);

                if (!gameMaster.enemyMovementPaths.TryGetValue(enemyData.movementCurve.ToLower(), out outputData.movementCurve))
                    throw new Exception("Can't find movement curve " + "\"" + enemyData.movementCurve.ToLower() + "\" in dictionary");

                outputData.animations = new TextureAnimator.AnimationState[enemyData.animations.Count];
                for (i = 0; i < enemyData.animations.Count; i++)
                {
                    if (!gameMaster.enemyAnimStates.TryGetValue(enemyData.animations[i].ToLower(), out outputData.animations[i]))
                        throw new Exception("Can't find animation " + "\"" + enemyData.animations[i].ToLower() + "\" in dictionary");
                }

                outputData.patternsByDifficulty = new EnemyData.Difficulty[enemyData.patternsByDifficulty.Count];
                for (i = 0; i < enemyData.patternsByDifficulty.Count; i++)
                {
                    outputData.patternsByDifficulty[i] = new EnemyData.Difficulty();
                    outputData.patternsByDifficulty[i].patterns = new Pattern[enemyData.patternsByDifficulty[i].Count];
                    for (k = 0; k < enemyData.patternsByDifficulty[i].Count; k++)
                    {
                        if (!gameMaster.enemyPatterns.TryGetValue(enemyData.patternsByDifficulty[i][k].ToLower(), out outputData.patternsByDifficulty[i].patterns[k]))
                            throw new Exception("Can't find enemy pattern " + "\"" + enemyData.patternsByDifficulty[i][k].ToLower() + "\" in dictionary");
                    }
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this enemy data!", e.Message);
            }

            return outputData;
        }

        public Pattern ReadPatternData(string file)
        {
            PatternBullets outputData = new PatternBullets();

            string text = File.ReadAllText(file);
            JSONBulletPattern patternData = JsonConvert.DeserializeObject<JSONBulletPattern>(text);

            int i, k;
            AudioData audio = null;
            BulletElement.BulletStage stage;

            string colorString;

            try
            {
                outputData.burstCount = patternData.burstCount;
                outputData.bulletsInBurst = patternData.bulletsInBurst;
                outputData.overallDegrees = patternData.overallDegrees;
                outputData.degreeOffset = patternData.degreeOffset;
                outputData.randomDegreeOffset = patternData.randomDegreeOffset;
                outputData.aimed = patternData.aimed;
                outputData.initialDelay = patternData.initialDelay;

                outputData.burstDelay = patternData.burstDelay;
                outputData.perBulletDelay = patternData.perBulletDelay;

                for (i = 0; i < patternData.spawnSounds.Count; i++)
                {
                    if(gameMaster.sfx.TryGetValue(patternData.spawnSounds[i].ToLower(), out audio))
                        outputData.spawnSounds.Add(audio);
                }

                outputData.damage = patternData.damage;

                for (i = 0; i < patternData.offsets.Count; i++)
                    outputData.offsets.Add(new Vector2(patternData.offsets[i].x, patternData.offsets[i].y));

                outputData.turnOffsets = patternData.turnOffsets;

                for (i = 0; i < patternData.bulletStages.Count; i++)
                {
                    stage = new BulletElement.BulletStage();
                    stage.bulletType = patternData.bulletStages[i].bulletType.ToLower();

                    colorString = patternData.bulletStages[i].bulletColor;
                    colorString = colorString.Substring(0, 1).ToUpper() + colorString.Substring(1).ToLower();
                    stage.bulletColor = (int)Enum.Parse<BulletElement.BulletColor>(colorString);
                    stage.Color = new Vector4(patternData.bulletStages[i].color.r,
                                              patternData.bulletStages[i].color.g, 
                                              patternData.bulletStages[i].color.b,
                                              patternData.bulletStages[i].color.a);
                    stage.framesToChangeTint = patternData.bulletStages[i].framesTochangeTint;
                    stage.renderScale = patternData.bulletStages[i].renderScale;


                    stage.angle = patternData.bulletStages[i].angle;
                    stage.startingSpeed = patternData.bulletStages[i].startingSpeed;
                    stage.endingSpeed = patternData.bulletStages[i].endingSpeed;
                    stage.framesToChangeSpeed = patternData.bulletStages[i].framesToChangeSpeed;
                    stage.rotate = patternData.bulletStages[i].rotate;
                    stage.reAim = patternData.bulletStages[i].reAim;
                    stage.keepOldAngle = patternData.bulletStages[i].keepOldAngle;
                    stage.modifyAngle = patternData.bulletStages[i].modifyAngle;
                    stage.affectedByTimescale = patternData.bulletStages[i].affectedByTimescale;

                    stage.framesToLast = patternData.bulletStages[i].framesToLast;
                    stage.turnAtStart = patternData.bulletStages[i].turnAtStart;
                    stage.initialMoveDelay = patternData.bulletStages[i].initialMoveDelay;
                    stage.turnAfterDelay = patternData.bulletStages[i].turnAfterDelay;

                    if (patternData.bulletStages[i].stageSound.ToLower() != "null")
                        gameMaster.sfx.TryGetValue(patternData.bulletStages[i].stageSound.ToLower(), out stage.stageSound);

                    stage.hasEffect = patternData.bulletStages[i].hasEffect;
                    if(stage.hasEffect)
                    {
                        colorString = patternData.bulletStages[i].effectColor;
                        colorString = colorString.Substring(0, 1).ToUpper() + colorString.Substring(1).ToLower();
                        stage.effectColor = Enum.Parse<BulletElement.BulletColor>(colorString);

                        stage.effectSize = new Vector2(patternData.bulletStages[i].effectSize.x, patternData.bulletStages[i].effectSize.y);
                        stage.effectOpacity = new Vector2(patternData.bulletStages[i].effectOpacity.start, patternData.bulletStages[i].effectOpacity.end);
                    }
                    outputData.bulletStages.Add(new PatternBullets.InstanceList());
                    outputData.bulletStages[i].instances.Add(stage);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this pattern data!", e.Message);
            }

            return outputData;
        }
        #endregion

        #region Game Settings
        public GameSettings ReadGameSettings(string file)
        {
            GameSettings outputData = new GameSettings();

            string text = File.ReadAllText(file);
            JSONGameSettings settingData = JsonConvert.DeserializeObject<JSONGameSettings>(text);

            int i;
            try
            {
                outputData.debugMode = settingData.debugMode;
                outputData.logAllFontChars = settingData.logAllFontChars;
                outputData.canToggleInvincible = settingData.pressIToToggleInvincible;
                outputData.logTimers = settingData.logTimers;

                outputData.maxPower = settingData.maxPower;
                outputData.powerLostOnDeath = settingData.powerLostOnDeath;
                outputData.powerTotalDroppedOnDeath = settingData.powerTotalDroppedOnDeath;
                outputData.powerLevelSplits = settingData.powerLevelSplits.ToArray();

                outputData.playerBoundsX = new Vector2(settingData.playerBoundsX.min, settingData.playerBoundsX.max);
                outputData.playerBoundsY = new Vector2(settingData.playerBoundsY.min, settingData.playerBoundsY.max);

                outputData.bulletBoundsX = new Vector2(settingData.bulletBoundsX.min, settingData.bulletBoundsX.max);
                outputData.bulletBoundsY = new Vector2(settingData.bulletBoundsY.min, settingData.bulletBoundsY.max);

                outputData.enemyBoundsX = new Vector2(settingData.enemyBoundsX.min, settingData.enemyBoundsX.max);
                outputData.enemyBoundsY = new Vector2(settingData.enemyBoundsY.min, settingData.enemyBoundsY.max);

                outputData.grazeDistance = settingData.grazeDistance;
                outputData.fullPowerCollection = settingData.fullPowerForCollection;
                outputData.shiftForCollection = settingData.shiftAtTopToCollect;

                outputData.maxItemCount = settingData.maxItemCount;
                outputData.collectionHeight = settingData.pointOfCollectionHeight;

                outputData.itemDisableHeight = settingData.itemDisableHeight;
                outputData.itemRandXRange = new Vector2(settingData.itemRandXVelRange.min, settingData.itemRandXVelRange.max);
                outputData.itemRandYRange = new Vector2(settingData.itemRandYVelRange.min, settingData.itemRandYVelRange.max);

                outputData.itemMaxFallSpeed = settingData.itemMaxFallSpeed;
                outputData.itemGravAccel = settingData.itemGravAccel;
                outputData.itemXDecel = settingData.itemXDecel;

                outputData.itemMagnetDist = settingData.itemMagnetDist;
                outputData.itemMagnetSpeed = settingData.itemMagnetSpeed;
                outputData.itemDrawSpeed = settingData.itemDrawSpeed;
                outputData.itemCollectDist = settingData.itemCollectDist;

                if (!gameMaster.shaders.TryGetValue(settingData.generalTextShader.ToLower(), out outputData.generalTextShader))
                    throw new Exception("Can't find shader " + "\"" + settingData.generalTextShader.ToLower() + "\" in dictionary");

                if (!gameMaster.shaders.TryGetValue(settingData.dialogueTextShader.ToLower(), out outputData.dialogueTextShader))
                    throw new Exception("Can't find shader " + "\"" + settingData.generalTextShader.ToLower() + "\" in dictionary");

                outputData.mainStageFolderNames = settingData.mainStages.ToArray();
                outputData.exStageFolderNames = settingData.exStages.ToArray();

                outputData.languages = settingData.languages.ToArray();

                for (i = 0; i < settingData.renderLayers.Count; i++)
                {
                    outputData.renderLayers[settingData.renderLayers[i].name.ToLower()] = i;
                    outputData.renderLayerSettings.Add(new GameMaster.RenderLayer(settingData.renderLayers[i].hasDepth, settingData.renderLayers[i].hasLighting));
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading the game's settings!", e.Message);
            }

            return outputData;
        }
        #endregion

        #region Items
        public ItemData ReadItemData(string file)
        {
            ItemData outputData = new ItemData();

            string text = File.ReadAllText(file);
            JSONItemData itemData = JsonConvert.DeserializeObject<JSONItemData>(text);

            int i;

            try
            {
                if (!gameMaster.shaders.TryGetValue(itemData.shader.ToLower(), out outputData.shader))
                    throw new Exception("Can't find shader " + "\"" + itemData.shader.ToLower() + "\" in dictionary");

                outputData.canBeAutoCollected = itemData.canBeAutoCollected;
                outputData.autoDraw = itemData.autoDraw;

                outputData.animations = new TextureAnimator.AnimationState[itemData.animStates.Count];
                for (i = 0; i < itemData.animStates.Count; i++)
                {
                    if (!gameMaster.itemAnimStates.TryGetValue(itemData.animStates[i].ToLower(), out outputData.animations[i]))
                        throw new Exception("Can't find animation " + "\"" + itemData.animStates[i].ToLower() + "\" in dictionary");
                }

                if(itemData.overrideDefaultValues)
                {
                    outputData.randXRange = new Vector2(itemData.randXVelRange.min, itemData.randXVelRange.max);
                    outputData.randYRange = new Vector2(itemData.randYVelRange.min, itemData.randYVelRange.max);
                    outputData.maxFallSpeed = itemData.maxFallSpeed;
                    outputData.gravAccel = itemData.gravAccel;
                    outputData.xDecel = itemData.xDecel;
                    outputData.magnetDist = itemData.magnetDist;
                    outputData.magnetSpeed = itemData.magnetSpeed;
                    outputData.drawSpeed = itemData.drawSpeed;
                    outputData.collectDist = itemData.collectDist;
                }
                else
                {
                    outputData.randXRange = gameMaster.itemRandXRange;
                    outputData.randYRange = gameMaster.itemRandYRange;
                    outputData.maxFallSpeed = gameMaster.itemMaxFallSpeed;
                    outputData.gravAccel = gameMaster.itemGravAccel;
                    outputData.xDecel = gameMaster.itemXDecel;
                    outputData.magnetDist = gameMaster.itemMagnetDist;
                    outputData.magnetSpeed = gameMaster.itemMagnetSpeed;
                    outputData.drawSpeed = gameMaster.itemDrawSpeed;
                    outputData.collectDist = gameMaster.itemCollectDist;
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this item data!", e.Message);
            }
            outputData.magnetDistSqr = outputData.magnetDist * outputData.magnetDist;
            outputData.collectDistSqr = outputData.collectDist * outputData.collectDist;
            return outputData;
        }
        #endregion

        #region Models
        public KeyValuePair<string, TexturedModel>[] ReadModelData(string file, Dictionary<string, Mesh> meshDic, Dictionary<string, Texture> texDic)
        {
            List<KeyValuePair<string, TexturedModel>> modelLinks = new List<KeyValuePair<string, TexturedModel>>();
            TexturedModel texturedModel;

            string text = File.ReadAllText(file);
            JSONModelData[] modelData = JsonConvert.DeserializeObject<JSONModelData[]>(text);

            int i;

            try
            {
                for (i = 0; i < modelData.Length; i++)
                {
                    texturedModel = new TexturedModel();

                    if (!gameMaster.shaders.TryGetValue(modelData[i].shader.ToLower(), out texturedModel.shader))
                        throw new Exception("Can't find shader " + "\"" + modelData[i].shader.ToLower() + "\" in dictionary");

                    if (!gameMaster.backgroundMeshes.TryGetValue(modelData[i].model.ToLower(), out texturedModel.modelMesh))
                        throw new Exception("Can't find background model " + "\"" + modelData[i].model.ToLower() + "\" in dictionary");

                    if (!gameMaster.backgroundTextures.TryGetValue(modelData[i].texture.ToLower(), out texturedModel.modelTex))
                        throw new Exception("Can't find background texture " + "\"" + modelData[i].texture.ToLower() + "\" in dictionary");

                    texturedModel.scale = modelData[i].scale;

                    modelLinks.Add(new KeyValuePair<string, TexturedModel>(modelData[i].name.ToLower(), texturedModel));
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this model data!", e.Message);
            }

            return modelLinks.ToArray();
        }

        public Mesh ReadObjData(string file)
        {
            Mesh finalMesh = new Mesh();

            List<Vector3> vertexData = new List<Vector3>();
            List<Vector2> texCoordData = new List<Vector2>();
            List<uint[]> triangleData = new List<uint[]>();
            List<uint[]> texIndicesData = new List<uint[]>();

            List<float> finalVertex = new List<float>();
            List<uint> finalTriangleData = new List<uint>();
            List<uint> finalTexIndices = new List<uint>();

            string[] tempStrings, subTempStrings, allLines = File.ReadAllLines(file);
            int i, t, h, f;
            try
            {
                for (i = 0; i < allLines.Length; i++)
                {
                    if (allLines[i].StartsWith("v "))
                    {
                        allLines[i] = allLines[i].Remove(0, 2);
                        tempStrings = allLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        vertexData.Add(new Vector3(ParseFloat(tempStrings[0]), ParseFloat(tempStrings[1]), ParseFloat(tempStrings[2])));
                    }
                    else if (allLines[i].StartsWith("vt "))
                    {
                        allLines[i] = allLines[i].Remove(0, 3);
                        tempStrings = allLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        texCoordData.Add(new Vector2(ParseFloat(tempStrings[0]), ParseFloat(tempStrings[1])));
                    }
                    else if (allLines[i].StartsWith("f "))
                    {
                        allLines[i] = allLines[i].Remove(0, 2);
                        tempStrings = allLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        triangleData.Add(new uint[tempStrings.Length]);
                        texIndicesData.Add(new uint[tempStrings.Length]);
                        for (t = 0; t < tempStrings.Length; t++)
                        {
                            subTempStrings = tempStrings[t].Split("/", StringSplitOptions.RemoveEmptyEntries);
                            if (subTempStrings.Length >= 1)
                                triangleData[triangleData.Count - 1][t] = (uint)(ParseRoundedNum(subTempStrings[0]) - 1);
                            if (subTempStrings.Length >= 2)
                                texIndicesData[texIndicesData.Count - 1][t] = (uint)(ParseRoundedNum(subTempStrings[1]) - 1);
                        }
                    }
                }

                for (i = 0; i < triangleData.Count; i++)
                {
                    for (t = 0; t < triangleData[i].Length - 2; t++)
                    {
                        if (t % 2 == 0)
                        {
                            finalTriangleData.Add(triangleData[i][t]);
                            finalTriangleData.Add(triangleData[i][t + 1]);
                            finalTriangleData.Add(triangleData[i][t + 2]);
                            finalTexIndices.Add(texIndicesData[i][t]);
                            finalTexIndices.Add(texIndicesData[i][t + 1]);
                            finalTexIndices.Add(texIndicesData[i][t + 2]);
                        }
                        else
                        {
                            finalTriangleData.Add(triangleData[i][t - 1]);
                            finalTriangleData.Add(triangleData[i][t + 1]);
                            finalTriangleData.Add(triangleData[i][t + 2]);
                            finalTexIndices.Add(texIndicesData[i][t - 1]);
                            finalTexIndices.Add(texIndicesData[i][t + 1]);
                            finalTexIndices.Add(texIndicesData[i][t + 2]);
                        }
                    }
                }


                List<uint> prevFinalTriData = new List<uint>();
                for (i = 0; i < finalTriangleData.Count; i++)
                    prevFinalTriData.Add(finalTriangleData[i]);

                List<int> vertexIndexes;
                Dictionary<uint, List<int>> duplicateVertices;

                for (i = vertexData.Count - 1; i >= 0; i--)
                {
                    vertexIndexes = new List<int>();
                    duplicateVertices = new Dictionary<uint, List<int>>();
                    for (t = 0; t < finalTriangleData.Count; t++)
                        if (finalTriangleData[t] == i)
                            vertexIndexes.Add(t);
                    for (t = 0; t < vertexIndexes.Count; t++)
                    {
                        if (!duplicateVertices.ContainsKey(finalTexIndices[vertexIndexes[t]]))
                            duplicateVertices.Add(finalTexIndices[vertexIndexes[t]], new List<int>());
                        duplicateVertices[finalTexIndices[vertexIndexes[t]]].Add(vertexIndexes[t]);
                        if (finalTexIndices.IndexOf(finalTexIndices[vertexIndexes[t]]) < vertexIndexes[t])
                        {
                            vertexIndexes.RemoveAt(t);
                            t--;
                        }
                    }
                    for (t = vertexIndexes.Count - 1; t >= 0; t--)
                    {
                        finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]]].Y);
                        finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]]].X);
                        finalVertex.Add(vertexData[i].Z);
                        finalVertex.Add(vertexData[i].Y);
                        finalVertex.Add(vertexData[i].X);

                        if (t > 0)
                            for (h = 0; h < finalTriangleData.Count; h++)
                                if (finalTriangleData[h] > i)
                                    finalTriangleData[h]++;
                    }

                    for (t = 1; t < vertexIndexes.Count; t++)
                        for (h = t; h < vertexIndexes.Count; h++)
                        {
                            for (f = 0; f < duplicateVertices[finalTexIndices[vertexIndexes[h]]].Count; f++)
                            {
                                finalTriangleData[duplicateVertices[finalTexIndices[vertexIndexes[h]]][f]]++;
                            }
                        }
                }

                finalVertex.Reverse();
                for (i = 0; i < finalTriangleData.Count; i += 3)
                {
                    finalTriangleData.Reverse(i, 3);
                }

                finalMesh.vertices = finalVertex.ToArray();
                finalMesh.triangleData = finalTriangleData.ToArray();
            }
            catch(Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this obj file!", e.Message);
            }

            return finalMesh;
        }
        #endregion

        #region Stages
        public BackgroundSection ReadBackgroundSectionData(string file)
        {
            const string NULL_STRING = "null";
            BackgroundSection outputData = new BackgroundSection();

            string text = File.ReadAllText(file);
            JSONBackgroundSectionData secData = JsonConvert.DeserializeObject<JSONBackgroundSectionData>(text);

            int loopCountA, loopCountB, loopCountC;
            int i, k, c, p;
            List<object> constructorArgs = new List<object>(20);
            List<string> constructorDataValues;

            TexturedModel[] models;
            List<Element> tempElementList = new List<Element>(20);

            string elementType;
            Type tempType;
            ConstructorInfo[] tempConInf;
            ParameterInfo[] tempParamInf;
            bool constructorIsPossible = false;

            int tempInt;
            float tempFloat;
            bool tempBool;

            try
            {
                outputData.secLength = secData.sectionLength;
                outputData.offset = new Vector3(secData.offset.x, secData.offset.y, secData.offset.z);

                for (i = 0; i < secData.objectSetup.Count; i++)
                {
                    loopCountA = secData.objectSetup[i].objectOptions.Count;
                    models = new TexturedModel[loopCountA];
                    for (k = 0; k < loopCountA; k++)
                    {
                        if (secData.objectSetup[i].objectOptions[k].ToLower() == NULL_STRING)
                        {
                            models[k] = null;
                            continue;
                        }

                        if (!gameMaster.backgroundModels.TryGetValue(secData.objectSetup[i].objectOptions[k].ToLower(), out models[k]))
                            throw new Exception("Can't find background textured model " + "\"" + secData.objectSetup[i].objectOptions[k].ToLower() + "\" in dictionary");
                    }
                    outputData.models.Add(models);

                    outputData.parentInds.Add(secData.objectSetup[i].parentIndex);
                    outputData.modelPos.Add(new Vector3(secData.objectSetup[i].pos.x, secData.objectSetup[i].pos.y, secData.objectSetup[i].pos.z));
                    outputData.modelRot.Add(new Vector3(secData.objectSetup[i].rot.x, secData.objectSetup[i].rot.y, secData.objectSetup[i].rot.z));
                    outputData.modelScale.Add(new Vector3(secData.objectSetup[i].scale.x, secData.objectSetup[i].scale.y, secData.objectSetup[i].scale.z));

                    tempElementList = new List<Element>(20);

                    loopCountA = secData.objectSetup[i].elements.Count;
                    for (k = 0; k < loopCountA; k++)
                    {
                        elementType = secData.objectSetup[i].elements[k].capsSensitiveName;
                        tempType = Type.GetType("DawnmakuEngine.Elements." + elementType, false, true);

                        if (tempType.BaseType != typeof(Element))
                        {
                            GameMaster.LogWarningMessage("There was a warning loading this background segment data!", "Gave element " + elementType + " to section data, this is not a valid Element.");
                            continue;
                        }

                        tempConInf = tempType.GetConstructors();
                        loopCountB = tempConInf.Length;

                        constructorDataValues = secData.objectSetup[i].elements[k].constructorValues;

                        for (c = 0; c < loopCountB; c++)
                        {
                            tempParamInf = tempConInf[c].GetParameters();
                            if (tempParamInf.Length != constructorDataValues.Count)
                                continue;
                            loopCountC = tempParamInf.Length;
                            constructorArgs.Clear();

                            constructorIsPossible = true;

                            for (p = 0; p < tempParamInf.Length; p++)
                            {
                                switch (tempParamInf[p].ParameterType.ToString().ToLower().Replace("system.", ""))
                                {
                                    case "byte":
                                    case "int16":
                                    case "int32":
                                    case "int64":
                                    case "uint16":
                                    case "uint32":
                                    case "uint64":
                                        if (!int.TryParse(constructorDataValues[p], out tempInt))
                                            constructorIsPossible = false;
                                        else
                                            constructorArgs.Add(tempInt);
                                        break;
                                    case "single":
                                    case "double":
                                    case "decimal":
                                        if (!float.TryParse(constructorDataValues[p], out tempFloat))
                                            constructorIsPossible = false;
                                        else
                                            constructorArgs.Add(tempFloat);
                                        break;
                                    case "string":
                                        constructorArgs.Add(constructorDataValues[p]);
                                        break;
                                    case "char":
                                        constructorArgs.Add(constructorDataValues[p][0]);
                                        break;
                                    case "boolean":
                                        if (!bool.TryParse(constructorDataValues[p], out tempBool))
                                            constructorIsPossible = false;
                                        else
                                            constructorArgs.Add(tempBool);
                                        break;
                                }
                                if (!constructorIsPossible)
                                    break;
                            }
                            if (constructorIsPossible)
                            {
                                tempElementList.Add((Element)tempConInf[c].Invoke(constructorArgs.ToArray()));
                                tempElementList[tempElementList.Count - 1].Disable();
                                break;
                            }

                        }
                    }
                    outputData.elements.Add(tempElementList);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this background segment data!", e.Message);
            }

            return outputData;
        }

        public void ReadStageData(string file, string[] bgmPaths)
        {
            StageData outputData = new StageData();

            string text = File.ReadAllText(file);
            JSONStageData data = JsonConvert.DeserializeObject<JSONStageData>(text);

            int i, k;
            string simplifiedText;

            StageData.AmbientLights newAmbient;
            StageData.SectionSpawns newSectionSpawn;
            BackgroundSection backgroundSegment;
            StageData.CamMovements newCamMovement;
            StageData.EnemySpawn newEnemySpawn;
            Vector3 tempColor;

            try
            {
                simplifiedText = SimplifyText(data.stageBGM, true);
                for (i = 0; i < bgmPaths.Length; i++)
                {
                    if (SimplifyText(bgmPaths[i], true).Contains(simplifiedText))
                    {
                        outputData.stageTrackFile = bgmPaths[i];
                        break;
                    }
                }
                simplifiedText = SimplifyText(data.bossBGM, true);
                for (i = 0; i < bgmPaths.Length; i++)
                {
                    if (SimplifyText(bgmPaths[i], true).Contains(simplifiedText))
                    {
                        outputData.bossTrackFile = bgmPaths[i];
                        break;
                    }
                }

                for (i = 0; i < data.ambientLights.Count; i++)
                {
                    newAmbient = new StageData.AmbientLights();
                    newAmbient.intensity = data.ambientLights[i].intensity;

                    tempColor.X = data.ambientLights[i].colorVal.r;
                    tempColor.Y = data.ambientLights[i].colorVal.g;
                    tempColor.Z = data.ambientLights[i].colorVal.b;
                    tempColor = ConvertColorToRgb(data.ambientLights[i].colorForm, tempColor, false);
                    newAmbient.r = tempColor.X;
                    newAmbient.g = tempColor.Y;
                    newAmbient.b = tempColor.Z;

                    newAmbient.time = data.ambientLights[i].startTime;
                    newAmbient.transitionTime = data.ambientLights[i].transitionTime;

                    outputData.ambientLights.Add(newAmbient);
                }

                for (i = 0; i < data.backgroundCamMovement.Count; i++)
                {
                    newCamMovement = new StageData.CamMovements();
                    newCamMovement.vel = new Vector3(data.backgroundCamMovement[i].vel.x, data.backgroundCamMovement[i].vel.y, data.backgroundCamMovement[i].vel.z);
                    newCamMovement.randMinPos = new Vector3(data.backgroundCamMovement[i].randMinPos.x, data.backgroundCamMovement[i].randMinPos.y, data.backgroundCamMovement[i].randMinPos.z);
                    newCamMovement.randMaxPos = new Vector3(data.backgroundCamMovement[i].randMaxPos.x, data.backgroundCamMovement[i].randMaxPos.y, data.backgroundCamMovement[i].randMaxPos.z);
                    newCamMovement.randMinRot = new Vector3(data.backgroundCamMovement[i].randMinRot.x, data.backgroundCamMovement[i].randMinRot.y, data.backgroundCamMovement[i].randMinRot.z);
                    newCamMovement.randMaxRot = new Vector3(data.backgroundCamMovement[i].randMaxRot.x, data.backgroundCamMovement[i].randMaxRot.y, data.backgroundCamMovement[i].randMaxRot.z);

                    newCamMovement.randDelay = data.backgroundCamMovement[i].randomizationDelay;
                    newCamMovement.time = data.backgroundCamMovement[i].startTime;
                    newCamMovement.transitionTime = data.backgroundCamMovement[i].transitionTime;

                    outputData.camMovements.Add(newCamMovement);
                }

                for (i = 0; i < data.backgroundSegments.Count; i++)
                {
                    newSectionSpawn = new StageData.SectionSpawns();

                    newSectionSpawn.time = data.backgroundSegments[i].time;
                    newSectionSpawn.spawnDirection = new Vector3(data.backgroundSegments[i].spawnDirection.x, data.backgroundSegments[i].spawnDirection.y, data.backgroundSegments[i].spawnDirection.z);
                    newSectionSpawn.segmentCount = data.backgroundSegments[i].segmentCount;
                    for (k = 0; k < data.backgroundSegments[i].segments.Count; k++)
                    {
                        if (!gameMaster.backgroundSections.TryGetValue(data.backgroundSegments[i].segments[k].ToLower(), out backgroundSegment))
                            throw new Exception("Can't find background section " + "\"" + data.backgroundSegments[i].segments[k].ToLower() + "\" in dictionary");
                        newSectionSpawn.section.Add(backgroundSegment);
                    }

                    outputData.secSpawns.Add(newSectionSpawn);
                }

                for (i = 0; i < data.enemySpawns.Count; i++)
                {
                    newEnemySpawn = new StageData.EnemySpawn();
                    if (!gameMaster.enemyData.TryGetValue(data.enemySpawns[i].enemy.ToLower(), out newEnemySpawn.enemy))
                        throw new Exception("Can't find enemy data " + "\"" + data.enemySpawns[i].enemy.ToLower() + "\" in dictionary");

                    newEnemySpawn.time = data.enemySpawns[i].time;
                    newEnemySpawn.pos = new Vector3(data.enemySpawns[i].pos.x, data.enemySpawns[i].pos.y, 0);

                    newEnemySpawn.itemSpawns = new ItemData[data.enemySpawns[i].items.Count];
                    for (k = 0; k < data.enemySpawns[i].items.Count; k++)
                    {
                        if (!gameMaster.itemData.TryGetValue(data.enemySpawns[i].items[k].ToLower(), out newEnemySpawn.itemSpawns[k]))
                            throw new Exception("Can't find item data " + "\"" + data.enemySpawns[i].items[k].ToLower() + "\" in dictionary");
                    }

                    outputData.enemySpawns.Add(newEnemySpawn);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this stage data!", e.Message);
            }

            stageData = outputData;
        }

        #endregion

        #endregion
    }
}

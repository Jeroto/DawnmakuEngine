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
using OpenTK.Graphics.OpenGL;

using System.Drawing;
using OpenTK.Graphics.ES20;
using System.Drawing.Text;
using SixLabors.ImageSharp.ColorSpaces;
//using System.Windows.Media;

namespace DawnmakuEngine
{
    class DataLoader
    {
        GameMaster gameMaster;
        public string directory, generalDir, bulletDataDir, playerDataDir, itemDataDir,
            generalTexDir, bulletTexDir, playerTexDir, playerOrbTexDir, playerFxTexDir, itemTexDir, enemyTexDir, UITexDir,
            generalAnimDir, bulletAnimDir, playerAnimDir, playerOrbAnimDir, playerFxAnimDir, itemAnimDir, enemyAnimDir, texAnimDir,
            playerPatternDir, playerOrbDir,
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
            directory = dir.FullName;
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
                }
            }
            dirs = new DirectoryInfo(playerDataDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch (dirs[i].Name)
                {
                    case "PlayerOrbs":
                        playerOrbDir = dirs[i].FullName;
                        break;
                    case "PlayerPatterns":
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

        /// <summary>
        /// Load consistent game data only once
        /// </summary>
        public void InitializeGame()
        {
            GameMaster.StartTimer();
            ShaderLoader();
            GameSettingsLoader();
            LoadAudio();
            LoadBullets();
            LoadPlayerOrbs();
            LoadPlayerEffects();
            LoadPlayers();
            LoadItems();
            LoadUI();
            GameMaster.LogTimeMilliseconds("game init");
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
            FontLoader();
        }

        public void LoadStage(string[] bgmPaths)
        {
            StageDataLoader(bgmPaths);
            //Stage-specific data
        }

        public void GameSettingsLoader()
        {
            string file = Directory.GetFiles(generalDir, "*.dwngame")[0];

            GameMaster.LogPositiveNotice("Loading game settings...\n");
            GameSettings settings = ReadGameSettings(file);
            GameMaster.debugMode = settings.runInDebugMode;
            GameMaster.ShowConsole(GameMaster.debugMode);

            gameMaster.logAllFontChars = settings.logAllFontChars && GameMaster.debugMode;
            gameMaster.canToggleInvincible = settings.canToggleInvincible && GameMaster.debugMode;
            gameMaster.logTimers = settings.logTimers && GameMaster.debugMode;


            gameMaster.maxPower = settings.maxPower;
            gameMaster.powerLostOnDeath = settings.powerLostOnDeath;
            gameMaster.powerTotalDroppedOnDeath = settings.powerTotalDroppedOnDeath;
            gameMaster.powerLevelSplits = settings.powerLevelSplits;
            gameMaster.fullPowerPOC = settings.fullPowerPOC;
            gameMaster.shiftForPOC = settings.shiftForPOC;

            gameMaster.playerBoundsX = settings.playerBoundsX;
            gameMaster.playerBoundsY = settings.playerBoundsY;
            gameMaster.grazeDistance = settings.grazeDistance;

            gameMaster.bulletBoundsX = settings.bulletBoundsX;
            gameMaster.bulletBoundsY = settings.bulletBoundsY;

            gameMaster.enemyBoundsX = settings.enemyBoundsX;
            gameMaster.enemyBoundsY = settings.enemyBoundsY;


            gameMaster.maxItemCount = settings.maxItemCount;
            gameMaster.pocHeight = settings.pocHeight;
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

        public void ShaderLoader()
        {
            string[] files = Directory.GetFiles(shaderDir, "*.dwnshader");

            GameMaster.LogPositiveNotice("\nShaders:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.shaders.Add(GetFileNameOnly(files[i]), ReadShaderData(files[i]));
            }
        }

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
            string[] files = Directory.GetFiles(bulletTexDir, "*.dwnsprites");
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
            string[] files = Directory.GetFiles(bulletAnimDir, "*.dwnanim");
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
                    gameMaster.bulletAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }

        public void BulletDataLoader()
        {
            string[] files = Directory.GetFiles(bulletDataDir, "*.dwnbullet");
            GameMaster.LogPositiveNotice("\nBullet Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.bulletData.Add(GetFileNameOnly(files[i]), ReadBulletData(files[i]));
            }
        }

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
            string[] files = Directory.GetFiles(playerOrbTexDir, "*.dwnsprites");
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
            string[] files = Directory.GetFiles(playerOrbAnimDir, "*.dwnanim");
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
                    gameMaster.playerOrbAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }
        public void PlayerOrbDataLoader()
        {
            string[] files = Directory.GetFiles(playerOrbDir, "*.dwnorb");

            GameMaster.LogPositiveNotice("\nPlayer Orb Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerOrbData.Add(GetFileNameOnly(files[i]), ReadOrbData(files[i]));
            }
        }
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
            string[] files = Directory.GetFiles(playerTexDir, "*.dwnsprites");
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
            string[] files = Directory.GetFiles(playerAnimDir, "*.dwnanim");
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
                    gameMaster.playerAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }

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
            string[] files = Directory.GetFiles(playerFxTexDir, "*.dwnsprites");
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
            string[] files = Directory.GetFiles(playerFxAnimDir, "*.dwnanim");
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
                    gameMaster.playerEffectAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }

        public void PlayerShotLoader()
        {
            string[] files = Directory.GetFiles(playerDataDir, "*.dwnshot");

            GameMaster.LogPositiveNotice("\nPlayer Shots:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.playerShot.Add(GetFileNameOnly(files[i]), ReadShotData(files[i]));
            }
        }
        public void PlayerTypeLoader()
        {
            string[] files = Directory.GetFiles(playerDataDir, "*.dwntype");

            GameMaster.LogPositiveNotice("\nPlayer Shot Types:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.playerTypes.Add(GetFileNameOnly(files[i]), ReadTypeData(files[i]));
            }
        }
        public void PlayerCharLoader()
        {
            string[] files = Directory.GetFiles(playerDataDir, "*.dwnchar");

            GameMaster.LogPositiveNotice("\nPlayer Characters:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.playerChars.Add(GetFileNameOnly(files[i]), ReadPlayerCharData(files[i]));
            }
        }
        public void PlayerPatternLoader()
        {
            string[] files = Directory.GetFiles(playerPatternDir, "*.dwnpattern");

            GameMaster.LogPositiveNotice("\nPlayer Patterns:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                if (!File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.playerPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(files[i], gameMaster.playerPatterns));
            }
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                if (File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.playerPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(files[i], gameMaster.playerPatterns));
            }
        }

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
            string[] files = Directory.GetFiles(itemTexDir, "*.dwnsprites");
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
            string[] files = Directory.GetFiles(itemAnimDir, "*.dwnanim");
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
                    gameMaster.itemAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }
        public void ItemDataLoader()
        {
            string[] files = Directory.GetFiles(itemDataDir, "*.dwnitem");

            GameMaster.LogPositiveNotice("\nItem Datas:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.itemData.Add(GetFileNameOnly(files[i]), ReadItemData(files[i]));
                gameMaster.itemTypes.Add(GetFileNameOnly(files[i]));
            }
        }

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

        public void EnemyPatternLoader()
        {
            string[] files = Directory.GetFiles(enemyPatternDir, "*.dwnpattern");

            GameMaster.LogPositiveNotice("\nEnemy Patterns:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                if (!File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.enemyPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(files[i], gameMaster.enemyPatterns));
            }
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                if (File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.enemyPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(files[i], gameMaster.enemyPatterns));
            }
        }
        public void EnemyMovementLoader()
        {
            string[] files = Directory.GetFiles(enemyMovementDir, "*.dwnbezier");

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
            string[] files = Directory.GetFiles(enemyTexDir, "*.dwnsprites");
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
            string[] files = Directory.GetFiles(enemyAnimDir, "*.dwnanim");
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
                    gameMaster.enemyAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }

        public void EnemyDataLoader()
        {
            string[] files = Directory.GetFiles(enemyDir, "*.dwnenemy");

            GameMaster.LogPositiveNotice("\nEnemy Data:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.enemyData.Add(GetFileNameOnly(files[i]), ReadEnemyData(files[i]));
            }
        }



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
            string[] files = Directory.GetFiles(backgroundModelDir, "*.dwnmodel");
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
            string[] files = Directory.GetFiles(backgroundSecDir, "*.dwnbacksec");
            GameMaster.LogPositiveNotice("\nBackground Section:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.backgroundSections.Add(GetFileNameOnly(files[i]), ReadBackgroundSectionData(files[i]));
            }
        }



        public void UITextureLoader()
        {
            string[] files = Directory.GetFiles(UITexDir, "*.png");

            GameMaster.LogPositiveNotice("\nUI Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                GameMaster.Log(GetFileNameOnly(files[i]));
                gameMaster.UITextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }

        public void FontLoader()
        {
            GameMaster.LogPositiveNotice("\nFonts:");
            string[] files = Directory.GetFiles(fontDir, "*.ttf");
            int i = 0;
            try
            {
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


        public void StageDataLoader(string[] bgmPaths)
        {
            string file = Directory.GetFiles(curStageDir, "*.dwnstage")[0];

            GameMaster.LogPositiveNotice("\nLoading Stage");
            try
            {
                ReadStageData(file, bgmPaths);
            }
            catch(Exception e)
            {
                GameMaster.LogErrorMessage("Failed to load stage file: " + file, e.Message);
            }
        }



        //Loading Assistance
        public string SimplifyText(string data)
        {
            return data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
        }

        public int[] FindIndexes(string text, string opener, int startIndex = 0, string closer = ";")
        {
            if (!text.Contains(opener))
                return new int[] { text.Length - 1, text.Length };
            int indexStart = text.IndexOf(opener, startIndex) + opener.Length;

            int indexEnd = text.IndexOf(closer, indexStart);
            if (indexEnd == -1)
                indexEnd = text.Length;
            return new int[] { indexStart, indexEnd };
        }

        public string GetInputSubstring(int[] indexes, string data)
        {
            return data.Substring(indexes[0], indexes[1] - indexes[0]);
        }
        
        public string GetInputSubstring(int index, string data)
        {
            return data.Substring(index, data.Length - index);
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
            string finalString = "", currentString;
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
            return finalString;
        }
        public string GetData(string[] data, string opener, ref int lineOffset, bool simplify = true, string closer = ";")
        {
            string finalString = "", currentString;
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
            return finalString;
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

        public float ParseFloat(int[] indexes, string data)
        {
            float numVal = 0;
            float.TryParse(GetInputSubstring(indexes, data), out numVal);
            return numVal;
        }
        public float ParseFloat(string data)
        {
            float numVal = 0;
            float.TryParse(data, out numVal);
            return numVal;
        }
        public Vector2 ParseVector2(int[] indexes, string data)
        {
            Vector2 vec2 = new Vector2();
            string[] tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
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
        public Vector3 ParseVector3(int[] indexes, string data)
        {
            Vector3 vec3 = new Vector3();
            string[] tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
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
            vec3.Z = ParseFloat(tempStrings[3]);
            return vec3;
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
        public int ParseRoundedNum(int[] indexes, string data)
        {
            return Round(ParseFloat(indexes, data));
        }
        public int ParseRoundedNum(string data)
        {
            return Round(ParseFloat(data));
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

        public Shader ReadShaderData(string file)
        {
            string[] allLines = File.ReadAllLines(file);
            string vert, frag, data;
            data = GetData(allLines, "vert=");
            vert = shaderDir + "\\" + GetInputSubstring(FindIndexes(data, "vert="), data);
            data = GetData(allLines, "frag=");
            frag = shaderDir + "\\" + GetInputSubstring(FindIndexes(data, "frag="), data);
            return new Shader(vert, frag);
        }

        public FontCharList CreateCharList(string file)
        {
            FontCharList charList = new FontCharList();
            char newChar;
            IDictionary<int, ushort> tempDict = new Dictionary<int, ushort>();

            Typeface typeFace = new OpenFontReader().Read(new FileStream(file, FileMode.Open, FileAccess.Read));
            
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
            if(gameMaster.logAllFontChars)
                GameMaster.LogNeutralNotice(charList.everyCharInFont);

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

        public Bitmap RenderFont(string fontName, string file, out List<SpriteSet.Sprite> sprites)
        {
            sprites = new List<SpriteSet.Sprite>();
            try
            {
                int i;
                const int size = 25;
                //Image<A8> textImage;
                string allCharacters = gameMaster.fontCharList[fontName].everyCharInFont;
                int width = 0, height = 0;

                //SixLabors.Fonts.FontFamily family = gameMaster.fonts.Find(fontName);
                //Font font = family.CreateFont(size);
                Bitmap bm = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
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
                return bm;
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

            return new Bitmap(1,1, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
        }

        public void InstallAndRenderFont(string file)
        {
            List<SpriteSet.Sprite> tempSprites;
            gameMaster.fonts.Install(file);
            string fontName = gameMaster.fonts.Families.Last().Name;
            gameMaster.fontNames.Add(fontName);
            gameMaster.fontCharList.Add(fontName, CreateCharList(file));
            Texture tempTex = new Texture(RenderFont(fontName, file, out tempSprites), true);
            gameMaster.fontSheets.Add(fontName, tempTex);
            gameMaster.fontGlyphSprites.Add(fontName, new SpriteSet(tempSprites, tempTex));
        }


        public BulletData ReadBulletData(string file)
        {
            BulletData thisData = new BulletData();
            int i, s;
            int[] indexes = { 0, 0 };
            string[] tempStrings, allLines = File.ReadAllLines(file);
            TextureAnimator.AnimationState state;
            string data;

            try
            {
                data = GetData(allLines, "shader=");
                thisData.shader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "shader="), data)];

                data = GetData(allLines, "isanimated=");
                thisData.isAnimated = ParseBool(FindIndexes(data, "isanimated="), data);

                data = GetData(allLines, "shouldspin=");
                thisData.shouldSpin = ParseBool(FindIndexes(data, "shouldspin="), data);

                data = GetData(allLines, "shouldturn=");
                thisData.shouldTurn = ParseBool(FindIndexes(data, "shouldturn="), data);

                data = GetData(allLines, "collidersize=");
                tempStrings = GetInputSubstring(FindIndexes(data, "collidersize="), data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisData.colliderSize = new Vector2[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                    thisData.colliderSize[i] = ParseVector2(tempStrings[i]) / 2;

                data = GetData(allLines, "collideroffset=");
                tempStrings = GetInputSubstring(FindIndexes(data, "collideroffset="), data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisData.colliderOffset = new Vector2[tempStrings.Length >= thisData.colliderSize.Length ? tempStrings.Length : thisData.colliderSize.Length];
                for (i = 0; i < thisData.colliderSize.Length; i++)
                    thisData.colliderOffset[i] = ParseVector2(tempStrings[Math.Clamp(i, 0, tempStrings.Length - 1)]);

                if (thisData.colliderOffset.Length > thisData.colliderSize.Length)
                {
                    Vector2[] prevColliderSizeList = thisData.colliderSize;
                    thisData.colliderSize = new Vector2[thisData.colliderOffset.Length];
                    for (i = 0; i < thisData.colliderSize.Length; i++)
                        thisData.colliderSize[i] = prevColliderSizeList[Math.Clamp(i, 0, prevColliderSizeList.Length - 1)];
                }

                if(CheckHasData(allLines, "boundsexitdistance="))
                {
                    data = GetData(allLines, "boundsexitdistance=");
                    thisData.boundsExitDist = ParseFloat(FindIndexes(data, "boundsexitdistance="), data);
                }
                else
                {
                    float dist;
                    for (i = 0; i < thisData.colliderSize.Length; i++)
                    {
                        dist = thisData.colliderSize[i].X + Math.Abs(thisData.colliderOffset[i].X);
                        if (dist > thisData.boundsExitDist)
                            thisData.boundsExitDist = dist;

                        dist = thisData.colliderSize[i].Y + Math.Abs(thisData.colliderOffset[i].Y);
                        if (dist > thisData.boundsExitDist)
                            thisData.boundsExitDist = dist;
                    }
                }

                data = GetData(allLines, "spritecolorcount=");
                thisData.spriteColors = (ushort)ParseRoundedNum(FindIndexes(data, "spritecolorcount="), data);

                data = GetData(allLines, "randomizesprite=");
                thisData.randomizeSprite = ParseBool(FindIndexes(data, "randomizesprite="), data);

                if (thisData.isAnimated)
                {
                    data = GetData(allLines, "animstates=", 0);
                    indexes = FindIndexes(data, "animstates=");
                    string[] stateStrings = GetInputSubstring(indexes, data).Split(':', StringSplitOptions.RemoveEmptyEntries);

                    thisData.animStates = new TextureAnimator.AnimationState[stateStrings.Length][];

                    for (i = 0; i < stateStrings.Length; i++)
                    {
                        tempStrings = stateStrings[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
                        thisData.animStates[i] = new TextureAnimator.AnimationState[tempStrings.Length];
                        for (s = 0; s < tempStrings.Length; s++)
                        {
                            gameMaster.bulletAnimStates.TryGetValue(tempStrings[s], out state);
                            thisData.animStates[i][s] = state;
                        }
                    }
                }
                else
                    thisData.animStates = new TextureAnimator.AnimationState[0][];
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this bullet data!", e.Message);
            }

            return thisData;
        }

        public Bezier ReadBezierData(string file)
        {
            Bezier thisBezier = new Bezier();
            int[] indexes = { 0, 0 };
            float numVal;
            ushort shortVal;
            Vector2 vec2Val;
            string[] tempStrings, allLines = File.ReadAllLines(file);
            string data;

            try
            {
                data = GetData(allLines, "scale=");
                thisBezier.scale = ParseRoundedNum(FindIndexes(data, "scale="), data);

                data = GetData(allLines, "autosetpoints=");
                thisBezier.AutoSetPoints = ParseBool(FindIndexes(data, "autosetpoints="), data);

                data = GetData(allLines, "pointslist=", 0, true, "end");
                indexes = FindIndexes(data, "pointslist=", indexes[1], "end");
                tempStrings = GetInputSubstring(indexes[0], data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisBezier.points = new List<Bezier.Point>();
                indexes[1] = indexes[0];
                for (int i = 0; i < tempStrings.Length; i++)
                {
                    indexes = FindIndexes(tempStrings[i], "pos=", indexes[1]);
                    vec2Val = ParseVector2(tempStrings[i].Substring(indexes[0] + 1, indexes[1] - indexes[0] - 1));

                    numVal = ParseFloat(FindIndexes(tempStrings[i], "time=", indexes[1]), tempStrings[i]);

                    shortVal = (ushort)ParseRoundedNum(FindIndexes(tempStrings[i], "waittime=", indexes[1]), tempStrings[i]); ;

                    thisBezier.points.Add(new Bezier.Point(vec2Val, numVal, shortVal));
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this bezier data!", e.Message);
            }

            return thisBezier;
        }

        public KeyValuePair<string, SpriteSet>[] ReadSpriteData(string file, Texture tex)
        {
            int i = 0, s = 0, lineOffset = 0;
            int[] indexes = { 0, 0 };
            string[] spriteSets, sprites, spriteValues, allLines = File.ReadAllLines(file);
            string key = "", data;
            float coordinate;
            SpriteSet value = null;

            List<KeyValuePair<string, SpriteSet>> spritePairs = new List<KeyValuePair<string, SpriteSet>>();

            while (lineOffset < allLines.Length)
            {
                try
                {
                    data = GetData(allLines, "=", ref lineOffset);
                    if (data == "")
                        continue;
                    spriteSets = data.Split(';', StringSplitOptions.RemoveEmptyEntries);

                    for (i = 0; i < spriteSets.Length; i++)
                    {
                        spriteSets[i] += ";";
                        indexes = FindIndexes(spriteSets[i], "=");

                        key = spriteSets[i].Substring(0, indexes[0] - 1);
                        value = new SpriteSet();
                        sprites = GetInputSubstring(indexes, spriteSets[i]).Split(":", StringSplitOptions.RemoveEmptyEntries);

                        for (s = 0; s < sprites.Length; s++)
                        {
                            value.sprites.Add(new SpriteSet.Sprite());
                            spriteValues = sprites[s].Split(",", StringSplitOptions.RemoveEmptyEntries);
                            coordinate = ParseFloat(spriteValues[0]);
                            value.sprites[s].left = coordinate / tex.Width;
                            coordinate = ParseFloat(spriteValues[1]);
                            value.sprites[s].top = 1 - (coordinate / tex.Height);
                            coordinate = ParseFloat(spriteValues[2]);
                            value.sprites[s].right = coordinate / tex.Width;
                            coordinate = ParseFloat(spriteValues[3]);
                            value.sprites[s].bottom = 1 - (coordinate / tex.Height);

                            value.sprites[s].tex = tex;
                        }

                        spritePairs.Add(new KeyValuePair<string, SpriteSet>(key, value));
                    }
                }
                catch (Exception e)
                {
                    GameMaster.LogErrorMessage("There was an error loading this sprite data!", e.Message);
                }
            }

            return spritePairs.ToArray();
        }

        public KeyValuePair<string, TextureAnimator.AnimationState>[] ReadSpriteAnimData(string file, Dictionary<string, SpriteSet> sprites)
        {
            int i = 0, lineOffset = 0;
            int[] indexes = { 0, 0 };
            string[] frameSets, allLines = File.ReadAllLines(file);
            string stringVal, key, data;
            SpriteSet set;
            TextureAnimator.AnimationState thisState;
            List<KeyValuePair<string, TextureAnimator.AnimationState>> animPairs = new List<KeyValuePair<string, TextureAnimator.AnimationState>>();


            while (lineOffset < allLines.Length)
            {
                try
                {
                    data = GetData(allLines, "name=", ref lineOffset, true, "/");

                    if (data[data.Length - 1] == '/')
                        data = data.Remove(data.Length - 1);

                    thisState = new TextureAnimator.AnimationState();
                    indexes = FindIndexes(data, "name=");
                    key = GetInputSubstring(indexes, data);

                    thisState.autoTransition = ParseRoundedNum(FindIndexes(data, "autotransition="), data);

                    thisState.loop = ParseBool(FindIndexes(data, "loop="), data);

                    indexes = FindIndexes(data, "frames=", 0, "end");
                    frameSets = GetInputSubstring(indexes, data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                    thisState.animFrames = new TextureAnimator.AnimationFrame[frameSets.Length];
                    for (i = 0; i < frameSets.Length; i++)
                    {
                        thisState.animFrames[i] = new TextureAnimator.AnimationFrame();
                        indexes = FindIndexes(frameSets[i], "duration=");
                        thisState.animFrames[i].frameDuration = ParseFloat(indexes, frameSets[i]);

                        indexes = FindIndexes(frameSets[i], "spriteset=", indexes[1]);
                        stringVal = GetInputSubstring(indexes, frameSets[i]);

                        indexes = FindIndexes(frameSets[i], "sprite=", indexes[1]);
                        sprites.TryGetValue(stringVal, out set);
                        if (set != null)
                            thisState.animFrames[i].sprite = set.sprites[ParseRoundedNum(indexes, frameSets[i])];
                    }

                    animPairs.Add(new KeyValuePair<string, TextureAnimator.AnimationState>(key, thisState));
                }
                catch (Exception e)
                {
                    GameMaster.LogErrorMessage("There was an error loading this sprite anim!", e.Message);
                }
            }

            return animPairs.ToArray(); ;
        }

        public PlayerOrbData ReadOrbData(string file)
        {
            PlayerOrbData thisData = new PlayerOrbData();
            int i = 0;
            int[] indexes = { 0, 0 };
            float numVal = 0;
            string[] tempStrings, allLines = File.ReadAllLines(file);
            string data;

            try
            {
                data = GetData(allLines, "shader=");
                thisData.shader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "shader="), data)];

                data = GetData(allLines, "activepowerlevels=");
                indexes = FindIndexes(data, "activepowerlevels=");
                tempStrings = GetInputSubstring(indexes, data).Split(",");
                thisData.activePowerLevels = new bool[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.activePowerLevels[i] = ParseBool(tempStrings[i]);
                }

                data = GetData(allLines, "unfocuspos=");
                indexes = FindIndexes(data, "unfocuspos=");
                thisData.unfocusPosition = ParseVector2(indexes, data);

                data = GetData(allLines, "focusedpos=");
                indexes = FindIndexes(data, "focusedpos=");
                thisData.focusPosition = ParseVector2(indexes, data);

                data = GetData(allLines, "framestomove=");
                indexes = FindIndexes(data, "framestomove=");
                thisData.framesToMove = ParseFloat(indexes, data);

                data = GetData(allLines, "rotatedegreespersecond=");
                indexes = FindIndexes(data, "rotatedegreespersecond=");
                thisData.rotateDegreesPerSecond = ParseFloat(indexes, data);

                data = GetData(allLines, "animstates=");
                indexes = FindIndexes(data, "animstates=");
                tempStrings = GetInputSubstring(indexes, data).Split(",");
                thisData.animStates = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animStates[i] = gameMaster.playerOrbAnimStates[tempStrings[i]];
                }

                data = GetData(allLines, "startanimframe=");
                indexes = FindIndexes(data, "startanimframe=");
                thisData.startAnimFrame = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "leavebehind=");
                indexes = FindIndexes(data, "leavebehind=");
                switch (GetInputSubstring(indexes, data))
                {
                    case "focus":
                    case "focused":
                    case "shift":
                    case "shifting":
                    case "holdingshift":
                        thisData.leaveBehindWhileFocused = true;
                        break;
                    case "unfocus":
                    case "unfocused":
                    case "unshift":
                    case "unshifting":
                    case "notshift":
                    case "notshifting":
                    case "notholdingshift":
                        thisData.leaveBehindWhileUnfocused = true;
                        break;
                }

                data = GetData(allLines, "followprevious=");
                indexes = FindIndexes(data, "followprevious=");
                thisData.followPrevious = ParseBool(indexes, data);

                if (thisData.followPrevious)
                {
                    data = GetData(allLines, "followdist=");
                    indexes = FindIndexes(data, "followdist=");
                    numVal = ParseFloat(indexes, data);
                    thisData.followDist = numVal;
                    thisData.followDistSq = numVal * numVal;
                }


                data = GetData(allLines, "unfocuspatternsbypower=");
                indexes = FindIndexes(data, "unfocuspatternsbypower=");
                tempStrings = GetInputSubstring(indexes, data).Split(":");
                thisData.unfocusedPatterns = new Pattern[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.unfocusedPatterns[i] = gameMaster.playerPatterns[tempStrings[i]];
                }

                data = GetData(allLines, "focusedpatternsbypower=");
                indexes = FindIndexes(data, "focusedpatternsbypower=");
                tempStrings = GetInputSubstring(indexes, data).Split(":");
                thisData.focusedPatterns = new Pattern[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.focusedPatterns[i] = gameMaster.playerPatterns[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player orb data!", e.Message);
            }

            return thisData;
        }
        public PlayerShotData ReadShotData(string file)
        {
            PlayerShotData thisData = new PlayerShotData();
            int i = 0;
            int[] indexes = { 0, 0 };
            string[] tempStrings, allLines = File.ReadAllLines(file);
            string data;

            try
            {
                data = GetData(allLines, "bombname=");
                indexes = FindIndexes(data, "bombname=");
                tempStrings = GetInputSubstring(indexes, data).Replace('_', ' ').Split(",");
                thisData.bombName = new string[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.bombName[i] = tempStrings[i];
                }

                data = GetData(allLines, "movespeed=");
                indexes = FindIndexes(data, "movespeed=");
                thisData.moveSpeed = ParseFloat(indexes, data);

                data = GetData(allLines, "focusspeedpercent=");
                indexes = FindIndexes(data, "focusspeedpercent=");
                thisData.focusModifier = ParseFloat(indexes, data);

                data = GetData(allLines, "collidersize=");
                indexes = FindIndexes(data, "collidersize=");
                thisData.colliderSize = ParseFloat(indexes, data);

                data = GetData(allLines, "collideroffset=");
                indexes = FindIndexes(data, "collideroffset=");
                thisData.colliderOffset = ParseVector2(indexes, data);


                data = GetData(allLines, "orbs=");
                indexes = FindIndexes(data, "orbs=");
                tempStrings = GetInputSubstring(indexes, data).Split(",");
                thisData.orbData = new PlayerOrbData[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.orbData[i] = gameMaster.playerOrbData[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player shot data!", e.Message);
            }

            return thisData;
        }

        public PlayerTypeData ReadTypeData(string file)
        {
            PlayerTypeData thisData = new PlayerTypeData();
            int i = 0;
            int[] indexes = { 0, 0 };
            string[] tempStrings, allLines = File.ReadAllLines(file);
            string data;

            try
            {
                data = GetData(allLines, "name=");
                indexes = FindIndexes(data, "name=");
                tempStrings = GetInputSubstring(indexes, data).Replace('_', ' ').Split(",");
                thisData.name = new string[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.name[i] = tempStrings[i];
                }

                data = GetData(allLines, "desc=");
                indexes = FindIndexes(data, "desc=");
                tempStrings = GetInputSubstring(indexes, data).Replace('_', ' ').Split(",");
                thisData.desc = new string[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.desc[i] = tempStrings[i];
                }

                data = GetData(allLines, "shotdata=");
                indexes = FindIndexes(data, "shotdata=");
                tempStrings = GetInputSubstring(indexes, data).Split(",");
                thisData.shotData = new PlayerShotData[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.shotData[i] = gameMaster.playerShot[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player type data!", e.Message);
            }

            return thisData;
        }
        public PlayerCharData ReadPlayerCharData(string file)
        {
            PlayerCharData thisData = new PlayerCharData();
            int i = 0;
            int[] indexes = { 0, 0 };
            string[] tempStrings, allLines = File.ReadAllLines(file);
            string data;
            AudioData audioVal;

            try
            {
                data = GetData(allLines, "charactershader=");
                thisData.charShader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "charactershader="), data)];

                data = GetData(allLines, "hitboxshader=");
                thisData.hitboxShader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "hitboxshader="), data)];

                data = GetData(allLines, "focuseffectshader=");
                thisData.focusEffectShader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "focuseffectshader="), data)];

                data = GetData(allLines, "name=");
                indexes = FindIndexes(data, "name=");
                thisData.name = GetInputSubstring(indexes, data).Replace('_', ' ');
                data = GetData(allLines, "jpname=");
                indexes = FindIndexes(data, "jpname=");
                thisData.jpName = GetInputSubstring(indexes, data).Replace('_', ' ');

                data = GetData(allLines, "movespeed=");
                indexes = FindIndexes(data, "movespeed=");
                thisData.moveSpeed = ParseFloat(indexes, data);

                data = GetData(allLines, "focusspeedpercent=");
                indexes = FindIndexes(data, "focusspeedpercent=");
                thisData.focusModifier = ParseFloat(indexes, data);

                data = GetData(allLines, "collidersize=");
                indexes = FindIndexes(data, "collidersize=");
                thisData.colliderSize = ParseFloat(indexes, data) / 2;

                data = GetData(allLines, "collideroffset=");
                indexes = FindIndexes(data, "collideroffset=");
                thisData.colliderOffset = ParseVector2(indexes, data);

                data = GetData(allLines, "hitsound=");
                indexes = FindIndexes(data, "hitsound=");
                if (gameMaster.sfx.TryGetValue(GetInputSubstring(indexes, data), out audioVal))
                    thisData.hitSound = audioVal;

                data = GetData(allLines, "focussound=");
                indexes = FindIndexes(data, "focussound=");
                if (gameMaster.sfx.TryGetValue(GetInputSubstring(indexes, data), out audioVal))
                    thisData.focusSound = audioVal;

                data = GetData(allLines, "grazesound=");
                indexes = FindIndexes(data, "grazesound=");
                if (gameMaster.sfx.TryGetValue(GetInputSubstring(indexes, data), out audioVal))
                    thisData.grazeSound = audioVal;

                data = GetData(allLines, "shottypes=");
                indexes = FindIndexes(data, "shottypes=");
                tempStrings = GetInputSubstring(indexes, data).Split(",");
                thisData.types = new PlayerTypeData[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.types[i] = gameMaster.playerTypes[tempStrings[i]];
                }

                data = GetData(allLines, "animstates=");
                indexes = FindIndexes(data, "animstates=");
                tempStrings = GetInputSubstring(indexes, data).Split(",");
                thisData.animStates = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animStates[i] = gameMaster.playerAnimStates[tempStrings[i]];
                }

                data = GetData(allLines, "hitboxanim=");
                indexes = FindIndexes(data, "hitboxanim=");
                thisData.hitboxAnim = gameMaster.playerEffectAnimStates[GetInputSubstring(indexes, data)];

                data = GetData(allLines, "hitboxpixelinset=");
                indexes = FindIndexes(data, "hitboxpixelinset=");
                thisData.hitboxInsetAmount = ParseFloat(indexes, data) * 2;

                data = GetData(allLines, "focuseffectanim=");
                indexes = FindIndexes(data, "focuseffectanim=");
                thisData.focusEffectAnim = gameMaster.playerEffectAnimStates[GetInputSubstring(indexes, data)];
                data = GetData(allLines, "focuseffectrotspeed=");
                indexes = FindIndexes(data, "focuseffectrotspeed=");
                thisData.focusEffectRotSpeed = ParseFloat(indexes, data);
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this player character data!", e.Message);
            }

            return thisData;
        }

        public EnemyData ReadEnemyData(string file)
        {
            EnemyData thisData = new EnemyData();
            int i = 0;
            int[] indexes;
            string[] tempStrings, subTempStrings, allLines = File.ReadAllLines(file);
            string data;

            try
            {
                thisData.enemyName = GetFileNameOnly(file);

                data = GetData(allLines, "shader=");
                thisData.shader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "shader="), data)];

                data = GetData(allLines, "health=");
                indexes = FindIndexes(data, "health=");
                thisData.health = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "iframes=");
                indexes = FindIndexes(data, "iframes=");
                thisData.invTime = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "deathscore=");
                indexes = FindIndexes(data, "deathscore=");
                thisData.deathScoreValue = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "collidersize=");
                indexes = FindIndexes(data, "collidersize=");
                thisData.colliderSize = ParseFloat(indexes, data);

                data = GetData(allLines, "collideroffset=");
                indexes = FindIndexes(data, "collideroffset=");
                thisData.colliderOffset = ParseVector2(indexes, data);

                data = GetData(allLines, "movementcurve=");
                indexes = FindIndexes(data, "movementcurve=");
                thisData.movementCurve = gameMaster.enemyMovementPaths[GetInputSubstring(indexes, data)];

                data = GetData(allLines, "animations=");
                indexes = FindIndexes(data, "animations=");
                tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.animations = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animations[i] = gameMaster.enemyAnimStates[tempStrings[i]];
                }

                data = GetData(allLines, "patternsbydifficulty=", 0, true, "end");
                indexes = FindIndexes(data, "patternsbydifficulty=", 0, "end");
                tempStrings = GetInputSubstring(indexes[0], data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisData.patternsByDifficulty = new EnemyData.Difficulty[tempStrings.Length];

                for (int p = 0; p < tempStrings.Length; p++)
                {
                    thisData.patternsByDifficulty[p] = new EnemyData.Difficulty();

                    subTempStrings = tempStrings[p].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    thisData.patternsByDifficulty[p].patterns = new Pattern[subTempStrings.Length];
                    for (i = 0; i < subTempStrings.Length; i++)
                    {
                        thisData.patternsByDifficulty[p].patterns[i] = gameMaster.enemyPatterns[subTempStrings[i]];
                    }
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this enemy data!", e.Message);
            }

            return thisData;
        }

        public Pattern ReadPatternData(string file, Dictionary<string, Pattern> patternDic)
        {
            Pattern finalPattern = new Pattern();
            int i = 0;
            int[] indexes;
            float numVal = 0;
            int intVal = 0;
            bool boolVal = false;
            string stringVal = "";
            AudioData audioVal;
            int patternType = 0;
            string[] tempStrings, subTempStrings, allLines = File.ReadAllLines(file);
            string patternBase = null,
                data;

            /*data = File.ReadAllText(file);
            data = SimplifyText(data);*/

            try
            {
                data = GetData(allLines, "patternbase=");
                if (data.Contains("patternbase="))
                {
                    indexes = FindIndexes(data, "patternbase=");
                    patternBase = GetInputSubstring(indexes, data);
                    finalPattern = patternDic[patternBase].CopyPattern();
                }

                data = GetData(allLines, "patterntype=");
                indexes = FindIndexes(data, "patterntype=");
                switch (GetInputSubstring(indexes, data))
                {
                    case "bullet":
                    case "bullets":
                    case "bul":
                    case "b":
                        patternType = 0;
                        break;
                    case "curvylaser":
                    case "curvelaser":
                    case "curvy":
                    case "curve":
                    case "cur":
                    case "c":
                    case "laser":
                    case "las":
                    case "l":
                        patternType = 1;
                        break;
                    case "playerbeam":
                    case "beam":
                    case "be":
                        patternType = 3;
                        break;
                    default:
                        patternType = 999;
                        GameMaster.LogWarning("Pattern type " + GetInputSubstring(indexes, data) + "not recognized.");
                        break;
                }

                data = GetData(allLines, "burstcount=");
                indexes = FindIndexes(data, "burstcount=");
                if (TryParseRoundedNum(indexes, data, out intVal))
                    finalPattern.burstCount = intVal;

                data = GetData(allLines, "bulletsinburst=");
                indexes = FindIndexes(data, "bulletsinburst=");
                if (TryParseRoundedNum(indexes, data, out intVal))
                    finalPattern.bulletsInBurst = intVal;

                data = GetData(allLines, "overalldegrees=");
                indexes = FindIndexes(data, "overalldegrees=");
                if (TryParseFloat(indexes, data, out numVal))
                    finalPattern.overallDegrees = numVal;

                data = GetData(allLines, "degreeoffset=");
                indexes = FindIndexes(data, "degreeoffset=");
                if (TryParseFloat(indexes, data, out numVal))
                    finalPattern.degreeOffset = numVal;

                data = GetData(allLines, "randomdegreeoffset=");
                indexes = FindIndexes(data, "randomdegreeoffset=");
                if (TryParseFloat(indexes, data, out numVal))
                    finalPattern.randomDegreeOffset = numVal;

                data = GetData(allLines, "aimed=");
                indexes = FindIndexes(data, "aimed=");
                if (TryParseBool(indexes, data, out boolVal))
                    finalPattern.aimed = boolVal;


                data = GetData(allLines, "burstdelay=");
                indexes = FindIndexes(data, "burstdelay=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.burstDelay = new List<float>();
                    for (i = 0; i < tempStrings.Length; i++)
                        finalPattern.burstDelay.Add(ParseFloat(tempStrings[i]));
                }

                data = GetData(allLines, "initialdelay=");
                indexes = FindIndexes(data, "initialdelay=");
                if (TryParseFloat(indexes, data, out numVal))
                    finalPattern.initialDelay = numVal;

                data = GetData(allLines, "perbulletdelay=");
                indexes = FindIndexes(data, "perbulletdelay=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.perBulletDelay = new List<float>();
                    for (i = 0; i < tempStrings.Length; i++)
                        finalPattern.perBulletDelay.Add(ParseFloat(tempStrings[i]));
                }

                data = GetData(allLines, "spawnsounds=");
                indexes = FindIndexes(data, "spawnsounds=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.spawnSounds = new List<AudioData>();
                    for (i = 0; i < tempStrings.Length; i++)
                        if(gameMaster.sfx.TryGetValue(tempStrings[i], out audioVal))
                            finalPattern.spawnSounds.Add(audioVal);
                }

                data = GetData(allLines, "damage=");
                indexes = FindIndexes(data, "damage=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.damage = new List<ushort>();
                    for (i = 0; i < tempStrings.Length; i++)
                        finalPattern.damage.Add((ushort)Math.Clamp(ParseRoundedNum(tempStrings[i]), 0, ushort.MaxValue));
                }

                data = GetData(allLines, "offsets=");
                indexes = FindIndexes(data, "offsets=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.offsets = new List<Vector2>();
                    for (i = 0; i < tempStrings.Length; i += 2)
                        finalPattern.offsets.Add(new Vector2(ParseFloat(tempStrings[i]), ParseFloat(tempStrings[i + 1])));
                }

                data = GetData(allLines, "turnoffsets=");
                indexes = FindIndexes(data, "turnoffsets=");

                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.turnOffsets = new List<bool>();
                    for (i = 0; i < tempStrings.Length; i++)
                        finalPattern.turnOffsets.Add(ParseBool(tempStrings[i]));
                }

                switch (patternType)
                {
                    case 0:
                        {
                            PatternBullets thisPattern = new PatternBullets();

                            if (patternBase != null && patternDic[patternBase].GetType() == typeof(PatternBullets))
                                thisPattern = (PatternBullets)((PatternBullets)patternDic[patternBase]).CopyPattern();


                            thisPattern.burstCount = finalPattern.burstCount;
                            thisPattern.bulletsInBurst = finalPattern.bulletsInBurst;
                            thisPattern.overallDegrees = finalPattern.overallDegrees;
                            thisPattern.degreeOffset = finalPattern.degreeOffset;
                            thisPattern.randomDegreeOffset = finalPattern.randomDegreeOffset;
                            thisPattern.aimed = finalPattern.aimed;
                            thisPattern.burstDelay = finalPattern.burstDelay;
                            thisPattern.initialDelay = finalPattern.initialDelay;
                            for (i = 0; i < finalPattern.perBulletDelay.Count; i++)
                                thisPattern.perBulletDelay.Add(finalPattern.perBulletDelay[i]);
                            for (i = 0; i < finalPattern.spawnSounds.Count; i++)
                                thisPattern.spawnSounds.Add(finalPattern.spawnSounds[i]);
                            for (i = 0; i < finalPattern.damage.Count; i++)
                                thisPattern.damage.Add(finalPattern.damage[i]);
                            for (i = 0; i < finalPattern.offsets.Count; i++)
                                thisPattern.offsets.Add(new Vector2(finalPattern.offsets[i].X, finalPattern.offsets[i].Y));
                            for (i = 0; i < finalPattern.turnOffsets.Count; i++)
                                thisPattern.turnOffsets.Add(finalPattern.turnOffsets[i]);


                            data = GetData(allLines, "bulletstages=", 0, true, "fileend");
                            indexes = FindIndexes(data, "bulletstages=", 0, "fileend");
                            if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                            {
                                tempStrings = GetInputSubstring(indexes[0], data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                                if (patternBase == null)
                                    thisPattern.bulletStages = new List<PatternBullets.InstanceList>();
                                Elements.BulletElement.BulletStage thisInstance;
                                string[] subSubstrings;
                                for (int b = 0; b < tempStrings.Length; b++)
                                {
                                    if (patternBase == null || thisPattern.bulletStages.Count < b + 1)
                                        thisPattern.bulletStages.Add(new PatternBullets.InstanceList());
                                    subTempStrings = tempStrings[b].Split('/', StringSplitOptions.RemoveEmptyEntries);
                                    thisPattern.bulletStages[b].instances = new List<Elements.BulletElement.BulletStage>();
                                    for (i = 0; i < Math.Clamp(subTempStrings.Length, 1, int.MaxValue); i++)
                                    {
                                        if (b == 0)
                                            thisPattern.bulletStages[b].instances.Add(new Elements.BulletElement.BulletStage());
                                        else if (i != 0)
                                            thisPattern.bulletStages[b].instances.Add(thisPattern.bulletStages[b].instances[i - 1].CopyValues());
                                        else
                                            thisPattern.bulletStages[b].instances.Add(thisPattern.bulletStages[b - 1].instances[thisPattern.bulletStages[b - 1].instances.Count - 1].CopyValues());
                                        thisInstance = thisPattern.bulletStages[b].instances[i];

                                        indexes = FindIndexes(subTempStrings[i], "framestolast=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.framesToLast = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "bullettype=");
                                        if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                            thisInstance.spriteType = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]);

                                        indexes = FindIndexes(subTempStrings[i], "bulletcolor=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.bulletColor = Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletColor)).Length - 1);
                                        else if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                            thisInstance.bulletColor = (int)Enum.Parse<BulletElement.BulletColor>(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), true);

                                        indexes = FindIndexes(subTempStrings[i], "animatedsprite=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.animatedSprite = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "renderscale=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.renderScale = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "movementdirection=");
                                        if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                            thisInstance.movementDirection = ParseVector2(indexes, subTempStrings[i]);

                                        indexes = FindIndexes(subTempStrings[i], "startingspeed=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.startingSpeed = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "endingspeed=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.endingSpeed = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "framestochangespeed=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.framesToChangeSpeed = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "colorr=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.r = (byte)Math.Clamp(numVal, 0, 255);
                                        indexes = FindIndexes(subTempStrings[i], "colorg=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.g = (byte)Math.Clamp(numVal, 0, 255);
                                        indexes = FindIndexes(subTempStrings[i], "colorb=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.b = (byte)Math.Clamp(numVal, 0, 255);
                                        indexes = FindIndexes(subTempStrings[i], "colora=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.a = (byte)Math.Clamp(numVal, 0, 255);

                                        indexes = FindIndexes(subTempStrings[i], "colortint=");
                                        if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                        {
                                            subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                            thisInstance.r = (byte)Math.Clamp(ParseRoundedNum(subSubstrings[0]), 0, 255);
                                            thisInstance.g = (byte)Math.Clamp(ParseRoundedNum(subSubstrings[1]), 0, 255);
                                            thisInstance.b = (byte)Math.Clamp(ParseRoundedNum(subSubstrings[2]), 0, 255);
                                            thisInstance.a = (byte)Math.Clamp(ParseRoundedNum(subSubstrings[3]), 0, 255);
                                        }


                                        indexes = FindIndexes(subTempStrings[i], "framestochangetint=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.framesToChangeTint = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "rotate=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.rotate = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "reaim=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.reAim = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "keepoldangle=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.keepOldAngle = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "modifyangle=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.modifyAngle = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "turnatstart=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.turnAtStart = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "initialmovedelay=");
                                        if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                            thisInstance.initialMoveDelay = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "turnafterdelay=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.turnAfterDelay = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "stagesound=");
                                        if (TryGetInputSubstring(indexes, subTempStrings[i], out stringVal))
                                        {
                                            if (gameMaster.sfx.TryGetValue(stringVal, out audioVal))
                                                thisInstance.stageSound = audioVal;
                                            else
                                                thisInstance.stageSound = null;
                                        }
                                        

                                        indexes = FindIndexes(subTempStrings[i], "haseffect=");
                                        if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                            thisInstance.hasEffect = boolVal;

                                        if (thisInstance.hasEffect)
                                        {

                                            indexes = FindIndexes(subTempStrings[i], "effectcolor=");
                                            if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                                thisInstance.effectColor = (Elements.BulletElement.BulletColor)Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletColor)).Length - 1);

                                            indexes = FindIndexes(subTempStrings[i], "effectduration=");
                                            if (TryParseFloat(indexes, subTempStrings[i], out numVal))
                                                thisInstance.effectDuration = numVal;

                                            {
                                                indexes = FindIndexes(subTempStrings[i], "effectsize=");
                                                if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                                {
                                                    subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                                    thisInstance.effectSize = new Vector2(ParseFloat(subSubstrings[0]), ParseFloat(subSubstrings[1]));
                                                }

                                                indexes = FindIndexes(subTempStrings[i], "effectopacity=");
                                                if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                                {
                                                    subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);

                                                    thisInstance.effectOpacity = new Vector2(ParseFloat(subSubstrings[0]), ParseFloat(subSubstrings[1]));
                                                }
                                            }

                                            indexes = FindIndexes(subTempStrings[i], "affectedbytimescale=");
                                            if (TryParseBool(indexes, subTempStrings[i], out boolVal))
                                                thisInstance.affectedByTimescale = boolVal;

                                        }

                                    }
                                }
                            }


                            finalPattern = thisPattern;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this pattern data!", e.Message);
            }

            return finalPattern;
        }

        public GameSettings ReadGameSettings(string file)
        {
            GameSettings thisData = new GameSettings();
            int[] indexes = { 0, 0 };
            string[] substrings, tempSubstrings, allLines = File.ReadAllLines(file);
            string data, key;
            int i, value;
            GameMaster.RenderLayer layerSettings;

            try
            {
                data = GetData(allLines, "runindebugmode=");
                indexes = FindIndexes(data, "runindebugmode=");
                thisData.runInDebugMode = ParseBool(indexes, data);
                data = GetData(allLines, "logallfontchars=");
                indexes = FindIndexes(data, "logallfontchars=");
                thisData.logAllFontChars = ParseBool(indexes, data);
                data = GetData(allLines, "pressitotoggleinvincible=");
                indexes = FindIndexes(data, "pressitotoggleinvincible=");
                thisData.canToggleInvincible = ParseBool(indexes, data);
                data = GetData(allLines, "logtimers=");
                indexes = FindIndexes(data, "logtimers=");
                thisData.logTimers = ParseBool(indexes, data);

                data = GetData(allLines, "maxpower=");
                indexes = FindIndexes(data, "maxpower=");
                thisData.maxPower = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "powerlostondeath=");
                indexes = FindIndexes(data, "powerlostondeath=");
                thisData.powerLostOnDeath = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "powertotaldroppedondeath=");
                indexes = FindIndexes(data, "powertotaldroppedondeath=");
                thisData.powerTotalDroppedOnDeath = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "powerlevelsplits=");
                indexes = FindIndexes(data, "powerlevelsplits=");
                substrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.powerLevelSplits = new int[substrings.Length];
                thisData.maxPowerLevel = substrings.Length;
                for (i = 0; i < substrings.Length; i++)
                    thisData.powerLevelSplits[i] = ParseRoundedNum(substrings[i]);

                data = GetData(allLines, "fullpowerforpoc=");
                indexes = FindIndexes(data, "fullpowerforpoc=");
                thisData.fullPowerPOC = ParseBool(indexes, data);

                data = GetData(allLines, "shiftattoptocollectitems=");
                indexes = FindIndexes(data, "shiftattoptocollectitems=");
                thisData.shiftForPOC = ParseBool(indexes, data);

                data = GetData(allLines, "playerboundsx=");
                indexes = FindIndexes(data, "playerboundsx=");
                thisData.playerBoundsX = ParseVector2(indexes, data);

                data = GetData(allLines, "playerboundsy=");
                indexes = FindIndexes(data, "playerboundsy=");
                thisData.playerBoundsY = ParseVector2(indexes, data);

                data = GetData(allLines, "grazedistance=");
                indexes = FindIndexes(data, "grazedistance=");
                thisData.grazeDistance = ParseFloat(indexes, data);


                data = GetData(allLines, "bulletboundsx=");
                indexes = FindIndexes(data, "bulletboundsx=");
                thisData.bulletBoundsX = ParseVector2(indexes, data);

                data = GetData(allLines, "bulletboundsy=");
                indexes = FindIndexes(data, "bulletboundsy=");
                thisData.bulletBoundsY = ParseVector2(indexes, data);


                data = GetData(allLines, "enemyboundsx=");
                indexes = FindIndexes(data, "enemyboundsx=");
                thisData.enemyBoundsX = ParseVector2(indexes, data);

                data = GetData(allLines, "enemyboundsy=");
                indexes = FindIndexes(data, "enemyboundsy=");
                thisData.enemyBoundsY = ParseVector2(indexes, data);


                data = GetData(allLines, "maxitemcount=");
                indexes = FindIndexes(data, "maxitemcount=");
                thisData.maxItemCount = (ushort)ParseRoundedNum(indexes, data);

                data = GetData(allLines, "pointofcollectionheight=");
                indexes = FindIndexes(data, "pointofcollectionheight=");
                thisData.pocHeight = ParseRoundedNum(indexes, data);

                data = GetData(allLines, "itemdisableheight=");
                indexes = FindIndexes(data, "itemdisableheight=");
                thisData.itemDisableHeight = ParseRoundedNum(indexes, data);


                data = GetData(allLines, "itemrandxvelrange=");
                indexes = FindIndexes(data, "itemrandxvelrange=");
                thisData.itemRandXRange = ParseVector2(indexes, data);

                data = GetData(allLines, "itemrandyvelrange=");
                indexes = FindIndexes(data, "itemrandyvelrange=");
                thisData.itemRandYRange = ParseVector2(indexes, data);

                data = GetData(allLines, "itemmaxfallspeed=");
                indexes = FindIndexes(data, "itemmaxfallspeed=");
                thisData.itemMaxFallSpeed = ParseFloat(indexes, data);

                data = GetData(allLines, "itemgravaccel=");
                indexes = FindIndexes(data, "itemgravaccel=");
                thisData.itemGravAccel = ParseFloat(indexes, data);

                data = GetData(allLines, "itemxdecel=");
                indexes = FindIndexes(data, "itemxdecel=");
                thisData.itemXDecel = ParseFloat(indexes, data);

                data = GetData(allLines, "itemmagnetdist=");
                indexes = FindIndexes(data, "itemmagnetdist=");
                thisData.itemMagnetDist = ParseFloat(indexes, data);

                data = GetData(allLines, "itemmagnetspeed=");
                indexes = FindIndexes(data, "itemmagnetspeed=");
                thisData.itemMagnetSpeed = ParseFloat(indexes, data);

                data = GetData(allLines, "itemdrawspeed=");
                indexes = FindIndexes(data, "itemdrawspeed=");
                thisData.itemDrawSpeed = ParseFloat(indexes, data);

                data = GetData(allLines, "itemcollectdist=");
                indexes = FindIndexes(data, "itemcollectdist=");
                thisData.itemCollectDist = ParseFloat(indexes, data);


                data = GetData(allLines, "generaltextshader=");
                thisData.generalTextShader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "generaltextshader="), data)];
                data = GetData(allLines, "dialoguetextshader=");
                thisData.dialogueTextShader = gameMaster.shaders[GetInputSubstring(FindIndexes(data, "dialoguetextshader="), data)];


                data = GetData(allLines, "mainstages=");
                indexes = FindIndexes(data, "mainstages=");
                substrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.mainStageFolderNames = new string[substrings.Length];
                for (i = 0; i < substrings.Length; i++)
                    thisData.mainStageFolderNames[i] = substrings[i];

                data = GetData(allLines, "exstages=");
                indexes = FindIndexes(data, "exstages=");
                substrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.exStageFolderNames = new string[substrings.Length];
                for (i = 0; i < substrings.Length; i++)
                    thisData.exStageFolderNames[i] = substrings[i];

                data = GetData(allLines, "languages=");
                indexes = FindIndexes(data, "languages=");
                substrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.languages = new string[substrings.Length];
                for (i = 0; i < substrings.Length; i++)
                    thisData.languages[i] = substrings[i];


                data = GetData(allLines, "renderlayerindexes=");
                indexes = FindIndexes(data, "renderlayerindexes=");
                substrings = GetInputSubstring(indexes, data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                for (i = 0; i < substrings.Length; i++)
                {
                    tempSubstrings = substrings[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    indexes = FindIndexes(tempSubstrings[0], "layername=", 0, ",");
                    key = GetInputSubstring(indexes, tempSubstrings[0]);
                    indexes = FindIndexes(tempSubstrings[1], "index=", 0, ",");
                    value = ParseRoundedNum(indexes, tempSubstrings[1]);
                    thisData.renderLayers.Add(key, value);
                }


                data = GetData(allLines, "renderlayersettings=", 0, true, "endOfFile");
                indexes = FindIndexes(data, "renderlayersettings=", 0, "endOfFile");
                substrings = GetInputSubstring(indexes, data).Split(':', StringSplitOptions.RemoveEmptyEntries);
                for (i = 0; i < substrings.Length; i++)
                {
                    layerSettings = new GameMaster.RenderLayer();
                    indexes = FindIndexes(substrings[i], "hasdepth=");
                    layerSettings.hasDepth = ParseBool(indexes, substrings[i]);

                    thisData.renderLayerSettings.Add(layerSettings);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading the game's settings!", e.Message);
            }

            return thisData;
        }

        public ItemData ReadItemData(string file)
        {
            ItemData thisData = new ItemData();
            string[] tempStrings, allLines = File.ReadAllLines(file);
            int[] indexes = { 0, 0 };
            string data;
            int i;

            try
            {
                data = GetData(allLines, "overridedefaultvalues=");
                indexes = FindIndexes(data, "overridedefaultvalues=");

                thisData.randXRange = gameMaster.itemRandXRange;
                thisData.randYRange = gameMaster.itemRandYRange;
                thisData.maxFallSpeed = gameMaster.itemMaxFallSpeed;
                thisData.gravAccel = gameMaster.itemGravAccel;
                thisData.xDecel = gameMaster.itemXDecel;
                thisData.magnetDist = gameMaster.itemMagnetDist;
                thisData.magnetSpeed = gameMaster.itemMagnetSpeed;
                thisData.drawSpeed = gameMaster.itemDrawSpeed;
                thisData.collectDist = gameMaster.itemCollectDist;

                if (ParseBool(indexes, data))
                {
                    data = GetData(allLines, "itemrandxvelrange=");
                    indexes = FindIndexes(data, "itemrandxvelrange=");
                    if (indexes[0] >= 0)
                        thisData.randXRange = ParseVector2(indexes, data);

                    data = GetData(allLines, "itemrandyvelrange=");
                    indexes = FindIndexes(data, "itemrandyvelrange=");
                    if (indexes[0] >= 0)
                        thisData.randYRange = ParseVector2(indexes, data);

                    data = GetData(allLines, "itemmaxfallspeed=");
                    indexes = FindIndexes(data, "itemmaxfallspeed=");
                    if (indexes[0] >= 0)
                        thisData.maxFallSpeed = ParseFloat(indexes, data);

                    data = GetData(allLines, "itemgravaccel=");
                    indexes = FindIndexes(data, "itemgravaccel=");
                    if (indexes[0] >= 0)
                        thisData.gravAccel = ParseFloat(indexes, data);

                    data = GetData(allLines, "itemxdecel=");
                    indexes = FindIndexes(data, "itemxdecel=");
                    if (indexes[0] >= 0)
                        thisData.xDecel = ParseFloat(indexes, data);

                    data = GetData(allLines, "itemmagnetdist=");
                    indexes = FindIndexes(data, "itemmagnetdist=");
                    if (indexes[0] >= 0)
                        thisData.magnetDist = ParseFloat(indexes, data);

                    data = GetData(allLines, "itemmagnetspeed=");
                    indexes = FindIndexes(data, "itemmagnetspeed=");
                    if (indexes[0] >= 0)
                        thisData.magnetSpeed = ParseFloat(indexes, data);

                    data = GetData(allLines, "itemdrawspeed=");
                    indexes = FindIndexes(data, "itemdrawspeed=");
                    if (indexes[0] >= 0)
                        thisData.drawSpeed = ParseFloat(indexes, data);

                    data = GetData(allLines, "itemcollectdist=");
                    indexes = FindIndexes(data, "itemcollectdist=");
                    if (indexes[0] >= 0)
                        thisData.collectDist = ParseFloat(indexes, data);
                }

                data = GetData(allLines, "shader=");
                indexes = FindIndexes(data, "shader=");
                thisData.shader = gameMaster.shaders[GetInputSubstring(indexes, data)];

                data = GetData(allLines, "canbeautocollectedattop=");
                indexes = FindIndexes(data, "canbeautocollectedattop=");
                thisData.canBePOC = ParseBool(indexes, data);

                data = GetData(allLines, "autodraw=");
                indexes = FindIndexes(data, "autodraw=");
                thisData.autoDraw = ParseBool(indexes, data);

                data = GetData(allLines, "animstates=");
                indexes = FindIndexes(data, "animstates=");
                tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.animations = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animations[i] = gameMaster.itemAnimStates[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this item data!", e.Message);
            }
            thisData.magnetDistSqr = thisData.magnetDist * thisData.magnetDist;
            thisData.collectDistSqr = thisData.collectDist * thisData.collectDist;
            return thisData;
        }


        public KeyValuePair<string, TexturedModel>[] ReadModelData(string file, Dictionary<string, Mesh> meshDic, Dictionary<string, Texture> texDic)
        {
            List<KeyValuePair<string, TexturedModel>> modelLinks = new List<KeyValuePair<string, TexturedModel>>();
            TexturedModel texturedModel;
            int[] indexes = { 0, 0 };
            int lineOffset = 0;
            string[] allLines = File.ReadAllLines(file);
            string data, key;
            try
            {
                while (lineOffset < allLines.Length - 1)
                {
                    texturedModel = new TexturedModel();

                    data = GetData(allLines, "name=", ref lineOffset);
                    indexes = FindIndexes(data, "name=");
                    key = data.Substring(indexes[0], indexes[1] - indexes[0]);

                    data = GetData(allLines, "shader=", ref lineOffset);
                    indexes = FindIndexes(data, "shader=");
                    texturedModel.shader = gameMaster.shaders[GetInputSubstring(indexes, data)];

                    data = GetData(allLines, "scale=", ref lineOffset);
                    indexes = FindIndexes(data, "scale=");
                    texturedModel.scale = ParseFloat(indexes, data);

                    data = GetData(allLines, "model=", ref lineOffset);
                    indexes = FindIndexes(data, "model=");
                    texturedModel.modelMesh = meshDic[data.Substring(indexes[0], indexes[1] - indexes[0])];

                    data = GetData(allLines, "tex=", ref lineOffset);
                    indexes = FindIndexes(data, "tex=");
                    texturedModel.modelTex = texDic[data.Substring(indexes[0], indexes[1] - indexes[0])];

                    modelLinks.Add(new KeyValuePair<string, TexturedModel>(key, texturedModel));
                }
            }
            catch(Exception e)
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


                        /*for (t = 3; t < tempStrings.Length; t++)
                        {
                            subTempStrings = tempStrings[t - 3].Split("/", StringSplitOptions.RemoveEmptyEntries);
                            if (subTempStrings.Length >= 1)
                                triangleData.Add((uint)ParseRoundedNum(subTempStrings[0]));
                            if (subTempStrings.Length >= 2)
                                texIndicesData.Add((uint)ParseRoundedNum(subTempStrings[1]));

                            subTempStrings = tempStrings[t - 1].Split("/", StringSplitOptions.RemoveEmptyEntries);
                            if (subTempStrings.Length >= 1)
                                triangleData.Add((uint)ParseRoundedNum(subTempStrings[0]));
                            if (subTempStrings.Length >= 2)
                                texIndicesData.Add((uint)ParseRoundedNum(subTempStrings[1]));

                            subTempStrings = tempStrings[t].Split("/", StringSplitOptions.RemoveEmptyEntries);
                            if (subTempStrings.Length >= 1)
                                triangleData.Add((uint)ParseRoundedNum(subTempStrings[0]));
                            if (subTempStrings.Length >= 2)
                                texIndicesData.Add((uint)ParseRoundedNum(subTempStrings[1]));
                        }*/
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
                //for (i = 0; i < vertexData.Count / 3; i++)
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
                    //for (t = 0; t < vertexIndexes.Count; t++)
                    {
                        finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]]].Y);
                        finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]]].X);
                        finalVertex.Add(vertexData[i].Z);
                        finalVertex.Add(vertexData[i].Y);
                        finalVertex.Add(vertexData[i].X);

                        //finalVertex.Add(vertexData[i * 3]);
                        //finalVertex.Add(vertexData[i * 3 + 1]);
                        //finalVertex.Add(vertexData[i * 3 + 2]);
                        //finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]] * 2]);
                        //finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]] * 2 + 1]);

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

                /*for (i = 0; i < finalTriangleData.Count; i+=3)
                    Console.WriteLine("Prev tri {0,4}: {1,3},{2,3}{3,3} \t uv: {4,3} \t new tri {0,4}: {5,3},{6,3},{7,3}", 
                        i, prevFinalTriData[i], prevFinalTriData[i+1], prevFinalTriData[i+2], finalTexIndices[i],
                        finalTriangleData[i], finalTriangleData[i+1], finalTriangleData[i+2]);*/

                /*for (i = 0; i < finalVertex.Count; i+=5)
                    Console.WriteLine("{0,10}, {1,10}, {2,10} {3,10}, {4,10}", finalVertex[i], finalVertex[i + 1], finalVertex[i + 2],
                        finalVertex[i + 3], finalVertex[i + 4]);*/


                /*for (i = 0; i < finalTriangleData.Count; i++)
                    Console.WriteLine("Vertex {5,3}:\n\t{0,10}, {1,10}, {2,10} {3,10}, {4,10}",
                        finalVertex[(int)finalTriangleData[i]*5], finalVertex[(int)finalTriangleData[i] * 5 + 1], finalVertex[(int)finalTriangleData[i] * 5 + 2],
                        finalVertex[(int)finalTriangleData[i] * 5 + 3], finalVertex[(int)finalTriangleData[i] * 5 + 4], finalTriangleData[i]);*/

                finalMesh.vertices = finalVertex.ToArray();
                finalMesh.triangleData = finalTriangleData.ToArray();
            }
            catch(Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this obj file!", e.Message);
            }

            return finalMesh;
        }
        public Mesh ReadObjDataOld(string file)
        {
            Mesh finalMesh = new Mesh();

            List<float> vertexData = new List<float>();
            List<float> texCoordData = new List<float>();
            List<uint[]> triangleData = new List<uint[]>();
            List<uint[]> texIndicesData = new List<uint[]>();

            List<float> finalVertex = new List<float>();
            List<uint> finalTriangleData = new List<uint>();
            List<uint> finalTexIndices = new List<uint>();

            string[] tempStrings, subTempStrings, allLines = File.ReadAllLines(file);
            int i, t, h, f;

            for (i = 0; i < allLines.Length; i++)
            {
                if(allLines[i].StartsWith("v "))
                {
                    allLines[i] = allLines[i].Remove(0, 2);
                    tempStrings = allLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    for (t = 0; t < tempStrings.Length; t++)
                    {
                        vertexData.Add(ParseFloat(tempStrings[t]));
                    }
                }
                else if (allLines[i].StartsWith("vt "))
                {
                    allLines[i] = allLines[i].Remove(0, 3);
                    tempStrings = allLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    for (t = 0; t < tempStrings.Length - 1; t++)
                    {
                        texCoordData.Add(ParseFloat(tempStrings[t]));
                    }
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


                    /*for (t = 3; t < tempStrings.Length; t++)
                    {
                        subTempStrings = tempStrings[t - 3].Split("/", StringSplitOptions.RemoveEmptyEntries);
                        if (subTempStrings.Length >= 1)
                            triangleData.Add((uint)ParseRoundedNum(subTempStrings[0]));
                        if (subTempStrings.Length >= 2)
                            texIndicesData.Add((uint)ParseRoundedNum(subTempStrings[1]));

                        subTempStrings = tempStrings[t - 1].Split("/", StringSplitOptions.RemoveEmptyEntries);
                        if (subTempStrings.Length >= 1)
                            triangleData.Add((uint)ParseRoundedNum(subTempStrings[0]));
                        if (subTempStrings.Length >= 2)
                            texIndicesData.Add((uint)ParseRoundedNum(subTempStrings[1]));

                        subTempStrings = tempStrings[t].Split("/", StringSplitOptions.RemoveEmptyEntries);
                        if (subTempStrings.Length >= 1)
                            triangleData.Add((uint)ParseRoundedNum(subTempStrings[0]));
                        if (subTempStrings.Length >= 2)
                            texIndicesData.Add((uint)ParseRoundedNum(subTempStrings[1]));
                    }*/
                }
            }

            for (i = 0; i < triangleData.Count; i++)
            {
                for (t = 0; t < triangleData[i].Length-2; t++)
                {
                    if(t % 2 == 0)
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
                        finalTriangleData.Add(triangleData[i][t-1]);
                        finalTriangleData.Add(triangleData[i][t+1]);
                        finalTriangleData.Add(triangleData[i][t+2]);
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

            for (i = vertexData.Count / 3 - 1; i >= 0; i--)
            //for (i = 0; i < vertexData.Count / 3; i++)
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
                //for (t = 0; t < vertexIndexes.Count; t++)
                {
                    finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]] * 2 + 1]);
                    finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]] * 2]);
                    finalVertex.Add(vertexData[i * 3 + 2]);
                    finalVertex.Add(vertexData[i * 3 + 1]);
                    finalVertex.Add(vertexData[i * 3]);

                    //finalVertex.Add(vertexData[i * 3]);
                    //finalVertex.Add(vertexData[i * 3 + 1]);
                    //finalVertex.Add(vertexData[i * 3 + 2]);
                    //finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]] * 2]);
                    //finalVertex.Add(texCoordData[(int)finalTexIndices[vertexIndexes[t]] * 2 + 1]);

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
            for (i = 0; i < finalTriangleData.Count; i+=3)
            {
                finalTriangleData.Reverse(i, 3);
            }

            /*for (i = 0; i < finalTriangleData.Count; i+=3)
                Console.WriteLine("Prev tri {0,4}: {1,3},{2,3}{3,3} \t uv: {4,3} \t new tri {0,4}: {5,3},{6,3},{7,3}", 
                    i, prevFinalTriData[i], prevFinalTriData[i+1], prevFinalTriData[i+2], finalTexIndices[i],
                    finalTriangleData[i], finalTriangleData[i+1], finalTriangleData[i+2]);*/

            /*for (i = 0; i < finalVertex.Count; i+=5)
                Console.WriteLine("{0,10}, {1,10}, {2,10} {3,10}, {4,10}", finalVertex[i], finalVertex[i + 1], finalVertex[i + 2],
                    finalVertex[i + 3], finalVertex[i + 4]);*/


            /*for (i = 0; i < finalTriangleData.Count; i++)
                Console.WriteLine("Vertex {5,3}:\n\t{0,10}, {1,10}, {2,10} {3,10}, {4,10}",
                    finalVertex[(int)finalTriangleData[i]*5], finalVertex[(int)finalTriangleData[i] * 5 + 1], finalVertex[(int)finalTriangleData[i] * 5 + 2],
                    finalVertex[(int)finalTriangleData[i] * 5 + 3], finalVertex[(int)finalTriangleData[i] * 5 + 4], finalTriangleData[i]);*/

            finalMesh.vertices = finalVertex.ToArray();
            finalMesh.triangleData = finalTriangleData.ToArray();

            return finalMesh;
        }

        public BackgroundSection ReadBackgroundSectionData(string file)
        {
            BackgroundSection thisData = new BackgroundSection();
            string[] tempStrings, allLines = File.ReadAllLines(file);
            int[] indexes;
            int i, lineOffset = 0;
            string data;
            TexturedModel[] modelArray;

            try
            {
                data = GetData(allLines, "sectionlength=");
                indexes = FindIndexes(data, "sectionlength=");
                thisData.secLength = ParseFloat(indexes, data);

                data = GetData(allLines, "offset=");
                indexes = FindIndexes(data, "offset=");
                thisData.offset = ParseVector3(indexes, data);

                while(lineOffset < allLines.Length - 1)
                {
                    data = GetData(allLines, "object=", ref lineOffset, true, ":");
                    indexes = FindIndexes(data, "object=");
                    tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    modelArray = new TexturedModel[tempStrings.Length];
                    for (i = 0; i < tempStrings.Length; i++)
                        modelArray[i] = gameMaster.backgroundModels[tempStrings[i]];
                    thisData.models.Add(modelArray);

                    indexes = FindIndexes(data, "pos=");
                    if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                        thisData.modelPos.Add(ParseVector3(indexes, data));
                    else
                        thisData.modelPos.Add(Vector3.Zero);

                    indexes = FindIndexes(data, "rot=");
                    if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                        thisData.modelRot.Add(ParseVector3(indexes, data));
                    else
                        thisData.modelRot.Add(Vector3.Zero);

                    indexes = FindIndexes(data, "scale=");
                    if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                        thisData.modelScale.Add(ParseVector3(indexes, data));
                    else
                        thisData.modelScale.Add(Vector3.One);
                }
            }
            catch(Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this background segment data!", e.Message);
            }

            return thisData;
        }

        public void ReadStageData(string file, string[] bgmPaths)
        {
            StageData thisData = new StageData();
            string[] tempStrings, subTempStrings, allLines = File.ReadAllLines(file);
            int[] indexes;
            int i, f, lineOffset = 0;
            string data;
            StageData.SectionSpawns newSectionSpawn;
            StageData.CamVelocities newCamVel;
            StageData.EnemySpawn newEnemySpawn;

            try
            {
                data = GetData(allLines, "stagebgm=");
                data = GetInputSubstring(FindIndexes(data, "stagebgm="), data);
                for (i = 0; i < bgmPaths.Length; i++)
                    if (SimplifyText(bgmPaths[i]).Contains(data))
                        thisData.stageTrackFile = bgmPaths[i];
                data = GetData(allLines, "bossbgm=");
                data = GetInputSubstring(FindIndexes(data, "bossbgm="), data);
                for (i = 0; i < bgmPaths.Length; i++)
                    if (SimplifyText(bgmPaths[i]).Contains(data))
                        thisData.bossTrackFile = bgmPaths[i];

                data = GetData(allLines, "backgroundsegments=", 0, true, "enemyspawns=");
                data = data.Remove(data.Length - "enemyspawns=".Length, "enemyspawns=".Length);
                data = data.Remove(0, "backgroundsegments=".Length);
                tempStrings = data.Split(':', StringSplitOptions.RemoveEmptyEntries);
                for (i = 0; i < tempStrings.Length; i++)
                {
                    newSectionSpawn = new StageData.SectionSpawns();

                    indexes = FindIndexes(tempStrings[i], "spawndirection=");
                    newSectionSpawn.spawnDirection = ParseVector3(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "segmentcount=");
                    newSectionSpawn.segmentCount = (byte)ParseRoundedNum(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "segments=");
                    subTempStrings = GetInputSubstring(indexes, tempStrings[i]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    for (f = 0; f < subTempStrings.Length; f++)
                        newSectionSpawn.section.Add(gameMaster.backgroundSections[subTempStrings[f]]);
                    indexes = FindIndexes(tempStrings[i], "time=");
                    newSectionSpawn.time = (uint)ParseRoundedNum(indexes, tempStrings[i]);

                    thisData.secSpawns.Add(newSectionSpawn);
                }

                data = GetData(allLines, "backgroundcamvelocities=", 0, true, "backgroundsegments=");
                data = data.Remove(data.Length - "backgroundsegments=".Length, "backgroundsegments=".Length);
                data = data.Remove(0, "backgroundcamvelocities=".Length);
                tempStrings = data.Split(':', StringSplitOptions.RemoveEmptyEntries);
                for (i = 0; i < tempStrings.Length; i++)
                {
                    newCamVel = new StageData.CamVelocities();

                    indexes = FindIndexes(tempStrings[i], "vel=");
                    newCamVel.vel = ParseVector3(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "randminpos=");
                    newCamVel.randMinPos = ParseVector3(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "randmaxpos=");
                    newCamVel.randMaxPos = ParseVector3(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "randminrot=");
                    newCamVel.randMinRot = ParseVector3(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "randmaxrot=");
                    newCamVel.randMaxRot = ParseVector3(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "randomizationdelay=");
                    newCamVel.randDelay = (uint)ParseRoundedNum(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "starttime=");
                    newCamVel.time = (uint)ParseRoundedNum(indexes, tempStrings[i]);
                    indexes = FindIndexes(tempStrings[i], "transitiontime=");
                    newCamVel.transitionTime = ParseFloat(indexes, tempStrings[i]);

                    thisData.camVel.Add(newCamVel);
                }

                while (lineOffset < allLines.Length - 1)
                {
                    data = GetData(allLines, "enemy=", ref lineOffset, true, ":");

                    newEnemySpawn = new StageData.EnemySpawn();

                    if(thisData.enemySpawns.Count > 0 && !data.Contains("enemy="))
                        newEnemySpawn.enemy = thisData.enemySpawns[thisData.enemySpawns.Count - 1].enemy;
                    else
                    {
                        indexes = FindIndexes(data, "enemy=");
                        newEnemySpawn.enemy = gameMaster.enemyData[GetInputSubstring(indexes, data)];
                    }

                    if (thisData.enemySpawns.Count > 0 && !data.Contains("time="))
                        newEnemySpawn.time = thisData.enemySpawns[thisData.enemySpawns.Count - 1].time;
                    else
                    {
                        indexes = FindIndexes(data, "time=");
                        newEnemySpawn.time = (uint)ParseRoundedNum(indexes, data);
                    }

                    if (thisData.enemySpawns.Count > 0 && !data.Contains("pos="))
                        newEnemySpawn.pos = thisData.enemySpawns[thisData.enemySpawns.Count - 1].pos;
                    else
                    {
                        indexes = FindIndexes(data, "pos=");
                        newEnemySpawn.pos = new Vector3(ParseVector2(indexes, data));
                    }

                    if (thisData.enemySpawns.Count > 0 && !data.Contains("items="))
                        newEnemySpawn.itemSpawns = thisData.enemySpawns[thisData.enemySpawns.Count - 1].itemSpawns;
                    else
                    {
                        indexes = FindIndexes(data, "items=");
                        tempStrings = GetInputSubstring(indexes, data).Split(',', StringSplitOptions.RemoveEmptyEntries);
                        newEnemySpawn.itemSpawns = new ItemData[tempStrings.Length];
                        for (i = 0; i < tempStrings.Length; i++)
                            newEnemySpawn.itemSpawns[i] = gameMaster.itemData[tempStrings[i]];
                    }

                    thisData.enemySpawns.Add(newEnemySpawn);
                }

            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error loading this stage data!", e.Message);
            }
            stageData = thisData;
        }
    }
}

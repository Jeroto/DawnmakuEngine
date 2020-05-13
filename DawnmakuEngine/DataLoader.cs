using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;
using static DawnmakuEngine.DawnMath;
using OpenTK;
using System.Linq;
using OpenTK.Platform.Windows;

namespace DawnmakuEngine
{
    class DataLoader
    {
        GameMaster gameMaster;
        public string directory, generalDir, bulletDataDir, playerDataDir, generalTexDir, bulletTexDir, playerTexDir, playerOrbTexDir, enemyTexDir, UITexDir,
            generalAnimDir, bulletAnimDir, playerAnimDir, playerOrbAnimDir, enemyAnimDir, texAnimDir,
            playerPatternDir, playerOrbDir,
            stageDir, curStageDir, enemyDir, enemyPatternDir, enemyMovementDir;

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
                if(dirs[i].Name == "Data")
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
                        dirs = dirs[i].GetDirectories();
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
                }
            }
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
                }
            }
            dirs = new DirectoryInfo(playerDataDir).GetDirectories();
            for (i = 0; i < dirs.Length; i++)
            {
                switch(dirs[i].Name)
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
                    dirs = dirs[i].GetDirectories();
                    break;
                }
            }
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
            dirs = dirs[0].Parent.Parent.GetDirectories();
        }

        public string GetFileNameOnly(string path)
        {
            string[] filename = path.Split('\\', StringSplitOptions.RemoveEmptyEntries);
            return filename[filename.Length - 1].Split('.', StringSplitOptions.RemoveEmptyEntries)[0].ToLower().Replace(" ", "");
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
        public void LoadEnemies()
        {
            EnemyPatternLoader();
            EnemyMovementLoader();
            EnemyTextureLoader();
            EnemySpriteLoader();
            EnemyAnimLoader();
            EnemyDataLoader();
        }

        public void BulletTextureLoader()
        {
            string[] files = Directory.GetFiles(bulletTexDir, "*.png");

            Console.WriteLine("\nBullet Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedBulletTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
                Console.WriteLine(GetFileNameOnly(files[i]));
            }
        }

        public void BulletSpriteLoader()
        {
            string[] files = Directory.GetFiles(bulletTexDir, "*.dwnsprites");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            Console.WriteLine("\nBullet Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                spritePairs = ReadSpriteData(File.ReadAllText(files[i]), gameMaster.loadedBulletTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    gameMaster.bulletSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                    gameMaster.bulletTypes.Add(spritePairs[e].Key);
                    Console.WriteLine(spritePairs[e].Key);
                }
            }
        }
        public void BulletAnimLoader()
        {
            string[] files = Directory.GetFiles(bulletAnimDir, "*.dwnanim");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            Console.WriteLine("\nBullet Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                animPairs = ReadAnimData(File.ReadAllText(files[i]), gameMaster.bulletSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    gameMaster.loadedBulletAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                    Console.WriteLine(animPairs[p].Key);
                }
            }
        }

        public void BulletDataLoader()
        {
            string[] files = Directory.GetFiles(bulletDataDir, "*.dwnbullet");
            Console.WriteLine("\nBullet Data:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.bulletData.Add(GetFileNameOnly(files[i]), ReadBulletData(File.ReadAllText(files[i])));
                Console.WriteLine(GetFileNameOnly(files[i]));
            }
        }

        public void PlayerOrbTextureLoader()
        {
            string[] files = Directory.GetFiles(playerOrbTexDir, "*.png");

            Console.WriteLine("\nPlayer Orb Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPlayerOrbTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
                Console.WriteLine(GetFileNameOnly(files[i]));
            }
        }

        public void PlayerOrbSpriteLoader()
        {
            string[] files = Directory.GetFiles(playerOrbTexDir, "*.dwnsprites");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            Console.WriteLine("\nPlayer Orb Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                spritePairs = ReadSpriteData(File.ReadAllText(files[i]), gameMaster.loadedPlayerOrbTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    gameMaster.playerOrbSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                    Console.WriteLine(spritePairs[e].Key);
                }
            }
        }
        public void PlayerOrbAnimLoader()
        {
            string[] files = Directory.GetFiles(playerOrbAnimDir, "*.dwnanim");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            Console.WriteLine("\nPlayer Orb Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                animPairs = ReadAnimData(File.ReadAllText(files[i]), gameMaster.playerOrbSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    gameMaster.loadedPlayerOrbAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                    Console.WriteLine(animPairs[p].Key);
                }
            }
        }
        public void PlayerTextureLoader()
        {
            string[] files = Directory.GetFiles(playerTexDir, "*.png");

            Console.WriteLine("\nPlayer Textures:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPlayerTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
                Console.WriteLine(GetFileNameOnly(files[i]));
            }
        }


        public void PlayerOrbDataLoader()
        {
            string[] files = Directory.GetFiles(playerOrbDir, "*.dwnorb");

            Console.WriteLine("\nPlayer Orb Data:");
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPlayerOrbData.Add(GetFileNameOnly(files[i]), ReadOrbData(File.ReadAllText(files[i])));
                Console.WriteLine(GetFileNameOnly(files[i]));
            }
        }

        public void PlayerSpriteLoader()
        {
            string[] files = Directory.GetFiles(playerTexDir, "*.dwnsprites");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            Console.WriteLine("\nPlayer Sprites:");
            for (int i = 0; i < files.Length; i++)
            {
                spritePairs = ReadSpriteData(File.ReadAllText(files[i]), gameMaster.loadedPlayerTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    gameMaster.playerSprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                    Console.WriteLine(spritePairs[e].Key);
                }
            }
        }
        public void PlayerAnimLoader()
        {
            string[] files = Directory.GetFiles(playerAnimDir, "*.dwnanim");
            KeyValuePair<string, TextureAnimator.AnimationState>[] animPairs;
            int p;

            Console.WriteLine("\nPlayer Anims:");
            for (int i = 0; i < files.Length; i++)
            {
                animPairs = ReadAnimData(File.ReadAllText(files[i]), gameMaster.playerSprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    gameMaster.loadedPlayerAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                    Console.WriteLine(animPairs[p].Key);
                }
            }
        }
        public void PlayerShotLoader()
        {
            string[] files = Directory.GetFiles(playerDataDir, "*.dwnshot");

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPlayerShot.Add(GetFileNameOnly(files[i]), ReadShotData(File.ReadAllText(files[i])));
            }
        }
        public void PlayerTypeLoader()
        {
            string[] files = Directory.GetFiles(playerDataDir, "*.dwntype");

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPlayerTypes.Add(GetFileNameOnly(files[i]), ReadTypeData(File.ReadAllText(files[i])));
            }
        }
        public void PlayerCharLoader()
        {
            string[] files = Directory.GetFiles(playerDataDir, "*.dwnchar");

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPlayerChars.Add(GetFileNameOnly(files[i]), ReadPlayerCharData(File.ReadAllText(files[i])));
            }
        }
        public void PlayerPatternLoader()
        {
            string[] files = Directory.GetFiles(playerPatternDir, "*.dwnpattern");
            string text;
            for (int i = 0; i < files.Length; i++)
            {
                text = File.ReadAllText(files[i]).Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
                Console.WriteLine(GetFileNameOnly(files[i]));
                if (!File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.loadedPlayerPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(text, gameMaster.loadedPlayerPatterns));
            }
            for (int i = 0; i < files.Length; i++)
            {
                text = File.ReadAllText(files[i]).Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
                Console.WriteLine(GetFileNameOnly(files[i]));
                if (File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.loadedPlayerPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(text, gameMaster.loadedPlayerPatterns));
            }
        }

        public void EnemyPatternLoader()
        {
            string[] files = Directory.GetFiles(enemyPatternDir, "*.dwnpattern");
            string text;
            for (int i = 0; i < files.Length; i++)
            {
                text = File.ReadAllText(files[i]).Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower(); ;
                if (!File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.loadedEnemyPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(text, gameMaster.loadedEnemyPatterns));
            }
            for (int i = 0; i < files.Length; i++)
            {
                text = File.ReadAllText(files[i]).Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower(); ;
                if (File.ReadAllText(files[i]).Contains("patternbase=", StringComparison.OrdinalIgnoreCase))
                    gameMaster.loadedEnemyPatterns.Add(GetFileNameOnly(files[i]), ReadPatternData(text, gameMaster.loadedEnemyPatterns));
            }
        }
        public void EnemyMovementLoader()
        {
            string[] files = Directory.GetFiles(enemyMovementDir, "*.dwnbezier");

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.enemyMovementPaths.Add(GetFileNameOnly(files[i]), ReadBezierData(File.ReadAllText(files[i])));
            }
        }
        public void EnemyTextureLoader()
        {
            string[] files = Directory.GetFiles(enemyTexDir, "*.png");

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedEnemyTextures.Add(GetFileNameOnly(files[i]), new Texture(files[i], false));
            }
        }

        public void EnemyDataLoader()
        {
            string[] files = Directory.GetFiles(enemyDir, "*.dwnenemy");
            
            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedEnemyData.Add(GetFileNameOnly(files[i]), ReadEnemyData(File.ReadAllText(files[i]), GetFileNameOnly(files[i])));
            }
        }

        public void EnemySpriteLoader()
        {
            string[] files = Directory.GetFiles(enemyTexDir, "*.dwnsprites");
            KeyValuePair<string, SpriteSet>[] spritePairs;

            for (int i = 0; i < files.Length; i++)
            {
                spritePairs = ReadSpriteData(File.ReadAllText(files[i]), gameMaster.loadedEnemyTextures[GetFileNameOnly(files[i])]);
                for (int e = 0; e < spritePairs.Length; e++)
                {
                    gameMaster.enemySprites.Add(spritePairs[e].Key, spritePairs[e].Value);
                }
            }
        }

        public void EnemyAnimLoader()
        {
            string[] files = Directory.GetFiles(enemyAnimDir, "*.dwnanim");
            KeyValuePair<string,TextureAnimator.AnimationState>[] animPairs;
            int p;
            for (int i = 0; i < files.Length; i++)
            {
                animPairs = ReadAnimData(File.ReadAllText(files[i]), gameMaster.enemySprites);
                for (p = 0; p < animPairs.Length; p++)
                {
                    gameMaster.loadedEnemyAnimStates.Add(animPairs[p].Key, animPairs[p].Value);
                }
            }
        }

        public int[] FindIndexes(string text, string opener, int startIndex = 0, string closer = ";")
        {
            if (!text.Contains(opener))
                return new int[] { text.Length - 1, text.Length };
            int indexStart = text.IndexOf(opener, startIndex) + opener.Length;

            int indexEnd = text.IndexOf(closer, indexStart);
            if (indexEnd == -1)
                indexEnd = text.Length;
            return new int[] { indexStart, indexEnd  };
        }


        public BulletData ReadBulletData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            BulletData thisData = new BulletData();
            int i, s;
            int[] indexes = { 0, 0 };
            bool boolVal;
            float numVal;
            string[] tempStrings, stateStrings;
            TextureAnimator.AnimationState state;

            try
            {
                indexes = FindIndexes(data, "isanimated=");
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisData.isAnimated = boolVal;

                indexes = FindIndexes(data, "shouldspin=");
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisData.shouldSpin = boolVal;

                indexes = FindIndexes(data, "shouldturn=");
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisData.shouldTurn = boolVal;

                indexes = FindIndexes(data, "collidersize=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.colliderSize = numVal;

                indexes = FindIndexes(data, "collideroffset=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                float.TryParse(tempStrings[0], out numVal);
                thisData.colliderOffsetX = numVal;
                float.TryParse(tempStrings[1], out numVal);
                thisData.colliderOffsetY = numVal;

                indexes = FindIndexes(data, "spritecolorcount=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.spriteColors = (ushort)Round(numVal);

                indexes = FindIndexes(data, "randomizesprite=");
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisData.randomizeSprite = boolVal;

                if (thisData.isAnimated)
                {
                    indexes = FindIndexes(data, "animstates=");
                    stateStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(':', StringSplitOptions.RemoveEmptyEntries);
                    thisData.animStates = new TextureAnimator.AnimationState[stateStrings.Length][];
                    for (i = 0; i < stateStrings.Length; i++)
                    {
                        tempStrings = stateStrings[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
                        thisData.animStates[i] = new TextureAnimator.AnimationState[tempStrings.Length];
                        for (s = 0; s < tempStrings.Length; s++)
                        {
                            gameMaster.loadedBulletAnimStates.TryGetValue(tempStrings[s], out state);
                            thisData.animStates[i][s] = state;
                        }
                    }
                }
                else
                    thisData.animStates = new TextureAnimator.AnimationState[0][];
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this bezier!");
                Console.WriteLine(e.Message);
            }

            return thisData;
        }

        public Bezier ReadBezierData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            Bezier thisBezier = new Bezier();
            int[] indexes = {0,0};
            float numVal1, numVal2, numVal3, numVal4;
            ushort shortVal;
            bool boolVal;
            string[] tempStrings, tempSubstrings; 
            try
            {
                indexes = FindIndexes(data, "scale=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal1);
                thisBezier.scale = Round(numVal1);

                indexes = FindIndexes(data, "autosetpoints=", indexes[1]);
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisBezier.AutoSetPoints = boolVal;

                indexes = FindIndexes(data, "points=", indexes[1], "end");
                tempStrings = data.Substring(indexes[0], data.Length - indexes[0]).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisBezier.points = new List<Bezier.Point>();
                indexes[1] = indexes[0];
                for (int i = 0; i < tempStrings.Length; i++)
                {
                    indexes = FindIndexes(tempStrings[i], "pos=");
                    tempSubstrings = tempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    float.TryParse(tempSubstrings[0], out numVal1);
                    float.TryParse(tempSubstrings[1], out numVal2);

                    indexes = FindIndexes(tempStrings[i], "time=", indexes[1]);
                    float.TryParse(tempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal3);

                    indexes = FindIndexes(tempStrings[i], "waittime=", indexes[1]);
                    float.TryParse(tempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal4);
                    shortVal = (ushort)Round(numVal4);

                    thisBezier.points.Add(new Bezier.Point(new Vector2(numVal1, numVal2), numVal3, shortVal));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this bezier!");
                Console.WriteLine(e.Message);
            }

            return thisBezier;
        }

        public KeyValuePair<string,SpriteSet>[] ReadSpriteData(string data, Texture tex)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            int i = 0, s = 0;
            int[] indexes = {0,0};
            string[] spriteSets, sprites, spriteValues;
            string key = "";
            float coordinate;
            SpriteSet value = null;

            List<KeyValuePair<string, SpriteSet>> spritePairs = new List<KeyValuePair<string, SpriteSet>>();
            try
            {
                spriteSets = data.Split(';', StringSplitOptions.RemoveEmptyEntries);

                for (i = 0; i < spriteSets.Length; i++)
                {
                    spriteSets[i] += ";";
                    indexes = FindIndexes(spriteSets[i], "=");

                    key = spriteSets[i].Substring(0, indexes[0] - 1);
                    value = new SpriteSet();
                    sprites = spriteSets[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(":", StringSplitOptions.RemoveEmptyEntries);

                    for (s = 0; s < sprites.Length; s++)
                    {
                        value.sprites.Add(new SpriteSet.Sprite());
                        spriteValues = sprites[s].Split(",", StringSplitOptions.RemoveEmptyEntries);
                        float.TryParse(spriteValues[0], out coordinate);
                        value.sprites[s].left = coordinate / tex.Width;
                        float.TryParse(spriteValues[1], out coordinate);
                        value.sprites[s].top = 1 - (coordinate / tex.Height);
                        float.TryParse(spriteValues[2], out coordinate);
                        value.sprites[s].right = coordinate / tex.Width;
                        float.TryParse(spriteValues[3], out coordinate);
                        value.sprites[s].bottom = 1 - (coordinate / tex.Height);

                        value.sprites[s].tex = tex;
                    }

                    spritePairs.Add(new KeyValuePair<string, SpriteSet>(key, value));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this sprite data!");
                Console.WriteLine(e.Message);
            }

            return spritePairs.ToArray();
        }

        public KeyValuePair<string,TextureAnimator.AnimationState>[] ReadAnimData(string data, Dictionary<string, SpriteSet> sprites)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            int i = 0, s = 0;
            int[] indexes = { 0, 0 };
            string[] animStates, frameSets;
            float numVal;
            bool boolVal;
            string stringVal, key;
            SpriteSet set;
            TextureAnimator.AnimationState thisState;
            List<KeyValuePair<string, TextureAnimator.AnimationState>> animPairs = new List<KeyValuePair<string, TextureAnimator.AnimationState>>();

            try
            {
                animStates = data.Split("/", StringSplitOptions.RemoveEmptyEntries);
                for (s = 0; s < animStates.Length; s++)
                {
                    thisState = new TextureAnimator.AnimationState();
                    indexes = FindIndexes(animStates[s], "name=");
                    key = animStates[s].Substring(indexes[0], indexes[1] - indexes[0]);

                    indexes = FindIndexes(animStates[s], "autotransition=", indexes[1]);
                    float.TryParse(animStates[s].Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                    thisState.autoTransition = Round(numVal);

                    indexes = FindIndexes(animStates[s], "loop=", indexes[1]);
                    bool.TryParse(animStates[s].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                    thisState.loop = boolVal;

                    indexes = FindIndexes(animStates[s], "frames=", indexes[1], "end");
                    frameSets = animStates[s].Substring(indexes[0], indexes[1] - indexes[0]).Split(':', StringSplitOptions.RemoveEmptyEntries);
                    thisState.animFrames = new TextureAnimator.AnimationFrame[frameSets.Length];
                    for (i = 0; i < frameSets.Length; i++)
                    {
                        thisState.animFrames[i] = new TextureAnimator.AnimationFrame();
                        indexes = FindIndexes(frameSets[i], "duration=");
                        float.TryParse(frameSets[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisState.animFrames[i].frameDuration = numVal;

                        indexes = FindIndexes(frameSets[i], "spriteset=", indexes[1]);
                        stringVal = frameSets[i].Substring(indexes[0], indexes[1] - indexes[0]);

                        indexes = FindIndexes(frameSets[i], "sprite=", indexes[1]);
                        float.TryParse(frameSets[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        sprites.TryGetValue(stringVal, out set);
                        if (set != null)
                            thisState.animFrames[i].sprite = set.sprites[Round(numVal)];
                    }

                    animPairs.Add(new KeyValuePair<string, TextureAnimator.AnimationState>(key, thisState));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this sprite data!");
                Console.WriteLine(e.Message);
            }

            return animPairs.ToArray(); ;
        }

        public PlayerOrbData ReadOrbData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            PlayerOrbData thisData = new PlayerOrbData();
            int i = 0;
            int[] indexes = { 0, 0 };
            float numVal = 0, numVal2 = 0;
            bool boolVal;
            string[] tempStrings, subTempStrings;

            try
            {
                indexes = FindIndexes(data, "activepowerlevels=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(",");
                thisData.activePowerLevels = new bool[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    bool.TryParse(tempStrings[i], out boolVal);
                    thisData.activePowerLevels[i] = boolVal;
                }

                indexes = FindIndexes(data, "unfocuspos=");
                subTempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                float.TryParse(subTempStrings[0], out numVal);
                float.TryParse(subTempStrings[1], out numVal2);
                thisData.unfocusPosition = new Vector2(numVal, numVal2);

                indexes = FindIndexes(data, "focuspos=", indexes[1]);
                subTempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                float.TryParse(subTempStrings[0], out numVal);
                float.TryParse(subTempStrings[1], out numVal2);
                thisData.focusPosition = new Vector2(numVal, numVal2);

                indexes = FindIndexes(data, "framestomove=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.framesToMove = numVal;

                indexes = FindIndexes(data, "rotatedegreespersecond=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.rotateDegreesPerSecond = numVal;

                indexes = FindIndexes(data, "animstates=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(",");
                thisData.animStates = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animStates[i] = gameMaster.loadedPlayerOrbAnimStates[tempStrings[i]];
                }

                indexes = FindIndexes(data, "startanimframe=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.startAnimFrame = Round(numVal);

                indexes = FindIndexes(data, "leavebehind=");
                switch(data.Substring(indexes[0], indexes[1] - indexes[0]))
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

                indexes = FindIndexes(data, "followprevious=");
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisData.followPrevious = boolVal;

                if(thisData.followPrevious)
                {
                    indexes = FindIndexes(data, "followdist=");
                    float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                    thisData.followDist = numVal;
                    thisData.followDistSq = numVal * numVal;
                }


                indexes = FindIndexes(data, "unfocuspatternsbypower=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(":");
                thisData.unfocusedPatterns = new Pattern[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.unfocusedPatterns[i] = gameMaster.loadedPlayerPatterns[tempStrings[i]];
                }

                indexes = FindIndexes(data, "focusedpatternsbypower=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(":");
                thisData.focusedPatterns = new Pattern[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.focusedPatterns[i] = gameMaster.loadedPlayerPatterns[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this player orb data!");
                Console.WriteLine(e.Message);
            }

            return thisData;
        }
        public PlayerShotData ReadShotData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            PlayerShotData thisData = new PlayerShotData();
            int i = 0;
            int[] indexes = { 0, 0 };
            float numVal = 0, numVal2 = 0;
            string[] tempStrings, subTempStrings;

            try
            {
                indexes = FindIndexes(data, "bombname=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Replace('_', ' ').Split(",");
                thisData.bombName = new string[tempStrings.Length]; 
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.bombName[i] = tempStrings[i];
                }

                indexes = FindIndexes(data, "movespeed=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.moveSpeed = numVal;

                indexes = FindIndexes(data, "focusspeedpercent=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.focusModifier = numVal;

                indexes = FindIndexes(data, "collidersize=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.colliderSize = numVal;

                indexes = FindIndexes(data, "collideroffset=");
                subTempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                float.TryParse(subTempStrings[0], out numVal);
                float.TryParse(subTempStrings[1], out numVal2);
                thisData.colliderOffset = new Vector2(numVal, numVal2);


                indexes = FindIndexes(data, "orbs=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(",");
                thisData.orbData = new PlayerOrbData[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.orbData[i] = gameMaster.loadedPlayerOrbData[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this player shot data!");
                Console.WriteLine(e.Message);
            }

            return thisData;
        }

        public PlayerTypeData ReadTypeData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            PlayerTypeData thisData = new PlayerTypeData();
            int i = 0;
            int[] indexes = { 0, 0 };
            float numVal = 0, numVal2 = 0;
            string[] tempStrings, subTempStrings;

            try
            {
                indexes = FindIndexes(data, "name=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Replace('_', ' ').Split(",");
                thisData.name = new string[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.name[i] = tempStrings[i];
                }

                indexes = FindIndexes(data, "desc=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Replace('_', ' ').Split(",");
                thisData.desc = new string[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.desc[i] = tempStrings[i];
                }

                indexes = FindIndexes(data, "shotdata=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(",");
                thisData.shotData = new PlayerShotData[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.shotData[i] = gameMaster.loadedPlayerShot[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this player type data!");
                Console.WriteLine(e.Message);
            }

            return thisData;
        }
        public PlayerCharData ReadPlayerCharData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            PlayerCharData thisData = new PlayerCharData();
            int i = 0;
            int[] indexes = { 0, 0 };
            float numVal = 0, numVal2 = 0;
            string[] tempStrings, subTempStrings;

            try
            {
                indexes = FindIndexes(data, "name=");
                thisData.name = data.Substring(indexes[0], indexes[1] - indexes[0]).Replace('_', ' ');
                indexes = FindIndexes(data, "jpname=");
                thisData.jpName = data.Substring(indexes[0], indexes[1] - indexes[0]).Replace('_', ' ');

                indexes = FindIndexes(data, "movespeed=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.moveSpeed = numVal;

                indexes = FindIndexes(data, "focusspeedpercent=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.focusModifier = numVal;

                indexes = FindIndexes(data, "collidersize=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.colliderSize = numVal;

                indexes = FindIndexes(data, "collideroffset=");
                subTempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                float.TryParse(subTempStrings[0], out numVal);
                float.TryParse(subTempStrings[1], out numVal2);
                thisData.colliderOffset = new Vector2(numVal, numVal2);

                indexes = FindIndexes(data, "shottypes=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(",");
                thisData.types = new PlayerTypeData[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.types[i] = gameMaster.loadedPlayerTypes[tempStrings[i]];
                }

                indexes = FindIndexes(data, "animstates=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(",");
                thisData.animStates = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animStates[i] = gameMaster.loadedPlayerAnimStates[tempStrings[i]];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this player type data!");
                Console.WriteLine(e.Message);
            }

            return thisData;
        }

        public EnemyData ReadEnemyData(string data, string filename)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            EnemyData thisData = new EnemyData();
            int i = 0;
            int[] indexes;
            float numVal = 0, numVal2 = 0;
            string[] tempStrings, subTempStrings;

            try
            {
                thisData.enemyName = filename;

                indexes = FindIndexes(data, "health=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.health = Round(numVal);

                indexes = FindIndexes(data, "iframes=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.invTime = Round(numVal);

                indexes = FindIndexes(data, "deathscore=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.deathScoreValue = Round(numVal);

                indexes = FindIndexes(data, "collidersize=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisData.colliderSize = numVal;

                indexes = FindIndexes(data, "collideroffset=");
                subTempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                float.TryParse(subTempStrings[0], out numVal);
                float.TryParse(subTempStrings[1], out numVal2);
                thisData.colliderOffset = new Vector2(numVal, numVal2);

                indexes = FindIndexes(data, "movementcurve=");
                thisData.movementCurve = gameMaster.enemyMovementPaths[data.Substring(indexes[0], indexes[1] - indexes[0])];

                indexes = FindIndexes(data, "animations=");
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisData.animations = new TextureAnimator.AnimationState[tempStrings.Length];
                for (i = 0; i < tempStrings.Length; i++)
                {
                    thisData.animations[i] = gameMaster.loadedEnemyAnimStates[tempStrings[i]];
                }

                indexes = FindIndexes(data, "patternsbydifficulty=", 0, "end");
                tempStrings = data.Substring(indexes[0], data.Length - indexes[0]).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisData.patternsByDifficulty = new EnemyData.Difficulty[tempStrings.Length];

                for (int p = 0; p < tempStrings.Length; p++)
                {
                    thisData.patternsByDifficulty[p] = new EnemyData.Difficulty();

                    subTempStrings = tempStrings[p].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    thisData.patternsByDifficulty[p].patterns = new Pattern[subTempStrings.Length];
                    for (i = 0; i < subTempStrings.Length; i++)
                    {
                        thisData.patternsByDifficulty[p].patterns[i] = gameMaster.loadedEnemyPatterns[subTempStrings[i]];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this enemy data!");
                Console.WriteLine(e.Message);
            }

            return thisData;
        }

        public Pattern ReadPatternData(string data, Dictionary<string, Pattern> patternDic)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("{", "").Replace("}", "").ToLower();
            Pattern finalPattern = new Pattern();
            int i = 0;
            int[] indexes;
            float numVal = 0;
            bool boolVal = false;
            int patternType = 0;
            string[] tempStrings, subTempStrings;
            string patternBase = null;
            try
            {
                if (data.Contains("patternbase="))
                {
                    indexes = FindIndexes(data, "patternbase=");
                    patternBase = data.Substring(indexes[0], indexes[1] - indexes[0]);
                    finalPattern = patternDic[patternBase].CopyPattern();
                }

                indexes = FindIndexes(data, "patterntype=");
                switch (data.Substring(indexes[0], indexes[1] - indexes[0]))
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
                        Console.WriteLine("Pattern type " + data.Substring(indexes[0], indexes[1] - indexes[0]) + "not recognized");
                        break;
                }

                indexes = FindIndexes(data, "burstcount=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.burstCount = Round(numVal);

                indexes = FindIndexes(data, "bulletsinburst=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.bulletsInBurst = Round(numVal);

                indexes = FindIndexes(data, "overalldegrees=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.overallDegrees = numVal;

                indexes = FindIndexes(data, "degreeoffset=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.degreeOffset = numVal;

                indexes = FindIndexes(data, "randomdegreeoffset=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.randomDegreeOffset = numVal;

                indexes = FindIndexes(data, "aimed=");
                if(bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                    finalPattern.aimed = boolVal;

                indexes = FindIndexes(data, "burstdelay=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.burstDelay = numVal;

                indexes = FindIndexes(data, "initialdelay=");
                if(float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                    finalPattern.initialDelay = numVal;

                indexes = FindIndexes(data, "perbulletdelay=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.perBulletDelay = new List<float>();
                    for (i = 0; i < tempStrings.Length; i++)
                    {
                        float.TryParse(tempStrings[i], out numVal);
                        finalPattern.perBulletDelay.Add(numVal);
                    }
                }

                indexes = FindIndexes(data, "damage=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.damage = new List<ushort>();
                    for (i = 0; i < tempStrings.Length; i++)
                    {
                        float.TryParse(tempStrings[i], out numVal);
                        finalPattern.damage.Add((ushort)Math.Clamp(Round(numVal), 0, ushort.MaxValue));
                    }
                }

                indexes = FindIndexes(data, "offsets=");
                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    float xval;
                    finalPattern.offsets = new List<Vector2>();
                    for (i = 0; i < tempStrings.Length; i += 2)
                    {
                        float.TryParse(tempStrings[i], out xval);
                        float.TryParse(tempStrings[i + 1], out numVal);
                        finalPattern.offsets.Add(new Vector2(xval, numVal));
                    }
                }

                indexes = FindIndexes(data, "turnoffsets=");

                if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                {
                    tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    finalPattern.turnOffsets = new List<bool>();
                    for (i = 0; i < tempStrings.Length; i++)
                    {
                        bool.TryParse(tempStrings[i], out boolVal);
                        finalPattern.turnOffsets.Add(boolVal);
                    }
                }

                switch(patternType)
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
                            for (i = 0; i < finalPattern.damage.Count; i++)
                                thisPattern.damage.Add(finalPattern.damage[i]);
                            for (i = 0; i < finalPattern.offsets.Count; i++)
                                thisPattern.offsets.Add(new Vector2(finalPattern.offsets[i].X, finalPattern.offsets[i].Y));
                            for (i = 0; i < finalPattern.turnOffsets.Count; i++)
                                thisPattern.turnOffsets.Add(finalPattern.turnOffsets[i]);


                            indexes = FindIndexes(data, "bulletstages=", 0, "end");
                            if (indexes[0] != data.Length - 1 || indexes[1] != data.Length)
                            {
                                tempStrings = data.Substring(indexes[0], data.Length - indexes[0]).Split(':', StringSplitOptions.RemoveEmptyEntries);
                                if(patternBase == null)
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
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.framesToLast = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "bullettype=");
                                        if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                            thisInstance.spriteType = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]);

                                        indexes = FindIndexes(subTempStrings[i], "bulletcolor=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.bulletColor = Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletColor)).Length - 1);
                                        else if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                            thisInstance.bulletColor = (int)Enum.Parse<BulletElement.BulletColor>(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), true);

                                        indexes = FindIndexes(subTempStrings[i], "animatedsprite=");
                                        if (bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.animatedSprite = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "renderscale=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.renderScale = numVal;

                                        {
                                            float xVal;
                                            indexes = FindIndexes(subTempStrings[i], "movementdirection=");
                                            if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                            {
                                                subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                                float.TryParse(subSubstrings[0], out xVal);
                                                float.TryParse(subSubstrings[1], out numVal);
                                                thisInstance.movementDirection = new Vector2(xVal, numVal);
                                            }
                                        }

                                        indexes = FindIndexes(subTempStrings[i], "startingspeed=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.startingSpeed = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "endingspeed=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.endingSpeed = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "framestochangespeed=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.framesToChangeSpeed = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "colorr=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.r = (byte)Math.Clamp(numVal, 0, 255);
                                        indexes = FindIndexes(subTempStrings[i], "colorg=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.g = (byte)Math.Clamp(numVal, 0, 255);
                                        indexes = FindIndexes(subTempStrings[i], "colorb=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.b = (byte)Math.Clamp(numVal, 0, 255);
                                        indexes = FindIndexes(subTempStrings[i], "colora=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.a = (byte)Math.Clamp(numVal, 0, 255);

                                        indexes = FindIndexes(subTempStrings[i], "colortint=");
                                        if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                        {
                                            subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                            float.TryParse(subSubstrings[0], out numVal);
                                            thisInstance.r = (byte)Math.Clamp(numVal, 0, 255);
                                            float.TryParse(subSubstrings[1], out numVal);
                                            thisInstance.g = (byte)Math.Clamp(numVal, 0, 255);
                                            float.TryParse(subSubstrings[2], out numVal);
                                            thisInstance.b = (byte)Math.Clamp(numVal, 0, 255);
                                            float.TryParse(subSubstrings[3], out numVal);
                                            thisInstance.a = (byte)Math.Clamp(numVal, 0, 255);
                                        }


                                        indexes = FindIndexes(subTempStrings[i], "framestochangetint=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.framesToChangeTint = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "rotate=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.rotate = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "reaim=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.reAim = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "keepoldangle=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.keepOldAngle = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "modifyangle=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.modifyAngle = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "turnatstart=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.turnAtStart = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "initialmovedelay=");
                                        if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                            thisInstance.initialMoveDelay = numVal;

                                        indexes = FindIndexes(subTempStrings[i], "turnafterdelay=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.turnAfterDelay = boolVal;

                                        indexes = FindIndexes(subTempStrings[i], "haseffect=");
                                        if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
                                            thisInstance.hasEffect = boolVal;

                                        if (thisInstance.hasEffect)
                                        {

                                            indexes = FindIndexes(subTempStrings[i], "effectcolor=");
                                            if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                                thisInstance.effectColor = (Elements.BulletElement.BulletColor)Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletColor)).Length - 1);

                                            indexes = FindIndexes(subTempStrings[i], "effectduration=");
                                            if (float.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out numVal))
                                                thisInstance.effectDuration = numVal;

                                            {
                                                float xVal;
                                                indexes = FindIndexes(subTempStrings[i], "effectsize=");
                                                if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                                {
                                                    subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                                    float.TryParse(subSubstrings[0], out xVal);
                                                    float.TryParse(subSubstrings[1], out numVal);
                                                    thisInstance.effectSize = new Vector2(xVal, numVal);
                                                }

                                                indexes = FindIndexes(subTempStrings[i], "effectopacity=");
                                                if (indexes[0] != subTempStrings[i].Length - 1 || indexes[1] != subTempStrings[i].Length)
                                                {
                                                    subSubstrings = subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                                    float.TryParse(subSubstrings[0], out xVal);
                                                    float.TryParse(subSubstrings[1], out numVal);
                                                    thisInstance.effectOpacity = new Vector2(xVal, numVal);
                                                }
                                            }

                                            indexes = FindIndexes(subTempStrings[i], "affectedbytimescale=");
                                            if (bool.TryParse(subTempStrings[i].Substring(indexes[0], indexes[1] - indexes[0]), out boolVal))
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
                Console.WriteLine("There was an error loading this pattern data!");
                Console.WriteLine(e.Message);
            }

            return finalPattern;
        }
    }
}

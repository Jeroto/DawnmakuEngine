using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;
using static DawnmakuEngine.DawnMath;
using OpenTK;

namespace DawnmakuEngine
{
    class DataLoader
    {
        GameMaster gameMaster;
        public string directory, stageDirectory, enemyDir, enemyPatternDir, enemyMovementDir;

        public DataLoader(string stageName)
        {
            gameMaster = GameMaster.gameMaster;
            directory = new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).AbsolutePath;
            Console.WriteLine(directory);
            /*while(directory[directory.Length - 1] != '/')
            {
                directory = directory.Remove(directory.Length - 1, 1);
            }*/
            DirectoryInfo dir = Directory.GetParent(directory);
            directory = dir.FullName;
            Console.WriteLine(directory);
            DirectoryInfo[] dirs = dir.GetDirectories();
            int i = 0;
            for (i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == "Stages")
                {
                    dirs = dirs[i].GetDirectories();
                    break;
                }
            }
            for (i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == stageName)
                {
                    stageDirectory = dirs[i].FullName;
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
                if (dirs[i].Name == "Patterns")
                {
                    enemyPatternDir = dirs[i].FullName;
                }
                else if (dirs[i].Name == "Movement")
                {
                    enemyMovementDir = dirs[i].FullName;
                }
            }
            dirs = dirs[0].Parent.Parent.GetDirectories();
        }

        public void EnemyPatternLoader()
        {
            string[] files = Directory.GetFiles(enemyPatternDir, "*.dwnepattern");

            gameMaster.loadedPatterns = new EnemyPattern[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPatterns[i] = ReadPatternData(File.ReadAllText(files[i]));
            }
        }

        public void EnemyLoader()
        {
            string[] files = Directory.GetFiles(enemyDir, "*.dwnenemy");
            gameMaster.loadedEnemyData = new EnemyData[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
        }
        public void EnemyMovementLoader()
        {
            string[] files = Directory.GetFiles(enemyMovementDir, "*.dwnbezier");
            gameMaster.enemyMovementPaths = new Bezier[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                gameMaster.loadedPatterns[i] = ReadPatternData(File.ReadAllText(files[i]));
            }
        }

        public int[] FindIndexes(string text, string opener, int startIndex = 0, string closer = ";")
        {
            int indexStart = text.IndexOf(opener, startIndex) + opener.Length;
            int indexEnd = text.IndexOf(closer, indexStart);
            return new int[] { indexStart, indexEnd  };
        }

        public Bezier ReadBezierData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("{", "").Replace("}", "");
            Bezier thisBezier = new Bezier();
            int[] indexes;
            float numVal1, numVal2, numVal3;
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
                for (int i = 0; i < tempStrings.Length; i++)
                {
                    indexes = FindIndexes(data, "pos=", indexes[1]);
                    tempSubstrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    float.TryParse(tempSubstrings[0], out numVal1);
                    float.TryParse(tempSubstrings[1], out numVal2);

                    indexes = FindIndexes(data, "time=", indexes[1]);
                    float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal3);

                    thisBezier.points.Add(new Bezier.Point(new Vector2(numVal1, numVal2), numVal3));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this bezier!");
                Console.WriteLine(e.Message);
            }

            return thisBezier;
        }

        public EnemyPattern ReadPatternData(string data)
        {
            data = data.Replace(" ", "").Replace("\n", "").Replace("{", "").Replace("}", "");
            EnemyPattern thisPattern = new EnemyPattern();
            int i = 0;
            int[] indexes;
            float numVal = 0;
            bool boolVal = false;
            string[] tempStrings, subTempStrings;
            try
            {
                indexes = FindIndexes(data, "burstcount=");
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.burstCount = Round(numVal);

                indexes = FindIndexes(data, "bulletsinburst=", indexes[1]);
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.bulletsInBurst = Round(numVal);

                indexes = FindIndexes(data, "overalldegrees=", indexes[1]);
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.overallDegrees = numVal;

                indexes = FindIndexes(data, "degreeoffset=", indexes[1]);
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.degreeOffset = numVal;

                indexes = FindIndexes(data, "randomdegreeoffset=", indexes[1]);
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.randomDegreeOffset = numVal;

                indexes = FindIndexes(data, "aimed=", indexes[1]);
                bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                thisPattern.aimed = boolVal;

                indexes = FindIndexes(data, "burstdelay=", indexes[1]);
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.burstDelay = numVal;

                indexes = FindIndexes(data, "initialdelay=", indexes[1]);
                float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                thisPattern.initialDelay = numVal;

                indexes = FindIndexes(data, "perbulletdelay=", indexes[1]);
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisPattern.perBulletDelay = new List<float>();
                for (i = 0; i < tempStrings.Length; i++)
                {
                    float.TryParse(tempStrings[i], out numVal);
                    thisPattern.perBulletDelay.Add(numVal);
                }

                indexes = FindIndexes(data, "custombullet=", indexes[1]);
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisPattern.customBullet = new List<int>();
                for (i = 0; i < tempStrings.Length; i++)
                {
                    float.TryParse(tempStrings[i], out numVal);
                    thisPattern.customBullet.Add(Round(numVal));
                }

                indexes = FindIndexes(data, "customhasscript=", indexes[1]);
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisPattern.customHasScript = new List<bool>();
                for (i = 0; i < tempStrings.Length; i++)
                {
                    bool.TryParse(tempStrings[i], out boolVal);
                    thisPattern.customHasScript.Add(boolVal);
                }

                indexes = FindIndexes(data, "damage=", indexes[1]);
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisPattern.damage = new List<ushort>();
                for (i = 0; i < tempStrings.Length; i++)
                {
                    float.TryParse(tempStrings[i], out numVal);
                    thisPattern.damage.Add((ushort)Math.Clamp(Round(numVal), 0, ushort.MaxValue));
                }

                {
                    indexes = FindIndexes(data, "offsets=", indexes[1]);
                    tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                    float xval;
                    thisPattern.offsets = new List<Vector2>();
                    for (i = 0; i < tempStrings.Length; i += 2)
                    {
                        float.TryParse(tempStrings[i], out xval);
                        float.TryParse(tempStrings[i + 1], out numVal);
                        thisPattern.offsets.Add(new Vector2(xval, numVal));
                    }
                }

                indexes = FindIndexes(data, "turnoffsets=", indexes[1]);
                tempStrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                thisPattern.turnOffsets = new List<bool>();
                for (i = 0; i < tempStrings.Length; i++)
                {
                    bool.TryParse(tempStrings[i], out boolVal);
                    thisPattern.turnOffsets.Add(boolVal);
                }


                indexes = FindIndexes(data, "bulletstages=", indexes[1], "end");
                tempStrings = data.Substring(indexes[0], data.Length - indexes[0]).Split(':', StringSplitOptions.RemoveEmptyEntries);
                thisPattern.bulletStages = new List<EnemyPattern.InstanceList>();
                Elements.BulletElement.BulletStage thisInstance;
                string[] subSubstrings;
                for (int b = 0; b < tempStrings.Length; b++)
                {
                    thisPattern.bulletStages.Add(new EnemyPattern.InstanceList());
                    subTempStrings = tempStrings[b].Split('/', StringSplitOptions.RemoveEmptyEntries);
                    thisPattern.bulletStages[b].instances = new List<Elements.BulletElement.BulletStage>();
                    for (i = 0; i < Math.Clamp(subTempStrings.Length, 1, int.MaxValue); i++)
                    {
                        thisPattern.bulletStages[b].instances.Add(new Elements.BulletElement.BulletStage());
                        thisInstance = thisPattern.bulletStages[b].instances[i];

                        indexes = FindIndexes(data, "framestolast=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.framesToLast = numVal;

                        indexes = FindIndexes(data, "spritetype=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.spriteType = (Elements.BulletElement.BulletType)Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletType)).Length - 1);

                        indexes = FindIndexes(data, "bulletcolor=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.bulletColor = (Elements.BulletElement.BulletColor)Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletColor)).Length - 1);

                        indexes = FindIndexes(data, "animatedsprite=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.animatedSprite = boolVal;

                        indexes = FindIndexes(data, "customsprite=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.customSprite = gameMaster.loadedBulletTextures[Math.Clamp(Round(numVal), 0, gameMaster.loadedBulletTextures.Length - 1)];

                        indexes = FindIndexes(data, "customanim=", indexes[1]);
                        subSubstrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                        thisInstance.customAnim = new Elements.TextureAnimator.AnimationState[subSubstrings.Length];
                        for (int a = 0; a < subSubstrings.Length; a++)
                        {
                            float.TryParse(subSubstrings[a], out numVal);
                            thisInstance.customAnim[a] = gameMaster.loadedBulletAnimStates[Math.Clamp(Round(numVal), 0, gameMaster.loadedBulletAnimStates.Length - 1)];
                        }

                        indexes = FindIndexes(data, "renderscale=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.renderScale = numVal;

                        {
                            float xVal;
                            indexes = FindIndexes(data, "movementdirection=", indexes[1]);
                            subSubstrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                            float.TryParse(subSubstrings[0], out xVal);
                            float.TryParse(subSubstrings[1], out numVal);
                            thisInstance.movementDirection = new Vector2(xVal, numVal);
                        }

                        indexes = FindIndexes(data, "startingspeed=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.startingSpeed = numVal;

                        indexes = FindIndexes(data, "endingspeed=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.endingSpeed = numVal;

                        indexes = FindIndexes(data, "framestochangespeed=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.framesToChangeSpeed = numVal;

                        indexes = FindIndexes(data, "rotate=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.rotate = boolVal;

                        indexes = FindIndexes(data, "aimatplayer=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.aimAtPlayer= boolVal;

                        indexes = FindIndexes(data, "keepoldangle=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.keepOldAngle = boolVal;

                        indexes = FindIndexes(data, "modifyangle=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.modifyAngle = boolVal;

                        indexes = FindIndexes(data, "turnatstart=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.turnAtStart = boolVal;

                        indexes = FindIndexes(data, "initialmovedelay=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.initialMoveDelay = numVal;

                        indexes = FindIndexes(data, "turnafterdelay=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.turnAfterDelay = boolVal;

                        indexes = FindIndexes(data, "haseffect=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.hasEffect = boolVal;

                        indexes = FindIndexes(data, "effectcolor=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.effectColor = (Elements.BulletElement.BulletColor)Math.Clamp(Round(numVal), 0, Enum.GetValues(typeof(Elements.BulletElement.BulletColor)).Length - 1);

                        indexes = FindIndexes(data, "effectduration=", indexes[1]);
                        float.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out numVal);
                        thisInstance.effectDuration = numVal;

                        {
                            float xVal;
                            indexes = FindIndexes(data, "effectsize=", indexes[1]);
                            subSubstrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                            float.TryParse(subSubstrings[0], out xVal);
                            float.TryParse(subSubstrings[1], out numVal);
                            thisInstance.effectSize = new Vector2(xVal, numVal);

                            indexes = FindIndexes(data, "effectopacity=", indexes[1]);
                            subSubstrings = data.Substring(indexes[0], indexes[1] - indexes[0]).Split(',', StringSplitOptions.RemoveEmptyEntries);
                            float.TryParse(subSubstrings[0], out xVal);
                            float.TryParse(subSubstrings[1], out numVal);
                            thisInstance.effectOpacity = new Vector2(xVal, numVal);
                        }

                        indexes = FindIndexes(data, "affectedbytimescale=", indexes[1]);
                        bool.TryParse(data.Substring(indexes[0], indexes[1] - indexes[0]), out boolVal);
                        thisInstance.affectedByTimescale = boolVal;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error loading this pattern data!");
                Console.WriteLine(e.Message);
            }

            return thisPattern;
        }
    }
}

using DawnmakuEngine.Data;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Elements
{
    class StageElement : Element
    {
        public StageData data;
        public float backgroundTime, enemyTime;
        GameMaster gameMaster = GameMaster.gameMaster;
        Entity backgroundContainer = new Entity("Background");
        public List<float> sectionLength = new List<float>();
        Entity backgroundCam;

        int sectionSpawnIndex, secIndividualIndex, camVelIndex, enemyIndex;
        float sectionTransition, camTransition,
            camRandomTime, camRandomCur;
        Vector3 curVel = Vector3.Zero, prevPos, newPos;
        Vector3 prevRot, newRot;

        public StageElement(StageData stageData) : base(true)
        {
            data = stageData;
        }

        public override void PostCreate()
        {
            float lengthOffset = 0;
            for (int i = 0; i < data.secSpawns[0].segmentCount; i++)
            {
                secIndividualIndex = DawnMath.Repeat(secIndividualIndex + 1, data.secSpawns[0].section.Count - 1);
                data.secSpawns[0].section[secIndividualIndex].SpawnSegmentLocal(backgroundContainer, data.secSpawns[0].spawnDirection * lengthOffset);
                lengthOffset += data.secSpawns[0].section[secIndividualIndex].secLength;
                sectionLength.Add(data.secSpawns[0].section[secIndividualIndex].secLength);
            }
            backgroundCam = Entity.FindEntity("BackgroundCamera");
            camRandomTime = data.camVel[0].randDelay;
            camRandomCur = 0;
            prevPos = new Vector3(GameMaster.Random(data.camVel[0].randMinPos.X, data.camVel[0].randMaxPos.X),
                GameMaster.Random(data.camVel[0].randMinPos.Y, data.camVel[0].randMaxPos.Y),
                GameMaster.Random(data.camVel[0].randMinPos.Z, data.camVel[0].randMaxPos.Z));
            newPos = new Vector3(GameMaster.Random(data.camVel[0].randMinPos.X, data.camVel[0].randMaxPos.X),
                GameMaster.Random(data.camVel[0].randMinPos.Y, data.camVel[0].randMaxPos.Y),
                GameMaster.Random(data.camVel[0].randMinPos.Z, data.camVel[0].randMaxPos.Z));

            prevRot = new Vector3(GameMaster.Random(data.camVel[0].randMinRot.X, data.camVel[0].randMaxRot.X),
                GameMaster.Random(data.camVel[0].randMinRot.Y, data.camVel[0].randMaxRot.Y),
                GameMaster.Random(data.camVel[0].randMinRot.Z, data.camVel[0].randMaxRot.Z));
            newRot = new Vector3(GameMaster.Random(data.camVel[0].randMinRot.X, data.camVel[0].randMaxRot.X),
                GameMaster.Random(data.camVel[0].randMinRot.Y, data.camVel[0].randMaxRot.Y),
                GameMaster.Random(data.camVel[0].randMinRot.Z, data.camVel[0].randMaxRot.Z));
            base.PostCreate();
        }

        public override void OnUpdate()
        {
            backgroundTime += gameMaster.timeScale;
            enemyTime += gameMaster.timeScale;

            BackgroundIndexes();
            BackgroundCam();
            Background();

            EnemySpawns();

            base.OnUpdate();
        }

        public void BackgroundIndexes()
        {
            if (data.secSpawns[Math.Clamp(sectionSpawnIndex + 1, 0, data.secSpawns.Count - 1)].time <= backgroundTime)
            {
                sectionSpawnIndex = Math.Clamp(sectionSpawnIndex + 1, 0, data.secSpawns.Count - 1);
                sectionTransition = 0;
                secIndividualIndex = DawnMath.Repeat(secIndividualIndex, data.secSpawns[sectionSpawnIndex].section.Count - 1);
            }
            if (data.camVel[Math.Clamp(camVelIndex + 1, 0, data.camVel.Count - 1)].time <= backgroundTime)
            {
                camVelIndex = Math.Clamp(camVelIndex + 1, 0, data.camVel.Count - 1);
                camTransition = 0;
            }
        }

        public void Background()
        {
            sectionTransition += gameMaster.timeScale;

            for (int i = 0; i < backgroundContainer.ChildCount; i++)
            {
                backgroundContainer.GetChild(i).LocalPosition += curVel * gameMaster.timeScale;
            }

            if (backgroundContainer.GetChild(0).LocalPosition.Z > backgroundCam.LocalPosition.Z + sectionLength[0] + 50)
            {
                backgroundContainer.GetChild(0).AttemptDelete();
                sectionLength.RemoveAt(0);
                data.secSpawns[sectionSpawnIndex].section[secIndividualIndex].SpawnSegmentLocal(backgroundContainer,
                     backgroundContainer.GetLastChild().LocalPosition + (data.secSpawns[sectionSpawnIndex].spawnDirection * 
                     (data.secSpawns[sectionSpawnIndex].section[secIndividualIndex].secLength / 2 + sectionLength[sectionLength.Count - 1] / 2)));
                sectionLength.Add(data.secSpawns[sectionSpawnIndex].section[secIndividualIndex].secLength);
                secIndividualIndex = DawnMath.Repeat(secIndividualIndex + 1, data.secSpawns[sectionSpawnIndex].section.Count - 1);
            }
        }

        public void BackgroundCam()
        {
            camTransition += gameMaster.timeScale;
            camRandomCur += gameMaster.timeScale;
            if (data.camVel[camVelIndex].transitionTime > 0)
                curVel = DawnMath.Lerp(curVel, data.camVel[camVelIndex].vel, camTransition / data.camVel[camVelIndex].transitionTime);
            else
                curVel = data.camVel[camVelIndex].vel;

            if(camRandomCur >= camRandomTime)
            {
                camRandomCur -= camRandomTime;
                camRandomTime = data.camVel[camVelIndex].randDelay;

                prevPos = newPos;
                prevRot = newRot;

                newPos = new Vector3(GameMaster.Random(data.camVel[camVelIndex].randMinPos.X, data.camVel[camVelIndex].randMaxPos.X),
                    GameMaster.Random(data.camVel[camVelIndex].randMinPos.Y, data.camVel[camVelIndex].randMaxPos.Y),
                    GameMaster.Random(data.camVel[camVelIndex].randMinPos.Z, data.camVel[camVelIndex].randMaxPos.Z));
                newRot = new Vector3(GameMaster.Random(data.camVel[camVelIndex].randMinRot.X, data.camVel[camVelIndex].randMaxRot.X),
                    GameMaster.Random(data.camVel[camVelIndex].randMinRot.Y, data.camVel[camVelIndex].randMaxRot.Y),
                    GameMaster.Random(data.camVel[camVelIndex].randMinRot.Z, data.camVel[camVelIndex].randMaxRot.Z));
            }
            else
            {
                backgroundCam.LocalPosition = DawnMath.EaseInOut(prevPos, newPos, camRandomCur / camRandomTime);
                Vector3 easedRot = DawnMath.EaseInOut(prevRot, newRot, camRandomCur / camRandomTime);
                backgroundCam.LocalRotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(easedRot.X),
                    MathHelper.DegreesToRadians(easedRot.Y), MathHelper.DegreesToRadians(easedRot.Z));
            }
        }

        public void EnemySpawns()
        {
            if(data.enemySpawns.Count > enemyIndex && data.enemySpawns[enemyIndex].time <= enemyTime)
            {
                EnemyElement.SpawnEnemy(data.enemySpawns[enemyIndex].enemy, data.enemySpawns[enemyIndex].pos, data.enemySpawns[enemyIndex].itemSpawns);
                enemyIndex++;
            }
        }
    }
}

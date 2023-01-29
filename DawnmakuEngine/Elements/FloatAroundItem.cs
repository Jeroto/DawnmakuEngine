using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Data;
using DawnmakuEngine.Data.Resources;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    public class FloatAroundItem : Element
    {
        public static List<FloatAroundItem> itemList = new List<FloatAroundItem>();

        public List<ItemData> itemData = new List<ItemData>();
        public Vector2 velocity;
        public bool nearPlayer;
        protected GameMaster gameMaster = GameMaster.gameMaster;

        public float disableCollectTime = 0;
        public int curData;
        public float dataChangeTime = 0;
        public float dataChangeSpeed = 0;
        public float lifeTime = 1200;

        public bool rockTimePlus = true;
        public float rockTime = 0;
        public float rockSpeed = 60;
        public float rockAmount = 10;

        public override void OnUpdate()
        {
            if (disableCollectTime <= 0)
            {
                disableCollectTime -= gameMaster.timeScale;
            }

            if(dataChangeTime > dataChangeSpeed)
            {
                dataChangeTime -= dataChangeSpeed;
                curData = ItemElement.Random(0, itemData.Count);
                UpdateGraphics();
            }

            if(rockTimePlus)
            {
                rockTime += (1 / rockSpeed) * gameMaster.timeScale;

                if (rockTime >= 1)
                {
                    rockTime = 1;
                    rockTimePlus = false;
                }
            }
            else
            {
                rockTime -= (1 / rockSpeed) * gameMaster.timeScale;

                if(rockTime <= 0)
                {
                    rockTime = 0;
                    rockTimePlus = true;
                }
            }


            entityAttachedTo.LocalRotationDegrees = new Vector3(0, 0, MathHelper.Lerp(-rockAmount, rockAmount, rockTime));

            if (nearPlayer)
            {
                entityAttachedTo.LocalPosition += new Vector3(velocity * gameMaster.timeScale * 0.6f);
            }
            else
            {
                entityAttachedTo.LocalPosition += new Vector3(velocity * gameMaster.timeScale);
                dataChangeTime += gameMaster.timeScale;
            }

            if (lifeTime > 0)
            {
                if (velocity.X < 0 && entityAttachedTo.WorldPosition.X <= gameMaster.playerBoundsX.X)
                    velocity.X = -velocity.X;
                else if (velocity.X > 0 && entityAttachedTo.WorldPosition.X >= gameMaster.playerBoundsX.Y)
                    velocity.X = -velocity.X;

                if (velocity.Y < 0 && entityAttachedTo.WorldPosition.Y <= gameMaster.playerBoundsY.X)
                    velocity.Y = -velocity.Y;
                else if (velocity.Y > 0 && entityAttachedTo.WorldPosition.Y >= gameMaster.playerBoundsY.Y)
                    velocity.Y = -velocity.Y;

                lifeTime -= gameMaster.timeScale;
            }
            else
            {
                if(entityAttachedTo.LocalPosition.X < gameMaster.bulletBoundsX.X || gameMaster.bulletBoundsX.Y < entityAttachedTo.LocalPosition.X)
                    entityAttachedTo.Disable();
                else if(entityAttachedTo.LocalPosition.Y < gameMaster.bulletBoundsY.X || gameMaster.bulletBoundsY.Y < entityAttachedTo.LocalPosition.Y)
                    entityAttachedTo.Disable();
            }
        }

        public void Collect()
        {
            BaseResource resource;
            for (int i = 0; i < itemData[curData].resourcesModifiedOnPickup.Count; i++)
            {
                if (!gameMaster.resources.TryGetValue(itemData[curData].resourcesModifiedOnPickup[i].name, out resource))
                    continue;
                resource.ModifyValue(itemData[curData].resourcesModifiedOnPickup[i].values);
            }

            entityAttachedTo.Disable();
        }

        public void UpdateGraphics()
        {
            TextureAnimator animator = entityAttachedTo.GetElement<TextureAnimator>();
            animator.animationStates = itemData[curData].animations;
            animator.UpdateAnim(false);

            entityAttachedTo.GetElement<MeshRenderer>().shader = itemData[curData].shader;
        }

        public FloatAroundItem(List<ItemData> datas) : base()
        {
            itemData = datas;
        }
        public FloatAroundItem(List<ItemData> datas, Vector2 vel) : this(datas)
        {
            velocity = vel;
        }



        public static Entity SpawnItem(List<ItemData> data, Vector3 position, int startingIndex = 0, float dataChangeSpeed = 240, float lifeTime = 1200, float disableCollectTime = 0)
        {
            const float MIN_X_VEL = -1, MAX_X_VEL = 1;
            const float MIN_Y_VEL = -1, MAX_Y_VEL = 1;
            Vector2 randomVel = new Vector2(ItemElement.Random(MIN_X_VEL, MAX_X_VEL), ItemElement.Random(MIN_Y_VEL, MAX_Y_VEL));
            FloatAroundItem newItem;

            for (int i = 0; i < itemList.Count; i++)
                if (!itemList[i].entityAttachedTo.enabled)
                    return ResetItem(i, data, position, randomVel, startingIndex, dataChangeSpeed, lifeTime, disableCollectTime);

            if (itemList.Count >= GameMaster.gameMaster.maxItemCount)
                return ResetItem(0, data, position, randomVel, startingIndex, dataChangeSpeed, lifeTime, disableCollectTime);

            Entity newSpawn = new Entity("item" + itemList.Count, position);
            MeshRenderer renderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "items",
                OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, data[startingIndex].shader, data[startingIndex].animations[0].animFrames[0].sprite.tex);
            newSpawn.AddElement(renderer);
            newItem = new FloatAroundItem(data, randomVel);
            newSpawn.AddElement(newItem);

            newItem.dataChangeSpeed = dataChangeSpeed;
            newItem.disableCollectTime = disableCollectTime;
            newItem.lifeTime = lifeTime;

            SpriteRenderer spriteRend = new SpriteRenderer(renderer);
            newSpawn.AddElement(spriteRend);

            newSpawn.AddElement(new TextureAnimator(data[startingIndex].animations, spriteRend));


            newItem.UpdateGraphics();

            itemList.Add(newItem);
            return newSpawn;
        }

        public static Entity ResetItem(int index, List<ItemData> data, Vector3 position, Vector2 vel, int startingIndex = 0, float dataChangeSpeed = 240, float lifeTime = 600, float disableCollectTime = 0)
        {
            Entity newSpawn = itemList[index].entityAttachedTo;
            if (!newSpawn.enabled)
                newSpawn.Enable();

            newSpawn.WorldPosition = position;

            itemList[index].itemData = data;
            itemList[index].velocity = vel;
            itemList[index].nearPlayer = false;
            itemList[index].disableCollectTime = disableCollectTime;
            itemList[index].curData = startingIndex;
            itemList[index].dataChangeSpeed = dataChangeSpeed;
            itemList[index].dataChangeTime = 0;
            itemList[index].lifeTime = lifeTime;

            itemList[index].UpdateGraphics();

            itemList.Add(itemList[index]);
            itemList.RemoveAt(index);
            return newSpawn;
        }
    }
}

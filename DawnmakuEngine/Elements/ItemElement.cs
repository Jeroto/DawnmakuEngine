using DawnmakuEngine.Data;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using System.Data;
using DawnmakuEngine.Data.Resources;

namespace DawnmakuEngine.Elements
{
    class ItemElement : Element
    {
        public static List<ItemElement> itemList = new List<ItemElement>();
        public static Random itemDropRandom = new Random();

        public ItemData itemData;
        public Vector2 velocity;
        public bool drawToPlayer, magnetToPlayer;
        protected GameMaster gameMaster = GameMaster.gameMaster;

        public bool halfItemFallSpeed = false;
        public float disableCollectTime = 0;

        public override void OnUpdate()
        {
            if (disableCollectTime <= 0)
            {
                if (gameMaster.pointOfCollection)
                    drawToPlayer = true;
            }
            else
                disableCollectTime -= gameMaster.timeScale;

            if (!drawToPlayer && !magnetToPlayer)
            {
                velocity.X = Math.Clamp(Math.Abs(velocity.X) - itemData.xDecel * gameMaster.timeScale, 0, Math.Abs(velocity.X)) * Math.Sign(velocity.X);

                float maxFallSpeed = -itemData.maxFallSpeed;
                if (halfItemFallSpeed)
                    maxFallSpeed /= 2;
                velocity.Y = Math.Clamp(velocity.Y - itemData.gravAccel * gameMaster.timeScale, maxFallSpeed, velocity.Y);
            }

            entityAttachedTo.LocalPosition += new Vector3(velocity * gameMaster.timeScale);

            if (entityAttachedTo.WorldPosition.Y <= gameMaster.itemDisableHeight)
                entityAttachedTo.Disable();
        }

        public void Collect()
        {
            BaseResource resource;
            for (int i = 0; i < itemData.resourcesModifiedOnPickup.Count; i++)
            {
                if (!gameMaster.resources.TryGetValue(itemData.resourcesModifiedOnPickup[i].name, out resource))
                    continue;
                resource.ModifyValue(itemData.resourcesModifiedOnPickup[i].values);
            }

            entityAttachedTo.Disable();
        }

        public ItemElement(ItemData data) : base() 
        {
            itemData = data;
        }
        public ItemElement(ItemData data, Vector2 vel) : this(data)
        {
            velocity = vel;
        }



        public static Entity SpawnItem(ItemData data, Vector3 position, float disableCollectTime = 0)
        {
            Vector2 randomVel = new Vector2(Random(data.randXRange), Random(data.randYRange));
            ItemElement newItemEle;

            for (int i = 0; i < itemList.Count; i++)
                if (!itemList[i].entityAttachedTo.enabled)
                    return ResetItem(i, data, position, randomVel, disableCollectTime);

            if (itemList.Count >= GameMaster.gameMaster.maxItemCount)
                return ResetItem(0, data, position, randomVel, disableCollectTime);

            Entity newSpawn = new Entity("item"+itemList.Count, position);
            MeshRenderer renderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "items",
                OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, data.shader, data.animations[0].animFrames[0].sprite.tex);
            newSpawn.AddElement(renderer);
            newItemEle = new ItemElement(data, randomVel);
            newItemEle.drawToPlayer = data.autoDraw;
            newSpawn.AddElement(newItemEle);

            SpriteRenderer spriteRend = new SpriteRenderer(renderer);
            newSpawn.AddElement(spriteRend);

            newSpawn.AddElement(new TextureAnimator(data.animations, spriteRend));

            itemList.Add(newItemEle);
            return newSpawn;
        }

        public static Entity ResetItem(int index, ItemData data, Vector3 position, Vector2 vel, float disableCollectTime = 0)
        {
            Entity newSpawn = itemList[index].entityAttachedTo;
            if (!newSpawn.enabled)
                newSpawn.Enable();
            else
            {
                itemList[index].Collect();
                newSpawn.Enable();
            }
            newSpawn.WorldPosition = position;

            itemList[index].itemData = data;
            itemList[index].velocity = vel;
            itemList[index].drawToPlayer = data.autoDraw;
            itemList[index].magnetToPlayer = false;
            itemList[index].disableCollectTime = disableCollectTime;
            itemList[index].halfItemFallSpeed = false;

            TextureAnimator animator = newSpawn.GetElement<TextureAnimator>();
            animator.animationStates = data.animations;
            animator.UpdateAnim(false);

            newSpawn.GetElement<MeshRenderer>().shader = data.shader;

            itemList.Add(itemList[index]);
            itemList.RemoveAt(index);
            return newSpawn;
        }

        //Random Functions
        public static void UpdateSeed(int newSeed)
        {
            itemDropRandom = new System.Random(newSeed);
            GameMaster.Log("Items' Random is set to " + newSeed);
        }

        public static int Random(int lower, int upper)
        {
            return itemDropRandom.Next(lower, upper);
        }

        public static float Random(float lower, float upper)
        {

            return (((float)itemDropRandom.NextDouble() * Math.Abs(lower - upper)) + lower);
        }

        public static float Random(Vector2 constraints)
        {

            return (((float)itemDropRandom.NextDouble() * Math.Abs(constraints.X - constraints.Y)) + constraints.X);
        }
    }
}

using DawnmakuEngine.Data;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using System.Data;

namespace DawnmakuEngine.Elements
{
    class ItemElement : Element
    {
        public static List<ItemElement> itemList = new List<ItemElement>();
        public static Random itemDropRandom = new Random();

        public ItemData itemData;
        public Vector2 velocity;
        public bool drawToPlayer, magnetToPlayer;
        GameMaster gameMaster = GameMaster.gameMaster;

        public override void OnUpdate()
        {
            if (gameMaster.pointOfCollection)
                drawToPlayer = true;
            
            if(!drawToPlayer && !magnetToPlayer)
            {
                velocity.X = Math.Clamp(Math.Abs(velocity.X) - itemData.xDecel * gameMaster.timeScale, 0, Math.Abs(velocity.X)) * Math.Sign(velocity.X);
                velocity.Y = Math.Clamp(velocity.Y - itemData.gravAccel * gameMaster.timeScale, -itemData.maxFallSpeed, velocity.Y);
            }
            entityAttachedTo.LocalPosition += new Vector3(velocity * gameMaster.timeScale);

            if (entityAttachedTo.WorldPosition.Y <= gameMaster.itemDisableHeight)
                entityAttachedTo.Disable();

            base.OnUpdate();
        }

        public void Collect()
        {

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



        public static Entity SpawnItem(ItemData data, Vector3 position)
        {
            Vector2 randomVel = new Vector2(Random(data.randXRange), Random(data.randYRange));
            ItemElement newItemEle;

            for (int i = 0; i < itemList.Count; i++)
                if (!itemList[i].entityAttachedTo.enabled)
                    return ResetItem(i, data, position, randomVel);

            if (itemList.Count >= GameMaster.gameMaster.maxItemCount)
                return ResetItem(0, data, position, randomVel);

            Entity newSpawn = new Entity("item"+itemList.Count, position);
            MeshRenderer renderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "items",
                OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, data.shader, data.animations[0].animFrames[0].sprite.tex, true);
            newSpawn.AddElement(renderer);
            newItemEle = new ItemElement(data, randomVel);
            newItemEle.drawToPlayer = data.autoDraw;
            newSpawn.AddElement(newItemEle);
            newSpawn.AddElement(new TextureAnimator(data.animations, renderer));

            itemList.Add(newItemEle);
            return newSpawn;
        }

        public static Entity ResetItem(int index, ItemData data, Vector3 position, Vector2 vel)
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

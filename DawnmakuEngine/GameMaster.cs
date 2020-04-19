using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;

namespace DawnmakuEngine
{
    class GameMaster
    {
        protected static int randomSeed = 0;
        protected static Random random = new Random();
        public static GameMaster gameMaster = new GameMaster();

        public float timeScale = 1, timeScaleUpdate = 1, frameTime = 1/60f;

        public int killzoneDetectIndex;

        public int difficulty;

        public Entity playerEntity;

        public Shader spriteShader = new Shader("Shaders/Shader.vert", "Shaders/TransparentShader.frag");


        public Texture[] loadedBulletTextures = new Texture[] { null };
        public TextureAnimator.AnimationState[] loadedBulletAnimStates = new TextureAnimator.AnimationState[] { null };
        public EnemyPattern[] loadedPatterns;

        public Entity[] bulletSpawns = new Entity[] { null };

        public Texture[] loadedEnemyTextures = new Texture[] { null };
        public TextureAnimator.AnimationState[] loadedEnemyAnimStates = new TextureAnimator.AnimationState[] { null };
        public Bezier[] enemyMovementPaths = new Bezier[] { null };
        public EnemyData[] loadedEnemyData;




        //Bullet Sprite Rects
        public Texture bulletSheet = new Texture("TouhouBullets.png", false);

        public float[] bulletColumns = new float[] { 0f,0.02272727f,0.04545455f,0.06818182f,0.09090909f,0.1136364f,0.1363636f,0.1590909f,0.1818182f,0.2045455f,0.2272727f,
            0.25f,0.2727273f,0.2954545f,0.3181818f,0.3409091f,0.3636364f,0.3863636f,0.4090909f,0.4318182f,0.4545455f,0.4772727f,0.5f,0.5227273f,0.5454546f,0.5681818f,
            0.5909091f,0.6136364f,0.6363636f,0.6590909f,0.6818182f,0.7045454f,0.7272727f,0.75f,0.7727273f,0.7954546f,0.8181818f,0.8409091f,0.8636364f,0.8863636f,
            0.9090909f,0.9318182f,0.9545454f,0.9772727f, 1f };
        public float[] roundBulletSprites = new float[] { 1f, 0.984375f };
        public float[] waveBulletSprites = new float[] { 0.984375f, 0.96875f };
        public float[] smallShardSprites = new float[] { 0.96875f, 0.953125f };
        public float[] thickShardSprites = new float[] { 0.953125f, 0.9375f };
        public float[] crystalBulletSprites = new float[] { 0.9375f, 0.921875f };
        public float[] kunaiBulletSprites = new float[] { 0.921875f, 0.90625f };
        public float[] shellBulletSprites = new float[] { 0.90625f, 0.890625f };
        public float[] pelletBulletSprites = new float[] { 0.890625f, 0.875f };
        public float[] roundOultineSprites = new float[] { 0.875f, 0.859375f };
        public float[] daggerBulletSprites = new float[] { 0.859375f, 0.84375f };
        public float[] starBulletSprites = new float[] { 0.84375f, 0.828125f };
        public float[] fireBulletSprites1 = new float[] { 0.828125f, 0.8125f };
        public float[] fireBulletSprites2 = new float[] { 0.8125f, 0.796875f };
        public float[] fireBulletSprites3 = new float[] { 0.796875f, 0.78125f };
        public float[] fireBulletSprites4 = new float[] { 0.78125f, 0.765625f };
        public float[] bigDropBulletSprites1 = new float[] { 0.765625f, 0.75f };
        public float[] bigDropBulletSprites2 = new float[] { 0.75f, 0.734375f };
        public float[] bigDropBulletSprites3 = new float[] { 0.734375f, 0.71875f };
        public float[] smallDropBulletSprites1 = new float[] { 0.71875f, 0.703125f };
        public float[] smallDropBulletSprites2 = new float[] { 0.703125f, 0.6875f };
        public float[] smallDropBulletSprites3 = new float[] { 0.6875f, 0.671875f };
        public float[] charmBulletSprites = new float[] { 0.671875f, 0.65625f };
        public float[] charmWhiteBulletSprites = new float[] { 0.65625f, 0.640625f };
        public float[] beamWideSprites = new float[] { 0.640625f, 0.625f };
        public float[] beam2pxSprites = new float[] { 0.625f, 0.609375f };
        public float[] beam3pxSprites = new float[] { 0.609375f, 0.59375f };
        public float[] beam4pxSprites = new float[] { 0.59375f, 0.578125f };
        public float[] beam1pxSprites = new float[] { 0.578125f, 0.5625f };
        public float[] heartBulletSprites = new float[] { 0.5625f, 0.546875f };
        public float[] butterflyBulletSprites1 = new float[] { 0.546875f, 0.53125f };
        public float[] butterflyBulletSprites2 = new float[] { 0.53125f, 0.515625f };
        public float[] butterflyBulletSprites3 = new float[] { 0.515625f, 0.5f };
        public float[] butterflyBulletSprites4 = new float[] { 0.5f, 0.484375f };
        public float[] popcornBulletSprites = new float[] { 0.484375f, 0.46875f };
        public float[] popcornDarkBulletSprites = new float[] { 0.46875f, 0.453125f };
        public float[] coinBulletSprites = new float[] { 0.453125f, 0.4375f };
        public float[] dropletBulletSprites = new float[] { 0.4375f, 0.421875f }; 

        public float[] arrowBulletSprites = new float[] { 0.421875f, 0.390625f };
        public float[] restBulletSprites = new float[] { 0.390625f, 0.359375f };
        public float[] noteBulletSprites1 = new float[] { 0.359375f, 0.328125f };
        public float[] noteBulletSprites2 = new float[] { 0.328125f, 0.296875f };
        public float[] noteBulletSprites3 = new float[] { 0.296875f, 0.265625f };

















        public Vector2 CheckDistOutsideCamBounds(Vector2 position)
        {
            return Vector2.Zero; //Implement
        }


        //All random functions
        public void GenerateRandomSeed()
        {
            randomSeed = (int)DawnMath.Repeat(DateTime.UtcNow.Ticks, int.MaxValue, int.MinValue);
            UpdateSeed(randomSeed);
        }

        public void UpdateSeed(int newSeed)
        {
            randomSeed = newSeed;
            random = new System.Random(randomSeed);
            Console.WriteLine("Game's Random is set to " + randomSeed);
        }

        public static int Random(int lower, int upper)
        {
            return random.Next(lower, upper);
        }

        public static float Random(float lower, float upper)
        {

            return (((float)random.NextDouble() * Math.Abs(lower - upper)) + lower);
        }

        public static float Random(Vector2 constraints)
        {

            return (((float)random.NextDouble() * Math.Abs(constraints.X - constraints.Y)) + constraints.X);
        }
    }
}

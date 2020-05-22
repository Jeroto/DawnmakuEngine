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

        public int maxPowerLevel = 8,
            currentPowerLevel = 0, prevPowerLevel;
        public float maxPower = 800;
        public float currentPower = 800;

        public bool PowerLevelChange { get { return currentPowerLevel != prevPowerLevel; } }

        public Entity playerEntity;
        public int playerTypeIndex, playerShotIndex;

        public delegate void ElementFunction();

        public event ElementFunction PostCreate;
        public event ElementFunction OnUpdate;
        public event ElementFunction PreRender;


        public Shader spriteShader = new Shader("Shaders/Shader.vert", "Shaders/TransparentShader.frag");



        //Loaded Bullet Data
        public List<string> bulletTypes = new List<string>();
        public Dictionary<string, Texture> bulletSheets = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> bulletSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, Entity> bulletSpawns = new Dictionary<string, Entity>();
        public Dictionary<string, BulletData> bulletData = new Dictionary<string, BulletData>();
        public Dictionary<string, Texture> loadedBulletTextures = new Dictionary<string, Texture>();
        public Dictionary<string, TextureAnimator.AnimationState> loadedBulletAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();

        //Player Orbs
        public Dictionary<string, Texture> loadedPlayerOrbTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> playerOrbSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> loadedPlayerOrbAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, PlayerOrbData> loadedPlayerOrbData = new Dictionary<string, PlayerOrbData>();

        //Loaded Player Data
        public Dictionary<string, Texture> loadedPlayerTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> playerSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> loadedPlayerAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, PlayerCharData> loadedPlayerChars = new Dictionary<string, PlayerCharData>();
        public Dictionary<string, PlayerTypeData> loadedPlayerTypes = new Dictionary<string, PlayerTypeData>();
        public Dictionary<string, PlayerShotData> loadedPlayerShot = new Dictionary<string, PlayerShotData>();
        public Dictionary<string, Pattern> loadedPlayerPatterns = new Dictionary<string, Pattern>();

        //Loaded Enemy Data
        public Dictionary<string, Texture> loadedEnemyTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> enemySprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> loadedEnemyAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, Bezier> enemyMovementPaths = new Dictionary<string, Bezier>();
        public Dictionary<string, EnemyData> loadedEnemyData = new Dictionary<string, EnemyData>();
        public Dictionary<string, Pattern> loadedEnemyPatterns = new Dictionary<string, Pattern>();

        //Loaded Background Data
        public Dictionary<string, Mesh> backgroundModels = new Dictionary<string, Mesh>();
        public Dictionary<string, Texture> backgroundTextures = new Dictionary<string, Texture>();






        public void GameMasterUpdate()
        {
            prevPowerLevel = currentPowerLevel;
        }

        public void ElementUpdate()
        {
            PostCreate?.Invoke();
            OnUpdate?.Invoke();
        }

        public void ElementPreRender()
        {
            PreRender?.Invoke();
        }









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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;

using OpenTK;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace DawnmakuEngine
{
    class GameMaster
    {
        protected static int randomSeed = 0;
        protected static Random random = new Random();
        public static GameMaster gameMaster = new GameMaster();
        public static bool debugMode = false;

        public float timeScale = 1, timeScaleUpdate = 1, frameTime = 1/60f;
        public int windowWidth = 1920, windowHeight = 1080;

        public bool paused = false;

        public int killzoneDetectIndex;

        public static List<RenderLayer> renderLayerSettings = new List<RenderLayer>();
        public static Dictionary<string, int> layerIndexes = new Dictionary<string, int>();

        public int difficulty;
        public uint highScore, score;

        public int maxPowerLevel = 8,
            currentPowerLevel = 0, prevPowerLevel,
            maxPower = 800, currentPower = 800,
            powerLostOnDeath, powerTotalDroppedOnDeath;

        public int[] powerLevelSplits;

        public string[] mainStageNames, exStageNames,
            languages;

        public bool PowerLevelChange { get { return currentPowerLevel != prevPowerLevel; } }

        public Entity playerEntity;
        public Vector3 playerWorldPos;
        public int playerTypeIndex, playerShotIndex;
        public bool pointOfCollection = false,
            fullPowerPOC = false, shiftForPOC = false;
        public int pocHeight = 100, itemDisableHeight = -100;

        public delegate void ElementFunction();

        public event ElementFunction PostCreate;
        public event ElementFunction OnUpdate;
        public event ElementFunction PreRender;


        public Shader spriteShader = new Shader("Data/General/Shaders/Shader.vert", "Data/General/Shaders/TransparentShader.frag");

        public ushort maxItemCount = 200;
        //Default Item Data Values
        public Vector2 itemRandXRange, itemRandYRange;
        public float itemMaxFallSpeed, itemGravAccel, itemXDecel,
            itemMagnetDist, itemMagnetSpeed,
            itemCollectDist;

        //Loaded Shaders
        public Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();

        //Loaded Bullet Data
        public List<string> bulletTypes = new List<string>();
        public Dictionary<string, Texture> bulletSheets = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> bulletSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, Entity> bulletSpawns = new Dictionary<string, Entity>();
        public Dictionary<string, BulletData> bulletData = new Dictionary<string, BulletData>();
        public Dictionary<string, Texture> bulletTextures = new Dictionary<string, Texture>();
        public Dictionary<string, TextureAnimator.AnimationState> bulletAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();

        //Player Orbs
        public Dictionary<string, Texture> playerOrbTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> playerOrbSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> playerOrbAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, PlayerOrbData> playerOrbData = new Dictionary<string, PlayerOrbData>();

        //Loaded Player Data
        public Dictionary<string, Texture> playerTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> playerSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> playerAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, PlayerCharData> playerChars = new Dictionary<string, PlayerCharData>();
        public Dictionary<string, PlayerTypeData> playerTypes = new Dictionary<string, PlayerTypeData>();
        public Dictionary<string, PlayerShotData> playerShot = new Dictionary<string, PlayerShotData>();
        public Dictionary<string, Pattern> playerPatterns = new Dictionary<string, Pattern>();

        public Dictionary<string, Texture> playerEffectTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> playerEffectSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> playerEffectAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();

        //Loaded Item Data
        public List<string> itemTypes = new List<string>();
        public Dictionary<string, Texture> itemTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> itemSprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> itemAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, ItemData> itemData = new Dictionary<string, ItemData>();

        //Loaded Enemy Data
        public Dictionary<string, Texture> enemyTextures = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> enemySprites = new Dictionary<string, SpriteSet>();
        public Dictionary<string, TextureAnimator.AnimationState> enemyAnimStates = new Dictionary<string, TextureAnimator.AnimationState>();
        public Dictionary<string, Bezier> enemyMovementPaths = new Dictionary<string, Bezier>();
        public Dictionary<string, EnemyData> enemyData = new Dictionary<string, EnemyData>();
        public Dictionary<string, Pattern> enemyPatterns = new Dictionary<string, Pattern>();

        //Loaded Background Data
        public Dictionary<string, Mesh> backgroundMeshes = new Dictionary<string, Mesh>();
        public Dictionary<string, Texture> backgroundTextures = new Dictionary<string, Texture>();
        public Dictionary<string, TexturedModel> backgroundModels = new Dictionary<string, TexturedModel>();
        public Dictionary<string, BackgroundSection> backgroundSections = new Dictionary<string, BackgroundSection>();

        //Loaded UI 
        public Dictionary<string, Texture> UITextures = new Dictionary<string, Texture>();
        public FontCollection fonts = new FontCollection();
        public Dictionary<string, FontCharList> fontCharList = new Dictionary<string, FontCharList>();
        public Dictionary<string, Texture> fontSheets = new Dictionary<string, Texture>();
        public Dictionary<string, SpriteSet> fontGlyphSprites = new Dictionary<string, SpriteSet>();



        public class RenderLayer
        {
            public bool hasDepth = false;
        }


        public void GameMasterUpdate()
        {
            prevPowerLevel = currentPowerLevel;

            if (!paused)
                timeScale = timeScaleUpdate;
            else
                timeScale = 0;

            if(playerEntity != null)
                playerWorldPos = playerEntity.WorldPosition;
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



        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public void ShowConsole(bool show)
        {
            const int SW_HIDE = 0;
            const int SW_SHOW = 5;
            ShowWindow(GetConsoleWindow(), show ? SW_SHOW : SW_HIDE);
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

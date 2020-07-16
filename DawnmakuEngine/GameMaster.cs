using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DawnmakuEngine.Elements;
using DawnmakuEngine.Data;
using OpenTK;
using SixLabors.Fonts;
using System.Diagnostics;

namespace DawnmakuEngine
{
    public class GameMaster
    {
        protected static int randomSeed = 0;
        protected static Random random = new Random();
        public static GameMaster gameMaster = new GameMaster();
        public static Stopwatch timeLogger = new Stopwatch();

        public static Shader lastBoundShader;
        public static Texture lastBoundTexture;
        public static Mesh lastBoundMesh;

        public static bool debugMode = true;
        public bool logAllFontChars = false, canToggleInvincible = true, logTimers = false;
        public bool invincible = false;

        public AudioController audioManager = new AudioController();

        public float timeScale = 1, timeScaleUpdate = 1, frameTime = 1/60f;
        public int windowWidth = 1920, windowHeight = 1080;

        public bool paused = false;

        public byte killzoneDetectIndex;

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
        public string curCharName;
        public int playerTypeIndex, playerShotIndex;
        public float grazeDistance = 10;
        public bool pointOfCollection = false,
            fullPowerPOC = false, shiftForPOC = false;
        public int pocHeight = 100, itemDisableHeight = -100;
        public Vector2 playerBoundsX, playerBoundsY;

        public Vector2 bulletBoundsX, bulletBoundsY,
            enemyBoundsX, enemyBoundsY;

        public delegate void ElementFunction();

        public event ElementFunction PostCreate;
        public event ElementFunction OnUpdate;
        public event ElementFunction PreRender;

        public bool enemyBulletSpawnSoundPlayed = false, enemyBulletStageSoundPlayed = false,
            playerBulletSpawnSoundPlayed = false, playerBulletStageSoundPlayed = false;

        private float masterVolume = 1, bgmVolume = 1, sfxVolume = .75f,
            bulletSpawnVolume = .1f, bulletStageVolume = 0.75f, 
            playerShootVolume = .75f, playerBulletVolume = .1f, playerDeathVolume = 1, 
            enemyDeathVolume = 1;

        public float BgmVol { get { return masterVolume * bgmVolume; } }
        public float SfxVol { get { return masterVolume * sfxVolume; } }
        public float BulletSpawnVol { get { return masterVolume * sfxVolume * bulletSpawnVolume; } }
        public float BulletStageVol { get { return masterVolume * sfxVolume * bulletStageVolume; } }
        public float PlayerShootVol { get { return masterVolume * sfxVolume * playerShootVolume; } }
        public float PlayerBulletVol { get { return masterVolume * sfxVolume * playerBulletVolume; } }
        public float PlayerDeathVol { get { return masterVolume * sfxVolume * playerDeathVolume; } }
        public float EnemyDeathVol { get { return masterVolume * sfxVolume * enemyDeathVolume; } }




        public Shader generalTextShader, dialogueTextShader;

        public ushort maxItemCount = 200;
        //Default Item Data Values
        public Vector2 itemRandXRange, itemRandYRange;
        public float itemMaxFallSpeed, itemGravAccel, itemXDecel,
            itemMagnetDist, itemDrawSpeed, itemMagnetSpeed,
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

        //Loaded Sfx
        public Dictionary<string, AudioData> sfx = new Dictionary<string, AudioData>();

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
        public List<string> fontNames = new List<string>();
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

            if (canToggleInvincible && InputScript.IDown)
                invincible = !invincible;

            killzoneDetectIndex = (byte)DawnMath.Repeat(killzoneDetectIndex + 1, 6);

            if (playerEntity != null)
                playerWorldPos = playerEntity.WorldPosition;
        }

        public void ElementUpdate()
        {
            StartTimer();
            PostCreate?.Invoke();
            LogTimeMilliseconds("all post creates");
            StartTimer();
            OnUpdate?.Invoke();
            LogTimeMilliseconds("all updates");
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

        public static void ShowConsole(bool show)
        {
            const int SW_HIDE = 0;
            const int SW_SHOW = 5;
            ShowWindow(GetConsoleWindow(), show ? SW_SHOW : SW_HIDE);
        }

        public static void Log(string text)
        {
            if (!debugMode)
                return;
            Console.WriteLine(text);
        }
        public static void LogSecondary(string text)
        {
            if (!debugMode)
                return;
            Console.Write("> ");
            Console.WriteLine(text);
        }
        public static void LogPositiveNotice(string text)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogNeutralNotice(string text)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogNegativeNotice(string text)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void StartTimer()
        {
            if (!gameMaster.logTimers)
                return;
            timeLogger.Restart();
        }
        public static void LogTimeMilliseconds(string item, string opening = "Time taken for ")
        {
            LogTimeCustMilliseconds(item, timeLogger.Elapsed.TotalMilliseconds, opening);
        }
        public static void LogTimeTicks(string item, string opening = "Time taken for ")
        {
            LogTimeCustTicks(item, timeLogger.ElapsedTicks, opening);
        }
        public static void LogTimeMillisecondsAndTicks(string item, string opening = "Time taken for ")
        {
            LogTimeCustMillisecondsAndTicks(item, timeLogger.Elapsed.TotalMilliseconds, timeLogger.ElapsedTicks, opening);
        }
        public static void LogTimeCustMilliseconds(string item, double milliseconds, string opening = "Time taken for ")
        {
            if (!gameMaster.logTimers)
                return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" | ");
            Console.Write(opening);
            Console.Write(item);
            Console.Write(" | ");
            Console.Write(milliseconds);
            Console.WriteLine(" milliseconds");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogTimeCustTicks(string item, long ticks, string opening = "Time taken for ")
        {
            if (!gameMaster.logTimers)
                return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" | ");
            Console.Write(opening);
            Console.Write(item);
            Console.Write(" | ");
            Console.Write(ticks);
            Console.WriteLine(" ticks");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogTimeCustMillisecondsAndTicks(string item, double milliseconds, long ticks, string opening = "Time taken for ")
        {
            if (!gameMaster.logTimers)
                return;
            timeLogger.Stop();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" | ");
            Console.Write(opening);
            Console.Write(item);
            Console.Write(" | ");
            Console.Write(milliseconds);
            Console.Write(" milliseconds, or ");
            Console.Write(ticks);
            Console.WriteLine(" ticks");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogError(string text)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[!] ");
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogErrorMessage(string text, string message)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[!] ");
            Console.WriteLine(text);
            Console.Write("> ");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogWarning(string text)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[!] ");
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogWarningMessage(string text, string message)
        {
            if (!debugMode)
                return;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[!] ");
            Console.WriteLine(text);
            Console.Write("> ");
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
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
            Log("Game's Random is set to " + randomSeed);
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

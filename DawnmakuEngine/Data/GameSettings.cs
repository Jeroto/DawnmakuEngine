using DawnmakuEngine.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    class GameSettings
    {
        //Debug Values
        public bool debugMode = false, logAllFontChars = false, canToggleInvincible = true, logTimers = false;

        //Misc Values
        public Shader generalTextShader, dialogueTextShader;
        public bool fullPowerCollection = false, shiftForCollection = false;
        public int maxPower, powerLostOnDeath, powerTotalDroppedOnDeath, maxPowerLevel;
        public int[] powerLevelSplits = null;
        public string[] mainStageFolderNames, exStageFolderNames,
            languages;
        public Dictionary<string, int> renderLayers = new Dictionary<string, int>();
        public List<GameMaster.RenderLayer> renderLayerSettings = new List<GameMaster.RenderLayer>();

        //Player Values
        public Vector2 playerBoundsX, playerBoundsY;
        public float grazeDistance;


        public Vector2 bulletBoundsX, bulletBoundsY,
            enemyBoundsX, enemyBoundsY;

        //Default Item Data Values
        public Vector2 itemRandXRange, itemRandYRange;
        public float itemMaxFallSpeed, itemGravAccel, itemXDecel,
            itemMagnetDist, itemDrawSpeed, itemMagnetSpeed,
            itemCollectDist;
        public int collectionHeight, itemDisableHeight;
        public ushort maxItemCount;
    }
}

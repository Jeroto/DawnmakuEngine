using DawnmakuEngine.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Data
{
    class GameSettings
    {
        public Shader generalTextShader, dialogueTextShader;
        public bool runInDebugMode = false,
            fullPowerPOC = false, shiftForPOC = false;
        public int maxPower, powerLostOnDeath, powerTotalDroppedOnDeath, maxPowerLevel;
        public int[] powerLevelSplits = null;
        public string[] mainStageFolderNames, exStageFolderNames,
            languages;
        public Dictionary<string, int> renderLayers = new Dictionary<string, int>();
        public List<GameMaster.RenderLayer> renderLayerSettings = new List<GameMaster.RenderLayer>();

        //Player Values
        public Vector2 playerBoundsX, playerBoundsY;
        public float grazeDistance;


        public Vector2 bulletBoundsX, bulletBoundsY;

        //Default Item Data Values
        public Vector2 itemRandXRange, itemRandYRange;
        public float itemMaxFallSpeed, itemGravAccel, itemXDecel,
            itemMagnetDist, itemDrawSpeed, itemMagnetSpeed,
            itemCollectDist;
        public int pocHeight, itemDisableHeight;
        public ushort maxItemCount;
    }
}

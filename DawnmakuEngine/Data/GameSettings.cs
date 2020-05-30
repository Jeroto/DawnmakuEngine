using DawnmakuEngine.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data
{
    class GameSettings
    {
        public bool runInDebugMode = false,
            fullPowerPOC = false, shiftForPOC = false;
        public int maxPower, powerLostOnDeath, powerTotalDroppedOnDeath, maxPowerLevel;
        public int[] powerLevelSplits = null;
        public string[] mainStageFolderNames, exStageFolderNames,
            languages;
        public Dictionary<string, int> renderLayers = new Dictionary<string, int>();
        public List<GameMaster.RenderLayer> renderLayerSettings = new List<GameMaster.RenderLayer>();
    }
}

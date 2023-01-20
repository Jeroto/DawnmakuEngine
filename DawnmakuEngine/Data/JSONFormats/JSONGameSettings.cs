using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONGameSettings
    {
        public bool debugMode = false;
        public bool logAllFontChars = false;
        public bool pressIToToggleInvincible = false;
        public bool logTimers = false;

        public Vector2 aspectRatio;

        public List<JSONResourceModification> resourcesModifiedOnDeath = new List<JSONResourceModification>();
        public List<JSONObjectAndCount> itemsDroppedOnDeath = new List<JSONObjectAndCount>();

        public int maxPower;
        public int powerLostOnDeath;
        public int powerTotalDroppedOnDeath;
        public List<int> powerLevelSplits = new List<int>();
        public bool fullPowerForCollection;
        public bool shiftAtTopToCollect;

        public float grazeDistance;
        
        public JSONMinMax playerBoundsX;
        public JSONMinMax playerBoundsY;

        public JSONMinMax bulletBoundsX;
        public JSONMinMax bulletBoundsY;

        public JSONMinMax enemyBoundsX;
        public JSONMinMax enemyBoundsY;

        public ushort maxItemCount;
        public int pointOfCollectionHeight;

        public int itemDisableHeight;
        public JSONMinMax itemRandXVelRange;
        public JSONMinMax itemRandYVelRange;
        public float itemMaxFallSpeed;
        public float itemGravAccel;
        public float itemXDecel;

        public float itemMagnetDist;
        public float itemMagnetSpeed;
        public float itemDrawSpeed;
        public float itemCollectDist;

        public string generalTextShader;
        public string dialogueTextShader;

        public List<string> mainStages = new List<string>();
        public List<string> exStages = new List<string>();

        public List<string> languages= new List<string>();
        public List<RenderLayer> renderLayers = new List<RenderLayer>();

        public class RenderLayer
        {
            public string name;
            public bool hasDepth;
            public bool hasLighting;
        }
    }
}

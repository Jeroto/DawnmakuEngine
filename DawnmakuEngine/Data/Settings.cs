using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data
{
    public class Settings
    {
        public int windowSizeIndex = 0;
        public int frameCap = 60;
        public bool fullscreen = false; 
        public float masterVolume = 0.1f, bgmVolume = 1, sfxVolume = .25f,
             bulletSpawnVolume = .02f, bulletStageVolume = 0.1f,
             playerShootVolume = .75f, playerBulletVolume = .2f, playerDeathVolume = 1f,
             enemyDeathVolume = 0.5f;
    }
}

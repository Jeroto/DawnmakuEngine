using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONCharacter
    {
        public string characterShader;
        public List<string> name = new List<string>();
        public float moveSpeed;
        public float focusSpeedPercent;
        public float colliderSize;
        public float colliderOffsetX, colliderOffsetY;

        public string hitSound;
        public string focusSound;
        public string grazeSound;

        public List<string> shotTypes = new List<string>();
        public List<string> animStates = new List<string>();

        public string hitboxAnim;
        public int hitboxPixelInset;
        public string hitboxShader;

        public string focusEffectAnim;
        public float focusEffectRotSpeed;
        public string focusEffectShader;
    }
}

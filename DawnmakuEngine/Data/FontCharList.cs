using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DawnmakuEngine.Data
{
    class FontCharList
    {
        //public Dictionary<int, ushort> fontChars = new Dictionary<int, ushort>();
        public string everyCharInFont = "";
        public List<char> charList = new List<char>();
        public HashSet<char> charListHash = new HashSet<char>();
        public Dictionary<ushort, int> indexes = new Dictionary<ushort, int>();

        public static SpriteSet.Sprite GetGlyphSprite(Dictionary<string, SpriteSet> dict, string fontName, char charToGet)
        {
            GameMaster gameMaster = GameMaster.gameMaster;
            FontCharList charList = gameMaster.fontCharList[fontName];
            if(charList.charList.Contains(charToGet))
                return dict[fontName].sprites[charList.charList.IndexOf(charToGet)];

            FontFamily[] families = gameMaster.fonts.Families.ToArray();

            for (int i = 0; i < families.Length; i++)
            {
                fontName = families[i].Name;
                charList = gameMaster.fontCharList[fontName];
                if (charList.charListHash.Contains(charToGet))
                    return dict[fontName].sprites[charList.charList.IndexOf(charToGet)];
            }

            return null;
        }
        public static SpriteSet.Sprite GetGlyphSprite(string fontName, char charToGet)
        {
            return GetGlyphSprite(GameMaster.gameMaster.fontGlyphSprites, fontName, charToGet);
        }
    }
}

using System;

namespace DawnmakuEngine
{
    class Engine
    {
        static void Main(string[] args)
        {
            using (Game window = new Game(1280, 960, "Dawnmaku"))
            {
                window.Run(60.0);
            }
        }
    }
}

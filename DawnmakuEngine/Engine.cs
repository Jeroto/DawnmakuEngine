using System;

namespace DawnmakuEngine
{
    class Engine
    {
        static void Main(string[] args)
        {
            using (Game window = new Game(800, 600, "Dawnmaku"))
            {
                window.Run(60.0);
            }
        }
    }
}

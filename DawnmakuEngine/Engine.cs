using System;
using System.Text;

namespace DawnmakuEngine
{
    class Engine
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GameMaster.ShowConsole(false);
            using (Game window = new Game(1280, 960, "Dawnmaku"))
            {
                
                window.Run(60.0);
            }
        }
    }
}

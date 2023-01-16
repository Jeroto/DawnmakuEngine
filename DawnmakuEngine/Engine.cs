using System;
using System.Text;
using OpenTK.Windowing.Desktop;

namespace DawnmakuEngine
{
    class Engine
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GameMaster.ShowConsole(false);

            GameMaster.gameMaster.gameWindowSettings.RenderFrequency = 60;
            GameMaster.gameMaster.gameWindowSettings.UpdateFrequency = 60;
            GameMaster.gameMaster.nativeWindowSettings.Title = "Dawnmaku";
            GameMaster.gameMaster.nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(1280, 960);
            GameMaster.gameMaster.nativeWindowSettings.WindowState = OpenTK.Windowing.Common.WindowState.Normal;
            GameMaster.gameMaster.nativeWindowSettings.StartFocused = true;
            GameMaster.gameMaster.nativeWindowSettings.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;
            GameMaster.gameMaster.nativeWindowSettings.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            GameMaster.gameMaster.nativeWindowSettings.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            GameMaster.gameMaster.nativeWindowSettings.StartFocused = true;

            using (Game window = new Game(GameMaster.gameMaster.gameWindowSettings, GameMaster.gameMaster.nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}

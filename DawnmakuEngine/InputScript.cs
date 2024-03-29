﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Input;
using Typography.OpenFont.Tables;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DawnmakuEngine
{
    class InputScript
    {
        private static bool pause = false;
        private static Vector2 directionalInput = Vector2.Zero;
        private static bool focus = false;
        private static bool shoot = false;
        private static bool bomb = false;
        private static bool special = false;

        private static bool prevPause = false;
        private static bool prevFocus = false;
        private static bool prevShoot = false;
        private static bool prevBomb = false;
        private static bool prevSpecial = false;

        private static bool iPressed;
        private static bool prevIPressed;

        public class ExtraInputData
        {
            //input
            enum OutputType { Float, Bool }
            public float outputFloat = 0;
            public bool outputBool = false;
        }

        public static bool IDown
        { 
            get { return iPressed && !prevIPressed;  }
        }

        public static Vector2 DirectionalInput
        {
            get { return directionalInput; }
        }
        public static bool Pause
        {
            get { return pause; }
        }
        public static bool Focus
        {
            get { return focus; }
        }
        public static bool Shoot
        {
            get { return shoot; }
        }
        public static bool Bomb
        {
            get { return bomb; }
        }
        public static bool Special
        {
            get { return special; }
        }

        public static bool pauseDown
        {
            get { return pause && !prevPause; }
        }
        public static bool pauseUp
        {
            get { return !pause && prevPause; }
        }
        public static bool focusDown
        {
            get { return focus && !prevFocus; }
        }
        public static bool focusUp
        {
            get { return !focus && prevFocus; }
        }
        public static bool shootDown
        {
            get { return shoot && !prevShoot; }
        }
        public static bool shootUp
        {
            get { return !shoot && prevShoot; }
        }
        public static bool bombDown
        {
            get { return bomb && !prevBomb; }
        }
        public static bool bombUp
        {
            get { return !bomb && prevBomb; }
        }
        public static bool specialDown
        {
            get { return special && !prevSpecial; }
        }
        public static bool specialUp
        {
            get { return !special && prevSpecial; }
        }

        public static void GetInput()
        {
            KeyboardState keyboardState = GameMaster.window.KeyboardState;

            prevIPressed = iPressed;
            iPressed = keyboardState.IsKeyDown(Keys.I);

            //GamePadState gamepadState = GamePad.GetState(0);
            directionalInput.X = keyboardState.IsKeyDown(Keys.Left) ? -1 : keyboardState.IsKeyDown(Keys.Right) ? 1 : 0;
            directionalInput.Y = keyboardState.IsKeyDown(Keys.Down) ? -1 : keyboardState.IsKeyDown(Keys.Up) ? 1 : 0;

            prevPause = pause;
            prevFocus = focus;
            prevShoot = shoot;
            prevBomb = bomb;
            prevSpecial = special;

            pause = keyboardState.IsKeyDown(Keys.Escape);
            focus = keyboardState.IsKeyDown(Keys.LeftShift);
            shoot = keyboardState.IsKeyDown(Keys.Z);
            bomb = keyboardState.IsKeyDown(Keys.X);
            special = keyboardState.IsKeyDown(Keys.C);
        }

        public static void ResetUpDown()
        {
            prevFocus = focus;
            prevShoot = shoot;
            prevBomb = bomb;
            prevSpecial = special;
        }

        public static void ClearInput()
        {
            directionalInput.X = 0;
            directionalInput.Y = 0;

            focus = false;
            shoot = false;
            bomb = false;
            special = false;
        }
    }
}

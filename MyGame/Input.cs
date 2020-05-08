using OpenTK.Input;
using System;

namespace MyGame
{
    public static class Input
    {
        private static KeyboardState state;

        private static int keys;

        private static bool[] lastFrame;
        private static bool[] curFrame;

        private static bool firstFrame = true;

        static Input()
        {
            //Game.window.UpdateFrame += UpdateKeyboard;

            keys = Enum.GetNames(typeof(Key)).Length;

            lastFrame = new bool[keys];
            curFrame = new bool[keys];
        }

        public static bool GetKeyDown(Key key)
        {
            return !lastFrame[(int)key] && curFrame[(int)key];
        }

        public static bool GetKey(Key key)
        {
            return curFrame[(int)key];
        }

        public static bool GetKeyUp(Key key)
        {
            return lastFrame[(int)key] && !curFrame[(int)key];
        }

        public static void UpdateKeyboard(object sender, OpenTK.FrameEventArgs e)
        {
            state = Keyboard.GetState();
            if(!firstFrame)
            {
                //lastFrame = curFrame;
                curFrame.CopyTo(lastFrame, 0);
            } 
            else
            {
                firstFrame = false;
            }

            for (int i = 0; i < keys; i++)
            {
                curFrame[i] = state.IsKeyDown((Key)i);
            }
        }
    }
}

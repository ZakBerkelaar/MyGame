using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace MyGame
{
    public static class Input
    {
        private static KeyboardState state;

        private static int keys;

        private static bool[] lastFrame;
        private static bool[] curFrame;

        private static bool firstFrame = true;

        private static List<Control> controls;

        static Input()
        {
            //Game.window.UpdateFrame += UpdateKeyboard;

            keys = Enum.GetNames(typeof(Keys)).Length;

            lastFrame = new bool[keys];
            curFrame = new bool[keys];

            controls = new List<Control>();
        }

        [Obsolete("Use create control instead")]
        public static bool GetKeyDown(Keys key)
        {
            return !lastFrame[(int)key] && curFrame[(int)key];
        }

        [Obsolete("Use create control instead")]
        public static bool GetKey(Keys key)
        {
            return curFrame[(int)key];
        }

        [Obsolete("Use create control instead")]
        public static bool GetKeyUp(Keys key)
        {
            return lastFrame[(int)key] && !curFrame[(int)key];
        }

        public static void UpdateKeyboard(KeyboardState newState)
        {
            state = newState;
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
                curFrame[i] = state.IsKeyDown((Keys)i);
            }
        }

        public static IControl CreateControl(IDString id, Keys key)
        {
            return new Control(id, key);
        }

        private class Control : IControl
        {
            private static Config controlConfig;

            private Keys key;
            public IDString IDString { get; }
            public bool IsDownFrame => !lastFrame[(int)key] && curFrame[(int)key];
            public bool IsDown => curFrame[(int)key];
            public bool IsUpFrame => lastFrame[(int)key] && !curFrame[(int)key];

            static Control()
            {
                controlConfig = new Config(new IDString("Config", "Controls"));
            }

            public Control(IDString iDString, Keys defaultKey)
            {
                IDString = iDString;
                if (controlConfig.TryGetValue(iDString, out string keyStr))
                {
                    key = (Keys)Enum.Parse(typeof(Keys), keyStr, true);
                }
                else
                {
                    key = defaultKey;
                    controlConfig.Write(iDString, defaultKey.ToString());
                }
            }
        }
    }
}

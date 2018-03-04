using System.Runtime.InteropServices;


namespace SmartKick
{
    public static class KeyboardControl
    {

        private static int Shift = 0x10;
        private static int Ctrl = 0x11;
        private static int KeyQ = 0x51;


        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);


        public static bool IsSmartKickHotkeysPressed()
        {

            if (GetAsyncKeyState(Shift) != 0 && GetAsyncKeyState(Ctrl) != 0 && GetAsyncKeyState(KeyQ) != 0)
                return true;
            else
                return false;

        }



    }
}

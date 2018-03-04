using System;
using System.Windows.Threading;

namespace SmartKick
{

    public delegate void HotkeyPress();

    public class StatusChecker
    {

        DispatcherTimer checkTimer;
        public static HotkeyPress HotkeyPress;

        public StatusChecker()
        {
            checkTimer = new DispatcherTimer();
            checkTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            checkTimer.Tick += CheckTimerTick;
            checkTimer.Start();
        }



        private void CheckTimerTick(object sender, EventArgs e)
        {
            if (KeyboardControl.IsSmartKickHotkeysPressed())
                HotkeyPress();

        }



        


    }
}

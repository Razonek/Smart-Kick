using Caliburn.Micro;


namespace SmartKick
{
    public class SmartKickViewModel : Screen
    {


        StatusChecker checker;
        Connection connection;



        private string description;
        public string Description
        {
            get { return description; }
            private set
            {
                description = value;
                NotifyOfPropertyChange("Description");
            }
        }



        public SmartKickViewModel()
        {
            this.DisplayName = "Smart Kick";
            checker = new StatusChecker();
            connection = new Connection();

            Description =
                @"Use combination 'Shift + Ctrl + Q' 
to kick your character in any moment you need.
It will close your TCP connection to 
Tibia Game Server immediately.
No need to relaunch game & rewrite passwords.";
        }




    }
}

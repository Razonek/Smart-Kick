﻿using Caliburn.Micro;
using System.Windows;

namespace SmartKick
{
    public class Bootstrapper : BootstrapperBase
    {

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<SmartKickViewModel>();
        }

        
    }
}

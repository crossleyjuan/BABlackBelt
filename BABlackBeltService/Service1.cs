using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ServerLib;

namespace BABlackBeltService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ChatServer.StartListener(9000);

        }

        protected override void OnStop()
        {
            ChatServer.StopListener();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CpuDispatcherOS
{
    public class DispatcherBack : Dispatcher
    {
        private int _currentTaskProcess = -1;
        private int _leftOfTask;
        private double executePart = 0.2;

        // S Y S T E M
        public int SystemWaitsGenTime;
        private int _systemWaitsSince;

        protected override void ExecuteTask()
        {

        }
    }
}

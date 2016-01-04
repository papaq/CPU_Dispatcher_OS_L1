using System;
using System.Collections.Generic;
using System.Linq;

namespace CpuDispatcherOS
{
    public class DispatcherFore : Dispatcher
    {
        private int _currentTaskProcess = -1;
        private int _leftOfTask;
        private double executePart = 0.8;

        // S Y S T E M
        public int SystemWaitsGenTime;
        private int _systemWaitsSince;
        
        protected override void ExecuteTask()
        {

        }
    }
}

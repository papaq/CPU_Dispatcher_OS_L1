using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CpuDispatcherOS
{
    public class DispatcherBack : Dispatcher
    {
        private int _currentTaskProcess = -1;
        private const double ExecutePart = 0.2;

        // S Y S T E M
        public int SystemWaitsGenTime;

        public DispatcherBack() : base("backgr")
        {

        }

        protected override void ExecuteTask()
        {

        }

        protected override void UpdateWaitOption(int waitTime, int except, int timeAppear)
        {
            foreach (var task in ListOfTasks.Where(task => task.Start == -1))
                task.Wait = CurrentTime - task.Appear;
        }
    }
}

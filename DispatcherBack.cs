using System;
using System.Linq;

namespace CpuDispatcherOS
{
    public class DispatcherBack : Dispatcher
    {
        private const double ExecutePart = 0.2;
        
        public DispatcherBack() : base("backgr")
        {

        }

        protected override void ExecuteTask()
        {
            CurrentTime += Convert.ToInt16(Tick*(1 - ExecutePart));
            var tempTime = CurrentTime;

            while (tempTime < CurrentTime + Convert.ToInt16(Tick * ExecutePart))
            {
                var task = ListOfTasks.Find(t => t.State != "done" && t.Appear < tempTime);

                // No not done task in a queue found
                if (null == task)
                {
                    SystemWaitsGenTime++;
                    tempTime++;
                    continue;
                }

                ListOfSequence.Add(task.Index.ToString());

                // Task was not processed before
                if (task.State == "wait")
                {
                    task.Start = tempTime;
                    task.State = "process";
                }

                // Left less time than needed for a task
                if (task.LeftToProcess > CurrentTime + Convert.ToInt16(Tick*ExecutePart) - tempTime)
                {
                    task.LeftToProcess -= CurrentTime + Convert.ToInt16(Tick*ExecutePart) - tempTime;
                    return;
                }

                // Task can be done until the end of tick
                tempTime += task.LeftToProcess;
                task.LeftToProcess = 0;
                task.Finish = tempTime;
                task.State = "done";
            }
        }

        protected override void UpdateWaitOption(int waitTime, int exceptIdx, int timeAppear)
        {
            foreach (var task in ListOfTasks.Where(t => t.State != "done" && t.Index != exceptIdx))
                task.Wait += waitTime;
        }
    }
}

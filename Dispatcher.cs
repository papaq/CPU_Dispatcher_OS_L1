using System;
using System.Collections.Generic;
using System.Linq;

namespace CpuDispatcherOS
{
    public abstract class Dispatcher
    {
        protected int CurrentTime = 0;

        // T A S K S
        private int _currentIndex;
        protected int NextAppears;

        // O P T I O N S
        public int CurrentTick = -1;

        public int Tick;
        public int WeightFrom;
        public int WeightTo;
        public int FreqFrom;
        public int FreqTo;

        public List<TaskItem> ListOfTasks = new List<TaskItem>();
        protected readonly Random Rnd = new Random();

        protected Dispatcher()
        {
            NextAppears = Rnd.Next(FreqFrom, FreqTo);
        }
        
        protected void CreateTask()
        {
            while (true)
            {
                if (NextAppears > CurrentTime + Tick || Tick < 1)
                    return;
                ListOfTasks.Add(new TaskItem()
                {
                    Index = _currentIndex++,
                    Weight = Rnd.Next(WeightFrom, WeightTo + 1),
                    Appear = NextAppears,
                    Start = -1,
                    Finish = -1,
                    State = "wait",
                });
                NextAppears += Rnd.Next(FreqFrom, FreqTo + 1);
            }
        }

        protected abstract void ExecuteTask();

        private void UpdateWaitOption()
        {
            foreach (var task in ListOfTasks.Where(task => task.Start == -1))
                task.Wait = CurrentTime - task.Appear;
        }

        public void MakeOneTick()
        {
            CurrentTick++;
            CreateTask();
            ExecuteTask();

            CurrentTime += Tick;
            UpdateWaitOption();
        }
    }
}

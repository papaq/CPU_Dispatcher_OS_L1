using System;
using System.Collections.Generic;
using System.Linq;

namespace CpuDispatcherOS
{
    public abstract class Dispatcher
    {
        protected int CurrentTime = 0;
        protected int Quantum = 100;

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

        private string _kind;

        public List<TaskItem> ListOfTasks = new List<TaskItem>();
        protected readonly Random Rnd = new Random();

        protected Dispatcher(string kind)
        {
            NextAppears = Rnd.Next(FreqFrom, FreqTo);
            _kind = kind;
        }
        
        protected void CreateTask()
        {
            while (true)
            {
                if (NextAppears > CurrentTime + Tick || Tick < 1)
                    return;
                int nextWeight = Rnd.Next(WeightFrom, WeightTo + 1);
                ListOfTasks.Add(new TaskItem()
                {
                    Index = _currentIndex++,
                    Weight = nextWeight,
                    LeftToProcess = nextWeight,
                    Appear = NextAppears,
                    Start = -1,
                    Finish = -1,
                    State = "wait",
                });
                NextAppears += Rnd.Next(FreqFrom, FreqTo + 1);
            }
        }

        protected abstract void ExecuteTask();

        protected abstract void UpdateWaitOption(int waitTime, int except, int timeAppear);

        public void MakeOneTick()
        {
            CurrentTick++;
            CurrentTime = CurrentTick * Tick;

            UpdateWaitOption(Convert.ToInt16(Tick*(_kind == "foregr" ? .8 : .2)), -1, CurrentTime);

            CreateTask();
            ExecuteTask();
        }
    }
}

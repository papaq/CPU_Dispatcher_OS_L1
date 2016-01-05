using System;
using System.Collections.Generic;

namespace CpuDispatcherOS
{
    public abstract class Dispatcher
    {
        // S Y S T E M
        public int SystemWaitsGenTime;
        protected int CurrentTime;

        // T A S K S
        private int _currentIndex;
        private int _nextAppears;

        // O P T I O N S
        public int CurrentTick = -1;

        public int Tick;
        public int WeightFrom;
        public int WeightTo;
        public int FreqFrom;
        public int FreqTo;

        private readonly string _kind;

        public List<TaskItem> ListOfTasks = new List<TaskItem>();
        public List<string> ListOfSequence = new List<string>(); 
        protected readonly Random Rnd = new Random();

        protected Dispatcher(string kind)
        {
            _nextAppears = Rnd.Next(FreqFrom, FreqTo);
            _kind = kind;
        }
        
        protected void CreateTask()
        {
            while (true)
            {
                if (_nextAppears > CurrentTime + Tick || Tick < 1)
                    return;
                int nextWeight = Rnd.Next(WeightFrom, WeightTo + 1);
                ListOfTasks.Add(new TaskItem()
                {
                    Index = _currentIndex++,
                    Weight = nextWeight,
                    LeftToProcess = nextWeight,
                    Appear = _nextAppears,
                    Start = -1,
                    Finish = -1,
                    Kind = _kind,
                    State = "wait",
                });
                _nextAppears += Rnd.Next(FreqFrom, FreqTo + 1);
            }
        }

        protected abstract void ExecuteTask();

        protected abstract void UpdateWaitOption(int waitTime, int except, int timeAppear);

        public void MakeOneTick()
        {
            ListOfSequence.Clear();
            ListOfSequence.Add(_kind);
            CurrentTick++;
            CurrentTime = CurrentTick * Tick;

            UpdateWaitOption(Convert.ToInt16(Tick*(_kind == "foregr" ? .8 : .2)), -1, CurrentTime);

            CreateTask();
            ExecuteTask();
        }
    }
}

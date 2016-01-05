using System;
using System.Collections.Generic;
using System.Linq;

namespace CpuDispatcherOS
{
    public class DispatcherFore : Dispatcher
    {
        private const double ExecutePart = 0.8;
        private int _currentTask = -1;
        private int _leftOfQuantum;

        // S Y S T E M
        public int SystemWaitsGenTime = 0;

        public DispatcherFore() : base("foregr")
        {
            _leftOfQuantum = Quantum;
        }
        
        protected override void ExecuteTask()
        {
            var tempTime = CurrentTime;

            while (true)
            {
                // Task was interrupted during last quantum
                TaskItem task;
                if (_leftOfQuantum < Quantum && -1 != _currentTask)
                {
                    task = ListOfTasks.Find(t => t.Index == _currentTask);

                    // Task needs less, than a quantum
                    if (task.LeftToProcess < _leftOfQuantum)
                    {
                        tempTime += task.LeftToProcess;

                        UpdateWaitOption(task.LeftToProcess, task.Index, tempTime);

                        task.LeftToProcess = 0;
                        task.State = "done";
                        task.Finish = tempTime;
                    }

                    // Task cannot be done during this quantum
                    else
                    {
                        tempTime += _leftOfQuantum;
                        task.LeftToProcess -= _leftOfQuantum;

                        UpdateWaitOption(Quantum, task.Index, tempTime);
                    }
                }
                _leftOfQuantum = Quantum;

                task = ListOfTasks.Find(t => t.State != "done" && t.Index > _currentTask && t.Appear < tempTime);

                // No task in a queue after index is found
                if (null == task)
                {
                    _currentTask = -1;
                    SystemWaitsGenTime ++;
                    continue;
                }

                // Task was not processed before
                if (task.State == "wait")
                {
                    task.Start = tempTime;
                    task.State = "process";
                }

                // More than a quantum is left
                if (tempTime < CurrentTime + Tick * ExecutePart - Quantum)
                {
                    // Task needs more time than a quantum
                    if (task.LeftToProcess > Quantum)
                    {
                        task.LeftToProcess -= Quantum;
                        tempTime += Quantum;

                        UpdateWaitOption(Quantum, task.Index, tempTime);
                    }

                    // Task can be done during next quantum
                    else
                    {
                        tempTime += task.LeftToProcess;

                        UpdateWaitOption(task.LeftToProcess, task.Index, tempTime);

                        task.LeftToProcess = 0;
                        task.State = "done";
                        task.Finish = tempTime;
                    }
                }

                // Less time than a quantum until the end of tick is left
                else
                {
                    _leftOfQuantum = CurrentTime + Convert.ToInt16(Tick*ExecutePart) - tempTime;

                    // Task needs even less time than left until end of tick
                    if (task.LeftToProcess < Quantum - _leftOfQuantum)
                    {
                        tempTime += task.LeftToProcess;

                        UpdateWaitOption(task.LeftToProcess, task.Index, tempTime);

                        task.LeftToProcess = 0;
                        task.State = "done";
                        task.Finish = tempTime;
                    }

                    // Task will be interrupted
                    else
                    {
                        task.LeftToProcess += _leftOfQuantum - Quantum;

                        UpdateWaitOption(Quantum - _leftOfQuantum, 
                            task.Index, CurrentTime + Convert.ToInt16(Tick * ExecutePart));

                        _currentTask = task.Index;
                        return;
                    }
                }

            }
        }

        protected override void UpdateWaitOption(int waitTime, int exceptIdx, int timeAppear)
        {
            foreach (var task in ListOfTasks.Where(t => t.State != "done" && t.Index != exceptIdx && t.Appear < timeAppear))
                task.Wait += waitTime;
        }
    }
}

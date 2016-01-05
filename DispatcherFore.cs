using System;
using System.Linq;

namespace CpuDispatcherOS
{
    public class DispatcherFore : Dispatcher
    {
        private const double ExecutePart = 0.8;
        private int _currentTask = -1;
        private int _leftOfQuantum;
        private int _quantum = 100;

        public DispatcherFore() : base("foregr")
        {
            _leftOfQuantum = _quantum;
        }
        
        protected override void ExecuteTask()
        {
            var tempTime = CurrentTime;

            while (tempTime < CurrentTime + Convert.ToInt16(Tick * ExecutePart))
            {
                // Task was interrupted during last quantum
                TaskItem task;
                if (_leftOfQuantum < _quantum && -1 != _currentTask)
                {
                    task = ListOfTasks.Find(t => t.Index == _currentTask);
                    ListOfSequence.Add(task.Index.ToString());

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

                        UpdateWaitOption(_quantum, task.Index, tempTime);
                    }
                }
                _leftOfQuantum = _quantum;

                task = ListOfTasks.Find(t => t.State != "done" && t.Index > _currentTask && t.Appear < tempTime);

                // No task in a queue after index is found
                if (null == task)
                {
                    _currentTask = -1;
                    SystemWaitsGenTime ++;
                    tempTime ++;
                    continue;
                }

                ListOfSequence.Add(task.Index.ToString());
                _currentTask = task.Index;

                // Task was not processed before
                if (task.State == "wait")
                {
                    task.Start = tempTime;
                    task.State = "process";
                    task.Wait = task.Start - task.Appear;
                }

                // More than a quantum is left
                if (tempTime < CurrentTime + Tick * ExecutePart - _quantum)
                {
                    // Task needs more time than a quantum
                    if (task.LeftToProcess > _quantum)
                    {
                        task.LeftToProcess -= _quantum;
                        tempTime += _quantum;

                        UpdateWaitOption(_quantum, task.Index, tempTime);
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

                    // Task needs even less time than left until end of tick
                    if (task.LeftToProcess < CurrentTime + Convert.ToInt16(Tick * ExecutePart) - tempTime)
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
                        _leftOfQuantum = _quantum + tempTime - CurrentTime - Convert.ToInt16(Tick * ExecutePart);

                        task.LeftToProcess += _leftOfQuantum - _quantum;

                        UpdateWaitOption(_quantum - _leftOfQuantum, 
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

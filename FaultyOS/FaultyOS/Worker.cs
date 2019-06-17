using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FaultyOS
{
  public class Worker
  {
    private int currentState;

    private bool completed = false;

    private Timer timer;

    public Worker()
    {

    }

    public void StartWork()
    {
      using (new SingleGlobalInstance(200000))
      {
        while (currentState <= 100)
        {
          Thread.Sleep(1000);
          Console.WriteLine(currentState++);
        }
        Console.WriteLine("Work completed, stopping");
        Console.ReadKey();
      }
    }
  }
}

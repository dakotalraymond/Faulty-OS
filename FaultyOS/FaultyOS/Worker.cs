using System;
using System.IO;
using System.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace FaultyOS
{
  public class Worker
  {
    private int currentState = 0;

    private volatile bool completed = false;

    private int partialState = 1000;

    public Worker()
    {

    }

    public void StartWork()
    {
      using (new SingleGlobalInstance(200000))
      {
        Thread processThread = new Thread(KeepProcessesAlive);
        processThread.Start();
        if (File.Exists("state.txt"))
        {
          string lastLine = File.ReadLines("state.txt").Last();
          var lines = lastLine.Split(' ');
          currentState = int.Parse(lines[0]);
          partialState = int.Parse(lines[1]);
        }
        else
        {
          using (FileStream fs = File.Create("state.txt"))
          {
            // Just create the file. 
          }
        }
        while (currentState <= 100)
        {
          Thread.Sleep(100);
          partialState -= 100;
          if (partialState <= 0)
          {
            Interlocked.Increment(ref currentState);
            Console.WriteLine(currentState);
            partialState = 1000;
          }
          using (StreamWriter tw = new StreamWriter("state.txt", true))
          {
            tw.WriteLine($"{currentState} {partialState}");
          }
        }

        completed = true;
        Console.WriteLine("Work completed, stopping");
      }
      Console.ReadKey();
    }

    private void KeepProcessesAlive()
    {
      while (!completed)
      {

        System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("FaultyOS");
        if (proc.Length < 5)
        {
          System.Diagnostics.Process.Start("FaultyOS.exe");
        }
      }
    }
  }
}

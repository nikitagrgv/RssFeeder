using System;

namespace RssFeeder.Model;

public class Timer
{
    private const uint MinPeriodInSeconds = 5;

    // Turn off AutoReset to manually control timer restarting
    private readonly System.Timers.Timer _timer = new System.Timers.Timer {AutoReset = false};

    public Timer(uint periodInSeconds)
    {
        PeriodInSeconds = periodInSeconds;
        _timer.Elapsed += (_, _) => OnTimerElapsed();
    }

    public uint PeriodInSeconds
    {
        get => (uint) _timer.Interval / 1000;
        set
        {
            if (value < MinPeriodInSeconds)
            {
                _timer.Interval = MinPeriodInSeconds * 1000;
            }
            else
            {
                _timer.Interval = value * 1000;
            }
            
            Restart();
        }
    }

    public event Action TimerElapsed;
    
    private void OnTimerElapsed()
    {
        TimerElapsed?.Invoke();
        
        // Restart the timer only after all is done in the event handlers
        Restart();
    }

    public void Restart()
    {
        Stop();
        Start();
    }

    public void Start()
    {
        _timer.Start();
    }
    
    public void Stop()
    {
        _timer.Stop();
    }
    
}
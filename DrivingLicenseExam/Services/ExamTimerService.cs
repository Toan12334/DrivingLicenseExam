using System;
using System.Windows.Threading;

public class ExamTimerService
{
    private readonly DispatcherTimer _timer;
    private TimeSpan _timeRemaining;

    public event Action<TimeSpan>? TimeUpdated;
    public event Action? TimeExpired;

    public ExamTimerService(int durationInMinutes)
    {
        _timeRemaining = TimeSpan.FromMinutes(durationInMinutes);

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Tick += OnTick;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (_timeRemaining.TotalSeconds <= 0)
        {
            _timer.Stop();
            TimeExpired?.Invoke();
            return;
        }

        _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
        TimeUpdated?.Invoke(_timeRemaining);
    }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();

    public void Reset(int durationInMinutes)
    {
        Stop();
        _timeRemaining = TimeSpan.FromMinutes(durationInMinutes);
        TimeUpdated?.Invoke(_timeRemaining);
    }
}

using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class TimeService : ITimeService
{
    public Task WaitForOperation(int milliseconds)
    {
        var operation = Task.Delay(milliseconds);
        return operation;
    }

    public Timer StartTimer(TimerCallback timerCallback)
    {
        var startNow = 0;
        var interval = 5000;

        return new Timer(timerCallback, null, 4000, interval);
    }
}

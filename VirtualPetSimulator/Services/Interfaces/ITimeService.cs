namespace VirtualPetSimulator.Services.Interfaces;

public interface ITimeService
{
    Task WaitForOperation(int milliseconds);
    Task WaitForOperation(int milliseconds, CancellationToken token);
    Timer StartTimer(TimerCallback timerCallback);
}
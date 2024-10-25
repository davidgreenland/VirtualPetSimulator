namespace VirtualPetSimulator.Services.Interfaces;

public interface ITimeService
{
    Task WaitForOperation(int milliseconds);

    Timer StartTimer(TimerCallback timerCallback);
}
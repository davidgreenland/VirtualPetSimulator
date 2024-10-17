using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class TimeService : ITimeService
{
    public Task Delay(int milliSeconds)
    {
        return Task.Delay(milliSeconds);
    }
}

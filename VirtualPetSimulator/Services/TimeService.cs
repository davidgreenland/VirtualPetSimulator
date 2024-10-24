using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class TimeService : ITimeService
{
    public Task WaitForOperation(int milliSeconds)
    {
        return Task.Delay(milliSeconds);
    }
}

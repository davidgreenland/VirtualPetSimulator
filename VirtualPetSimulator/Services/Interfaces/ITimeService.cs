namespace VirtualPetSimulator.Services.Interfaces;

public interface ITimeService
{
    Task WaitForOperation(int milliseconds);
}
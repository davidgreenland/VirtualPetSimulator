namespace VirtualPetSimulator.Services.Interfaces;

public interface IOperationService
{ 
    Task RunOperation(int milliSeconds, string message);
}

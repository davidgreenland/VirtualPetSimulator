namespace VirtualPetSimulator.Services.Interfaces
{
    public interface IPetAction
    {
        Task<int> Execute();
    }
}
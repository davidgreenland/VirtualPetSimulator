namespace VirtualPetSimulator.Helpers.Interfaces;

public interface IValidator
{
    void ValidateNonNegative(int input, string paramName);
}
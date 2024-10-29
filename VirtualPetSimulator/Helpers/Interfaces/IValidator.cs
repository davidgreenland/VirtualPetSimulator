namespace VirtualPetSimulator.Helpers.Interfaces;

public interface IValidator
{
    bool Validate(int input, string paramName);
}
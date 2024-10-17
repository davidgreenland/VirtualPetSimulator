using NUnit.Framework;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Tests.Helpers;

public class ValidatorTests
{
    private Validator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new Validator();
    }

    [TestCase(-1)]
    [TestCase(-10)]
    [TestCase(-109)]
    [TestCase(-34567)]
    public void ValidateNonNegative_WhenGivenNegativeValue_ThrowsException(int negativeParam)
    {
        var paramName = "value";

        Assert.Throws<ArgumentOutOfRangeException>(() => _validator.ValidateNonNegative(negativeParam, paramName));
    }

    [TestCase(1)]
    [TestCase(4)]
    [TestCase(9)]
    [TestCase(490)]
    public void ValidateNonNegative_WhenGivenPositive_ShouldNotThrow(int positiveParam)
    {
        var paramName = "value";

        _validator.ValidateNonNegative(positiveParam, paramName);
    }
}

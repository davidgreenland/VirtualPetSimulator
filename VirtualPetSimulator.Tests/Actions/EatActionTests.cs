using NUnit.Framework;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;
using Moq;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Tests.Actions;

public class EatActionTests
{
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private Mock<ITimeService> _timeServiceMock;
    private EatAction _eatAction;
    private const int DEFAULT_FOOD_VALUE = 1;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _timeServiceMock = new Mock<ITimeService>();

        _testPet.Setup(pet => pet.Name).Returns("Simon");
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.DEFAULT);
        _validatorMock.Setup(x => x.Validate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
    }

    [Test]
    public async Task Execute_WhenNotHungry_NoFoodIsEaten()
    {
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.MIN);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);

        var portionsEaten = _eatAction.Execute();

        Assert.That(await portionsEaten, Is.EqualTo(0));
    }

    [Test]
    public async Task Execute_WhenNotHungry_ChangeHungerIsNotCalled()
    {
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.MIN);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);

        await _eatAction.Execute();

        _testPet.Verify(x => x.ChangeHunger(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Execute_WhenPetHasHungerAndNoFoodValueProvided_CallsChangeHungerWithDefault()
    {
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.MEDIUM);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);

        var portionsEaten = await _eatAction.Execute();

        _testPet.Verify(x => x.ChangeHunger(It.Is<int>(val => val == -DEFAULT_FOOD_VALUE)), Times.Once());
    }



    [TestCase(6, 1, 1)]
    [TestCase(3, 2, 2)]
    [TestCase(10, 4, 4)]
    [TestCase(6, 7, 6)]
    [TestCase(3, 11, 3)]
    [TestCase(10, 23, 10)]
    [TestCase(0, 23, 0)]
    public async Task Execute_WhenFeedAmountProvided_ReturnsCorrectPortionsEaten(int hunger, int foodValue, int expected)
    {
        hunger++;
        _testPet.Setup(x => x.Hunger)
            .Returns(() => --hunger);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, foodValue);

        var portionsEaten = _eatAction.Execute();

        Assert.That(await portionsEaten, Is.EqualTo(expected));
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(4)]
    [TestCase(7)]
    [TestCase(11)]
    [TestCase(23)]
    public async Task Execute_WhenFeedAmountProvided_CallsChangeHungerCorrectly(int foodValue)
    {
        var hunger = AttributeValue.DEFAULT + 1;
        _testPet.Setup(x => x.Hunger)
            .Returns(() => --hunger);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, foodValue);
        var expected = Math.Min(AttributeValue.DEFAULT, foodValue);

        await _eatAction.Execute();

        _testPet.Verify(x => x.ChangeHunger(It.Is<int>(val => val == -1)), Times.Exactly(expected));
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(4)]
    [TestCase(7)]
    [TestCase(11)]
    [TestCase(23)]
    public async Task Execute_WhenFeedAmountProvided_CallsUserCommsWithCorrectValue(int foodValue)
    {
        var hunger = AttributeValue.DEFAULT + 1;
        _testPet.Setup(x => x.Hunger)
            .Returns(() => --hunger);

        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, foodValue);
        var expectedAmount = Math.Min(foodValue, AttributeValue.DEFAULT);

        await _eatAction.Execute();

        _timeServiceMock.Verify(x => x.WaitForOperation(It.Is<int>(val => val == AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS), It.IsAny<CancellationToken>()), Times.Exactly(expectedAmount));
    }

    [TestCase(3)]
    [TestCase(1)]
    [TestCase(23)]
    [TestCase(-4)]
    [TestCase(108)]
    public async Task Execute_WhenFeedAmountProvided_CallsValidatorWithValue(int foodValue)
    {
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, foodValue);

        await _eatAction.Execute();

        _validatorMock.Verify(x => x.Validate(It.Is<int>(val => val == foodValue), It.IsAny<string>()), Times.Once());
    }

    [TestCase(-1)]
    [TestCase(-3)]
    [TestCase(-10000)]
    public async Task Execute_WhenFeedAmountNegative_DoesNotCallChangeHunger(int foodValue)
    {
        _validatorMock.Setup(x => x.Validate(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, foodValue);

        await _eatAction.Execute();

        _testPet.Verify(x => x.ChangeHunger(It.IsAny<int>()), Times.Never());
    }
}

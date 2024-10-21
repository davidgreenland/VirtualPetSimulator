﻿using NUnit.Framework;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Helpers.Interfaces;
using Moq;

namespace VirtualPetSimulator.Tests.Services;

public class EatActionTests
{
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private EatAction _eatAction;
    private const int DEFAULT_FOOD_VALUE = 1;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();

        _testPet.Setup(pet => pet.Name).Returns("Simon");
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.DEFAULT);
        _userCommunicationMock.Setup(mock => mock.RunOperation(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);
    }

    [Test]
    public async Task Execute_WhenNotHungry_NoFoodIsEaten()
    {
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.MIN);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);

        var portionsEaten = _eatAction.Execute();

        Assert.That(await portionsEaten, Is.EqualTo(0));
    }

    [Test]
    public async Task Execute_WhenNotHungry_ChangeHungerIsNotCalled()
    {
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.MIN);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);

        await _eatAction.Execute();

        _testPet.Verify(x => x.ChangeHunger(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Execute_WhenPetHasHunger_CallsChangeHungerWithDefault()
    {
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.MEDIUM);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);

        var portionsEaten = _eatAction.Execute();

        Assert.That(await portionsEaten, Is.EqualTo(DEFAULT_FOOD_VALUE));
        _testPet.Verify(x => x.ChangeHunger(It.Is<int>(val => val == -DEFAULT_FOOD_VALUE)), Times.Once());
    }

    [TestCase(2)]
    [TestCase(4)]
    [TestCase(6)]
    public async Task Execute_WhenFoodAmountHasValue_MethodCalledByMoreThanDefaultIncrement(int foodAmount)
    {
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, foodAmount);

        var portionsEaten = _eatAction.Execute();

        Assert.That(await portionsEaten, Is.EqualTo(foodAmount));
        _testPet.Verify(x => x.ChangeHunger(It.Is<int>(val => val == -foodAmount)), Times.Once());
    }

    [TestCase(6, 1, 1)]
    [TestCase(3, 2, 2)]
    [TestCase(10, 4, 4)]
    [TestCase(6, 7, 6)]
    [TestCase(3, 11, 3)]
    [TestCase(10, 23, 10)]
    public async Task Execute_WhenFeedingPet_ReturnsCorrectPortionsEaten(int hunger, int foodValue, int expected)
    {
        _testPet.Setup(x => x.Hunger).Returns(hunger);
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, foodValue);

        var portionsEaten = _eatAction.Execute();

        Assert.That(await portionsEaten, Is.EqualTo(expected));
    }

    [TestCase(1, -1)]
    [TestCase(2, -2)]
    [TestCase(4, -4)]
    [TestCase(7, -6)]
    [TestCase(11, -6)]
    [TestCase(23, -6)]
    public async Task Execute_WhenFeedingPet_CallsChangeHungerCorrectly(int foodValue, int expected)
    {
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, foodValue);

        await _eatAction.Execute();

        _testPet.Verify(x => x.ChangeHunger(It.Is<int>(val => val == expected)), Times.Once());
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(4, 4)]
    [TestCase(7, 6)]
    [TestCase(11, 6)]
    [TestCase(23, 6)]
    public async Task Execute_WhenFeedingDefaultPet_CallsUserCommsWithCorrectValue(int foodValue, int expected)
    {
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, foodValue);

        await _eatAction.Execute();

        _userCommunicationMock.Verify(x => x.RunOperation(It.Is<int>(val => val == expected), It.IsAny<string>()), Times.Once());
    }

    [TestCase(3)]
    [TestCase(-4)]
    [TestCase(108)]
    public async Task Execute_WhenCalled_CallsValidatorWithValue(int foodValue)
    {
        _eatAction = new EatAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, foodValue);

        await _eatAction.Execute();

        _validatorMock.Verify(x => x.IsNonNegative(It.Is<int>(val => val == foodValue), It.IsAny<string>()), Times.Once());
    }
}
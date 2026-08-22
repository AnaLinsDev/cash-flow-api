using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Validators.Tests.Expenses.Register;
public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Error_Title_Empty( string title)
    {
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED);
    }

    [Fact]
    public void Error_Date_Future()
    {
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE);
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)1000;

        var result = validator.Validate(request);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_Greater_Than_Zero(decimal amount)
    {
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;

        var result = validator.Validate(request);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();

        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
    }
}

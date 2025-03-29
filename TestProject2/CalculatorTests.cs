namespace TestProject2;

public class CalculatorTests
{
 
    [Fact]
    //UnitOfWork_StateUnderTest_ExpectedBehavior
    //MethodYouTest_Condition_Should_ExpectedBehaviour
    public void Add_ValidatorIsPassed_ReturnsComputedValues()
    {
        //Arrange
        //preparing some test data
        // preparing the class you want to test
        var sut = new Calculator();
        //Act
        //executing the exact method we test
        var res = sut.Add(1, 2);
        //Assert
        //we validate the result
        Assert.Equal(3, res);

    }

    
    
}
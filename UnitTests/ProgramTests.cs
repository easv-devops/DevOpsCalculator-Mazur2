using DevOpsCalculator;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests;

[Collection("Calculator")]
public class ProgramTests
{
    private CalculationService _calculationService;
    private AppDbContext _dbContext;

    public ProgramTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "Test_Database");
        _dbContext = new AppDbContext(optionsBuilder.Options);
        _calculationService = new CalculationService(_dbContext, new Calculator());
    }
    
    [Fact]
    public void TestShowHistory()
    {
        // Arrange
        var record1 = new CalculationRecord(1,2,"A",3);
        var record2 = new CalculationRecord(2,2,"A",4);
        _dbContext.CalculationRecords.Add(record1);
        _dbContext.CalculationRecords.Add(record2);
        _dbContext.SaveChanges();

        // Act
        var history = _calculationService.ShowHistory();

        // Assert
        Assert.Contains("ID: 1", history);
        Assert.Contains("ID: 2", history);
        // Assert other parts of the output as needed...
    }
}
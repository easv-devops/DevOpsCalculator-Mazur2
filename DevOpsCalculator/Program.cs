using DevOpsCalculator;
using Microsoft.EntityFrameworkCore;

var dbContext = new AppDbContext();
dbContext.Database.EnsureCreated();
var calculator = new Calculator();

var input = WriteWelcomeScreen();
while (input != "Q")
{
    double n1, n2, result;
    // Get the numbers
    Console.WriteLine("Enter the first number:");
    n1 = double.Parse(Console.ReadLine() ?? string.Empty);
    Console.WriteLine("Enter the second number:");
    n2 = double.Parse(Console.ReadLine() ?? string.Empty);

    switch (input.ToUpper())
    {
        case "A":
            result = calculator.Add(n1, n2);
            dbContext.CalculationRecords.Add(new CalculationRecord(n1, n2, "+", result));
            Console.WriteLine($"Result: {result}");
            break;
        case "S":
            result = calculator.Subtract(n1, n2);
            dbContext.CalculationRecords.Add(new CalculationRecord(n1, n2, "-", result));
            Console.WriteLine($"Result: {result}");
            break;
        case "M":
            result = calculator.Multiply(n1, n2);
            dbContext.CalculationRecords.Add(new CalculationRecord(n1, n2, "*", result));
            Console.WriteLine($"Result: {result}");
            break;
        case "D":
            result = calculator.Divide(n1, n2);
            dbContext.CalculationRecords.Add(new CalculationRecord(n1, n2, "/", result));
            Console.WriteLine($"Result: {result}");
            break;
        case "H":
            ShowHistory(dbContext);
            break;
        default:
            Console.WriteLine("Invalid input. Please try again.");
            break;
    }
    dbContext.SaveChanges();

    input = WriteWelcomeScreen();
}

string? WriteWelcomeScreen()
{
    Console.WriteLine(
        """
        --------------------------------------
        | Welcome to the console calculator! |
        --------------------------------------

        Select one of the options by writing character:
            A - Addition
            S - Substraction
            M - Multiply
            D - Divide

            H - History
            Q - Quit
        Your input:
        """);

    return Console.ReadLine();
}

void ShowHistory(AppDbContext db)
{
    foreach (var record in db.CalculationRecords)
    {
        Console.WriteLine($"ID: {record.Id}, Operator1: {record.Operand1}, Operator2: {record.Operand2}, Operator: {record.Operator}, Result: {record.Result}");
    }
}


#region Data

public class AppDbContext : DbContext
{
    public DbSet<CalculationRecord> CalculationRecords { get; set; } = default!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=calc;Username=root;Password=password");
    }
}

public class CalculationRecord
{
    public int Id { get; set; }
    public double Operand1 { get; set; }
    public double Operand2 { get; set; }
    public string Operator { get; set; }
    public double Result { get; set; }

    public CalculationRecord() { }
    public CalculationRecord(double n1, double n2, string oper, double result)
    {
        this.Operand1 = n1;
        this.Operand2 = n2;
        this.Operator = oper;
        this.Result = result;
    }
}

#endregion
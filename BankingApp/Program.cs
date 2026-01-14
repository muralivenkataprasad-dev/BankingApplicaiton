using BankingApp.Services;

namespace BankingApp;

class Program
{
    static async Task Main(string[] args)
    {
        // DB connection string
        var connectionString = "Host=localhost;Port=5432;Database=bankingdb;Username=postgres;Password=sql123";
        
        var databaseService = new DatabaseService(connectionString);
        var bankingService = new BankingService(databaseService);

        try
        {
            await databaseService.InitializeDatabase();
            await RunBankingApp(bankingService);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            Console.WriteLine("\nPlease make sure PostgreSQL is running and connection settings are correct.");
        }
    }

    static async Task RunBankingApp(BankingService bankingService)
    {
        Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
        Console.WriteLine();
        
        bool running = true;
        while (running)
        {
            DisplayMenu();
            Console.WriteLine();
            
            var input = Console.ReadLine()?.Trim().ToUpper();
            Console.WriteLine();
            
            switch (input)
            {
                case "D":
                    await HandleDeposit(bankingService);
                    break;
                case "W":
                    await HandleWithdraw(bankingService);
                    break;
                case "P":
                    await HandlePrintStatement(bankingService);
                    break;
                case "Q":
                    HandleQuit();
                    running = false;
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine();
                    break;
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("[D]eposit");
        Console.WriteLine("[W]ithdraw");
        Console.WriteLine("[P]rint statement");
        Console.WriteLine("[Q]uit");
    }

    static async Task HandleDeposit(BankingService bankingService)
    {
        Console.WriteLine("Please enter the amount to deposit:");
        
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            try
            {
                await bankingService.Deposit(amount);
                Console.WriteLine($"Thank you. ${amount:F2} has been deposited to your account.");
                Console.WriteLine();
                Console.WriteLine("Is there anything else you'd like to do?");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Invalid amount. Please enter a positive number.");
            Console.WriteLine();
        }
    }

    static async Task HandleWithdraw(BankingService bankingService)
    {
        Console.WriteLine("Please enter the amount to withdraw:");
        
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            try
            {
                await bankingService.Withdraw(amount);
                Console.WriteLine($"Thank you. ${amount:F2} has been withdrawn.");
                Console.WriteLine();
                Console.WriteLine("Is there anything else you'd like to do?");
                Console.WriteLine();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Invalid amount. Please enter a positive number.");
            Console.WriteLine();
        }
    }

    static async Task HandlePrintStatement(BankingService bankingService)
    {
        try
        {
            var transactions = await bankingService.GetStatement();
            
            if (transactions.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No transactions found.");
            }
            else
            {
                Console.WriteLine("Date                  | Amount  | Balance");
                foreach (var transaction in transactions)
                {
                    var date = transaction.TransactionDate.ToString("d MMM yyyy h:mm:sstt");
                    var amount = transaction.Amount.ToString("F2");
                    var balance = transaction.Balance.ToString("F2");
                    
                    Console.WriteLine($"{date,-22}| {amount,8}| {balance,8}");
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Is there anything else you'd like to do?");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine();
        }
    }

    static void HandleQuit()
    {
        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
        Console.WriteLine("Have a nice day!");
    }
}

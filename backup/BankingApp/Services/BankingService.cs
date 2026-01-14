using BankingApp.Models;

namespace BankingApp.Services;

public class BankingService
{
    private readonly DatabaseService _databaseService;
    private const int DEFAULT_ACCOUNT_ID = 1;

    public BankingService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<decimal> Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive");
        }

        var account = await _databaseService.GetAccount(DEFAULT_ACCOUNT_ID);
        var newBalance = account.Balance + amount;
        
        await _databaseService.AddTransaction(DEFAULT_ACCOUNT_ID, amount, newBalance);
        
        return amount;
    }

    public async Task<decimal> Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive");
        }

        var account = await _databaseService.GetAccount(DEFAULT_ACCOUNT_ID);
        var newBalance = account.Balance - amount;
        
        if (newBalance < 0)
        {
            throw new InvalidOperationException("Insufficient funds");
        }

        await _databaseService.AddTransaction(DEFAULT_ACCOUNT_ID, -amount, newBalance);
        
        return amount;
    }

    public async Task<List<Transaction>> GetStatement()
    {
        return await _databaseService.GetTransactions(DEFAULT_ACCOUNT_ID);
    }

    public async Task<decimal> GetBalance()
    {
        var account = await _databaseService.GetAccount(DEFAULT_ACCOUNT_ID);
        return account.Balance;
    }
}

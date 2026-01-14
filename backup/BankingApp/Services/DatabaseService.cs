using Npgsql;
using BankingApp.Models;

namespace BankingApp.Services;

public class DatabaseService
{
    private readonly string _connectionString;
    private const int DEFAULT_ACCOUNT_ID = 1;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeDatabase()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        //To Create accounts table
        var createAccountsTable = @"
            CREATE TABLE IF NOT EXISTS accounts (
                id SERIAL PRIMARY KEY,
                balance DECIMAL(18, 2) NOT NULL DEFAULT 0,
                created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";
        
        using (var cmd = new NpgsqlCommand(createAccountsTable, connection))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        //To Create transactions table
        var createTransactionsTable = @"
            CREATE TABLE IF NOT EXISTS transactions (
                id SERIAL PRIMARY KEY,
                account_id INTEGER NOT NULL,
                transaction_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                amount DECIMAL(18, 2) NOT NULL,
                balance DECIMAL(18, 2) NOT NULL,
                FOREIGN KEY (account_id) REFERENCES accounts(id)
            )";
        
        using (var cmd = new NpgsqlCommand(createTransactionsTable, connection))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        //To Check if default account exists, if not create it
        var checkAccount = "SELECT COUNT(*) FROM accounts WHERE id = @id";
        using (var cmd = new NpgsqlCommand(checkAccount, connection))
        {
            cmd.Parameters.AddWithValue("id", DEFAULT_ACCOUNT_ID);
            var count = (long)(await cmd.ExecuteScalarAsync() ?? 0);
            
            if (count == 0)
            {
                var insertAccount = "INSERT INTO accounts (balance) VALUES (0.00)";
                using var insertCmd = new NpgsqlCommand(insertAccount, connection);
                await insertCmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Account> GetAccount(int accountId = DEFAULT_ACCOUNT_ID)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "SELECT id, balance, created_at FROM accounts WHERE id = @id";
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("id", accountId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Account
            {
                Id = reader.GetInt32(0),
                Balance = reader.GetDecimal(1),
                CreatedAt = reader.GetDateTime(2)
            };
        }

        throw new Exception("Account not found");
    }

    public async Task AddTransaction(int accountId, decimal amount, decimal newBalance)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            // Insert into transaction table
            var insertTransaction = @"
                INSERT INTO transactions (account_id, transaction_date, amount, balance) 
                VALUES (@accountId, @date, @amount, @balance)";
            
            using (var cmd = new NpgsqlCommand(insertTransaction, connection, transaction))
            {
                cmd.Parameters.AddWithValue("accountId", accountId);
                cmd.Parameters.AddWithValue("date", DateTime.Now);
                cmd.Parameters.AddWithValue("amount", amount);
                cmd.Parameters.AddWithValue("balance", newBalance);
                await cmd.ExecuteNonQueryAsync();
            }

            // Update account balance into the table
            var updateAccount = "UPDATE accounts SET balance = @balance WHERE id = @id";
            using (var cmd = new NpgsqlCommand(updateAccount, connection, transaction))
            {
                cmd.Parameters.AddWithValue("balance", newBalance);
                cmd.Parameters.AddWithValue("id", accountId);
                await cmd.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Transaction>> GetTransactions(int accountId = DEFAULT_ACCOUNT_ID)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT id, account_id, transaction_date, amount, balance 
            FROM transactions 
            WHERE account_id = @accountId 
            ORDER BY transaction_date ASC";
        
        using var cmd = new NpgsqlCommand(query, connection);
        cmd.Parameters.AddWithValue("accountId", accountId);

        var transactions = new List<Transaction>();
        using var reader = await cmd.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            transactions.Add(new Transaction
            {
                Id = reader.GetInt32(0),
                AccountId = reader.GetInt32(1),
                TransactionDate = reader.GetDateTime(2),
                Amount = reader.GetDecimal(3),
                Balance = reader.GetDecimal(4)
            });
        }

        return transactions;
    }
}

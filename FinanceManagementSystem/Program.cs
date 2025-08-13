using System;
using System.Collections.Generic;

// Define the record type to represent a transaction
public record Transaction(
    int Id,
    DateTime Date,
    decimal Amount,
    string Category
);

//  Define the interface for processing transactions
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Implement concrete transaction processors
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Processing {transaction.Amount:C} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Processing {transaction.Amount:C} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Processing {transaction.Amount:C} for {transaction.Category}");
    }
}

// d. Define base Account class
public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

// Define sealed SavingsAccount class
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance) { }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds.");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New Balance: {Balance:C}");
        }
    }
}

// FinanceApp to integrate everything
public class FinanceApp
{
    private List<Transaction> _transactions = new();

    public void Run()
    {
        // i. Instantiate SavingsAccount
        var account = new SavingsAccount("ACC-12345", 1000m);

        // ii. Create sample transactions
        var t1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 300m, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 200m, "Entertainment");

        // iii. Process transactions
        ITransactionProcessor mobileProcessor = new MobileMoneyProcessor();
        ITransactionProcessor bankProcessor = new BankTransferProcessor();
        ITransactionProcessor cryptoProcessor = new CryptoWalletProcessor();

        mobileProcessor.Process(t1);
        bankProcessor.Process(t2);
        cryptoProcessor.Process(t3);

        // iv. Apply transactions to account
        account.ApplyTransaction(t1);
        account.ApplyTransaction(t2);
        account.ApplyTransaction(t3);

        // v. Add all transactions to list
        _transactions.Add(t1);
        _transactions.Add(t2);
        _transactions.Add(t3);

        // Display all transactions
        Console.WriteLine("\n--- Transaction History ---");
        foreach (var tx in _transactions)
        {
            Console.WriteLine($"ID: {tx.Id}, Date: {tx.Date}, Amount: {tx.Amount:C}, Category: {tx.Category}");
        }
    }
}

// Main entry point
public class Program
{
    public static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}

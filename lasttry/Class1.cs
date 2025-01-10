using System;

namespace BankSystem
{
    // Interface for common operations
    public interface IBankOperations
    {
        void Deposit(double amount);
        void Withdraw(double amount);
        void CheckBalance();
    }

    // Base Class for Bank Account (Encapsulation + Common Properties)
    public abstract class BankAccount : IBankOperations
    {
        private string accountNumber;
        private string accountHolderName;
        protected double balance; // Protected to allow inheritance classes to access

        // Static member to track total accounts
        private static int totalAccounts = 0;

        // Constructor
        public BankAccount(string accountNumber, string accountHolderName, double initialBalance)
        {
            this.accountNumber = accountNumber;
            this.accountHolderName = accountHolderName;
            this.balance = initialBalance >= 0 ? initialBalance : throw new ArgumentException("Initial balance cannot be negative.");
            totalAccounts++; // Increment total accounts
        }

        // Read-only properties for account details
        public string AccountNumber => accountNumber;
        public string AccountHolderName => accountHolderName;

        // Static property to get total accounts
        public static int TotalAccounts => totalAccounts;

        // Implementation of Deposit
        public virtual void Deposit(double amount)
        {
            if (amount > 0)
            {
                balance += amount;
                Console.WriteLine($"₹{amount} deposited successfully. Current balance: ₹{balance}");
            }
            else
            {
                Console.WriteLine("Deposit amount must be greater than zero.");
            }
        }

        // Implementation of Withdraw (Base version)
        public virtual void Withdraw(double amount)
        {
            if (amount > 0 && amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"₹{amount} withdrawn successfully. Current balance: ₹{balance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance or invalid amount.");
            }
        }

        // Check Balance
        public void CheckBalance()
        {
            Console.WriteLine($"Account Balance: ₹{balance}");
        }
    }

    // Derived Class for Savings Account
    public class SavingsAccount : BankAccount
    {
        private double interestRate;

        // Constructor
        public SavingsAccount(string accountNumber, string accountHolderName, double initialBalance, double interestRate)
            : base(accountNumber, accountHolderName, initialBalance)
        {
            this.interestRate = interestRate;
        }

        // Method to calculate and add interest
        public void AddInterest()
        {
            double interest = balance * (interestRate / 100);
            balance += interest;
            Console.WriteLine($"Interest of ₹{interest} added. Current balance: ₹{balance}");
        }

        // Overriding Withdraw method to simulate savings account limits
        public override void Withdraw(double amount)
        {
            if (amount > balance)
            {
                Console.WriteLine("Savings account cannot be overdrawn.");
            }
            else
            {
                base.Withdraw(amount); // Use base class implementation
            }
        }
    }

    // Derived Class for Current Account
    public class CurrentAccount : BankAccount
    {
        private double overdraftLimit;

        // Constructor
        public CurrentAccount(string accountNumber, string accountHolderName, double initialBalance, double overdraftLimit)
            : base(accountNumber, accountHolderName, initialBalance)
        {
            this.overdraftLimit = overdraftLimit;
        }

        // Overriding Withdraw method to allow overdraft
        public override void Withdraw(double amount)
        {
            if (amount > 0 && (balance - amount >= -overdraftLimit))
            {
                balance -= amount;
                Console.WriteLine($"₹{amount} withdrawn successfully. Current balance: ₹{balance}");
            }
            else
            {
                Console.WriteLine("Withdrawal exceeds overdraft limit or invalid amount.");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            // Creating accounts
            Console.WriteLine("Creating a Savings Account...");
            SavingsAccount savings = new SavingsAccount("SA123", "Alice", 5000, 3.5);
            savings.CheckBalance();

            Console.WriteLine("\nPerforming Savings Account Operations...");
            savings.Deposit(2000);
            savings.Withdraw(1000);
            savings.AddInterest();

            Console.WriteLine("\nCreating a Current Account...");
            CurrentAccount current = new CurrentAccount("CA456", "Bob", 10000, 5000);
            current.CheckBalance();

            Console.WriteLine("\nPerforming Current Account Operations...");
            current.Deposit(5000);
            current.Withdraw(20000);
            current.Withdraw(5000);

            // Total Accounts
            Console.WriteLine($"\nTotal Accounts Created: {BankAccount.TotalAccounts}");
        }
    }
}


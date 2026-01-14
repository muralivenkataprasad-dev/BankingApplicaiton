# AwesomeGIC Bank - Banking Application

A simple banking system built with .NET and PostgreSQL that handles deposits, withdrawals, and account statements.

# Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL 12 or later

# How to Setup

# 1. Install PostgreSQL

Download and install PostgreSQL 

# 2. Create Database

Connect to PostgreSQL and create the database

Then run the schema from `database_schema.sql`

# 3. Update Connection String

Edit the connection string in `Program.cs` by replacing localhost, bankingdb, postgres and password.

# 4. Build and Run

# Database Schema
# accounts
- `id` - Primary key
- `balance` - Current account balance
- `created_at` - Account creation timestamp

# transactions
- `id` - Primary key
- `account_id` - Foreign key to accounts
- `transaction_date` - Transaction timestamp
- `amount` - Transaction amount (positive for deposit, negative for withdrawal)
- `balance` - Balance after transaction

-- Create database (run this first if database doesn't exist)
-- CREATE DATABASE bankingdb;

-- Connect to bankingdb and run the following:

-- Create accounts table
CREATE TABLE IF NOT EXISTS accounts (
    id SERIAL PRIMARY KEY,
    balance DECIMAL(18, 2) NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create transactions table
CREATE TABLE IF NOT EXISTS transactions (
    id SERIAL PRIMARY KEY,
    account_id INTEGER NOT NULL,
    transaction_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    amount DECIMAL(18, 2) NOT NULL,
    balance DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (account_id) REFERENCES accounts(id)
);

-- Insert a default account (optional)
INSERT INTO accounts (balance) VALUES (0.00);

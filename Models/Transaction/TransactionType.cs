namespace Banking.Accounts.Models.Transaction;

/// <summary>
/// Тип финансовой операции.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Пополнение.
    /// </summary>
    Deposit = 0,

    /// <summary>
    /// Снятие.
    /// </summary>
    Withdraw = 1,
}
namespace Banking.Accounts.Models.Account;

/// <summary>
/// Статус аккаунта.
/// </summary>
public enum Status
{
    /// <summary>
    /// Активный.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Замороженный.
    /// </summary>
    Frozen = 1,
}
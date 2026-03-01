using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Abstractions.Logic.Account;

/// <summary>
/// Интерфейс фабрики для создания экземпляров банковского счёта.
/// </summary
public interface IAccountFactory
{
    /// <summary>
    /// Создает экземпляр банковского счёта из существующих данных.
    /// Используется при загрузке данных из хранилища.
    /// </summary>
    /// <param name="id">
    /// Идентификатор счёта.
    /// </param>
    /// <param name="customerId">
    /// Идентификатор владельца счёта.
    /// </param>
    /// <param name="balance">
    /// Текущий баланс.
    /// </param>
    /// <param name="currency">
    /// Валюта счёта.
    /// </param>
    /// <param name="status">
    /// Статус счёта.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные объекты равны null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если переданы некорректные значения перечислений.
    /// </exception>
    /// <returns>
    /// Восстановленный экземпляр счёта.
    /// </returns>
    IAccount Create(
        AccountId id,
        CustomerId customerId,
        Balance balance,
        Currency currency,
        Status status);

    /// <summary>
    /// Создает новый экземпляр банковского счёта с начальными параметрами.
    /// </summary>
    /// <param name="customerId">
    /// Идентификатор владельца счёта.
    /// </param>
    /// <param name="currency">
    /// Валюта счёта.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если customerId равен null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если передано некорректное значение валюты.
    /// </exception>
    /// <returns>
    /// Новый экземпляр счёта.
    /// </returns>
    IAccount CreateNew(CustomerId customerId, Currency currency);
}
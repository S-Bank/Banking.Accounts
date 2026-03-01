namespace Banking.Accounts.Models.Account;

/// <summary>
/// Денежная сумма для проведения операций.
/// </summary>
public record Amount
{
    /// <summary>
    /// Значение в копейках.
    /// </summary>
    public long Units { get; private set; }

    /// <summary>
    /// Валюта.
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    /// Создает экземпляр суммы.
    /// </summary>
    /// <param name="units">
    ///  Значение в формате копеек.
    /// </param>
    /// <param name="currency">
    /// Валюта.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если сумма отрицательная.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если значение валюты выходит за значение enum.
    /// </exception>
    public Amount(long units, Currency currency)
    {
        if (units <= 0)
        {
            throw new ArgumentException("Сумма операции должна быть положительной.", nameof(units));
        }

        if (!Enum.IsDefined(typeof(Currency), currency))
        {
            throw new ArgumentOutOfRangeException(nameof(currency));
        }

        Units = units;
        Currency = currency;
    }

    /// <summary>
    /// Создает экземпляр суммы.
    /// </summary>
    /// <param name="units">
    /// Значение в десятичном формате.
    /// </param>
    /// <param name="currency">
    /// Валюта.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если сумма отрицательная или имеет более 2 знаков после запятой.
    /// </exception>
    public Amount(decimal units, Currency currency)
    {
        if (units <= 0)
        {
            throw new ArgumentException("Сумма операции должна быть положительной.", nameof(units));
        }

        if ((units * 100) % 1 != 0)
        {
            throw new ArgumentException("Сумма не может иметь более 2 знаков после запятой.", nameof(units));
        }

        Units = (long)(units * 100);
        Currency = currency;
    }
}
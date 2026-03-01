namespace Banking.Accounts.Models.Account;

/// <summary>
/// Баланс счета.
/// </summary>
public record Balance
{
    /// <summary>
    /// Значение баланса в копейках.
    /// </summary>
    public long Value { get; init; }

    /// <summary>
    /// Создает новый объект баланса.
    /// </summary>
    /// <param name="value">
    /// Значение баланса.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если передаваемый параметр меньше 0.
    /// </exception>
    public Balance(long value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Balance cannot be negative");
        }

        Value = value;
    }

    /// <summary>
    /// Создает новый объект баланса, увеличенный на указанную сумму.
    /// </summary>
    /// <param name="amount">
    /// Сумма пополнения.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    /// <returns>
    /// Новый экземпляр баланса.
    /// </returns>
    public Balance Add(Amount amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        return new Balance(Value + amount.Units);
    }

    /// <summary>
    /// Создает новый объект баланса, уменьшенный на указанную сумму.
    /// </summary>
    /// <param name="amount">
    /// Сумма списания.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается при попытке уйти в минус.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    /// <returns>
    /// Новый экземпляр баланса.
    /// </returns>
    public Balance Subtract(Amount amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        if (Value < amount.Units)
        {
            throw new InvalidOperationException("Недостаточно средств на балансе.");
        }

        return new Balance(Value - amount.Units);
    }

    /// <summary>
    /// Создает объект баланса из десятичного числа.
    /// </summary>
    /// <param name="amount">
    /// Сумма в десятичном формате
    /// </param>
    /// <returns>
    /// Новый экземпляр баланса.
    /// </returns>
    public static Balance FromDecimal(decimal amount)
        => new Balance((long)(amount * 100));

    /// <summary>
    /// Возвращает значение баланса в десятичном формате.
    /// </summary>
    /// <returns>
    /// Баланс в виде десятичного числа
    /// </returns>
    public decimal ToDecimal() => Value / 100m;
}
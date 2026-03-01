using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Accounts.Models.Outbox;

/// <summary>
/// Идентификатор cooбщения.
/// </summary>
/// <param name="Value">
/// Значение
/// </param>
public sealed record OutboxId(Guid Value);
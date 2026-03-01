using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Accounts.Models.Outbox;

public sealed class Outbox
{
    public OutboxId Id { get; private set; }

    public string Type { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset OccurredOnUtc { get; private set; }

    public DateTimeOffset? ProcessedOnUtc { get; private set; }

    public string? Error { get; private set; }

    public int ErrorCount { get; private set; }

    public Outbox(
        OutboxId id, 
        string type, 
        string content, 
        DateTimeOffset occurredOnUtc, 
        DateTimeOffset? processedOnUtc, 
        string? error, 
        int errorCount)
    {
        ArgumentNullException.ThrowIfNull(id);

        if (string.IsNullOrEmpty(type))
        {
            throw new ArgumentNullException(nameof(type));
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentNullException(nameof(content));
        }

        Id = id;
        Type = type;
        Content = content;
        OccurredOnUtc = occurredOnUtc;
        ProcessedOnUtc = processedOnUtc;
        Error = error;
        ErrorCount = errorCount;
    }

    public void MarkAsProcessed() => ProcessedOnUtc = DateTimeOffset.UtcNow;

    public void Fail(string errorMessage)
    {
        Error = errorMessage;
        ErrorCount++;
    }
}

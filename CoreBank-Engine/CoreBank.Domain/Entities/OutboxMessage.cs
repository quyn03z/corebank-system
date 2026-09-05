using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class OutboxMessage
{
    public Guid Id { get; set; }

    public string EventType { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int RetryCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ProcessedAt { get; set; }
}

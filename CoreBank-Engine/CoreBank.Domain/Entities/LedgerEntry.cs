using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class LedgerEntry
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public Guid AccountId { get; set; }

    public string EntryType { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal BalanceAfter { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}

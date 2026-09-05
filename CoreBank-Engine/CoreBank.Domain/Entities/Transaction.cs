using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public string IdempotencyKey { get; set; } = null!;

    public Guid FromAccountId { get; set; }

    public Guid ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public decimal Fee { get; set; }

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? FailureReason { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public virtual Account FromAccount { get; set; } = null!;

    public virtual ICollection<LedgerEntry> LedgerEntries { get; set; } = new List<LedgerEntry>();

    public virtual Account ToAccount { get; set; } = null!;
}

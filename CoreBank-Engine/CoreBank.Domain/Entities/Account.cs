using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string AccountNumber { get; set; } = null!;

    public Guid UserId { get; set; }

    public decimal Balance { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public byte[] RowVersion { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public virtual AccountLimit? AccountLimit { get; set; }

    public virtual ICollection<LedgerEntry> LedgerEntries { get; set; } = new List<LedgerEntry>();

    public virtual ICollection<Transaction> TransactionFromAccounts { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionToAccounts { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}

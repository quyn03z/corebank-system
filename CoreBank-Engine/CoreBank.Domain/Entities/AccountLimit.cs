using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class AccountLimit
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public decimal DailyLimit { get; set; }

    public decimal PerTransactionLimit { get; set; }

    public decimal CurrentDailySpent { get; set; }

    public DateOnly LastResetDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class OtpVerification
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid TransactionDraftId { get; set; }

    public string OtpCodeHash { get; set; } = null!;

    public DateTimeOffset ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public int Attempts { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class UserDevice
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTimeOffset TokenExpiryTime { get; set; }

    public string DeviceFingerprint { get; set; } = null!;

    public DateTimeOffset LastLoginAt { get; set; }

    public virtual User User { get; set; } = null!;
}

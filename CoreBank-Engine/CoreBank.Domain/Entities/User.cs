using System;
using System.Collections.Generic;

namespace CoreBank.Infrastructure.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<OtpVerification> OtpVerifications { get; set; } = new List<OtpVerification>();

    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
}

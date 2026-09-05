using System;
using System.Collections.Generic;
using CoreBank.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBank.Infrastructure;

public partial class CoreBankDbContext : DbContext
{
    public CoreBankDbContext()
    {
    }

    public CoreBankDbContext(DbContextOptions<CoreBankDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountLimit> AccountLimits { get; set; }

    public virtual DbSet<LedgerEntry> LedgerEntries { get; set; }

    public virtual DbSet<OtpVerification> OtpVerifications { get; set; }

    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDevice> UserDevices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=CoreBank_Engine;User Id=sa;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(e => e.AccountNumber, "IX_Accounts_AccountNumber");

            entity.HasIndex(e => e.UserId, "IX_Accounts_UserId");

            entity.HasIndex(e => e.AccountNumber, "UQ_Accounts_AccountNumber").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("VND");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Users");
        });

        modelBuilder.Entity<AccountLimit>(entity =>
        {
            entity.HasIndex(e => e.AccountId, "UQ_AccountLimits_AccountId").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CurrentDailySpent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DailyLimit)
                .HasDefaultValue(50000000.00m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastResetDate).HasDefaultValueSql("(CONVERT([date],sysdatetimeoffset()))");
            entity.Property(e => e.PerTransactionLimit)
                .HasDefaultValue(20000000.00m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithOne(p => p.AccountLimit)
                .HasForeignKey<AccountLimit>(d => d.AccountId)
                .HasConstraintName("FK_AccountLimits_Accounts");
        });

        modelBuilder.Entity<LedgerEntry>(entity =>
        {
            entity.HasIndex(e => new { e.AccountId, e.CreatedAt }, "IX_LedgerEntries_AccountId").IsDescending(false, true);

            entity.HasIndex(e => e.TransactionId, "IX_LedgerEntries_TransactionId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BalanceAfter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.EntryType)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.LedgerEntries)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LedgerEntries_Accounts");

            entity.HasOne(d => d.Transaction).WithMany(p => p.LedgerEntries)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LedgerEntries_Transactions");
        });

        modelBuilder.Entity<OtpVerification>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.TransactionDraftId, e.IsUsed }, "IX_OtpVerifications_Lookup");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.OtpCodeHash)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.OtpVerifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OtpVerifications_Users");
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasIndex(e => new { e.Status, e.CreatedAt }, "IX_OutboxMessages_Status_CreatedAt").HasFilter("([Status]='PENDING')");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.EventType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDING");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(e => new { e.FromAccountId, e.CreatedAt }, "IX_Transactions_FromAccount").IsDescending(false, true);

            entity.HasIndex(e => new { e.ToAccountId, e.CreatedAt }, "IX_Transactions_ToAccount").IsDescending(false, true);

            entity.HasIndex(e => e.IdempotencyKey, "UQ_Transactions_IdempotencyKey").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FailureReason).HasMaxLength(500);
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdempotencyKey)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDING");

            entity.HasOne(d => d.FromAccount).WithMany(p => p.TransactionFromAccounts)
                .HasForeignKey(d => d.FromAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_FromAccount");

            entity.HasOne(d => d.ToAccount).WithMany(p => p.TransactionToAccounts)
                .HasForeignKey(d => d.ToAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_ToAccount");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ_Users_PhoneNumber").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("CUSTOMER");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserDevice>(entity =>
        {
            entity.HasIndex(e => e.RefreshToken, "IX_UserDevices_RefreshToken");

            entity.HasIndex(e => e.UserId, "IX_UserDevices_UserId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.DeviceFingerprint)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastLoginAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserDevices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserDevices_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using Accounts.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api.Data;

public class AccountsDbContext(DbContextOptions<AccountsDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
}
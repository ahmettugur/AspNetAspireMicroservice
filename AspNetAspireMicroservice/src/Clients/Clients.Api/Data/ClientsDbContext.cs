using Clients.Api.Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace Clients.Api.Data;

public class ClientsDbContext(DbContextOptions<ClientsDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
}
using DBA.Tables;
using Microsoft.EntityFrameworkCore;

namespace DBA;

public class DbaDbContext : DbContext
{
    public DbaDbContext(DbContextOptions<DbaDbContext> options)
        : base(options)
    {

    }
    public DbaDbContext() { }
    //list of tables that will be created
    public DbSet<S_Scripts> S_Scripts { get; set; }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBA;

public class DbaDbContextFactory : IDesignTimeDbContextFactory<DbaDbContext>
{
    public DbaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbaDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? "Host=localhost;Database=SecurityApp;Username=USER;Password=PASS";

        optionsBuilder.UseNpgsql(connectionString);

        return new DbaDbContext(optionsBuilder.Options);
    }
    
}

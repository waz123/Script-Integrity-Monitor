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

        optionsBuilder.UseNpgsql("Host=localhost;Database=SecurityApp;Username=postgres;Password=admin");

        return new DbaDbContext(optionsBuilder.Options);
    }
    
}

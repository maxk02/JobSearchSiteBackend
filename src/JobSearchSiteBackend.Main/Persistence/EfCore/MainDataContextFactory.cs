// using JobSearchSiteBackend.Core.Persistence;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
//
// namespace JobSearchSiteBackend.Infrastructure.Persistence.EfCore;
//
// public class MainDataContextFactory : IDesignTimeDbContextFactory<MainDataContext>
// {
//     public MainDataContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<MainDataContext>();
//         optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=JobSearchSite_Dev;Trusted_Connection=True;MultipleActiveResultSets=true",
//             b => b.MigrationsAssembly(typeof(MainDataContextFactory).Assembly.FullName));
//         
//         return new MainDataContext(optionsBuilder.Options);
//     }
// }

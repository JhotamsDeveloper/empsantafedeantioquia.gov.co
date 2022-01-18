using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using model;
using persistenDatabase.Config;

namespace persistenDatabase
{
    public class ApplicationDbContext :
        IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<BiddingParticipant> BiddingParticipants { get; set; }
        public DbSet<PQRSD> PQRSDs { get; set; }
        public DbSet<FileDocument> FileDocuments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //Validaciones
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new CategoryConfig(builder.Entity<Category>());
            new BiddingParticipantConfig(builder.Entity<BiddingParticipant>());
            new MasterConfig(builder.Entity<Master>());
            new DocumentConfig(builder.Entity<Document>());
            new FileDocumentConfig(builder.Entity<FileDocument>());
            new PQRSDConfig(builder.Entity<PQRSD>());
            new ProductConfig(builder.Entity<Product>());
            new EmployeeConfig(builder.Entity<Employee>());

        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_CodeDB
{
    public partial class CommonServiceDbContext : DbContext
    {
        public CommonServiceDbContext(DbContextOptions<CommonServiceDbContext> options) : base(options)
        {
            Console.WriteLine($"This is {nameof(CommonServiceDbContext)}  DbContextOptions");
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}

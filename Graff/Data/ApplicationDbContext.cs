using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Graff.Models;

namespace Graff.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Graff.Models.Pessoa> Pessoa { get; set; }
        public DbSet<Graff.Models.Produto> Produto { get; set; }
        public DbSet<Graff.Models.Lance> Lance { get; set; }
    }
}

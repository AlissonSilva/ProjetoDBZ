using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Personagem> Personagens { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
    }
}
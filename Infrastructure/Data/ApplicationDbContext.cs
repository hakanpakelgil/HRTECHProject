using ApplicationCore.Entities;
using HRTechProject.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<Departman> Departmanlar { get; set; }
        public DbSet<Meslek> Meslekler { get; set; }
        public DbSet<Sirket> Sirketler { get; set; }
        public DbSet<Izin> Izinler { get; set; }
        public DbSet<Harcama> Harcamalar { get; set; }
        public DbSet<Avans> Avanslar { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}

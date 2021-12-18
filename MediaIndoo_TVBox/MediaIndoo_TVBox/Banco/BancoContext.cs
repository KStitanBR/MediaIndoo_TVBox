using MediaIndoo_TVBox.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaIndoo_TVBox.Banco
{
    public class BancoContext : DbContext
    {

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Videos> Videos { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }

        public BancoContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Constantes.CaminhoDoBanco}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Videos>().HasKey(l => l.Codigo);
            modelBuilder.Entity<Mensagem>().HasKey(l => l.Codigo);
            modelBuilder.Entity<Usuario>().HasKey(l => l.Codigo);
            modelBuilder.Entity<Player>().HasKey(l => l.Codigo);


            //modelBuilder.Entity<Usuario>().HasData(
    //new Usuario()
    //{
    //    Codigo = 1,
    //    Nome = "ADMIN",
    //    Email = "ADMIN",
    //    Senha = "1"
    //});
    //        //#if DEBUG
            //            modelBuilder.Entity<Usuario>().HasData(
            //                new Usuario()
            //                {
            //                    Codigo = 1,
            //                    Nome = "ADMIN",
            //                    Email = "ADMIN",
            //                    Senha = "1"
            //                });
            //#endif

            base.OnModelCreating(modelBuilder);
        }
    }
}

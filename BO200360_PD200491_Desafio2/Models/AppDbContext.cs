﻿using Microsoft.EntityFrameworkCore;

namespace BO200360_PD200491_Desafio2.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Cv> Cvs { get; set; }
    }
}
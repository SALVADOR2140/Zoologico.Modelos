using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zoologico.Modelos;

    public class ZoologicoAPIContext : DbContext
    {
        public ZoologicoAPIContext (DbContextOptions<ZoologicoAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Zoologico.Modelos.Animal> Animales { get; set; } = default!;

public DbSet<Zoologico.Modelos.Especie> Especies { get; set; } = default!;

public DbSet<Zoologico.Modelos.Raza> Razas { get; set; } = default!;
    }

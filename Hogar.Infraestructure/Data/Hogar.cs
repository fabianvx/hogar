using System;
using System.Collections.Generic;
using Hogar.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Hogar.Infraestructure.Data;

public partial class HogarContext : DbContext
{
    public HogarContext(DbContextOptions<HogarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Noticia> Noticia { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Noticia>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Usuario).HasMaxLength(9);

            entity.HasOne(d => d.UsuarioNavigation).WithMany()
                .HasForeignKey(d => d.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Noticia_Usuario");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(9);

            entity.HasOne(d => d.TipoNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.Tipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

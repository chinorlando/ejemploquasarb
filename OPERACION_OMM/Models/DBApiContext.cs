using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OPERACION_OMM.Models;

public partial class DBApiContext : DbContext
{
    public DBApiContext()
    {
    }

    public DBApiContext(DbContextOptions<DBApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cuenta> Cuenta { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.NroCuenta).HasName("PK__cuenta__6390221118B050C0");

            entity.ToTable("cuenta");

            entity.Property(e => e.NroCuenta)
                .HasMaxLength(14)
                .HasColumnName("nro_cuenta");
            entity.Property(e => e.Moneda)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("moneda");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
            entity.Property(e => e.Saldo)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("saldo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.Fecha).HasName("PK__movimien__E1141323ADF2CBFD");

            entity.ToTable("movimiento");

            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Importe)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("importe");
            entity.Property(e => e.NroCuenta)
                .HasMaxLength(14)
                .HasColumnName("nro_cuenta");
            entity.Property(e => e.Tipo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipo");

            entity.HasOne(d => d.oCuenta).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.NroCuenta)
                .HasConstraintName("FK_MOVIMEINTO_CUENTA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

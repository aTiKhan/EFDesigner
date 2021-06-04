//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v3.0.0.2
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MultiContext.Context2
{
   /// <inheritdoc/>
   public partial class Context2 : DbContext
   {
      #region DbSets
      public virtual Microsoft.EntityFrameworkCore.DbSet<global::MultiContext.Context2.Entity1> Entity1 { get; set; }
      public virtual Microsoft.EntityFrameworkCore.DbSet<global::MultiContext.Context2.Entity2> Entity2 { get; set; }
      public virtual Microsoft.EntityFrameworkCore.DbSet<global::MultiContext.Context2.Entity3> Entity3 { get; set; }

      #endregion DbSets

      /// <summary>
      /// Default connection string
      /// </summary>
      public static string ConnectionString { get; set; } = @"Data Source=.\sqlexpress;Initial Catalog=Test;Integrated Security=True";

      /// <inheritdoc />
      public Context2(DbContextOptions<Context2> options) : base(options)
      {
      }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder);

      /// <inheritdoc />
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         CustomInit(optionsBuilder);
      }

      partial void OnModelCreatingImpl(ModelBuilder modelBuilder);
      partial void OnModelCreatedImpl(ModelBuilder modelBuilder);

      /// <inheritdoc />
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);
         OnModelCreatingImpl(modelBuilder);

         modelBuilder.HasDefaultSchema("dbo");

         modelBuilder.Entity<global::MultiContext.Context2.Entity1>()
                     .ToTable("Entity1")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<global::MultiContext.Context2.Entity1>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<global::MultiContext.Context2.Entity1>()
                     .HasMany<global::MultiContext.Context2.Entity3>(p => p.Entity3)
                     .WithOne()
                     .HasForeignKey("Entity1_Entity3_Id")
                     .IsRequired();

         modelBuilder.Entity<global::MultiContext.Context2.Entity2>()
                     .ToTable("Entity2")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<global::MultiContext.Context2.Entity2>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<global::MultiContext.Context2.Entity2>()
                     .HasMany<global::MultiContext.Context2.Entity3>(p => p.Entity3)
                     .WithOne()
                     .HasForeignKey("Entity2_Entity3_Id")
                     .IsRequired();

         modelBuilder.Entity<global::MultiContext.Context2.Entity3>()
                     .ToTable("Entity3")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<global::MultiContext.Context2.Entity3>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();

         OnModelCreatedImpl(modelBuilder);
      }
   }
}
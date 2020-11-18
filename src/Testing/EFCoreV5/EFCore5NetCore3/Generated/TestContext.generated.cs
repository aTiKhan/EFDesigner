//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v3.0.0.1
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

namespace EFCore5NetCore3
{
   /// <inheritdoc/>
   public partial class TestContext : DbContext
   {
      #region DbSets
      public virtual Microsoft.EntityFrameworkCore.DbSet<global::EFCore5NetCore3.Detail3> Detail3 { get; set; }
      public virtual Microsoft.EntityFrameworkCore.DbSet<global::EFCore5NetCore3.Master> Masters { get; set; }

      #endregion DbSets

      /// <summary>
      /// Default connection string
      /// </summary>
      public static string ConnectionString { get; set; } = @"Data Source=.\sqlexpress;Initial Catalog=Test;Integrated Security=True";

      /// <inheritdoc />
      public TestContext(DbContextOptions<TestContext> options) : base(options)
      {
      }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder);

      /// <inheritdoc />
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseLazyLoadingProxies();

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

         modelBuilder.Owned<global::EFCore5NetCore3.Detail1>();

         modelBuilder.Owned<global::EFCore5NetCore3.Detail2>();

         modelBuilder.Entity<global::EFCore5NetCore3.Detail3>().ToTable("Detail3").HasKey(t => t.Id);
         modelBuilder.Entity<global::EFCore5NetCore3.Detail3>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

         modelBuilder.Entity<global::EFCore5NetCore3.Master>().ToTable("Masters").HasKey(t => t.Id);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().HasIndex(t => t.Fb);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().Property(t => t.Fa).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().HasIndex(t => t.Fa);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().HasOne<global::EFCore5NetCore3.Detail3>(p => p.ToZeroOrOneDetail3).WithMany().HasForeignKey(k => k.Fb);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().HasOne<global::EFCore5NetCore3.Detail3>(p => p.ToOneDetail3).WithMany().HasForeignKey(k => k.Fa);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail1).Property(p => p.Id).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail1).Property(p => p.Fa).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail1).Property(p => p.Fb).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail1).Property(p => p.Fc).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail1).Property(p => p.Property1);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail1).Property(p => p.Id).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail1).Property(p => p.Fa).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail1).Property(p => p.Fb).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail1).Property(p => p.Fc).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail1).Property(p => p.Property1);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().Navigation(p => p.ToOneDetail1).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsMany(p => p.ToManyDetail1).WithOwner("Master_ToManyDetail1").HasForeignKey("Master_ToManyDetail1Id");
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsMany(p => p.ToManyDetail1).Property<int>("Id");
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsMany(p => p.ToManyDetail1).HasKey("Id");
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail2).Property(p => p.Id).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail2).Property(p => p.Fc);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail2).Property(p => p.Fb);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToZeroOrOneDetail2).Property(p => p.Property1);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsMany(p => p.ToManyDetail2).WithOwner("Master_ToManyDetail2").HasForeignKey("Master_ToManyDetail2Id");
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsMany(p => p.ToManyDetail2).Property<int>("Id");
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsMany(p => p.ToManyDetail2).HasKey("Id");
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail2).Property(p => p.Id).IsRequired();
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail2).Property(p => p.Fc);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail2).Property(p => p.Fb);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().OwnsOne(p => p.ToOneDetail2).Property(p => p.Property1);
         modelBuilder.Entity<global::EFCore5NetCore3.Master>().Navigation(p => p.ToOneDetail2).IsRequired();

         OnModelCreatedImpl(modelBuilder);
      }
   }
}

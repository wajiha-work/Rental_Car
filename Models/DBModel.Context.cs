﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rental_Car.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_rental_carEntities : DbContext
    {
        public db_rental_carEntities()
            : base("name=db_rental_carEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tb_bookings> tb_bookings { get; set; }
        public virtual DbSet<tb_cars> tb_cars { get; set; }
        public virtual DbSet<tb_users> tb_users { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IntelMarkEntities1 : DbContext
    {
        public IntelMarkEntities1()
            : base("name=IntelMarkEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }
    
        [DbFunction("IntelMarkEntities1", "fn_GetCustomerDetailsById")]
        public virtual IQueryable<fn_GetCustomerDetailsById_Result> fn_GetCustomerDetailsById(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fn_GetCustomerDetailsById_Result>("[IntelMarkEntities1].[fn_GetCustomerDetailsById](@Id)", idParameter);
        }
    
        public virtual ObjectResult<usp_GetCustomerById_Result> usp_GetCustomerById(Nullable<int> id, string name)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetCustomerById_Result>("usp_GetCustomerById", idParameter, nameParameter);
        }
    }
}
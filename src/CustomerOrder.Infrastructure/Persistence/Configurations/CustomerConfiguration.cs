using CustomerOrder.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace CustomerOrder.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("Customers");
            HasKey(c => c.Id);

            Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(200);

            Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);

            // enail with index and unique constraint

            Property(c => c.Email)
               .IsRequired()
               .HasMaxLength(256)
               .HasColumnAnnotation(
                   IndexAnnotation.AnnotationName,
                   new IndexAnnotation(new IndexAttribute("IX_Customers_Email") { IsUnique = true }));


            // handle m-n 
            HasMany(c => c.Orders)
               .WithMany(o => o.Customers)
               .Map(m =>
               {
                   m.ToTable("CustomerOrders");
                   m.MapLeftKey("CustomerId");
                   m.MapRightKey("OrderId");
               });
        }
    }
}

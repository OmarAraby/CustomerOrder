using CustomerOrder.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace CustomerOrder.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");
            HasKey(o => o.Id);

            Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Orders_OrderNumber") { IsUnique = true }));

            Property(o => o.OrderDate).IsRequired();
            Property(o => o.TotalAmount).HasPrecision(18, 2);
        }
    }
}

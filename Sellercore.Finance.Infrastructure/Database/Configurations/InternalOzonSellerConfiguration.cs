using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sellercore.Finance.Ozon.Domain.Entities;

namespace Sellercore.Finance.Infrastructure.Database.Configurations;

public class InternalOzonSellerConfiguration : IEntityTypeConfiguration<InternalOzonSeller>
{
    public void Configure(EntityTypeBuilder<InternalOzonSeller> builder)
    {
        builder
            .HasMany(s => s.Products)
            .WithOne(p => p.Seller)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
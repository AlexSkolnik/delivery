using DeliveryApp.Core.Domain.Models.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.EntityConfigurations.CourierAggregate;

internal class TransportEntityTypeConfiguration : IEntityTypeConfiguration<TransportEntity>
{
    public void Configure(EntityTypeBuilder<TransportEntity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("transports");

        entityTypeBuilder.HasKey(entity => entity.Id);

        entityTypeBuilder
            .Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();

        entityTypeBuilder
            .Property(entity => entity.Name)
            .HasColumnName("name")
            .IsRequired();

        entityTypeBuilder
            .Property(entity => entity.Speed)
            .HasColumnName("speed")
            .IsRequired();
    }
}
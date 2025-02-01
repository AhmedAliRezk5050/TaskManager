using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TaskItemEntityTypeConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.CreatedTaskItems)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.UpdatedBy)
            .WithMany(u => u.updatedTaskItems)
            .HasForeignKey(t => t.UpdatedById)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

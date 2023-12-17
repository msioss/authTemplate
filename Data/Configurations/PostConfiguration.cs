using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffTracking.Data.Entities;

namespace StaffTracking.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasOne(p => p.Department).WithMany(u => u.Posts)
            .HasForeignKey(x => x.DepartmentId);
    }
}
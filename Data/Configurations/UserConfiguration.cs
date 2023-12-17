using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffTracking.Data.Entities;

namespace StaffTracking.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email);

        builder.HasOne(p => p.Employee).WithOne(u => u.User)
            .HasForeignKey<User>(x => x.Id);
    }
}
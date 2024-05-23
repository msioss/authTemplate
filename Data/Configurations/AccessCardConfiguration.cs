using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffTracking.Data.Entities;

namespace StaffTracking.Data.Configurations;

public class AccessCardConfiguration : IEntityTypeConfiguration<AccessCard>
{
    public void Configure(EntityTypeBuilder<AccessCard> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Employee).WithMany(x => x.AccessCards)
            .HasForeignKey(x => x.EmployeeId);

        builder.HasOne(x => x.Department).WithMany(x => x.AccessCards)
            .HasForeignKey(x => x.DepartmentId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffTracking.Data.Entities;

namespace StaffTracking.Data.Configurations;

public class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasOne(p => p.User).WithMany(u => u.AccessLogs)
            .HasForeignKey(x => x.UserId);
    }
}
using DeskFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Infrastructure.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.ReferenceNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(t => t.ReferenceNumber).IsUnique();
            builder.Property(t => t.Subject).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Status).HasConversion<string>();
            builder.Property(t => t.Priority).HasConversion<string>();

            builder.HasOne(t => t.Tenant)
                   .WithMany(t => t.Tickets)
                   .HasForeignKey(t => t.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.AssignedTo)
                   .WithMany(u => u.AssignedTickets)
                   .HasForeignKey(t => t.AssignedToUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

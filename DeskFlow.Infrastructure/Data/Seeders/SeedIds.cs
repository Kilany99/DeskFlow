using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Infrastructure.Data.Seeders
{
    public static class SeedIds
    {
        // Tenants
        public static readonly Guid AcmeTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid BetaTenantId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // Users
        public static readonly Guid SuperAdminId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        public static readonly Guid AcmeAdminId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        public static readonly Guid AcmeAgent1Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        public static readonly Guid AcmeAgent2Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        public static readonly Guid BetaAdminId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        public static readonly Guid BetaAgent1Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

        // Tickets
        public static readonly Guid AcmeTicket1Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1");
        public static readonly Guid AcmeTicket2Id = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2");
        public static readonly Guid AcmeTicket3Id = Guid.Parse("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3");
        public static readonly Guid BetaTicket1Id = Guid.Parse("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1");
        public static readonly Guid BetaTicket2Id = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2");
    }
}

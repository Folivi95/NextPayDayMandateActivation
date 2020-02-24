using Microsoft.EntityFrameworkCore;
using NextPayDay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextPayDay.Data
{
    public class OTPDbContext: DbContext
    {
        public OTPDbContext(DbContextOptions<OTPDbContext> options): base(options)
        {

        }

        public DbSet<OTPActivation> OTPActivations { get; set; }
    }
}

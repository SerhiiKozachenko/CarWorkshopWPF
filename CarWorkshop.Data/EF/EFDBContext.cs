using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Data.EF
{
    public class EFDBContext : DbContext, IUnitOfWork
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Workshop> Workshops { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }

        public EFDBContext() : base()
        {
            var user = this.Users.FirstOrDefault();
            if (user == null)
            {
                // init
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Add(
            //    new StoreGeneratedIdentityKeyConvention()
            //    );
        }

        async Task IUnitOfWork.SaveChangesAsync()
        {
            await this.SaveChangesAsync();
        }
    }
}

 using Microsoft.EntityFrameworkCore;
 using TaskManager.DataLayer.Tasks.Entities;

namespace Xrm.Pcc.DataLayer.EntityFramework
{
    public class TmContext : DbContext
    {
        public TmContext(DbContextOptions options) : base(options)
        { }

         

        protected  override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbTask>()
                  .HasKey(x => new { x.Id });
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kokoai_platform_api.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 必須調用基類的 OnModelCreating，以創建所有 Identity 相關的表格
            base.OnModelCreating(builder); 
            
            builder.HasPostgresExtension("uuid-ossp");
            builder.HasPostgresExtension("vector");
        }
    }
}
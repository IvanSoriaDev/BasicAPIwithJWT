using BasicAPIwithJWT.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicAPIwithJWT.Data;

public class DataContext : DbContext
{
    #region Entities
    public DbSet<AppUser> Users { get; set; }
    #endregion
    public DataContext(DbContextOptions options) : base(options)
    {
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;

public class AppDBContent : IdentityDbContext<IdentityUser>
{
    public AppDBContent(DbContextOptions<AppDBContent> options) : base(options) { }

    public DbSet<Car> Car { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<ShopCartItem> ShopCartItem { get; set; }
}
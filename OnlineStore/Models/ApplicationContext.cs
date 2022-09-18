using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders.Physical;

namespace OnlineStore.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Equipment> Equipments { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>().HasData(
            new Review
            {
                Id = 1,
                ReviewString = "Nice job",
                EquipmentId = 1,
                ReviewersName = "Bogdan"
            },
            new Review
            {
                Id = 2,
                ReviewString = "Cool worker",
                EquipmentId = 1,
                ReviewersName = "Masha"
            }
        );
        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                Id = 1,
                Name = "JCB",
                Price = 1800,
                DiscountPercentage = 10,
                Description = "Копка под кольца",
                NumberPhone = "+7 (917) 214-16-52",
                PathImg = "/Img/jcbVolvo.jpg",
                Photo = System.IO.File
                    .ReadAllBytes(@"C:\Users\fvoli\Desktop\dev\OnlineStore\OnlineStore\wwwroot\Img\jcbVolvo.jpg")
                    .ToArray(),
                Category = CategoryState.Экскаватор.ToString()
            },
            new Equipment
            {
                Id = 2,
                Name = "BobCat",
                Price = 1500,
                DiscountPercentage = 5,
                Description = "Копка под кольца",
                NumberPhone = "+7 (991) 722-32-93",
                PathImg = "/Img/bobCat.jpg",
                Photo = System.IO.File
                    .ReadAllBytes(@"C:\Users\fvoli\Desktop\dev\OnlineStore\OnlineStore\wwwroot\Img\bobCat.jpg")
                    .ToArray(),
                Category = CategoryState.Экскаватор.ToString()
            }
        );
    }
}
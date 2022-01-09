using Fiorella_second.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace Fiorella_second.DAL
{
 
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
      
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<IntroSlider> IntroSliders { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<AboutList> AboutList { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<ExpertImage> ExpertImage { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ComentSlider> Coments { get; set; }
        public DbSet<Settings> Settings { get; set; }



    }
}

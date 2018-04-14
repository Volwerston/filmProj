using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace FilmDatabase.Models
{
    public class FilmContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public FilmContext(DbContextOptions<FilmContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)//many to many, 3 tables
        {
            modelBuilder.Entity<Film>()
             .HasMany(c => c.Categories)
             .WithMany(s => s.Films)
             .Map(t => t.MapLeftKey("FilmId")
             .MapRightKey("CategoryId")
             .ToTable("CategoryFilm"));
        }
    }
}
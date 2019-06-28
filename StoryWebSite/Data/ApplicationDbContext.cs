using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoryWebSite.Models;

namespace StoryWebSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Story> Story { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<ImageBlock> ImageBlock { get; set; }
    }
}

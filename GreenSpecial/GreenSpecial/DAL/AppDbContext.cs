using GreenSpecial.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace GreenSpecial.DAL
{
	public class AppDbContext :  IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<Meal> Meals { get; set; }
		public DbSet<Setting> Settings { get; set; }

	}
}

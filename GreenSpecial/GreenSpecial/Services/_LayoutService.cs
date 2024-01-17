using GreenSpecial.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSpecial.Services
{
	public class _LayoutService
	{
		private readonly AppDbContext _context;

		public _LayoutService(AppDbContext context)
		{
			_context = context;
		}
		public async Task<Dictionary<string,string>> GetSettingAsync()
		{
			Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(c=>c.Key,c=>c.Value);
			return settings;
		}
	}
}

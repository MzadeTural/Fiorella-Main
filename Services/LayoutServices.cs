using Fiorella_second.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Services
{
    public class LayoutServices
    {
        private AppDbContext _context;

        public LayoutServices(AppDbContext context)
        {
            _context = context;
        }
        public Dictionary<string,string> GetSetting()
        {
            return _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
        }
    }
}

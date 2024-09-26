// Pages/Szavazatok.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Valasztasok.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Valasztasok.Pages
{
    public class SzavazatokModel : PageModel
    {
        private readonly ValasztasDbContext _context;

        public SzavazatokModel(ValasztasDbContext context)
        {
            _context = context;
        }

        public IList<Jelolt> Jelolt { get; set; }

        public async Task OnGetAsync()
        {
            Jelolt = await _context.Jeloltek.Include(j => j.Part).ToListAsync(); // Include Part
        }
    }
}

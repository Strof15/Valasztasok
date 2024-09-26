using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Valasztasok.Models;

namespace Valasztasok.Pages
{
    public class detailsModel : PageModel
    {
        private readonly Valasztasok.Models.ValasztasDbContext _context;

        public detailsModel(Valasztasok.Models.ValasztasDbContext context)
        {
            _context = context;
        }

        public Jelolt Jelolt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jelolt = await _context.Jeloltek.FirstOrDefaultAsync(m => m.Id == id);
            if (jelolt == null)
            {
                return NotFound();
            }
            else
            {
                Jelolt = jelolt;
            }
            return Page();
        }
    }
}

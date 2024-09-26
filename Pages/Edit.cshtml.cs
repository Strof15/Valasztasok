using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Valasztasok.Models;

namespace Valasztasok.Pages
{
    public class EditModel : PageModel
    {
        private readonly Valasztasok.Models.ValasztasDbContext _context;

        public EditModel(Valasztasok.Models.ValasztasDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Jelolt Jelolt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jelolt =  await _context.Jeloltek.FirstOrDefaultAsync(m => m.Id == id);
            if (jelolt == null)
            {
                return NotFound();
            }
            Jelolt = jelolt;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Jelolt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JeloltExists(Jelolt.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool JeloltExists(int id)
        {
            return _context.Jeloltek.Any(e => e.Id == id);
        }
    }
}

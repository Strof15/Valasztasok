using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valasztasok.Models;

namespace Valasztasok.Pages
{
    public class AdatFeltoltesModel : PageModel
    {
        public IWebHostEnvironment _env { get; set; }
        public ValasztasDbContext _context { get; set; }

        public AdatFeltoltesModel(ValasztasDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public IFormFile UploadFile { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var UploadFilePath = Path.Combine(_env.ContentRootPath,"Uploads",UploadFile.FileName);

            using(var stream = new FileStream(UploadFilePath, FileMode.Create))
            {
                await UploadFile.CopyToAsync(stream);
            }

            return Page();
        }
    }
}

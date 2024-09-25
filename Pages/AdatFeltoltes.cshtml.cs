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

            StreamReader sr = new StreamReader(UploadFilePath);
            while (!sr.EndOfStream)
            {
                var sor = sr.ReadLine();
                var elemek = sor.Split(' ');
                Jelolt ujJelolt = new();
                Part ujPart;
                if (!_context.Partok.Select(x=>x.RovidNev == elemek[4]).First())
                {
                ujPart = new();

                }
                else
                {
                    ujPart = _context.Partok.Select(x => x.RovidNev == elemek[4]).First();
                }

                ujJelolt.KeruletID = int.Parse(elemek[0]);
                ujJelolt.SzavazatSzam = int.Parse(elemek[1]);
                ujJelolt.KepviseloNev = $"{elemek[2]} {elemek[3]}";
                ujPart.RovidNev = elemek[4];
                ujJelolt.Part = ujPart;
                _context.Jeloltek.Add(ujJelolt);
                _context.Partok.Add(ujPart);
            }
            sr.Close();
            _context.SaveChanges();

            return Page();
        }
    }
}

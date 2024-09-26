using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valasztasok.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

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

        // Tároljuk a már hozzáadott pártokat
        private List<Part> hozzadottPartok = new List<Part>();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var UploadFilePath = Path.Combine(_env.ContentRootPath, "Uploads", UploadFile.FileName);

            using (var stream = new FileStream(UploadFilePath, FileMode.Create))
            {
                await UploadFile.CopyToAsync(stream);
            }

            using (StreamReader sr = new StreamReader(UploadFilePath))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine();
                    var elemek = sor.Split(' ');
                    Jelolt ujJelolt = new();
                    Part ujPart;

                    // Megpróbáljuk megtalálni a pártot az eddig hozzáadottak között
                    ujPart = hozzadottPartok.FirstOrDefault(x => x.RovidNev == elemek[4]);

                    if (ujPart == null)
                    {
                        // Megnézzük, hogy az adatbázisban van-e már ilyen párt
                        ujPart = _context.Partok.FirstOrDefault(x => x.RovidNev == elemek[4]);

                        if (ujPart == null)
                        {
                            // Ha nincs, akkor létrehozzuk
                            ujPart = new Part
                            {
                                RovidNev = elemek[4]
                            };
                            _context.Partok.Add(ujPart);
                        }

                        // Hozzárendelés a TeljesNev alapján
                        if (elemek[4] == "GYEP")
                        {
                            ujPart.TeljesNev = "Gyümölcsevõk Pártja";
                        }
                        else if (elemek[4] == "HEP")
                        {
                            ujPart.TeljesNev = "Hûsevõk Pártja";
                        }
                        else if (elemek[4] == "TISZ")
                        {
                            ujPart.TeljesNev = "Tejivók Szövetsége";
                        }
                        else if (elemek[4] == "ZEP")
                        {
                            ujPart.TeljesNev = "Zöldségevõk Pártja";
                        }
                        else if (elemek[4] == "-")
                        {
                            ujPart.TeljesNev = "Ismeretlen Párt";
                        }

                        hozzadottPartok.Add(ujPart);
                    }

                    // Létrehozzuk a jelöltet
                    ujJelolt.KeruletID = int.Parse(elemek[0]);
                    ujJelolt.SzavazatSzam = int.Parse(elemek[1]);
                    ujJelolt.KepviseloNev = $"{elemek[2]} {elemek[3]}";
                    ujJelolt.Part = ujPart;

                    _context.Jeloltek.Add(ujJelolt);
                }

                await _context.SaveChangesAsync();
            }

            return Page();
        }
    }
}

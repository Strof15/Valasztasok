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

        // T�roljuk a m�r hozz�adott p�rtokat
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

                    // Megpr�b�ljuk megtal�lni a p�rtot az eddig hozz�adottak k�z�tt
                    ujPart = hozzadottPartok.FirstOrDefault(x => x.RovidNev == elemek[4]);

                    if (ujPart == null)
                    {
                        // Megn�zz�k, hogy az adatb�zisban van-e m�r ilyen p�rt
                        ujPart = _context.Partok.FirstOrDefault(x => x.RovidNev == elemek[4]);

                        if (ujPart == null)
                        {
                            // Ha nincs, akkor l�trehozzuk
                            ujPart = new Part
                            {
                                RovidNev = elemek[4]
                            };
                            _context.Partok.Add(ujPart);
                        }

                        // Hozz�rendel�s a TeljesNev alapj�n
                        if (elemek[4] == "GYEP")
                        {
                            ujPart.TeljesNev = "Gy�m�lcsev�k P�rtja";
                        }
                        else if (elemek[4] == "HEP")
                        {
                            ujPart.TeljesNev = "H�sev�k P�rtja";
                        }
                        else if (elemek[4] == "TISZ")
                        {
                            ujPart.TeljesNev = "Tejiv�k Sz�vets�ge";
                        }
                        else if (elemek[4] == "ZEP")
                        {
                            ujPart.TeljesNev = "Z�lds�gev�k P�rtja";
                        }
                        else if (elemek[4] == "-")
                        {
                            ujPart.TeljesNev = "Ismeretlen P�rt";
                        }

                        hozzadottPartok.Add(ujPart);
                    }

                    // L�trehozzuk a jel�ltet
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

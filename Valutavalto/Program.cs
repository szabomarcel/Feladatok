using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Valto;

namespace Valutavalto
{
    internal class Program
    {
        static Valuta valto = null;
        static async Task Main(string[] args)
        {
            await valutavaltas();
            Console.WriteLine("Forintot szeretne átváltani euróra vagy dollárra? (eur/dollár/nem)");
            string valasz = Console.ReadLine();

            if (valasz.ToLower() == "eur" || valasz.ToLower() == "dollár")
            {
                string celValuta = valasz.ToLower();
                Console.WriteLine("Kérem adja meg az átváltani kívánt forint összegét:");
                string forintString = Console.ReadLine();
                if (double.TryParse(forintString, out double forint))
                {
                    double valtottOsszeg = 0.0;

                    if (celValuta == "eur")
                    {
                        if (valto.Rates.TryGetValue("EUR", out double eurRate))
                        {
                            valtottOsszeg = forint / eurRate;
                        }
                        else
                        {
                            Console.WriteLine("Nem található EUR árfolyam az API válaszban.");
                        }
                    }
                    else if (celValuta == "dollár")
                    {
                        if (valto.Rates.TryGetValue("USD", out double usdRate))
                        {
                            valtottOsszeg = forint / usdRate;
                        }
                        else
                        {
                            Console.WriteLine("Nem található USD árfolyam az API válaszban.");
                        }
                    }

                    Console.WriteLine($"{forint} forint {valtottOsszeg} {celValuta}.");
                }
                else
                {
                    Console.WriteLine("Érvénytelen számformátum.");
                }
            }
            double legjobbArfolyam = Math.Max(valto.Rates["EUR"], valto.Rates["USD"]);
            string legjobbValuta = legjobbArfolyam == valto.Rates["EUR"] ? "euró" : "dollár";
            Console.WriteLine($"A legjobb ajánlat: {legjobbValuta}");
            Console.WriteLine("Program vége.");
            Console.ReadLine();
        }

        private static async Task valutavaltas()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://infojegyzet.hu/webszerkesztes/php/valuta/api/v1/arfolyam/");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            valto = Valuta.FromJson(jsonResponse);
        }
    }
}

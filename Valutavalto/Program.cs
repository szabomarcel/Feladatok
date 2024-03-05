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
            List<Valuta> valtos = new List<Valuta>();
            await valutavaltas();
            Console.WriteLine("Átváltás: ");
            Console.ReadLine();
        }

        private static async Task valutavaltas()
        {
            List<Valuta> valto = new List<Valuta>();
            var client = new HttpClient();            
            var request = new HttpRequestMessage(HttpMethod.Get, "https://infojegyzet.hu/webszerkesztes/php/valuta/api/v1/arfolyam/");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}

using LogReader.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogReader.UI.Controllers
{
    public class LogsController : Controller
    {

        private const string _apiUrl = "https://localhost:44340/api";
        // GET: Logs
        public async Task<IActionResult> Index()
        {
            var controller = "Logs";
            var resultado = new BuscaViewModel();
            resultado.Logs = new List<Log>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}", _apiUrl, controller));
                if (result.IsSuccessStatusCode)
                {
                    resultado.Logs = JsonConvert.DeserializeObject<IEnumerable<Log>>(result.Content.ReadAsStringAsync().Result).ToList();
                }
            }
            controller = "Users";
            var usuarios = new List<User>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}", _apiUrl, controller));
                if (result.IsSuccessStatusCode)
                {
                    usuarios = JsonConvert.DeserializeObject<IEnumerable<User>>(result.Content.ReadAsStringAsync().Result).ToList();
                    //response = await result.Content.ReadAsStringAsync();
                }
            }
            ViewData["UserId"] = new SelectList(usuarios, "Id", "Nome", null);

            return View(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(BuscaViewModel v)
        {
            var resultado = new BuscaViewModel();
            resultado.Logs = new List<Log>();
            var controller = "Logs";

            using (var client = new HttpClient())
            {
                if (v.IP == null)
                    v.IP = "0";
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}/{2}/{3}", _apiUrl, controller, v.IP, v.UserId));
                if (result.IsSuccessStatusCode)
                {
                    resultado.Logs = JsonConvert.DeserializeObject<IEnumerable<Log>>(result.Content.ReadAsStringAsync().Result).ToList();
                }
            }
            controller = "Users";
            var usuarios = new List<User>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}", _apiUrl, controller));
                if (result.IsSuccessStatusCode)
                {
                    usuarios = JsonConvert.DeserializeObject<IEnumerable<User>>(result.Content.ReadAsStringAsync().Result).ToList();
                    //response = await result.Content.ReadAsStringAsync();
                }
            }
            ViewData["UserId"] = new SelectList(usuarios, "Id", "Nome", null);

            return View("Index", resultado);
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var controller = "Logs";
            var log = new Log();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}/{2}", _apiUrl, controller, id));
                if (result.IsSuccessStatusCode)
                {
                    log = JsonConvert.DeserializeObject<Log>(result.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return NotFound();
                }
            }


            return View(log);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IP,LogDate,Method,URL,Protocol,Result")] Log log)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var controler = "/Logs";
                    client.BaseAddress = new Uri(String.Format("{0}{1}", _apiUrl, controler));

                    var content = JsonConvert.SerializeObject(log);

                    var response = client.PostAsync(client.BaseAddress, new StringContent(content, Encoding.UTF8, "application/json"));

                }
                return RedirectToAction(nameof(Index));
            }
            return View(log);
        }


        // GET: Logs/CreateBatch
        public IActionResult CreateBatch()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

    

        [HttpPost]
        public ActionResult Upload(IList<IFormFile> files)
        {
            var filesQtde = files.Count;
            var lote = new Lote();
            lote.Logs = new List<Log>();
            if (files.Count == 0 )
            {
                ViewData["Erro"] = "Erro: Nenhum Arquivo(s) selecionado(s)";
                return View(ViewData);
            }
            foreach (IFormFile arquivo in files)
            {
                string[] permittedExtensions = { ".txt", ".pdf" };

                var ext = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    ViewData["Erro"] = "Erro: Formato do arquivo incorreto";
                    return View();
                }

                var result = new StringBuilder();
                using (var reader = new StreamReader(arquivo.OpenReadStream()))
                { 
                    while (reader.Peek() >= 0)
                    {
                        var linha = reader.ReadLine();
                        var logFilds = linha.ToString().Split("-");
                        Log l = new Log();
                        l.IP = logFilds[0].Trim();
                        l.LogDate = Convert.ToDateTime(logFilds[1].TrimStart());
                        var callDados = logFilds[2].Split(" ");
                        l.Method = callDados[1].Trim();
                        l.URL = callDados[2].Trim();
                        l.Protocol = callDados[3].Trim();
                        l.Result = logFilds[3].Trim();
                        lote.Logs.Add(l);
                    }
                }
            }

            if(lote.Logs.Count > 0)
            {
                using (var client = new HttpClient())
                {
                    var controler = "/Logs/Lote";
                    client.BaseAddress = new Uri(String.Format("{0}{1}", _apiUrl, controler));

                    var content = JsonConvert.SerializeObject(lote);

                    var response = client.PostAsync(client.BaseAddress, new StringContent(content, Encoding.UTF8, "application/json"));
                    if (response.Result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                      
                    }
                    else
                    {
                        ViewData["Erro"] = "Erro: " + response.Result.ReasonPhrase;
                        return View();
                    }
                }
                
            }

            var responseView = new { filesQtde };

            return Ok(responseView);
        }

      
        //GET: Logs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controller = "Logs";
            var log = new Log();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}/{2}", _apiUrl, controller, id));
                if (result.IsSuccessStatusCode)
                {
                    log = JsonConvert.DeserializeObject<Log>(result.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return NotFound();
                }
            }

            controller = "Users";
            var resultado = new List<User>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}", _apiUrl, controller));
                if (result.IsSuccessStatusCode)
                {
                    resultado = JsonConvert.DeserializeObject<IEnumerable<User>>(result.Content.ReadAsStringAsync().Result).ToList();
                    //response = await result.Content.ReadAsStringAsync();
                }
            }
            ViewData["UserId"] = new SelectList(resultado, "Id", "Nome", null);
            return View(log);
        }

        //POST: Logs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IP,LogDate,Method,URL,Protocol,Result,UserId")] Log log)
        {
            if (id != log.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var controler = "/Logs";
                        client.BaseAddress = new Uri(string.Format("{0}/{1}/{2}", _apiUrl, controler, id));

                        var content = JsonConvert.SerializeObject(log);

                        var response = client.PutAsync(client.BaseAddress, new StringContent(content, Encoding.UTF8, "application/json"));
                    }

                }
                catch (Exception)
                {
                    var exists = await LogExists(log.Id);
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var controller = "Users";
            var resultado = new List<User>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}", _apiUrl, controller));
                if (result.IsSuccessStatusCode)
                {
                    resultado = JsonConvert.DeserializeObject<IEnumerable<User>>(result.Content.ReadAsStringAsync().Result).ToList();
                    //response = await result.Content.ReadAsStringAsync();
                }
            }
            ViewData["UserId"] = resultado;
            return View(log);
        }

        //GET: Logs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controller = "Logs";
            var log = new Log();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}/{2}", _apiUrl, controller, id));
                if (result.IsSuccessStatusCode)
                {
                    log = JsonConvert.DeserializeObject<Log>(result.Content.ReadAsStringAsync().Result);
                }
            }
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controller = "Logs";
            var log = new Log();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.DeleteAsync(string.Format("{0}/{1}/{2}", _apiUrl, controller, id));
                if (result.IsSuccessStatusCode)
                {
                    log = JsonConvert.DeserializeObject<Log>(result.Content.ReadAsStringAsync().Result);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> LogExists(int id)
        {
            var controller = "Logs";
            var log = new Log();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}/{2}", _apiUrl, controller, id));
                if (result.IsSuccessStatusCode)
                {
                    log = JsonConvert.DeserializeObject<Log>(result.Content.ReadAsStringAsync().Result);
                }
            }
            if (log == null)
                return false;
            else
                return true;
        }

        private async Task<IEnumerable<User>> GetUsersDDL()
        {
            var controller = "Users";
            var resultado = new List<User>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(string.Format("{0}/{1}", _apiUrl, controller));
                if (result.IsSuccessStatusCode)
                {
                    resultado = JsonConvert.DeserializeObject<IEnumerable<User>>(result.Content.ReadAsStringAsync().Result).ToList();
                    //response = await result.Content.ReadAsStringAsync();
                }
            }
            return resultado;
        }
    }
}

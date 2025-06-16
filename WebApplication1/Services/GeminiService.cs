namespace WebApplication1.Services
{

    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyDqseX-88svYJy9EciGTXbn2o0nmP3SFaE";
        private readonly IConfiguration _configuration;


        public GeminiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            var body = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al llamar a Gemini: {response.StatusCode}\n{error}");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(resultJson);
            var text = doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString();

            return text ?? "Sin respuesta generada";
        }

        public string GenerateGeminiPrompt(int tipoPersona, string promptBase)
        {
            // Obtener la ruta base de la aplicación
            string basePath = AppContext.BaseDirectory;
            string archivoIA = null;
            if (tipoPersona == 0) { archivoIA = "PromtIANatural.txt"; } else { archivoIA = "PromtIAJuridico.txt"; }
            string promptTemplatePath = Path.Combine(basePath, "Recursos", archivoIA);

            // Leer el contenido del archivo de texto
            string promptTemplate = File.ReadAllText(promptTemplatePath); // Asegúrate de que la ruta sea correcta

            // Reemplazar solo el nombre de la empresa
            string finalPrompt = promptTemplate.Replace("{promptBase}", promptBase);

            return finalPrompt;
        }
    }
}

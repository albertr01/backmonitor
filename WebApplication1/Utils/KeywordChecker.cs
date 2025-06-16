namespace WebApplication1.Utils
{
    public class KeywordChecker
    {
        public static List<string> PalabrasClave = new List<string>
    {
        "inteligencia artificial",
        "machine learning",
        "ciberseguridad",
        "energías renovables",
        "crisis climática",
        "inflación",
        "criptomonedas",
        "salud pública",
        "tecnología 5G"
    };

        public static bool ContienePalabrasClave(string textoNoticia, out List<string> palabrasEncontradas)
        {
            palabrasEncontradas = new List<string>();
            string textoNormalizado = textoNoticia.ToLowerInvariant(); // Normalizamos el texto a minúsculas

            foreach (string palabraClave in PalabrasClave)
            {
                // Usamos Regex para una búsqueda de palabra completa insensible a mayúsculas/minúsculas
                string patron = @"\b" + System.Text.RegularExpressions.Regex.Escape(palabraClave.ToLowerInvariant()) + @"\b";

                if (System.Text.RegularExpressions.Regex.IsMatch(textoNormalizado, patron))
                {
                    palabrasEncontradas.Add(palabraClave);
                }
            }
            return palabrasEncontradas.Any();
        }
    }
}

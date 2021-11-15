using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoaClient.Models
{
    /// <summary>
    /// La clase <c>Ecuacion</c> Guarda el objeto de la ecuacion envia los datos y realiza los calculos.
    /// </summary>
    public class Ecuacion
    {
        /// <value>
        /// Guarda la ecuacion de forma <c>f(x)=ax**2+bx+c</c>.
        /// </value>
        private string eq;
        public string Eq { get => eq; set => eq = value; }

        /// <value>
        /// cliente http para consumir el API.
        /// </value>
        private HttpClient httpClient;

        /// <summary>
        /// Constructor resive la ecuacion y crea el cliente http.
        /// </summary>
        /// <param name="equa">ecuacion a calcular</param>
        public Ecuacion(string equa)
        {
            Eq = equa;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Constants.baseUri);
        }

        /// <summary>
        /// Constructor sin argumentos crea unicamente el cliente http.
        /// </summary>
        public Ecuacion()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Constants.baseUri);
        }

        /// <summary>
        /// Recive las soluciones de la ecuacion del API.
        /// </summary>
        /// <returns>un arreglo con las soluciones de la ecuacion.</returns>
        public async Task<ICollection<double>> Soluciones()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/Ecuacion/CalcularSoluciones?eqs={Eq}");
            var res = await httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode) return null;
            try {
                var resStream = await res.Content.ReadAsStringAsync();
                var sol = JsonConvert.DeserializeObject<ICollection<double>>(resStream);
                return sol;
            } catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Recive una tabla de valores con 30 valores.
        /// </summary>
        /// <returns>retorna una matriz con los valores.</returns>
        public async Task<ICollection<ICollection<double>>> TablaValores()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/Ecuacion/CalcularTablaDeValores?eqs={Eq}");
            var res = await httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode) return null;
            try {
                var resStream = await res.Content.ReadAsStringAsync();
                var sol = JsonConvert.DeserializeObject<ICollection<ICollection<double>>>(resStream);
                return sol;
            }
            catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Recive una tabla extendida con los valores para realizar una grafica.
        /// </summary>
        /// <returns>retorna una matriz con los valores.</returns>
        public async Task<ICollection<ICollection<double>>> TablaValoresXl()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/Ecuacion/CalcularTablaDeValoresXl?eqs={Eq}");
            var res = await httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode) return null;
            try {
                var resStream = await res.Content.ReadAsStringAsync();
                var sol = JsonConvert.DeserializeObject<ICollection<ICollection<double>>>(resStream);
                return sol;
            }
            catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Recive la derivada desde el API.
        /// </summary>
        /// <param name="n">Grado de la derivada.</param>
        /// <returns>retorna la deribada de grado asignado</returns>
        public async Task<string> Derivada(uint n)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/Ecuacion/CalcularDerivadas?eqs={Eq}&n={n}");
            var res = await httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode) return null;
            try {
                var sol = await res.Content.ReadAsStringAsync();
                return sol;
            }
            catch (Exception) {
                return null;
            }
        }
    }
}

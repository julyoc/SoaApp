using SoaClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoaClient.Services
{
    public class EcuacionService
    {
        /// <value>
        /// Objeto de la ecuacion.
        /// </value>
        private Ecuacion eq = new Ecuacion();
        public string Ecuacion { get => eq.Eq; set => eq.Eq = value; }

        /// <summary>
        /// Obtiene las soluciones del modelo.
        /// </summary>
        /// <returns>Retorna las soluciones.</returns>
        public Task<ICollection<double>> CalcularSoluciones()
        {
            return eq.Soluciones();
        }

        /// <summary>
        /// Obtiene las tablas de valores del modelo.
        /// </summary>
        /// <returns>Retorna la tabla de valores.</returns>
        public Task<ICollection<ICollection<double>>> CalcularTablaDeValores()
        {
            return eq.TablaValores();
        }

        /// <summary>
        /// Obtiene las tablas de valores extendidas del modelo.
        /// </summary>
        /// <returns>Retorna la tabla de valores.</returns>
        public Task<ICollection<ICollection<double>>> CalcularTablaDeValoresXl()
        {
            return eq.TablaValoresXl();
        }

        /// <summary>
        /// Obtiene la derivada del modelo.
        /// </summary>
        /// <param name="n">Grado de la derivada.</param>
        /// <returns>Retorna la deribada del grado asignado.</returns>
        public Task<string> CalcularDerivadas (uint n)
        {
            return eq.Derivada(n);
        }
    }
}

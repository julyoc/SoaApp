using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoaServer.Controllers
{
    /// <summary>
    /// clase de controlador <c>EcuacionController</c> estiende a la clase <c>Controller</c>
    /// Controlador Para los calculos con las ecuaciones.
    /// </summary>
    public class EcuacionController : Controller
    {
        private readonly ILogger<EcuacionController> _logger;

        public EcuacionController(ILogger<EcuacionController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: /Ecuacion/CalcularSoluciones?eqs=...
        /// Recibe un query con la ecuacion y calcula las soluciones.
        /// </summary>
        /// <param name="eqs">Ecuacion a calcular</param>
        /// <returns>Soluciones de la ecuacion</returns>
        public IActionResult CalcularSoluciones([FromQuery] string eqs)
        {
            var eq = new Ecuacion(eqs);
            var sol = eq.Soluciones();
            _logger.LogInformation("Calculo de la(s) soluciones:", sol);
            if (eq.D < 0) _logger.LogError("El determinante es menor que 0.", eq.D);
            return Ok(sol);
        }

        /// <summary>
        /// GET: /Ecuacion/CalcularTablaDeValores?eqs=...
        /// Recibe un query con la ecuacion y calcula una tabla de valores.
        /// </summary>
        /// <param name="eqs">Ecuacion a calcular</param>
        /// <returns>Tabla de valores <c>(x, y)</c></returns>
        public IActionResult CalcularTablaDeValores([FromQuery] string eqs)
        {
            var eq = new Ecuacion(eqs);
            var sol = eq.TablaValores();
            _logger.LogInformation("Calculo de la tabla de valores:", sol);
            return Ok(sol);
        }

        /// <summary>
        /// GET: /Ecuacion/CalcularTablaDeValoresXl?eqs=...
        /// Recibe un query con la ecuacion y calcula una tabla de valores para graficarla.
        /// </summary>
        /// <param name="eqs">Ecuacion a calcular</param>
        /// <returns>Tabla de valores <c>(x, y)</c></returns>
        public IActionResult CalcularTablaDeValoresXl([FromQuery] string eqs)
        {
            var eq = new Ecuacion(eqs);
            var sol = eq.TablaValoresXl();
            _logger.LogInformation("Calculo de la tabla de valores:", sol);
            return Ok(sol);
        }

        /// <summary>
        /// GET: /Ecuacion/CalcularTablaDeValores?eqs=....&n=....
        /// Recibe la ecuacion y el numero de derivada y cualcula la derivada asignada.
        /// </summary>
        /// <param name="eqs">Ecuacion a calcular</param>
        /// <param name="n">Numero de derivada</param>
        /// <returns>String de la ecuacion ya deribada</returns>
        public IActionResult CalcularDerivadas([FromQuery] string eqs, [FromQuery] uint n)
        {
            var eq = new Ecuacion(eqs);
            var sol = eq.Derivada(n);
            _logger.LogInformation($"Calculo de la {n} derivada:", sol);
            return Ok(sol);
        }
    }
}

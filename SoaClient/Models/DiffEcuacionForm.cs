using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoaClient.Models
{
    public class DiffEcuacionForm : EcuacionForm
    {
        [Required]
        [Range(1, 3, ErrorMessage = "No se puede obtener esa derivada.")]
        public int N { get; set; } = 1;
    }
}

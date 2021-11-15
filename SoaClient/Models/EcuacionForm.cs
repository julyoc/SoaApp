using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoaClient.Models
{
    public class EcuacionForm
    {
        [Required]
        //[RegularExpression(@"[=]+", ErrorMessage = "No es el formato")]
        public string Eq { get; set; }
    }
}

using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySqlConnector;

namespace SoaServer.Models
{
    /// <summary>
    /// La clase <c>Ecuacion</c> Guarda el objeto de la ecuacion y realiza los calculos.
    /// </summary>
    public class Ecuacion
    {
        /// <value>
        /// Guarda la ecuacion de forma <c>f(x)=ax**2+bx+c</c>.
        /// </value>
        private string eq;
        public string Eq { get => eq; set => eq = value; }

        /// <value>
        /// Guarda el valor de <c>A</c> de la ecuacion.
        /// </value>
        private double a;
        public double A { get => a; }

        /// <value>
        /// Guarda el valor de <c>B</c> de la ecuacion.
        /// </value>
        private double b;
        public double B { get => b; }

        /// <value>
        /// Guarda el valor de <c>C</c> de la ecuacion.
        /// </value>
        private double c;
        public double C { get => c; }

        /// <value>
        /// Retorna el valor calculado del discriminante <c>D</c>.
        /// </value>
        public double D { get => Math.Pow(B,2) - 4*A*C; }

        internal MySqlConnection Db { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// Constructor con un parametro.
        /// </summary>
        /// <param name="eq">Ingresa la ecuacion tipo string.</param>
        /// <example>
        /// <code>
        /// var eq = new Ecuacion("f(x)=x**2+3x-6");    
        /// </code>
        /// </example>
        public Ecuacion(string eq, MySqlConnection db)
        {
            Db = db;
            Eq = eq.Replace(" ", "");
            var eqt = Eq.Split("=")[1];
            try {
                if (eqt.Split("x**2")[0].Equals("-")) a = -1;
                a = double.Parse(eqt.Split("x**2")[0]);
            } catch(Exception) {
                a = 1;
            }
            var at = eqt.Split("x**2")[1];
            var re = new Regex(@"[x]+");
            if (re.IsMatch(at)) {
                if (at.Split("x")[0].Equals("-")) b = -1;
                else if (at.Split("x")[0].Equals("+")) b = 1;
                else if (at.Equals("x")) b = 1;
                else if (string.IsNullOrEmpty(at.Split("x")[0])) b = 1;
                else b = double.Parse(at.Split("x")[0]);
                try {
                    c = double.Parse(at.Split("x")[1]);
                } catch(Exception) { }
            } else {
                b = 0;
                try {
                    c = double.Parse(at);
                } catch(Exception) {
                    c = 0;
                }
            }
            Db.Open();
            insertEq().Wait();
        }

        /// <summary>
        /// Calcula las posibles soluciones de la ecuacion.
        /// </summary>
        /// <returns>Devuelve un arreglo con las soluciones o nulo si no existen</returns>
        /// <example>
        /// <code>
        /// eq.Soluciones();  //  { -3, 2 }
        /// </code>
        /// </example>
        public ICollection<double> Soluciones() 
        {
            if (D < 0) return null;
            if (D == 0) 
            {
                var s = -B / (2 * A);
                insertSols(s).Wait();
                return new double[] { s };
            }
            var s1 = (-B + Math.Sqrt(D)) / (2 * A);
            var s2 = (-B - Math.Sqrt(D)) / (2 * A);
            insertSols(s1).Wait();
            insertSols(s2).Wait();
            return new double[] { s1, s2 };
        }

        /// <summary>
        /// Calcula una tabla de valores la cual contiene 30 valores.
        /// </summary>
        /// <returns>Retorna una matriz con los valores de (x, y)</returns>
        /// <example>
        /// <code>
        /// eq.TablaValores();  //  { { .... }, { .... } }
        /// </code>
        /// </example>
        public ICollection<ICollection<double>> TablaValores()
        {
            var centro = -B / (2 * A);
            var arr = new Collection<double>();
            for (double i = centro - 3; i <= centro + 3; i+=0.1) arr.Add(i);
            var arr1 = new Collection<double>();
            foreach (var x in arr) arr1.Add(A*Math.Pow(x, 2) + B * x + C);
            return new Collection<double>[] { arr, arr1 };
        }

        /// <summary>
        /// Calcula una tabla de valores la cual contiene los valores necesarios para graficar.
        /// </summary>
        /// <returns>Retorna una matriz con los valores de (x, y)</returns>
        /// <example>
        /// <code>
        /// eq.TablaValoresXl();  //  { { .... }, { .... } }
        /// </code>
        /// </example>
        public ICollection<ICollection<double>> TablaValoresXl()
        {
            var centro = -B / (2 * A);
            var arr = new Collection<double>();
            for (double i = centro - 12; i <= centro + 12; i += 0.01) arr.Add(i);
            var arr1 = new Collection<double>();
            foreach (var x in arr) arr1.Add(A * Math.Pow(x, 2) + B * x + C);
            return new Collection<double>[] { arr, arr1 };
        }

        /// <summary>
        /// Retorna la derivada de la ecuacion.
        /// </summary>
        /// <param name="n">el numero de derivada</param>
        /// <returns>retorna un string con la derivada [n]</returns>
        /// <example>
        /// <code>
        /// eq.Derivada(1);  //  "2x+3"
        /// </code>
        /// </example>
        public string Derivada(uint n)
        {
            var ret = $"d{n}f(x)/dx{n}=";
            var aa = 2 * A;
            if (n >= 3) {
                insertDiff(ret + "0", (int)n).Wait();
                return ret + "0";
            }
            if (n == 2) {
                insertDiff(ret + $"{aa}", (int)n).Wait();
                return ret + $"{aa}";
            }
            if (n == 1) {
                if (B > 0) {
                    insertDiff(ret + $"{aa}x+{B}", (int)n).Wait();
                    return ret + $"{aa}x+{B}";
                }
                if (B == 0) {
                    insertDiff(ret + $"{aa}x", (int)n).Wait();
                    return ret + $"{aa}x";
                }
            }
            insertDiff(ret + $"{aa}x{B}", (int)n).Wait();
            return ret + $"{aa}x{B}";
        }

        internal async Task insertEq()
        {
            using var command = Db.CreateCommand();
            command.CommandText = @"INSERT INTO `ecuacion` (`eq`) VALUES (@equa)";
            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@equa",
                DbType = DbType.String,
                Value = Eq
            });
            await command.ExecuteNonQueryAsync();
            Id = (int)command.LastInsertedId;
        }

        internal async Task insertSols(double sols)
        {
            using var command = Db.CreateCommand();
            command.CommandText = @"INSERT INTO `soluciones` (`sol`, `idEq`) VALUES (@sl, @eq)";
            command.Parameters.Add(new MySqlParameter {
                ParameterName = "@sl",
                DbType = DbType.String,
                Value = sols
            });
            command.Parameters.Add(new MySqlParameter {
                ParameterName = "@eq",
                DbType = DbType.Int32,
                Value = Id
            });
            await command.ExecuteNonQueryAsync();
        }

        internal async Task insertDiff(string dif, int grado)
        {
            using var command = Db.CreateCommand();
            command.CommandText = @"INSERT INTO `derivada` (`gradoDif`, `dif`, `idEq`) VALUES (@gr, @df, @eq)";
            command.Parameters.Add(new MySqlParameter {
                ParameterName = "@gr",
                DbType = DbType.Int32,
                Value = grado
            });
            command.Parameters.Add(new MySqlParameter {
                ParameterName = "@df",
                DbType = DbType.String,
                Value = dif
            });
            command.Parameters.Add(new MySqlParameter {
                ParameterName = "@eq",
                DbType = DbType.Int32,
                Value = Id
            });
            await command.ExecuteNonQueryAsync();
        }
    }
}

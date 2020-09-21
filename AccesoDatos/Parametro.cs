using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos
{
    using System.Data;

    /// <summary>
    /// Clase que representa los parametros que utilizan los procedimientos almacenados
    /// </summary>
    public class Parametro
    {

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">El nombre.</param>
        /// <param name="tipo">El tipo.</param>
        /// <param name="valor">El valor.</param>
        public Parametro(string nombre, DbType tipo, object valor)
        {
            this.Nombre = nombre;
            this.Tipo = tipo;
            this.Valor = valor;
        }

        /// <summary>
        /// Obtiene o establece el nombre.
        /// </summary>
        /// <value>
        /// El nombre.
        /// </value>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo.
        /// </summary>
        /// <value>
        /// El tipo.
        /// </value>
        public DbType Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece el valor.
        /// </summary>
        /// <value>
        /// El valor.
        /// </value>
        public object Valor { get; set; }
    }
}

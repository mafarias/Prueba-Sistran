using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos
{
    using System.Data;

    /// <summary>
    /// Interface que define los metodos que implementa la clase DAL
    /// </summary>
    public interface IAccesoDatos
    {
        /// <summary>
        /// Obtiene registros ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Coleccion de registros agrupados en un DataTable</returns>
        DataTable Consultar(string nombreProcedimiento, params Parametro[] parametros);

        /// <summary>
        /// Inserta un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Autonuumerico generado por la base de datos</returns>
        int Insertar(string nombreProcedimiento, params Parametro[] parametros);

        /// <summary>
        /// Actualiza un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Si o no se realizo la actualizacion</returns>
        bool Actualizar(string nombreProcedimiento, params Parametro[] parametros);

        /// <summary>
        /// Elimina un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Si o no se realizo la eliminacion</returns>
        bool Eliminar(string nombreProcedimiento, params Parametro[] parametros);

        /// <summary>
        /// Obtiene un valor escalar ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Valor de diferente tipo que devuelve el procedimiento almacenado</returns>
        object Escalar(string nombreProcedimiento, params Parametro[] parametros);
    }
}

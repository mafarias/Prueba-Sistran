using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace AccesoDatos
{
    


    /// <summary>
    /// Clase que realiza la conexion a la base de datos y ejecuta los procedimientos almacenados
    /// </summary>
    public abstract class DAL : IAccesoDatos
    {
        /// <summary>
        /// Objeto que representa la base de datos
        /// </summary>
        public Database BaseDatos { get; private set; }

        /// <summary>
        /// Objeto que representa los registros de la base de datos
        /// </summary>
        private DataTable registros = new DataTable();

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public DAL()
        {
            //DatabaseProviderFactory factory = new DatabaseProviderFactory();
            DatabaseProviderFactory factory = new DatabaseProviderFactory();

            //this.BaseDatos = factory.CreateDefault();

            this.BaseDatos = DatabaseFactory.CreateDatabase("Conexion");
            //this.BaseDatos = factory.CreateDefault();

        }


        public DbTransaction CrearTransaccion(IsolationLevel nivelAislamiento = IsolationLevel.ReadCommitted)
        {
            DbConnection conexion = this.BaseDatos.CreateConnection();
            conexion.Open();
            return conexion.BeginTransaction(nivelAislamiento);
        }

        /// <summary>
        /// Obtiene registros ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <returns>Coleccion de registros agrupados en un DataTable</returns>
        public DataTable Consultar(string nombreProcedimiento)
        {
            this.Datos(nombreProcedimiento);
            return this.registros;
        }

        /// <summary>
        /// Obtiene registros ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Coleccion de registros agrupados en un DataTable</returns>
        public DataTable Consultar(string nombreProcedimiento, params Parametro[] parametros)
        {
            this.Datos(nombreProcedimiento, parametros);
            return this.registros;
        }

        /// <summary>
        /// Inserta un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Autonuumerico generado por la base de datos</returns>
        public int Insertar(string nombreProcedimiento, params Parametro[] parametros)
        {
            return Convert.ToInt32(this.Escalar(nombreProcedimiento, parametros));
        }

        /// <summary>
        /// Inserta un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="transaccion">Transaccion.</param>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>
        /// Autonuumerico generado por la base de datos
        /// </returns>
        /// <remarks>
        /// Autor: Miguel Escamilla - CIELINGENIERIA\mescamilla 
        /// FechaDeCreacion: 07/05/2015
        /// </remarks>
        public int Insertar(DbTransaction transaccion, string nombreProcedimiento, params Parametro[] parametros)
        {
            return Convert.ToInt32(this.Escalar(transaccion, nombreProcedimiento, parametros));
        }

        /// <summary>
        /// Actualiza un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Si o no se realizo la actualizacion</returns>
        public bool Actualizar(string nombreProcedimiento, params Parametro[] parametros)
        {
            if (this.Ejecutar(nombreProcedimiento, parametros) >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Elimina un registro ejecutando un procedimiento almacenado con parametros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Si o no se realizo la eliminacion</returns>
        public bool Eliminar(string nombreProcedimiento, params Parametro[] parametros)
        {
            if (this.Ejecutar(nombreProcedimiento, parametros) >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Obtiene un valor escalar ejecutando un procedimiento almacenado con parametros
        /// </summary><
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Valor de diferente tipo que devuelve el procedimiento almacenado</returns>
        public object Escalar(string nombreProcedimiento, params Parametro[] parametros)
        {
            object resultado;
            using (DbConnection conexion = this.BaseDatos.CreateConnection())
            {
                conexion.Open();
                using (DbCommand comando = conexion.CreateCommand())
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandText = nombreProcedimiento;
                    foreach (Parametro param in parametros)
                    {
                        this.BaseDatos.AddInParameter(comando, param.Nombre, param.Tipo, param.Valor);
                    }

                    resultado = comando.ExecuteScalar();
                }

                conexion.Close();
                conexion.Dispose();
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene un valor escalar ejecutando un procedimiento almacenado con parametros y una transacción
        /// </summary>
        /// <param name="transaccion">Transaccion.</param>
        /// <param name="nombreProcedimiento">The nombre procedimiento.</param>
        /// <param name="parametros">The parametros.</param>
        /// <returns>
        /// Retorna el Resultado de la Operacion 
        /// </returns>
        /// <remarks>

        public object Escalar(DbTransaction transaccion, string nombreProcedimiento, params Parametro[] parametros)
        {
            object resultado;

            using (DbCommand comando = transaccion.Connection.CreateCommand())
            {
                comando.Transaction = transaccion;
                comando.CommandTimeout = 20000;
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = nombreProcedimiento;
                foreach (Parametro param in parametros)
                {
                    this.BaseDatos.AddInParameter(comando, param.Nombre, param.Tipo, param.Valor);
                }

                resultado = comando.ExecuteScalar();
            }

            return resultado;
        }

        /// <summary>
        /// Ejecuta una lista de procedimientos almacenados con parametros
        /// </summary>
        /// <param name="parametros">Lista de procedimientos almacenados y parametros.</param>
        /// <returns>Si o no se ejecutaron la lista de procedimientos almacenados</returns>
        public bool Transaccion(Dictionary<string, List<Parametro>> parametros)
        {
            bool ejecutada = false;
            using (DbConnection conexion = this.BaseDatos.CreateConnection())
            {
                conexion.Open();
                DbTransaction transaccion = conexion.BeginTransaction();
                try
                {
                    foreach (KeyValuePair<string, List<Parametro>> parametro in parametros)
                    {
                        using (DbCommand comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = parametro.Key;
                            foreach (Parametro param in parametro.Value)
                            {
                                this.BaseDatos.AddInParameter(comando, param.Nombre, param.Tipo, param.Valor);
                            }

                            this.BaseDatos.ExecuteNonQuery(comando, transaccion);
                        }
                    }

                    transaccion.Commit();
                    ejecutada = true;
                }
                catch
                {
                    transaccion.Rollback();
                }
                finally
                {
                    conexion.Close();
                    conexion.Dispose();
                }
            }

            return ejecutada;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parametros que devuelve un valor entero
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        /// <returns>Numero de confirmacion o error de ejecucion del procedimiento almacenado</returns>
        private int Ejecutar(string nombreProcedimiento, params Parametro[] parametros)
        {
            int resultado;
            using (DbConnection conexion = this.BaseDatos.CreateConnection())
            {
                conexion.Open();
                using (DbCommand comando = conexion.CreateCommand())
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandText = nombreProcedimiento;

                    foreach (Parametro param in parametros)
                    {
                        this.BaseDatos.AddInParameter(comando, param.Nombre, param.Tipo, param.Valor);
                    }

                    resultado = comando.ExecuteNonQuery();
                }

                conexion.Close();
                conexion.Dispose();
            }

            return resultado;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parametros que devuelve una coleccion de registros
        /// </summary>
        /// <param name="nombreProcedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Los parametros.</param>
        private void Datos(string nombreProcedimiento, params Parametro[] parametros)
        {
            this.registros = new DataTable();
            using (DbConnection conexion = this.BaseDatos.CreateConnection())
            {
                conexion.Open();
                using (DbCommand comando = conexion.CreateCommand())
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandText = nombreProcedimiento;

                    foreach (Parametro param in parametros)
                    {
                        this.BaseDatos.AddInParameter(comando, param.Nombre, param.Tipo, param.Valor);
                    }

                    using (SqlDataReader lector = comando.ExecuteReader() as SqlDataReader)
                    {
                        this.registros.Load(lector);
                    }
                }

                conexion.Close();
                conexion.Dispose();
            }
        }
    }
}

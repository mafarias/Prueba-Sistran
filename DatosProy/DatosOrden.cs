﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Dominio;

namespace DatosProy
{
    public class DatosOrden :DAL
    {
        public List<Orden> consultaordenes()
        {
            try
            {
                DataTable registros = new DataTable();
                List<Orden> lista = new List<Orden>();
                registros = Consultar("SP_ConsultarOrdenes");
                foreach (DataRow item in registros.Rows)
                {
                    Orden orden = new Orden
                    {
                        Id = int.Parse(item["Id"].ToString()),
                        Cliente = item["Cliente"].ToString(),
                        Oficina = item["Oficina"].ToString(),
                        producto = item["producto"].ToString(),
                        FechaIngreso = DateTime.Parse(item["FechaIngreso"].ToString())

                    };
                    lista.Add(orden);
                }
                return lista;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int CrearOrden(Orden orden)
        {
            try
            {
                Parametro cliente = new Parametro("@Cliente", DbType.String, orden.Cliente);
                Parametro Fecha = new Parametro("@FechaIngreso", DbType.DateTime, orden.FechaIngreso);
                Parametro producto = new Parametro("@Producto", DbType.String, orden.producto);
                Parametro oficina = new Parametro("@Oficina", DbType.String, orden.producto);

                int i = Insertar("SP_CrearOrden", cliente, Fecha, producto, oficina);
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Zona
    {
        public static ML.Result zona_Cine(int idZona)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"Zona_Cine {idZona}").AsEnumerable().ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var registro in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Zona = new ML.Zona();

                            cine.IdCine = registro.IdCine;
                            cine.Nombre = registro.Nombre;
                            cine.Direccion = registro.Direccion;
                            cine.Ventas = float.Parse(registro.Ventas.ToString());
                            cine.Zona.IdZona = registro.IdZona.Value;
                            cine.Zona.Nombre = registro.Nombre;

                            result.Objects.Add(cine);

                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.Ex = e;
            }
            return result;
        }
    }
}

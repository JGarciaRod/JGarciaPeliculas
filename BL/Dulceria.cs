using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Dulceria
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw("GetAllDulceria").ToList();

                    if(query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var registro in query)
                        {
                            ML.Dulceria dulceria = new ML.Dulceria();

                            dulceria.IdDulce = registro.IdDulce;
                            dulceria.Nombre = registro.Nombre;
                            dulceria.Precio = float.Parse(registro.Precio.ToString());
                            dulceria.Imagen = registro.Imagen;

                            result.Objects.Add(dulceria);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetById(int idDulce)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgarciaCineContext context= new DL.JgarciaCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw($"GetByIdDulceria {idDulce}").AsEnumerable().SingleOrDefault();

                    result.Object = new List<object>();
                    if (query != null)
                    {
                        ML.Dulceria dulceria = new ML.Dulceria();
                        dulceria.IdDulce = query.IdDulce;
                        dulceria.Nombre = query.Nombre;
                        dulceria.Precio = float.Parse(query.Precio.ToString());
                        dulceria.Imagen = query.Imagen;

                        result.Object = dulceria;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        //public static ML.Result GetByIdEF()
        //{
        //    ML.Result result = new ML.Result();
        //    try
        //    {
        //        using (DL.JgarciaCineContext context =  new )
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.ErrorMessage = ex.Message;
        //        result.Ex = ex;
        //    }
        //    return result;
        //}
    }
}

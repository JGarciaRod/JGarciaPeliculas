using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    var query = context.Cines.FromSqlRaw("GetAllCines").ToList();

                    if(query != null)
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
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Correct = false;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetById(int IdCine)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"GetByIdCine {IdCine}").AsEnumerable().SingleOrDefault();

                    result.Object = new List<object>();

                    if (query != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.Zona = new ML.Zona();

                        cine.IdCine = query.IdCine;
                        cine.Nombre = query.Nombre;
                        cine.Direccion = query.Direccion;
                        cine.Ventas = float.Parse(query.Ventas.ToString());
                        cine.Zona.IdZona = query.IdZona.Value;
                        cine.Zona.Nombre = query.Nombre;

                        result.Object = cine;

                        result.Correct = true;
                    }
                }
            }
            catch(Exception ex)
            {
                result.ErrorMessage=ex.Message;
                result.Correct = false;
                result.Ex = ex;
            }
            return result;
        }



        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"AddCine '{cine.Nombre }' , '{cine.Direccion }',{cine.Zona.IdZona}, {cine.Ventas} ");

                    if(rowAffected > 0)
                    {
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
                result.ErrorMessage =ex.Message;
                result.Correct = false;
                result.Ex = ex;
            }
            return result;
        }


        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    int rowAffected = context.Database.ExecuteSqlRaw($"UpdateCine  {cine.IdCine},'{cine.Nombre}','{cine.Direccion}',{cine.Zona.IdZona}, {cine.Ventas} ");

                    if (rowAffected > 0)
                    {
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

        public static ML.Result Dell(int IdCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    int rowAfected = context.Database.ExecuteSqlRaw($"DeleteCine {IdCine}");

                    if(rowAfected > 0)
                    {
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
                result.ErrorMessage =ex.Message;
                result.Correct = false;
                result.Ex = ex;
            }
            return result;
        }

    }
}
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static string Encrip256(string pass)
        {
            string hash = string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));

                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }
            return hash;
        }


        public static ML.Result GetByEmail(string email)
        {
            ML.Result result = new ML.Result();
            try
            {
                DL.JgarciaCineContext context = new DL.JgarciaCineContext();

                var query = context.Usuarios.FromSqlRaw($"GetByEmail '{email}' ").AsEnumerable().SingleOrDefault();

                result.Object = new List<object>();

                if(query != null)
                {
                    ML.Usuario usuario = new ML.Usuario();
                    
                    usuario.Email = query.Email;
                    usuario.Password = query.Password;

                    result.Object = usuario;

                    result.Correct = true;
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

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    usuario.Password = Encrip256(usuario.Password);
                    int rowAffected = context.Database.ExecuteSqlRaw($"UsaurioAdd '{usuario.UserName}','{usuario.Email}','{usuario.Nombre}', '{usuario.Password}'");
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
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result Actualizar(string email, string password)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JgarciaCineContext context = new DL.JgarciaCineContext())
                {
                    password = Encrip256(password);
                    int rowAffwctwd = context.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{email}','{password}'");
                    if(rowAffwctwd > 0)
                    {
                        result.Correct = true;
                    }
                    else { result.Correct = false; }
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
    }
}

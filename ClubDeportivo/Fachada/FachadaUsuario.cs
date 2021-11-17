using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorio;

namespace Fachada
{
    public class FachadaUsuario
    {
        public Usuario Login(string email, string password)
        {
            RepoUsuario repoUser = new RepoUsuario();
            Usuario user = repoUser.BuscarPorEmail(email);
            if (user != null)
            {
                string desEncriptada = DesEncriptar(user.Password);
                if (desEncriptada == password)
                {
                    return user;
                }
                else
                {
                    return user = null;
                }
            }
            else
            {
                return user = null;
            }
        }

        public string AltaUsuario(string email, string password)
        {
            string msj = ValidarCamposAltaUsuario(email, password);
            
            if (msj == "")
            {
                string encriptada = Encriptar(password);
                Usuario user = new Usuario()
                {
                    Email = email,
                    Password = encriptada
                };
                RepoUsuario repoUser = new RepoUsuario();
                
                if (repoUser.BuscarPorEmail(email) == null)
                {
                    if (!repoUser.Alta(user))
                    {
                        msj = "Error al cargar usuario";
                    }
                }
                else
                {
                    msj = "Este email ya esta en uso";
                }
            }
            return msj;
        }

        public static string ValidarCamposAltaUsuario(string email, string password)
        {
            string msj = "";

            if (email == "" || password == "")
            {
                msj = "Deben completarse los campos";
            }
            else if (!ValidarEmail(email))
            {
                msj = "Formato de e-mail invalido";
            }
            else if (!ValidarPassword(password))
            {
                msj = "La contraseña no cumple con los requisitos de seguridad";
            }

            return msj;
        }

        //formato email: un @ y punto luego del mismo
        public static bool ValidarEmail(string email)
        {
            bool ok = false;         
            int cantArroba = 0;
            bool punto = false;

            for (int i = 0; i < email.Length; i++)
            {
               
                if (email[i].ToString() == "@")
                {
                    cantArroba++;
                    for (int j = i+1; j < email.Length; j++)
                    {
                        if (email[j].ToString() == ".")
                        {
                            punto = true;
                        }
                    }
                }
            }
            if (cantArroba == 1 && punto)
            {
                ok = true;
            }

            return ok;
        }

        //contraseña con al menos 6 caracteres que incluyan letras mayúsculas y minúsculas(al menos una de cada una) y dígitos(0 al 9)
        public static bool ValidarPassword(string password)
        {
            bool ok = false;            

            string letrasMayus = "ABCDEFGHIJKLMNÑOPKRSTUVWXYZ";
            string letrasMin = letrasMayus.ToLower();
            string numeros = "0123456789";
            string espacio = " ";

            bool mayus = Comparador(password, letrasMayus);
            bool minus = Comparador(password, letrasMin);
            bool num = Comparador(password, numeros);
            bool esp = !Comparador(password, espacio);

            if (password.Length >= 6 && mayus && minus && num && esp)
            {
                ok = true;
            }

            return ok;
        }

        public static bool Comparador(string t1, string t2)
        {
            bool ok = false;
            for (int i = 0; i < t1.Length; i++)
            {
                for (int j = 0; j < t2.Length; j++)
                {
                    if (t2[j] == t1[i])
                    {
                        ok = true;
                    }
                }
            }
            return ok;
        }
        public static string Encriptar(string password)
        {
            string txt;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(password);
            txt = Convert.ToBase64String(encryted);
            return txt;
        }

        public static string DesEncriptar(string password)
        {
            string txt;
            byte[] decryted = Convert.FromBase64String(password);
            txt = System.Text.Encoding.Unicode.GetString(decryted);
            return txt;
        }
    }
}

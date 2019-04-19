using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class Datos
    {
        private static Datos _ins = null;
        public static Datos ins
        {
            get
            {
                if (_ins == null) _ins = new Datos();
                return _ins;
            }
        }

        public Dictionary<string, object> Diccio = new Dictionary<string, object>();
        public Dictionary<string, bool> Diccio2 = new Dictionary<string, bool>();
        public List<Estampa> mostrar = new List<Estampa>();

        public static string[] Retornar(char[] Linea)
        {
            string[] Info = new string[3];
            string temporal = string.Empty;
            int contador = 0;

            bool comillas = false;

            for (int a = 0; a < Linea.Length; a++)
            {
                if (comillas)
                {
                    if (Linea[a] != '"')
                    {
                        temporal = temporal + Linea[a];
                    }
                    else
                    {
                        comillas = false;
                    }
                }
                else
                {
                    if (Linea[a] == '"')
                    {
                        comillas = true;
                    }
                    else if (Linea[a] == ',')
                    {
                        Info[contador] = temporal;
                        temporal = "";
                        contador++;
                    }
                    else if (Linea[a] != ',')
                    {
                        temporal = temporal + Linea[a];
                        if (a == (Linea.Length - 1))
                        {
                            Info[contador] = temporal;
                        }
                    }
                }
            }
            return Info;
        }

        public string path;
        public void GuardarArchivo(List<Estampa> Temporal)
        {
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine("Equipo,Jugador,Cantidad");
                foreach (var item in Temporal)
                {
                    var separar = item.nombre.Split('_');
                    sw.WriteLine($"{separar[0]},{separar[1]},{item.cantidad}");
                }
            }
        }
    }
}
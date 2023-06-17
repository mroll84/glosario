using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glosario.Infraestructure
{
    public static class Utiles
    {


        public static string GetParametroDeConfiguracion(string nombreParametro)
        {
            string resultado = String.Empty;

            try
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");

                var configuracion = builder.Build();

                resultado = configuracion.GetValue<string>(nombreParametro);

                if (resultado == null)
                    resultado = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public static string GetMensajeCompletoDeError(this Exception ex)
        {
            string resultado = ex.InnerException == null
                 ? ex.Message
                 : ex.Message + " --> " + ex.InnerException.GetMensajeCompletoDeError();

            return resultado;
        }


        public static EstructuraMensajeDeError GetMensajeCompletoDeError(this Exception ex, bool incluyeDetalle = true)
        {
            EstructuraMensajeDeError resultado = new EstructuraMensajeDeError();
            resultado.mensajeError = ex.InnerException == null
                 ? ex.Message
                 : ex.Message + " --> " + ex.InnerException.GetMensajeCompletoDeError();

            resultado.stackTrace = String.Empty;

            if (ex.StackTrace != null && ex.StackTrace.Trim().Length > 0)
            {
                resultado.stackTrace = ex.StackTrace.Trim();

                if (incluyeDetalle)
                    resultado.mensajeError += ". " + resultado.stackTrace;
            }

            resultado.trazaErrorLinq = String.Empty;

            return resultado;
        }
    }

    public class EstructuraMensajeDeError
    {
        public string mensajeError { get; set; }
        public string stackTrace { get; set; }
        public string trazaErrorLinq { get; set; }
    }

}

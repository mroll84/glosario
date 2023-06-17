using Glosario.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Glosario.Infraestructure
{
    public class TerminoBLL : ITermino, IDisposable
    {
        private readonly ProcesoGlosarioContext _context;        

        public TerminoBLL(ProcesoGlosarioContext contexto)
        {
            _context = contexto ?? new ProcesoGlosarioContext();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<string> AddTermino(TerminoSet termino)
        {
            string resultado = "";

            try
            {
                if (termino != null)
                {
                    foreach (PropertyInfo prop in termino.GetType().GetProperties())
                    {
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                        if (type == typeof(string) && prop.GetValue(termino) == null)
                        {
                            prop.SetValue(termino, "");
                        }
                    }

                    await _context.TerminoSets.AddAsync(termino).ConfigureAwait(false);
                }
                else
                    throw new ArgumentNullException(nameof(termino));
            }
            catch (Exception ex)
            {
                resultado = $"Error al intentar añadir el término con la estructura: " + JsonConvert.SerializeObject(termino);
            }

            return resultado;
        }

        public async Task<string> DeleteTermino(TerminoSet termino)
        {
            string resultado = "";

            try
            {
                if (termino != null)
                    _context.TerminoSets.Remove(termino);
            }
            catch (Exception ex)
            {
                resultado = $"Error al intentar eliminar el término con la estructura: " + JsonConvert.SerializeObject(termino);
            }

            return resultado;
        }

        public async Task<IList<TerminoSet>> GetAll(string comienzaPor = null)
        {
            IList<TerminoSet> resultado = null;

            if (String.IsNullOrEmpty(comienzaPor))
                resultado = await _context.TerminoSets.OrderBy(x => x.Nombre).ToListAsync();
            else
            {
                comienzaPor = comienzaPor.Trim().ToUpper();
                resultado = await _context.TerminoSets.Where(x => x.Nombre.ToUpper().StartsWith(comienzaPor)).OrderBy(x=>x.Nombre).ToListAsync();
            }

            if (!resultado.Any())
                resultado = null;

            return resultado;
        }

        public async Task<TerminoSet> GetById(int idTermino)
        {
            TerminoSet resultado = null;

            if (idTermino > 0)
                resultado = await _context.TerminoSets.Where(x => x.IdTermino == idTermino).FirstOrDefaultAsync();

            return resultado;
        }

        public async Task<string> Save()
        {
            string resultado = String.Empty;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                resultado = "Error al intentar guardar los cambios";
                resultado += ", " + ex.GetMensajeCompletoDeError(true).mensajeError;
            }

            return resultado;
        }

        public async Task<string> UpdateTermino(TerminoSet termino)
        {
            string resultado = String.Empty;

            try
            {
                if (termino != null)
                {
                    var registro = await _context.TerminoSets.Where(x => x.IdTermino == termino.IdTermino).FirstOrDefaultAsync();

                    if (registro != null)
                        _context.Entry(registro).CurrentValues.SetValues(termino);
                }
            }
            catch (Exception ex)
            {
                resultado = "Error al intentar actualizar el término, " + ex.GetMensajeCompletoDeError(true).mensajeError;
            }

            return resultado;
        }
    }
}

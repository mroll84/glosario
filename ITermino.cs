using Glosario.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glosario.Infraestructure
{
    public interface ITermino
    {
        Task<string> AddTermino(TerminoSet termino);
        Task<TerminoSet> GetById(int idTermino);
        Task<string> UpdateTermino(TerminoSet termino);
        Task<string> DeleteTermino(TerminoSet termino);
        Task<string> Save();
        Task<IList<TerminoSet>> GetAll(string comienzaPor=null);

    }
}

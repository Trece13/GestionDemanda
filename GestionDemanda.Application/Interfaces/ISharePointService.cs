using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Application.Interfaces
{
    public interface ISharePointService
    {
        Task<string> SubirArchivoAsync(string nombreArchivo, Stream contenido);
    }
}

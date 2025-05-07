using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Shared.DTOs
{
    public class SolicitudFiltroDto
    {
        public string? Pais { get; set; }
        public string? TipoSolicitud { get; set; }
        public string? GerenciaN1 { get; set; }
        public string? GerenciaN2 { get; set; }
        public string? ResponsableNombre { get; set; }
        public string? ResponsableCorreo { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        // Paginación
        public int Pagina { get; set; } = 1;
        public int ElementosPorPagina { get; set; } = 10;
    }
}

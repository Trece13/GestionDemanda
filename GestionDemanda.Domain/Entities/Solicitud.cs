using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Domain.Entities
{
    public class Solicitud
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Pais { get; set; }
        public string TipoSolicitud { get; set; }
        public string GerenciaN1 { get; set; }
        public string GerenciaN2 { get; set; }
        public string AutorizadoPor { get; set; }
        public string ResponsableNombre { get; set; }
        public string ResponsableCorreo { get; set; }
        public string NombreProyectoRelacionado { get; set; }
        public string Objetivo { get; set; }
        public string DescripcionFuncional { get; set; }
        public string DescripcionTecnica { get; set; }
        public string ImpactoCualitativo { get; set; }
        public string ImpactoCuantitativo { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "Pendiente Aprobación";
    }
}

using GestionDemanda.Application.Interfaces;
using GestionDemanda.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionDemanda.Domain.Entities;
using System.Threading.Tasks;

namespace GestionDemanda.Application.UseCases
{
    public class RegistrarSolicitudHandler : IRegistrarSolicitudHandler
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly ISharePointService _sharePoint;
        private readonly IAzureDevOpsService _devOps;

        public RegistrarSolicitudHandler(
            ISolicitudRepository solicitudRepository)
            //IPdfGenerator pdfGenerator,
            //ISharePointService sharePoint,
            //IAzureDevOpsService devOps)
        {
            _solicitudRepository = solicitudRepository;
            //_pdfGenerator = pdfGenerator;
            //_sharePoint = sharePoint;
            //_devOps = devOps;
        }

        public async Task<string> HandleAsync(SolicitudDto dto)
        {
            var solicitud = new Solicitud
            {
                Pais = dto.Pais,
                TipoSolicitud = dto.TipoSolicitud,
                GerenciaN1 = dto.GerenciaN1,
                GerenciaN2 = dto.GerenciaN2,
                AutorizadoPor = dto.AutorizadoPor,
                ResponsableNombre = dto.ResponsableNombre,
                ResponsableCorreo = dto.ResponsableCorreo,
                Titulo = dto.Titulo,
                NombreProyectoRelacionado = dto.NombreProyectoRelacionado,
                Objetivo = dto.Objetivo,
                DescripcionFuncional = dto.DescripcionFuncional,
                DescripcionTecnica = dto.DescripcionTecnica,
                ImpactoCualitativo = dto.ImpactoCualitativo,
                ImpactoCuantitativo = dto.ImpactoCuantitativo,
                FechaEntrega = dto.FechaEntrega
            };

            await _solicitudRepository.GuardarAsync(solicitud);

            //Stream pdf = _pdfGenerator.GenerarPdf(solicitud);
            //var urlPdf = await _sharePoint.SubirArchivoAsync($"{solicitud.Id}.pdf", pdf);
            //var urlPdf = "www.datadummie.com";
            
            
            
            //var workItemId = await _devOps.CrearWorkItemAsync(solicitud, urlPdf);

            return "";
        }
    }
}

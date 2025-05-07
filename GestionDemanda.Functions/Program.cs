using GestionDemanda.Application.Interfaces;
using GestionDemanda.Application.UseCases;
using GestionDemanda.Infrastructure.Repositories;
using GestionDemanda.Infrastructure.Services;
using GestionDemanda.Infrastructure.Simulated;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

// Aquí puedes registrar tus servicios (inyección de dependencias)
builder.Services.AddScoped<IRegistrarSolicitudHandler, RegistrarSolicitudHandler>();
builder.Services.AddScoped<ConsultarSolicitudesHandler>();
//builder.Services.AddSingleton<ISolicitudRepository, SolicitudRepositorySimulado>();
builder.Services.AddSingleton<ISolicitudRepository, SolicitudRepositoryCosmosDb>();
builder.Services.AddSingleton<IPdfGenerator, PdfGeneratorSimulado>();
builder.Services.AddSingleton<ISharePointService, SharePointService>();
//builder.Services.AddSingleton<ISharePointService, SharePointServiceSimulado>();
builder.Services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();
//builder.Services.AddSingleton<IAzureDevOpsService, AzureDevOpsServiceSimulado>();
builder.Services.AddSingleton<IEmailService, EmailServiceSimulado>();

builder.ConfigureFunctionsWebApplication();

// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();

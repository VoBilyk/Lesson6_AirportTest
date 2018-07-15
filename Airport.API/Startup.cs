using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Airport.DAL;
using Airport.DAL.Interfaces;
using Airport.DAL.Entities;
using Airport.BLL.Interfaces;
using Airport.BLL.Services;
using Airport.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace Airport.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Instance injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAeroplaneService, AeroplaneService>();
            services.AddScoped<IAeroplaneTypeService, AeroplaneTypeService>();
            services.AddScoped<ICrewService, CrewService>();
            services.AddScoped<IPilotService, PilotService>();
            services.AddScoped<IStewardessService, StewardessService>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IDepartureService, DepartureService>();
            services.AddScoped(_ => MapperConfiguration().CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public MapperConfiguration MapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AeroplaneType, AeroplaneTypeDto>();
                cfg.CreateMap<AeroplaneTypeDto, AeroplaneType>();

                cfg.CreateMap<Pilot, PilotDto>();
                cfg.CreateMap<PilotDto, Pilot>();

                cfg.CreateMap<Stewardess, StewardessDto>();
                cfg.CreateMap<StewardessDto, Stewardess>();

                cfg.CreateMap<AeroplaneDto, Aeroplane>()
                    .ForMember(model => model.AeroplaneType, dto => dto.Ignore());
                cfg.CreateMap<Aeroplane, AeroplaneDto>()
                    .ForMember(dto => dto.AeroplaneTypeId, model => model.MapFrom(m => m.AeroplaneType.Id));

                cfg.CreateMap<CrewDto, Crew>()
                    .ForMember(model => model.Pilot, dto => dto.Ignore())
                    .ForMember(model => model.Stewardesses, dto => dto.Ignore());
                cfg.CreateMap<Crew, CrewDto>()
                    .ForMember(dto => dto.PilotId, model => model.MapFrom(m => m.Pilot.Id))
                    .ForMember(dto => dto.StewardessesId, model => model.MapFrom(m => m.Stewardesses.Select(s => s.Id)));

                cfg.CreateMap<DepartureDto, Departure>()
                    .ForMember(model => model.Airplane, dto => dto.Ignore())
                    .ForMember(model => model.Crew, dto => dto.Ignore());
                cfg.CreateMap<Departure, DepartureDto>()
                    .ForMember(dto => dto.AirplaneId, model => model.MapFrom(m => m.Airplane.Id))
                    .ForMember(dto => dto.CrewId, model => model.MapFrom(m => m.Crew.Id));

                cfg.CreateMap<FlightDto, Flight>()
                    .ForMember(model => model.Tickets, dto => dto.Ignore());
                cfg.CreateMap<Flight, FlightDto>()
                    .ForMember(dto => dto.TicketsId, model => model.MapFrom(m => m.Tickets.Select(t => t.Id)));

                cfg.CreateMap<TicketDto, Ticket>()
                    .ForMember(model => model.Flight, dto => dto.Ignore());
                cfg.CreateMap<Ticket, TicketDto>()
                    .ForMember(dto => dto.FlightId, model => model.MapFrom(m => m.Flight.Id));
            });

            return config;
        }
    }
}

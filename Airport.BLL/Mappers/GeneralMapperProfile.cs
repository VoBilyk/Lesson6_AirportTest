﻿using System.Linq;
using AutoMapper;
using Airport.DAL.Entities;
using Airport.Shared.DTO;


namespace Airport.BLL.Mappers
{
    public class GeneralMapperProfile : Profile
    {
        public GeneralMapperProfile()
        {
            CreateMap<AeroplaneType, AeroplaneTypeDto>();
            CreateMap<AeroplaneTypeDto, AeroplaneType>();

            CreateMap<Pilot, PilotDto>();
            CreateMap<PilotDto, Pilot>();

            CreateMap<Stewardess, StewardessDto>();
            CreateMap<StewardessDto, Stewardess>()
                .ForMember(model => model.Crew, dto => dto.Ignore());

            CreateMap<AeroplaneDto, Aeroplane>()
                .ForMember(model => model.AeroplaneType, dto => dto.Ignore())
                .ForMember(model => model.LifetimeFullForm, dto => dto.MapFrom(m => m.Lifetime));

            CreateMap<Aeroplane, AeroplaneDto>()
                .ForMember(dto => dto.AeroplaneTypeId, model => model.MapFrom(m => m.AeroplaneType.Id))
                .ForMember(dto => dto.Lifetime, model => model.MapFrom(m => m.LifetimeFullForm));

            CreateMap<CrewDto, Crew>()
                .ForMember(model => model.Pilot, dto => dto.Ignore())
                .ForMember(model => model.Stewardesses, dto => dto.Ignore());
            CreateMap<Crew, CrewDto>()
                .ForMember(dto => dto.PilotId, model => model.MapFrom(m => m.Pilot.Id))
                .ForMember(dto => dto.StewardessesId, model => model.MapFrom(m => m.Stewardesses.Select(s => s.Id)));

            CreateMap<DepartureDto, Departure>()
                .ForMember(model => model.Airplane, dto => dto.Ignore())
                .ForMember(model => model.Crew, dto => dto.Ignore());

            CreateMap<Departure, DepartureDto>()
                .ForMember(dto => dto.AirplaneId, model => model.MapFrom(m => m.Airplane.Id))
                .ForMember(dto => dto.CrewId, model => model.MapFrom(m => m.Crew.Id));

            CreateMap<FlightDto, Flight>()
                .ForMember(model => model.Tickets, dto => dto.Ignore());
            CreateMap<Flight, FlightDto>()
                .ForMember(dto => dto.TicketsId, model => model.MapFrom(m => m.Tickets.Select(t => t.Id)));

            CreateMap<TicketDto, Ticket>()
                .ForMember(model => model.Flight, dto => dto.Ignore());
            CreateMap<Ticket, TicketDto>()
                .ForMember(dto => dto.FlightId, model => model.MapFrom(m => m.Flight.Id));
        }
    }
}

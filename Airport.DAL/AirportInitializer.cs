﻿using Airport.DAL.Entities;
using Bogus;
using System;
using System.Linq;

namespace Airport.DAL
{
    public static class AirportInitializer
    {
        public static void IntializateIfEmpty(AirportContext context)
        {
            if(context.Aeroplanes.Any()
                || context.AeroplaneTypes.Any()
                || context.Crews.Any()
                || context.Departures.Any()
                || context.Flights.Any()
                || context.Tickets.Any()
                || context.Pilots.Any()
                || context.Stewardesses.Any()
                )
            {
                return;
            }

            var pilotFaker = new Faker<Pilot>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.FirstName, f => f.Name.FirstName(Bogus.DataSets.Name.Gender.Male))
                .RuleFor(o => o.SecondName, f => f.Name.LastName())
                .RuleFor(o => o.BirthDate, f => f.Date.Past(30, new DateTime(1990, 1, 1)).ToUniversalTime())
                .RuleFor(o => o.Experience, f => f.Random.Number(5, 40));

            var stewardessFaker = new Faker<Stewardess>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.FirstName, f => f.Name.FirstName(Bogus.DataSets.Name.Gender.Female))
                .RuleFor(o => o.SecondName, f => f.Name.LastName())
                .RuleFor(o => o.BirthDate, f => f.Date.Past(20, new DateTime(2000, 1, 1)).ToUniversalTime());

            var crewFaker = new Faker<Crew>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Pilot, f => pilotFaker.Generate())
                .RuleFor(o => o.Stewardesses, f => stewardessFaker.Generate(Randomizer.Seed.Next(1, 4)));

            var aeroplaneTypeFaker = new Faker<AeroplaneType>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Model, f => "Boeing-" + f.Random.Number(100, 999))
                .RuleFor(o => o.Places, f => f.Random.Number(20, 300))
                .RuleFor(o => o.Carrying, f => f.Random.Number(20000, 200000));

            var aeroplaneFaker = new Faker<Aeroplane>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.AeroplaneType, f => aeroplaneTypeFaker.Generate())
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.LifetimeFullForm, f => f.Date.Timespan(new TimeSpan(5*365, 0, 0, 0)));

            var departureFaker = new Faker<Departure>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Airplane, f => aeroplaneFaker.Generate())
                .RuleFor(o => o.Crew, f => crewFaker.Generate())
                .RuleFor(o => o.Time, f => f.Date.Soon(200).ToUniversalTime());

            var ticketFaker = new Faker<Ticket>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Price, f => f.Random.Number(20, 100));

            var flightFaker = new Faker<Flight>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Name, f => $"{f.Random.String(4, 4, 'A', 'Z')}-{f.Random.Number(100, 999)}")
                .RuleFor(o => o.DeparturePoint, f => f.Address.City())
                .RuleFor(o => o.Destinition, f => f.Address.City())
                .RuleFor(o => o.Tickets, f => ticketFaker.Generate(Randomizer.Seed.Next(0, 10)))
                .RuleFor(o => o.DepartureTime, f => f.Date.Soon(365).ToUniversalTime())
                .RuleFor(o => o.ArrivalTime, (f, o) => (o.DepartureTime + f.Date.Timespan(new TimeSpan(8, 0, 0))).ToUniversalTime());


            context.Pilots.AddRange(pilotFaker.Generate(20));
            context.Stewardesses.AddRange(stewardessFaker.Generate(20));
            context.Crews.AddRange(crewFaker.Generate(20));

            context.Flights.AddRange(flightFaker.Generate(20));
            context.SaveChanges();

            context.Aeroplanes.AddRange(aeroplaneFaker.Generate(20));
            context.AeroplaneTypes.AddRange(aeroplaneTypeFaker.Generate(5));
            context.Departures.AddRange(departureFaker.Generate(20));

            context.SaveChanges();
        }
    }
}

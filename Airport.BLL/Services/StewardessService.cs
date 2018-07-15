using System;
using System.Collections.Generic;
using AutoMapper;
using Airport.BLL.Interfaces;
using Airport.DAL.Interfaces;
using Airport.DAL.Entities;
using Airport.Shared.DTO;


namespace Airport.BLL.Services
{
    public class StewardessService : IStewardessService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public StewardessService(IUnitOfWork uow, IMapper mapper)
        {
            this.db = uow;
            this.mapper = mapper;
        }


        public StewardessDto Get(Guid id)
        {
            return mapper.Map<Stewardess, StewardessDto>(db.StewardessRepositiry.Get(id));
        }

        public List<StewardessDto> GetAll()
        {
            return mapper.Map<List<Stewardess>, List<StewardessDto>>(db.StewardessRepositiry.GetAll());
        }

        public StewardessDto Create(StewardessDto stewardessDto)
        {
            var stewardess = mapper.Map<StewardessDto, Stewardess>(stewardessDto);
            stewardess.Id = Guid.NewGuid();

            db.StewardessRepositiry.Create(stewardess);
            db.SaveChanges();

            return mapper.Map<Stewardess, StewardessDto>(stewardess);
        }

        public StewardessDto Update(Guid id, StewardessDto stewardessDto)
        {
            var stewardess = mapper.Map<StewardessDto, Stewardess>(stewardessDto);
            stewardess.Id = id;

            db.StewardessRepositiry.Update(stewardess);
            db.SaveChanges();

            return mapper.Map<Stewardess, StewardessDto>(stewardess);
        }

        public void Delete(Guid id)
        {
            db.StewardessRepositiry.Delete(id);
            db.SaveChanges();
        }

        public void DeleteAll()
        {
            db.StewardessRepositiry.Delete();
            db.SaveChanges();
        }
    }
}
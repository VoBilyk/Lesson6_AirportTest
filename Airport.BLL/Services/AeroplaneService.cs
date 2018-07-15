using System;
using System.Collections.Generic;
using System.Text;
using Airport.BLL.Interfaces;
using Airport.DAL.Interfaces;
using Airport.DAL.Entities;
using Airport.Shared.DTO;
using AutoMapper;

namespace Airport.BLL.Services
{
    public class AeroplaneService : IAeroplaneService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public AeroplaneService(IUnitOfWork uow, IMapper mapper)
        {
            this.db = uow;
            this.mapper = mapper;
        }


        public AeroplaneDto Get(Guid id)
        {
            return mapper.Map<Aeroplane, AeroplaneDto>(db.AeroplaneRepository.Get(id));
        }

        public List<AeroplaneDto> GetAll()
        {
            return mapper.Map<List<Aeroplane>, List<AeroplaneDto>>(db.AeroplaneRepository.GetAll());
        }

        public AeroplaneDto Create(AeroplaneDto aeroplaneDto)
        {
            var aeroplane = mapper.Map<AeroplaneDto, Aeroplane>(aeroplaneDto);

            aeroplane.Id = Guid.NewGuid();
            aeroplane.AeroplaneType = db.AeroplaneTypeRepository.Get(aeroplaneDto.AeroplaneTypeId);
            db.AeroplaneRepository.Create(aeroplane);
            db.SaveChanges();

            return mapper.Map<Aeroplane, AeroplaneDto>(aeroplane);
        }

        public AeroplaneDto Update(Guid id, AeroplaneDto aeroplaneDto)
        {
            var aeroplane = mapper.Map<AeroplaneDto, Aeroplane>(aeroplaneDto);

            aeroplane.Id = id;
            aeroplane.AeroplaneType = db.AeroplaneTypeRepository.Get(aeroplaneDto.AeroplaneTypeId);
            db.AeroplaneRepository.Update(aeroplane);
            db.SaveChanges();

            return mapper.Map<Aeroplane, AeroplaneDto>(aeroplane);
        }

        public void Delete(Guid id)
        {
            db.AeroplaneRepository.Delete(id);
            db.SaveChanges();
        }

        public void DeleteAll()
        {
            db.AeroplaneRepository.Delete();
            db.SaveChanges();
        }
    }
}

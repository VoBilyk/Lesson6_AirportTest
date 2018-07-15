using System;
using System.Collections.Generic;
using Airport.BLL.Interfaces;
using Airport.DAL.Interfaces;
using Airport.DAL.Entities;
using Airport.Shared.DTO;
using AutoMapper;

namespace Airport.BLL.Services
{
    public class AeroplaneTypeService : IAeroplaneTypeService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public AeroplaneTypeService(IUnitOfWork uow, IMapper mapper)
        {
            this.db = uow;
            this.mapper = mapper;
        }


        public AeroplaneTypeDto Get(Guid id)
        {
            return mapper.Map<AeroplaneType, AeroplaneTypeDto>(db.AeroplaneTypeRepository.Get(id));
        }

        public List<AeroplaneTypeDto> GetAll()
        {
            return mapper.Map<List<AeroplaneType>, List<AeroplaneTypeDto>>(db.AeroplaneTypeRepository.GetAll());
        }

        public AeroplaneTypeDto Create(AeroplaneTypeDto aeroplaneTypeDto)
        {
            var aeroplaneType = mapper.Map<AeroplaneTypeDto, AeroplaneType>(aeroplaneTypeDto);
            aeroplaneType.Id = Guid.NewGuid();

            db.AeroplaneTypeRepository.Create(aeroplaneType);
            db.SaveChanges();

            return mapper.Map<AeroplaneType, AeroplaneTypeDto>(aeroplaneType);
        }

        public AeroplaneTypeDto Update(Guid id, AeroplaneTypeDto aeroplaneTypeDto)
        {
            var aeroplaneType = mapper.Map<AeroplaneTypeDto, AeroplaneType>(aeroplaneTypeDto);
            aeroplaneType.Id = id;

            db.AeroplaneTypeRepository.Update(aeroplaneType);
            db.SaveChanges();

            return mapper.Map<AeroplaneType, AeroplaneTypeDto>(aeroplaneType);
        }

        public void Delete(Guid id)
        {
            db.AeroplaneTypeRepository.Delete(id);
            db.SaveChanges();
        }

        public void DeleteAll()
        {
            db.AeroplaneTypeRepository.Delete();
            db.SaveChanges();
        }
    }
}
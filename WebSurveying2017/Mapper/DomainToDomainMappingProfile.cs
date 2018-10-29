using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Mapper
{
    public class DomainToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToDomainMappings"; }
        }

        public DomainToDomainMappingProfile()
        {
            CreateMap<Notification, Notification>();


        }

    }
}
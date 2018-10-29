using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace WebSurveying2017.Mapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            
            AutoMapper.Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
                x.AddProfile<DomainToDomainMappingProfile>();
            });
        }
    }
}
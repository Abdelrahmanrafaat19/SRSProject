using AutoMapper;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Profiles
{
    public class WeeklyHolidayProfile : Profile
    {
        public WeeklyHolidayProfile()
        {
            CreateMap<WeekHolidayEntity, WeeklyHolidayDto>().ReverseMap();
        }
    }
}

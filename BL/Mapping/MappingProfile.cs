using AutoMapper;
using BL.DTOConfiguration;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TbCarrier, DTOCarrier>().ReverseMap();
            CreateMap<TbCity, DTOCity>().ReverseMap();
            CreateMap<TbCountry, DTOCountry>().ReverseMap();
            CreateMap<TbPaymentMethod, DTOPaymentMethod>().ReverseMap();
            CreateMap<TbSetting, DTOSetting>().ReverseMap();
            CreateMap<TbShippingType, DTOShippingType>().ReverseMap();
            CreateMap<TbShippment, DTOShippment>().ReverseMap();
            CreateMap<TbRefreshTokens, DTORefreshToken>().ReverseMap();
            CreateMap<TbShippmentStatus, DTOShippmentStatus>().ReverseMap();
            CreateMap<TbSubscriptionPackage, DTOSubscriptionPackage>().ReverseMap();
            CreateMap<TbUserReceiver, DTOUserReceiver>().ReverseMap();
            CreateMap<TbUserSebder, DTOUserSebder>().ReverseMap();
            CreateMap<TbUserSubscription, DTOUserSubscription>().ReverseMap();
        }
    }
}

using AutoMapper;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Application
{
    public class AutoMapper: Profile
    {
        public AutoMapper()
        {
            CreateMap<Collateral, SetupDto>().ReverseMap();
            CreateMap<Document, SetupDto>().ReverseMap();
            CreateMap<SmtpConfig, ConfigDto>().ReverseMap();
            CreateMap<SMSConfig, ConfigDto>().ReverseMap() ;
            CreateMap<UserProfileDto,  UserProfile>().ReverseMap();
            CreateMap<UserProfileDto, AppIdentityUser>().ReverseMap();
            CreateMap<UserDocumentDto, UserDocument>().ReverseMap();
            CreateMap<UserCollateralDto, UserCollateral>().ReverseMap();
            CreateMap<LoanRequestDto, LoanRequest>().ReverseMap();
            CreateMap<AcctypeDto, AccType>().ReverseMap(); 

        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            CreateMap<InterestDto, Interest>().ReverseMap();
            CreateMap<ChargesDto, Charges>().ReverseMap();
            CreateMap<string, LoanUserDocument>().ConstructUsing(str => new LoanUserDocument { userDocumentlId = str });
            CreateMap<LoanSetupDto, LoanSetup>().ReverseMap();
            CreateMap<string, LoanDocument>().ConstructUsing(str => new LoanDocument { documentId = str });
            CreateMap<string, LoanCollateral>().ConstructUsing(str => new LoanCollateral { collateralId = str });
            CreateMap<DepositDto, BankDeposit>().ReverseMap();
            CreateMap<UserBankDto, UserBank>().ReverseMap();


        }
    }
}

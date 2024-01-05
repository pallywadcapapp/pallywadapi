using PallyWad.Domain.Dto;
using PallyWad.Domain;
using AutoMapper;

namespace PallyWad.Setup.Configuration
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Collateral, SetupDto>();
            CreateMap<Document, SetupDto>();
        }
    }
}

using AutoMapper;
using Questao5.Application.DTOs;
using Questao5.Domain.Entities;

namespace Questao5.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movimento, MovimentacaoContaCorrenteDTO>();
        }
    }
}

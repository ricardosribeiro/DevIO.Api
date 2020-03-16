using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Models;
using DevIO.Data.Context;

namespace DevIO.Api.Configurations
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();

            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
        }
    }
}

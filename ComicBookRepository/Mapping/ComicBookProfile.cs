using AutoMapper;
using ComicBookRepository.Data;
using JetBrains.Annotations;

namespace ComicBookRepository.Web {
    [UsedImplicitly]
    public class ComicBookProfile : Profile {
        public ComicBookProfile()
        {
            CreateMap<ComicBookTitle, ComicBookTitleDTO>().ReverseMap();
            CreateMap<ComicBookDetails, ComicBookDetailsDTO>()
                .ForMember(dst => dst.BookTitle, opt => opt.MapFrom(src => src.Title.Title))
                .ForMember(dst => dst.TitleId, opt => opt.MapFrom(src => src.TitleId));
            CreateMap<ComicBookDetailsDTO, ComicBookDetails>()
                .ForMember(dst => dst.TitleId, opt => opt.MapFrom(src => src.TitleId));
        }
    }
}

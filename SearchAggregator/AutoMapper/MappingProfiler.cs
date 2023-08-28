using AutoMapper;
using SearchAggregator.DTOs;
using SearchAggregator.SearchJsonModels.Bing;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;

namespace SearchAggregator.AutoMapper;

public class MappingProfiler : Profile
{
    public MappingProfiler() 
    {
        CreateMap<GoogleItemModel, GoogleDto>()
                .ForMember(google => google.Title, opt => 
                     opt.MapFrom(googleItemModel => googleItemModel.Title))
                .ForMember(google => google.Url, opt => 
                    opt.MapFrom(googleItemModel => googleItemModel.Link))
                .ForMember(google => google.Description, opt => 
                    opt.MapFrom(googleItemModel => googleItemModel.Snippet));

        CreateMap<BingItemModel, BingDto>()
              .ForMember(google => google.Title, opt =>
                   opt.MapFrom(bingItemModel => bingItemModel.Name))
              .ForMember(google => google.Url, opt =>
                  opt.MapFrom(bingItemModel => bingItemModel.Url))
              .ForMember(google => google.Description, opt =>
                  opt.MapFrom(bingItemModel => bingItemModel.Snippet));

        CreateMap<YandexItemModel, YandexDto>()
             .ForMember(google => google.Title, opt =>
                  opt.MapFrom(yandexItemModel => yandexItemModel.Title))
             .ForMember(google => google.Url, opt =>
                 opt.MapFrom(yandexItemModel => yandexItemModel.Url))
             .ForMember(google => google.Description, opt =>
                 opt.MapFrom(yandexItemModel => yandexItemModel.Headline));
    }
}
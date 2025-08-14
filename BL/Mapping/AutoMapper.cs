using AutoMapperCore = AutoMapper;

namespace BL.Mapping
{
    public class AutoMapper : IMapper
    {
        private readonly AutoMapperCore.IMapper _mapper;
        public AutoMapper(AutoMapperCore.IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
        
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
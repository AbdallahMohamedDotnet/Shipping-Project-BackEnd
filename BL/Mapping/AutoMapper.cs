using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class AutoMapper : IMapper
    {
        private readonly IMapper Mapper;
        public AutoMapper(IMapper Mapper)
        {
            this.Mapper = Mapper;
        }

        public TDestination Map<TSource, TDestination>()
        {
            return Mapper.Map<TSource, TDestination>();
        }
    }
}

using System;
using AutoMapper;

using Use_Wheels.Models.DTO;

namespace Use_Wheels
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<RegisterationRequestDTO, UserDTO>().ReverseMap();
            CreateMap<RegisterationRequestDTO, User>().ReverseMap();
			CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<CarDTO, Car>().ReverseMap();
			CreateMap<CategoryDTO, Category>().ReverseMap();
			CreateMap<CarDTO, Car>().ReverseMap();
            CreateMap<CarUpdateDTO, Car>().ReverseMap();
			CreateMap<OrderDTO, Orders>().ReverseMap();
        }
	}
}


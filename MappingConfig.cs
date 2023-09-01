using System;
using AutoMapper;

using Use_Wheels.Models.DTO;

namespace Use_Wheels
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<RegisterationRequestDTO, UserDTO>();
			CreateMap<CarDTO, Car>();
			CreateMap<CategoryDTO, Category>();
			CreateMap<CarDTO, Car>();
            CreateMap<CarUpdateDTO, Car>();
			CreateMap<OrderDTO, Orders>();
        }
	}
}


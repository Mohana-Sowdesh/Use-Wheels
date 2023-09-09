using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface IUserOrderServices
	{
        Task<Orders> CreateOrder(OrderDTO orderDTO);
    }
}


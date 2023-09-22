using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface IOrderServices
	{
        // Method to insert a new order into DB
        Task<Orders> CreateOrder(OrderDTO orderDTO);
    }
}


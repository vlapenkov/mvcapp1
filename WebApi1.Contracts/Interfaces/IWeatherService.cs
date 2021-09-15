using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi1.Contracts.Dto;

namespace WebApi1.Contracts.Interfaces
{
   public interface IWeatherService
    {
        
        [Get("/api/WeatherForecast")]
        Task<IEnumerable<WeatherForecast>> Get();
    }
}

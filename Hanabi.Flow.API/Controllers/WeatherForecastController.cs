﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hanabi.Flow.Common.Helpers;
using Hanabi.Flow.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hanabi.Flow.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,MyContext myContext)
        {
            _logger = logger;
            myContext.CreateTableByEntity(false);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = _summaries[rng.Next(_summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 获取配置文件内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetSetting()
        {
            
            return AppSettings.app("LoveIsFantasy");
        }
    }
}

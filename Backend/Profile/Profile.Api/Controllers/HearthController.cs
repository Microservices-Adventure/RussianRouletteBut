﻿using Microsoft.AspNetCore.Mvc;
using Profile.Api.Models;
using Profile.Api.Services;

namespace Profile.Api.Controllers
{
    [Route("/api/hearth")]
    [ApiController]
    public class HearthController : ControllerBase
    {
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IHealthService _healthService;

        public HearthController(IHostApplicationLifetime lifetime, IHealthService healthService)
        {
            _lifetime = lifetime;
            _healthService = healthService;
        }

        [HttpPost("kill")]
        public IActionResult Kill()
        {
            double cooldownTime = _healthService.CooldownTime();
            if (cooldownTime > 0)
            {
                return BadRequest("Cooldown time left: " + cooldownTime);
            }
            Thread kill = new Thread(() =>
            {
                Thread.Sleep(500);
                _lifetime.StopApplication();
            });
            kill.Start();
            return Ok();
        }

        [HttpGet("isLive")]
        public IActionResult IsLive()
        {
            return Ok(_healthService.IsLive());
        }

        [HttpGet("killAvailable")]
        public IActionResult KillAvailable()
        {
            double timeLeft = _healthService.CooldownTime();
            var response = new CooldownModel
            {
                IsCooldown = timeLeft > 0,
                SecondsLeft = timeLeft
            };
            return Ok(response);
        }
    }
}
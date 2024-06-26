﻿using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController:ControllerBase
    {
        private readonly IGenericService<Schedule, ScheduleDTO> _scheduleService;
        private readonly IGenericRepository<Schedule> _scheduleRepo;
        private readonly ILogger<ScheduleController> _logger;
        public IConfiguration _configuration;

        public ScheduleController(IConfiguration config, ILogger<ScheduleController> logger,
                IGenericService<Schedule, ScheduleDTO> scheduleService,
                IGenericRepository<Schedule> scheduleRepo)
        {
            _logger = logger;
            _configuration = config;
            _scheduleService = scheduleService;
            _scheduleRepo = scheduleRepo;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var scheduleList = await _scheduleService.GetAll();
            return Ok(scheduleList);
        }

        [HttpGet("GetAllByUserId")]
        //[Authorize]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            var scheduleList = await _scheduleRepo.GetAll(x => x.Include(x => x.Booking)
                    .ThenInclude(x => x.BookingUsers)
                    .ThenInclude(x => x.User),
                    x => x.Include(x => x.Booking)
                    .ThenInclude(x => x.Level),
                    x => x.Include(x => x.Booking)
                    .ThenInclude(x => x.Subject));
            scheduleList = scheduleList.Where(x => 
                    x.Booking.BookingUsers
                    .Any(y => 
                        y.UserId == userId));
            return Ok(scheduleList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var scheduleList = await _scheduleService.Get(id);
            return Ok(scheduleList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add([FromBody] ScheduleDTO dto)
        {
            var schedule = await _scheduleService.Add(dto);
            return Ok(schedule);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update([FromBody] ScheduleDTO dto)
        {
            var schedule = await _scheduleService.Update(dto);
            return Ok(schedule);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var schedule = await _scheduleService.Delete(id);
            return Ok(schedule);
        }
    }
}

﻿using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services;
using PRN231.Services.Interfaces;
using System.Xml;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly IGenericRepository<User> _userRepo;
        private readonly ILogger<UserController> _logger;
        public IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;

        public UserController(IConfiguration config, ILogger<UserController> logger,
                IGenericService<User, UserDTO> userService, IGenericRepository<User> userRepo,
                IFileStorageService fileStorageService)
        {
            _logger = logger;
            _configuration = config;
            _userService = userService;
            _userRepo = userRepo;
            _fileStorageService = fileStorageService;
        }

        [HttpPost("UploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] AvatarDTO avatar)
        {
            var user = await _userRepo.Get(avatar.UserId);
            //Delete old file
            if (user.Avatar.Contains("http://localhost:5176"))
            {
                await _fileStorageService.DeleteFileAsync(Path.GetFileName(user.Avatar));
            }

            Console.WriteLine(Path.GetFileName(user.Avatar));
            //store file 
            string filePath = await _fileStorageService.StoreFileAsync(avatar.File);
            if (filePath == null)
            {
                return BadRequest();
            }
            //update user avatar
            user.Avatar = $"http://localhost:5176/{filePath}";
            var updatedUser = await _userRepo.Update(user);

            return Ok(updatedUser);
        }


        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var customerList = await _userService.GetAll();
            return Ok(customerList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var customerList = await _userService.Get(id);
            return Ok(customerList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(UserDTO dto)
        {
            var brand = await _userService.Add(dto);
            return Ok(brand);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(UserInfoDTO dto)
        {
            var user = await _userService.Get(dto.Id);

            if (user == null)
            {
                return NotFound($"User with ID {dto.Id} not found.");
            }

            user.UserName = dto.ReceiverName;
            user.Email = dto.Email;
            user.Address = dto.Address;
            user.PhoneNumber = dto.PhoneNumber;
            user.Gender = dto.Gender;
            user.Avatar = dto.Avatar;

            var updatedUser = await _userRepo.Update(user);
            return Ok(updatedUser);
        }


        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _userService.Delete(id);
            return Ok(brand);
        }
        
    }

    public class AvatarDTO
    {
        public int UserId { get; set; }
        public IFormFile File { get; set; }
    }
}

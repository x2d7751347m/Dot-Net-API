﻿using System;
using System.Collections.Generic;
using AutoMapper;
using DotNetAPI.Data;
using DotNetAPI.Dtos;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEfController : ControllerBase
{
    DataContextEf _entityFramework;    
    IMapper _mapper;

    public UserEfController(IConfiguration config)
    {
        _entityFramework = new DataContextEf(config);

        _mapper = new Mapper(new MapperConfiguration(cfg =>{
            cfg.CreateMap<UserToAddDto, User>();
            cfg.CreateMap<UserSalary, UserSalary>().ReverseMap();
            cfg.CreateMap<UserJobInfo, UserJobInfo>().ReverseMap();
        }));

    }

    [HttpGet("Users")]
    // public IEnumerable<User> GetUsers()
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }

    [HttpGet("User/{userId:int}")]
    // public IEnumerable<User> GetUsers()
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users
            .FirstOrDefault(u => u.UserId == userId);

        if (user != null)
        {
            return user;
        }
        
        throw new Exception("Failed to Get User");
    }
    
    [HttpPut("User")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users
            .FirstOrDefault(u => u.UserId == user.UserId);
            
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            } 

            throw new Exception("Failed to Update User");
        }
        
        throw new Exception("Failed to Get User");
    }


    [HttpPost("User")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);
        
        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        } 

        throw new Exception("Failed to Add User");
    }

    [HttpDelete("User/{userId:int}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
            .FirstOrDefault(u => u.UserId == userId);
            
        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            } 

            throw new Exception("Failed to Delete User");
        }
        
        throw new Exception("Failed to Get User");
    }

    [HttpGet("UserSalary/{userId:int}")]
    public IEnumerable<UserSalary> GetUserSalaryEf(int userId)
    {
        return _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalaryEf(UserSalary userForInsert)
    {
        _entityFramework.UserSalary.Add(userForInsert);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed on save");
    }


    [HttpPut("UserSalary")]
    public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
    {
        UserSalary? userToUpdate = _entityFramework.UserSalary
            .FirstOrDefault(u => u.UserId == userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to Update");
    }


    [HttpDelete("UserSalary/{userId:int}")]
    public IActionResult DeleteUserSalaryEf(int userId)
    {
        UserSalary? userToDelete = _entityFramework.UserSalary
            .FirstOrDefault(u => u.UserId == userId);

        if (userToDelete != null)
        {
            _entityFramework.UserSalary.Remove(userToDelete);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to delete");
    }


    [HttpGet("UserJobInfo/{userId:int}")]
    public IEnumerable<UserJobInfo> GetUserJobInfoEf(int userId)
    {
        return _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
    {
        _entityFramework.UserJobInfo.Add(userForInsert);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding UserJobInfo failed on save");
    }


    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
    {
        UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
            .FirstOrDefault(u => u.UserId == userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to Update");
    }


    [HttpDelete("UserJobInfo/{userId:int}")]
    public IActionResult DeleteUserJobInfoEf(int userId)
    {
        UserJobInfo? userToDelete = _entityFramework.UserJobInfo
            .FirstOrDefault(u => u.UserId == userId);

        if (userToDelete != null)
        {
            _entityFramework.UserJobInfo.Remove(userToDelete);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to delete");
    }
}
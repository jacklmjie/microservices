﻿using Contact.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Contact.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var identity = new UserIdentity();

                if (!int.TryParse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == "sub")?.Value, out int userId))
                    throw new PlatformNotSupportedException("token错误");

                identity.UserId = userId;
                identity.Name = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "name").Value;

                return identity;
            }
        }
    }
}
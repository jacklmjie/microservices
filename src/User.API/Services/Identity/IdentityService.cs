﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using User.API.Application.Exceptions;

namespace User.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int GetUserIdentity()
        {
            if (!int.TryParse(_context.HttpContext.User.FindFirst("sub").Value, out int userId))
                throw new UserDomainException("token错误");
            return userId;
        }

        public string GetUserName()
        {
            return _context.HttpContext.User.FindFirst("name").Value;
        }
    }
}

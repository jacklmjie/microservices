﻿using System;

namespace User.Identity.Application.Exceptions
{
    public class UserIdentityDomainException : Exception
    {
        public UserIdentityDomainException() { }

        public UserIdentityDomainException(string message) : base(message) { }

        public UserIdentityDomainException(string message, Exception innerExpection) : base(message, innerExpection) { }
    }
}

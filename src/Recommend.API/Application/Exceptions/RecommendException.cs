﻿using System;

namespace Recommend.API.Application.Exceptions
{
    public class RecommendException : Exception
    {
        public RecommendException() { }

        public RecommendException(string message) : base(message) { }

        public RecommendException(string message, Exception innerExpection) : base(message, innerExpection) { }
    }
}

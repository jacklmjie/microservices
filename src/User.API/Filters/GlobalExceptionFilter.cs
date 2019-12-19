﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace User.API.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(IWebHostEnvironment env,
            ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "网络错误";
                if (_env.IsDevelopment())
                {
                    json.DevelopeMessage = context.Exception.StackTrace;
                }

                context.Result = new InternalServerErrorObjectResult(json);
            }

            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}

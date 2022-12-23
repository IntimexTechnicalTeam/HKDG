using Autofac;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Web.Framework;

namespace Web.Mvc
{
    public abstract class BaseController : Controller
    {
        public IComponentContext Services;

        /// <summary>
        /// 默认注入AutoFac的IComponentContext
        /// </summary>
        /// <param name="service"></param>
        public BaseController(IComponentContext service)
        {
            this.Services = service;         
        }

        IConfiguration _configuration;
        public IConfiguration Configuration
        {
            get
            {
                if (this._configuration == null)
                {
                    this._configuration = this.Services.Resolve(typeof(IConfiguration)) as IConfiguration;
                }

                return this._configuration;
            }
        }

        ILogger _logger = null;
        public ILogger Logger
        {
            get
            {
                if (this._logger == null)
                {
                    //ILoggerFactory loggerFactory = this.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;

                    ILoggerFactory loggerFactory = Services.Resolve<ILoggerFactory>();
                    ILogger logger = loggerFactory.CreateLogger(this.GetType().FullName);
                    this._logger = logger;
                }

                return this._logger;
            }
        }

        protected virtual void LogException(Exception ex)
        {
            string error = "\r\n 异常类型：" + ex.GetType().FullName + "\r\n 异常源：" + ex.Source + "\r\n 异常位置=" + ex.TargetSite + " \r\n 异常信息=" + ex.Message + " \r\n 异常堆栈：" + ex.StackTrace;

            this.Logger.LogError(error);
        }

        IMediator _mediator;
        public IMediator Mediator
        {
            get
            {
                if (_mediator == null)
                {
                    _mediator = Services.Resolve<IMediator>();
                }
                return this._mediator;
            }
        }

        IHttpContextAccessor _currentContext;
        public IHttpContextAccessor CurrentContext
        {
            get
            {
                if (_currentContext == null)
                {
                    _currentContext = Services.Resolve<IHttpContextAccessor>();
                }
                return this._currentContext;
            }
        }

        public string GetClientIP
        {
            get
            {

                return CurrentContext.HttpContext.GetClientIP();
            }
        }

        IHostEnvironment _hostEnvironment;
        public IHostEnvironment hostEnvironment
        {
            get
            {
                if (_hostEnvironment == null)
                {
                    _hostEnvironment = Services.Resolve<IHostEnvironment>();
                }
                return this._hostEnvironment;
            }
        }

        public string AutoGenerateNumber(string perfix = "BD")
        {
            return $"{perfix}{IdGenerator.NewId}";
        }

        /// <summary>
        /// 是否为移动设备
        /// </summary>
        public bool IsMobile => ToolUtil.IsMobile(Request.Headers["User-Agent"].ToString());

        public virtual ActionResult GetActionResult(string viewName)
        {
            
            if (IsMobile)
            {
                return View("Mobile" + viewName);
            }
            else
            {
                return View(viewName);
            }
        }
    }
}

using StackOverFlowProject.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StackOverFlowProject.ApiControllers
{
    public class AccountController : ApiController
    {
        IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        public string Get(string email)
        {
            if (_userService.GetUsersByEmail(email)!=null)
            {
                return "Found";
            }
            else
            {
                return "Not Found";
            }
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TicketManager.Web.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsUserAuthenticated()
        {
            return this.User.Identity?.IsAuthenticated ?? false;
        }
        protected string GetUserId()
        {
            string? userId = null;

            bool IsAuthenticated = this.IsUserAuthenticated();
            if (IsAuthenticated)
            {
                userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return userId;
        }
    }
}

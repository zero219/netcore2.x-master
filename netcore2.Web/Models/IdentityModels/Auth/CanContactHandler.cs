using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IdentityModels.Auth
{
    public class ActionRequirement : IAuthorizationRequirement
    {

    }
    //如果共享IAuthorizationRequirement,AuthorizationHandler,满足其中一个条件就通过
    public class CanContactHandler : AuthorizationHandler<ActionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionRequirement requirement)
        {
            if (context.User.IsInRole("Ordinary"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class CanAdminHandler : AuthorizationHandler<ActionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionRequirement requirement)
        {
            if (context.User.IsInRole("Administrators"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

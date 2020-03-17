using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IdentityModels.Auth
{
    public class EmailRequirement : IAuthorizationRequirement
    {
        public string RequiredEmail { get; set; }

        public EmailRequirement(string requiredEmail)
        {
            RequiredEmail = requiredEmail;
        }
    }

    public class EmailHandler : AuthorizationHandler<EmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailRequirement requirement)
        {
            var claim = context.User.Claims.FirstOrDefault(x => x.Value == requirement.RequiredEmail);

            if (claim != null)
            {
                if (claim.Value.EndsWith(requirement.RequiredEmail))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }


}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class OwnerHandler : AuthorizationHandler<OwnerRequirement, Gig>
    {
        UserManager<User> _userManager;

        public OwnerHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement, Gig resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            var currentUserId = _userManager.GetUserId(context.User);

            if (currentUserId == resource.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

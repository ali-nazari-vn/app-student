using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentApp.Infrastructure;
using static StudentApp.API.proto.checkPermissionService;

namespace StudentApp.API
{
    public class AccessControlAttribute : ActionFilterAttribute
    {
        private readonly string permissionFlag;

        public AccessControlAttribute(string permission)
        {
            this.permissionFlag = permission;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var checkPermissionService = context.HttpContext.RequestServices.GetService<checkPermissionServiceClient>();
            var externalLogServices = context.HttpContext.RequestServices.GetService<ExternalLogServices>();

            try
            {
                var res = await checkPermissionService
                    .checkPermissionAsync(new proto.checkPermissionRequest { Permission = permissionFlag });

                if (res.Isallowed == false)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    await base.OnActionExecutionAsync(context, next);
                }
            }
            catch (Exception e)
            {
                externalLogServices.ExternalLog(e);
                throw;
            }

        }
    }
}

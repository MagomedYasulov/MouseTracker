using Microsoft.AspNetCore.Mvc;
using MouseTracker.Web.Filters;

namespace MouseTracker.Web.Controllers
{
    [TypeFilter<ApiExceptionFilter>]
    public class BaseController : ControllerBase
    {

    }
}

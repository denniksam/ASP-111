namespace ASP_111.Services.AuthUser
{
    public interface IAuthUserService
    {
        Guid? GetUserId(HttpContext context);
    }
}

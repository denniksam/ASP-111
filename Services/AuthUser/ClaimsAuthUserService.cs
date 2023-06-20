using ASP_111.Data;
using System.Security.Claims;

namespace ASP_111.Services.AuthUser
{
    public class ClaimsAuthUserService : IAuthUserService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ClaimsAuthUserService> _logger;

        public ClaimsAuthUserService(DataContext dataContext, ILogger<ClaimsAuthUserService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public Guid? GetUserId(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                return null;
            }            
            Guid userId;
            try
            {   // извлекаем из Claims ID и ...
                userId = Guid.Parse(
                    context.User.Claims.First(
                        c => c.Type == ClaimTypes.Sid).Value);                    
            }
            catch (Exception ex)
            {
                _logger.LogError("Claims parse error {ex}", ex.Message);
                return null;
            }
            // ... находим по нему пользователя
            var user = _dataContext.Users.Find(userId);
            return user?.Id;
        }
    }
}

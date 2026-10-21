using WorkHub.Application.Common.Interfaces;

namespace WorkHub.Api.Services
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        public Guid? UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
                return Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }
}

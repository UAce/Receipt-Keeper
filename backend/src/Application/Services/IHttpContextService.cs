using Domain.Models;

namespace Application.Services
{
    public interface IHttpContextService
    {
        CurrentUser CurrentUser { get; }
        string IdentityId { get; }
    }
}

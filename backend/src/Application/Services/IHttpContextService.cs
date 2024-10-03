using Domain.Models;

namespace Application.Services
{
    public interface IHttpContextService
    {
        User CurrentUser { get; }
        string IdentityId { get; }
    }
}

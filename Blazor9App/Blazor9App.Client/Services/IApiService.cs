using Blazor9App.Client.Models;

namespace Blazor9App.Client.Services;

public interface IApiService
{
    Task<IEnumerable<Band>> CallLocalApiAsync();

    Task<IEnumerable<Band>> CallRemoteApiAsync();

}
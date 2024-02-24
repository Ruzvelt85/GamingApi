using GamingApi.Patterns;

namespace Yld.GamingApi.WebApi.Queries
{
    public record GetGameListQuery(int Offset, int Limit) : IQuery;
}

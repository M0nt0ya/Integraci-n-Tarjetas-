using Shared.Domain.Criteria;

namespace Shared.Domain.Interfaces
{
    public interface IOrderBy
    {
        string Field { get; }
        string Direction { get; }
    }
}
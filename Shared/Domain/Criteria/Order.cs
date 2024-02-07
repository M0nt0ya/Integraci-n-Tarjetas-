using Shared.Domain.Interfaces;

namespace Shared.Domain.Criteria
{
    public class OrderBy : IOrderBy
    {
        public required string Field { get; set; }
        public required string Direction { get; set; }
    }
}

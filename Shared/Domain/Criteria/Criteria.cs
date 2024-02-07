using Shared.Domain.Interfaces;

namespace Shared.Domain.Criteria
{
    public class Criteria<T>(object[] filters, int? limits = null, object[]? orderBy = null) : ICriteria<T>
    {
        public object[] Filters { get; private set; } = filters;
        public int? Limits { get; private set; } = limits;
        public object[]? OrderBy { get; private set; } = orderBy;
    }
}

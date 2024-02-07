using Shared.Domain.Criteria;

namespace Shared.Domain.Interfaces
{
    public interface ICriteria<T>
    {
        public object[] Filters { get; }
        public int? Limits { get; }
        public object[]? OrderBy { get; }
        //IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }
}
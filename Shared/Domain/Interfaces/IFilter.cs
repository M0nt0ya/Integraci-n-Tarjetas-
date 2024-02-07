using Shared.Domain.Criteria;

namespace Shared.Domain.Interfaces
{
    public interface IFilter
    {
        string Field { get; }
        string OperatorField { get; }
        object ValueField { get; }
    }
}
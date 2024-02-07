using System.Formats.Asn1;
using Shared.Domain.Interfaces;

namespace Shared.Domain.Criteria
{
    public class Filter : IFilter
    {
        public required string Field { get; set; }
        public required string OperatorField { get; set; }
        public required object ValueField { get; set; }
    }
}

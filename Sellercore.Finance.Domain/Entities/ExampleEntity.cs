using Shared.Domain.Common;

namespace Sellercore.Finance.Domain.Entities;

public class ExampleEntity : BaseEntity<long>
{
    public string ExampleField { get; set; }
}
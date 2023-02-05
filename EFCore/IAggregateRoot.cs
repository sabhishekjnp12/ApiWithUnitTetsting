namespace ApiWithUnitTetsting.EFCore;


/// <summary>
/// Defines the rule for a root aggregate to support the internal implementation of microservices.
/// </summary>
/// <remarks>
/// An aggregate refers to a cluster of domain objects grouped together to match transactional consistency.
/// Those objects could be instances of entities(one of which is the aggregate root or root entity) plus any additional value objects.
/// </remarks>
public interface IAggregateRoot
{
}

using CleanCRUDSolution.Domain.Common;

namespace CleanCRUDSolution.Domain.Entities.Base
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        // EF Core
        protected Entity() { }

        protected Entity(Guid id)
        {
            Id = Guard.NotEmpty(id, nameof(Id));
        }
    }
}

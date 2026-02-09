using CleanCRUDSolution.Domain.Common;
using CleanCRUDSolution.Domain.Entities.Base;

namespace CleanCRUDSolution.Domain.Entities
{
    public sealed class Country : Entity
    {
        public string Name { get; private set; }

        public Country(string name) : base(Guid.NewGuid())
        {
            Name = Guard.NotNullOrWhiteSpace(name, nameof(name));
        }

        public void Rename(string name)
        {
            Name = Guard.NotNullOrWhiteSpace(name, nameof(name));
        }
    }
}

using Microsoft.EntityFrameworkCore.PersistenceApi;

#pragma warning disable CS8618
namespace Microsoft.EntityFrameworkCore.WebApplication
{
    public class Order : IDeletable
    {
        protected Order()
        {
        }

        public Order(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            AddedAt = DateTime.Now;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime AddedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}

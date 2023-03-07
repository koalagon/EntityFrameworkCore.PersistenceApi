namespace Microsoft.EntityFrameworkCore.PersistenceApi
{
    /// <summary>
    /// Represent the entity supports the soft delete.
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// Indicates whether the entity is deleted or not.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Represent when the entity is deleted.
        /// </summary>
        DateTime? DeletedAt { get; set; }
    }
}

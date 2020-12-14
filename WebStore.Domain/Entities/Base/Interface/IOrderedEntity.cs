namespace WebStore.Domain.Entities.Base.Interface
{
    public interface IOrderedEntity : IEntity
    {
        int Order { get; set; }
    }
}

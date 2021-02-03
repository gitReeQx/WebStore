namespace WebStore.Domain.Entities.Base.Interface
{
    public interface INamedEntity: IEntity
    {
        string Name { get; set; }
    }
}

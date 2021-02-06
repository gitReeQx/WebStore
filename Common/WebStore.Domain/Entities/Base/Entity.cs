using WebStore.Domain.Entities.Base.Interface;

namespace WebStore.Domain.Entities.Base
{
    /// <summary>Сущность</summary>
    public abstract class Entity : IEntity
    {
        /// <summary>Идентификатор</summary>
        public int Id { get; set; }
    }
}

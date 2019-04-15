namespace Ndrome.Model
{
    public interface IEntity: IBase
    {
        bool IsDeleted { get; set; }
    }

    public class Entity : Base, IEntity
    {
        public bool IsDeleted { get; set; }
    }
}

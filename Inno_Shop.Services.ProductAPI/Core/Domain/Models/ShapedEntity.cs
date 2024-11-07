namespace Inno_Shop.Services.ProductAPI.Core.Domain.Models;

public class ShapedEntity
{
    public ShapedEntity()
    {
        Entity = new Entity();
    }
    public Guid Id { get; set; }
    public Entity Entity { get; set; }
}

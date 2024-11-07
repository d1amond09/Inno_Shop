using System.Dynamic;
using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Contracts;

public interface IDataShaper<T>
{
    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
    ShapedEntity ShapeData(T entity, string fieldsString);
}

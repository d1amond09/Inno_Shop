using System.Linq.Expressions;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;

public interface IRepositoryBase<T>
{
	IQueryable<T> FindAll(bool trackChanges);
	IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
	void Create(T entity);
	void Update(T entity);
	void Delete(T entity);
}


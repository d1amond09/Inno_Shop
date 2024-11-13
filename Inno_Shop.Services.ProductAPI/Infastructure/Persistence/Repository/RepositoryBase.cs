using System.Linq.Expressions;
using Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.Infastructure.Persistence.Repository;

public abstract class RepositoryBase<T>(AppDbContext db) : IRepositoryBase<T> where T : class
{
	protected readonly AppDbContext _db = db;

	public IQueryable<T> FindAll(bool trackChanges) =>
		!trackChanges ? _db.Set<T>()
		.AsNoTracking() : _db.Set<T>();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
		bool trackChanges) =>
		!trackChanges ? _db.Set<T>()
		.Where(expression)
		.AsNoTracking() : _db.Set<T>()
		.Where(expression);

	public void Create(T entity) => _db.Set<T>().Add(entity);
	public void Update(T entity) => _db.Set<T>().Update(entity);
	public void Delete(T entity) => _db.Set<T>().Remove(entity);
}

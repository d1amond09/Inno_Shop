using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;

public abstract class RequestParameters
{
	const int maxPageSize = 50;
	private int _pageSize = 10;

	public int PageNumber { get; set; } = 1;
	public int PageSize
	{
		get
		{
			return _pageSize;
		}
		set
		{
			_pageSize = value > maxPageSize ? 
				maxPageSize : value;
		}
	}
	public string OrderBy { get; set; } = string.Empty;
	public string Fields { get; set; } = string.Empty;
}



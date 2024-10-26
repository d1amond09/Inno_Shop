using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;

namespace Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;

public class ProductParameters : RequestParameters
{
	public double MinPrice { get; set; }
	public double MaxPrice { get; set; } = double.MaxValue;
	public bool ValidPriceRange => MaxPrice > MinPrice;
	public string SearchTerm { get; set; } = string.Empty;
    public ProductParameters()
    {
		OrderBy = "name";
    }
}

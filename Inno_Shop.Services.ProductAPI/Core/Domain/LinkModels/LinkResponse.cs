using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;

public class LinkResponse
{
	public bool HasLinks { get; set; }
	public List<Entity> ShapedEntities { get; set; }
	public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }
	public LinkResponse()
	{
		LinkedEntities = new LinkCollectionWrapper<Entity>();
		ShapedEntities = [];
	}
}


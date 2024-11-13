using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;

public class LinkCollectionWrapper<T> : LinkResourceBase
{
	public List<T> Value { get; set; } = [];
	public LinkCollectionWrapper()
	{ }
	public LinkCollectionWrapper(List<T> value)
	{
		Value = value;
	}
}


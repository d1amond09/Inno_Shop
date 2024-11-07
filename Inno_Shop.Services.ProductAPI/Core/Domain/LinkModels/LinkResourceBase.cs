using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;

public class LinkResourceBase
{
	public LinkResourceBase() { }
    public List<Link> Links { get; set; } = [];
}

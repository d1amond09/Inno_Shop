using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inno_Shop.Services.UserAPI.Core.Domain.RequestFeatures;

namespace Inno_Shop.Services.UserAPI.Core.Domain.RequestFeatures;

public class UserParameters : RequestParameters
{
	public string? SearchTerm { get; set; }
    public UserParameters()
    {
		OrderBy = "username";
    }
}

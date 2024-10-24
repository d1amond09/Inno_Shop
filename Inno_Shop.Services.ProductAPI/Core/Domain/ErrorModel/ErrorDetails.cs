﻿using System.Text.Json;

namespace Inno_Shop.Services.ProductAPI.Core.Domain.ErrorModel;

public class ErrorDetails
{
	public int StatusCode { get; set; }
	public string? Message { get; set; }
	public override string ToString() => JsonSerializer.Serialize(this);
}

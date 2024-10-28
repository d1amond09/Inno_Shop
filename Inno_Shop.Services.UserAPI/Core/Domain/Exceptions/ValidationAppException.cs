﻿namespace Inno_Shop.Services.UserAPI.Core.Domain.Exceptions;

public sealed class ValidationAppException(IReadOnlyDictionary<string, string[]> errors) : 
	Exception("One or more validation errors occurred")
{
	public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
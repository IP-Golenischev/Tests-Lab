﻿namespace HW4.Exceptions;

public class ProductNotFoundException : Exception
{
	public ProductNotFoundException()
	{
	}

	public ProductNotFoundException(string message) : base(message)
	{
	}
}
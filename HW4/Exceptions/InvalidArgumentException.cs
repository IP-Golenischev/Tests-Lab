﻿namespace HW4.Exceptions;

public class InvalidArgumentException : Exception
{
	public InvalidArgumentException()
	{
	}

	public InvalidArgumentException(string message) : base(message)
	{
	}
}
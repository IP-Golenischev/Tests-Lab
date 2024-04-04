namespace HW4.WebHost.Exceptions;

public class InvalidArgumentException : Exception
{
	public InvalidArgumentException()
	{
	}

	public InvalidArgumentException(string message) : base(message)
	{
	}
}

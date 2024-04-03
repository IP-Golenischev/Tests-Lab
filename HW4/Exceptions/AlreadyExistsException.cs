namespace HW4.Exceptions;

public class AlreadyExistsException : Exception
{
	public AlreadyExistsException()
	{
	}

	public AlreadyExistsException(string message) : base(message)
	{
	}
}

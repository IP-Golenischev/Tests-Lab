namespace HW4.WebHost.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException()
	{
	}

	public NotFoundException(string message) : base(message)
	{
	}
}

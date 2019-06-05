namespace NetReflectorCore
{
	public interface IReflectorAttribute
	{
		string Name { get; }
		string Description { get; }
        bool HasCustomFactory { get; }
	}
}

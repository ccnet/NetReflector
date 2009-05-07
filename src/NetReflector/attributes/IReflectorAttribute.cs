namespace Exortech.NetReflector
{
	public interface IReflectorAttribute
	{
		string Name { get; }
		string Description { get; }
        bool HasCustomFactory { get; }
	}
}

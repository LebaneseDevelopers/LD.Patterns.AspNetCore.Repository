namespace LD.Patterns.AspNetCore.Repository
{
	public interface IEntity<TKey>
	{
		TKey Id { get; set; }
	}
}

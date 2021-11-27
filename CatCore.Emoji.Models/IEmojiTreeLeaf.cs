namespace CatCore.Emoji.Models
{
	public interface IEmojiTreeLeaf
	{
		string Key { get; }
		int Depth { get; }
		string Url { get; }
	}
}
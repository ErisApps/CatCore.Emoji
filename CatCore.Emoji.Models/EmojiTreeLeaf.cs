namespace CatCore.Emoji.Models
{
	public class EmojiTreeLeaf : IEmojiTreeLeaf
	{
		public string Key { get; }
		public int Depth { get; }

		public string Url => $"https://raw.githubusercontent.com/googlefonts/noto-emoji/main/png/72/emoji_{Key}.png";

		public EmojiTreeLeaf(string key, int depth)
		{
			Key = key;
			Depth = depth;
		}
	}
}
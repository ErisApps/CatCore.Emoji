namespace CatCore.GNotoEmoji.Models
{
	public class EmojiTreeNodeBlock : EmojiTreeNodeBase, IEmojiTreeLeaf
	{
#pragma warning disable CS8766
		public string? Key { get; internal set; }
#pragma warning restore CS8766
		public int Depth { get; internal set; }

		public string Url => Key != null ? $"https://raw.githubusercontent.com/googlefonts/noto-emoji/main/png/72/emoji_{Key}.png" : string.Empty;

		public EmojiTreeNodeBlock()
		{
		}

		public EmojiTreeNodeBlock(string key, int depth)
		{
			Key = key;
			Depth = depth;
		}
	}
}
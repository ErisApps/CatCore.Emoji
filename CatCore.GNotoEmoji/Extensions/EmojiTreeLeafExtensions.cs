using CatCore.GNotoEmoji.Models;

namespace CatCore.GNotoEmoji.Extensions
{
	internal static class EmojiTreeLeafExtensions
	{
		internal static EmojiTreeNodeBlock UpgradeToBlock(this EmojiTreeLeaf emojiTreeLeaf)
		{
			return new EmojiTreeNodeBlock(emojiTreeLeaf.Key, emojiTreeLeaf.Depth);
		}
	}
}
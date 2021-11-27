using CatCore.Emoji.Models;

namespace CatCore.Emoji.SourceGeneration.GNoto.Extensions
{
	internal static class EmojiTreeLeafExtensions
	{
		internal static EmojiTreeNodeBlock UpgradeToBlock(this EmojiTreeLeaf emojiTreeLeaf)
		{
			return new EmojiTreeNodeBlock(emojiTreeLeaf.Key, emojiTreeLeaf.Depth);
		}
	}
}
using CatCore.Emoji.Models;

namespace CatCore.Emoji.SourceGeneration.Common.Extensions
{
	internal static class EmojiTreeLeafExtensions
	{
		internal static EmojiTreeNodeBlock UpgradeToBlock(this IEmojiTreeLeaf emojiTreeLeaf)
		{
			return new EmojiTreeNodeBlock(emojiTreeLeaf.Key, emojiTreeLeaf.Depth);
		}
	}
}
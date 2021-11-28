using System;

namespace CatCore.Emoji.Models
{
	public class EmojiTreeNodeBlock : EmojiTreeNodeBase, IEmojiTreeLeaf
	{
#pragma warning disable CS8766
		public string? Key { get; internal set; }
#pragma warning restore CS8766
		public int Depth { get; internal set; }

		public virtual string Url => throw new NotImplementedException("This is a baseclass that's used by the source generator, please use a specialized version instead.");

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
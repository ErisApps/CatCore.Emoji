using System;

namespace CatCore.Emoji.Models
{
	public class EmojiTreeLeaf : IEmojiTreeLeaf
	{
		public string Key { get; }
		public int Depth { get; }

		public virtual string Url => throw new NotImplementedException("This is a baseclass that's used by the source generator, please use a specialized version instead.");

		public EmojiTreeLeaf(string key, int depth)
		{
			Key = key;
			Depth = depth;
		}
	}
}
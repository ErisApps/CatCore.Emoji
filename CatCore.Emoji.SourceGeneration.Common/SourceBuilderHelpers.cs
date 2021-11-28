using System;
using System.Linq;
using System.Text;
using CatCore.Emoji.Models;

namespace CatCore.Emoji.SourceGeneration.Common
{
	internal static class SourceBuilderHelpers
	{
		private static string Indent(uint depth = 0) => string.Empty.PadLeft((int) depth * 4).Replace("    ", "	");

		internal static void AddToSourceBuilder(this EmojiTreeRoot emojiTreeRoot, StringBuilder sourceBuilder, string type, uint tabWidth = 0)
		{
			for (var i = 0; i < emojiTreeRoot.Count; i++)
			{
				var kvp = emojiTreeRoot.ElementAt(i);
				sourceBuilder.Append(Indent(tabWidth)).Append($"{{ '{kvp.Key}', ");

				switch (kvp.Value)
				{
					case EmojiTreeNodeBlock block:
						block.AddToSourceBuilder(sourceBuilder, type, tabWidth + 1);
						break;
					case EmojiTreeLeaf leaf:
						leaf.AddToSourceBuilder(sourceBuilder, type);
						break;
					default:
						throw new NotSupportedException();
				}

				sourceBuilder.AppendLine(i < emojiTreeRoot.Count - 1 ? "}," : "}");
			}
		}

		private static void AddToSourceBuilder(this EmojiTreeNodeBlock block, StringBuilder sourceBuilder, string type, uint tabWidth)
		{
			sourceBuilder.AppendLine(block.Key != null
				? $"new {type}{nameof(EmojiTreeNodeBlock)}(\"{block.Key}\", {block.Depth})"
				: $"new {type}{nameof(EmojiTreeNodeBlock)}");
			sourceBuilder.Append(Indent(tabWidth - 1)).Append('{');

			var propertyIndentation = Indent(tabWidth);

			for (var i = 0; i < block.Count; i++)
			{
				var kvp = block.ElementAt(i);
				sourceBuilder.AppendLine().Append(propertyIndentation).Append($"{{ '{kvp.Key}', ");

				switch (kvp.Value)
				{
					case EmojiTreeNodeBlock childBlock:
						childBlock.AddToSourceBuilder(sourceBuilder, type, tabWidth + 1);
						sourceBuilder.Append(i < block.Count - 1 ? "}," : '}');
						break;
					case EmojiTreeLeaf childLeaf:
						childLeaf.AddToSourceBuilder(sourceBuilder, type);
						sourceBuilder.Append(i < block.Count - 1 ? " }," : " }");
						break;
					default:
						throw new NotSupportedException();
				}
			}

			sourceBuilder.AppendLine().Append(Indent(tabWidth - 1)).Append('}');
		}

		private static void AddToSourceBuilder(this EmojiTreeLeaf leaf, StringBuilder sourceBuilder, string type)
		{
			sourceBuilder.Append($"new {type}{nameof(EmojiTreeLeaf)}(\"{leaf.Key}\", {leaf.Depth})");
		}
	}
}
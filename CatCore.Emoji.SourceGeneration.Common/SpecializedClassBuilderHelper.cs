using Microsoft.CodeAnalysis;

namespace CatCore.Emoji.SourceGeneration.Common
{
	internal static class SpecializedClassBuilderHelper
	{
		public static void GenerateSpecializedModels(GeneratorPostInitializationContext context, string type, string baseUrl)
		{
			GenerateEmojiTreeLeafSpecialization(context, type, baseUrl);
			GenerateEmojiTreeNodeBlockSpecialization(context, type, baseUrl);
		}

		private static void GenerateEmojiTreeLeafSpecialization(GeneratorPostInitializationContext context, string type, string baseUrl)
		{
			var emojiTreeLeafSpecialization = $@"namespace CatCore.Emoji.Models
{{
	public sealed class {type}EmojiTreeLeaf : EmojiTreeLeaf
	{{
		public override string Url => ""{baseUrl}"" + Key + "".png"";

		public {type}EmojiTreeLeaf(string key, int depth) : base(key, depth)
		{{
		}}
	}}
}}";

			context.AddSource($"{type}EmojiTreeLeaf", emojiTreeLeafSpecialization);
		}

		private static void GenerateEmojiTreeNodeBlockSpecialization(GeneratorPostInitializationContext context, string type, string baseUrl)
		{
			var emojiTreeNodeBlockSpecialization = $@"namespace CatCore.Emoji.Models
{{
	public sealed class {type}EmojiTreeNodeBlock : EmojiTreeNodeBlock
	{{
		public override string Url => Key != null ? ""{baseUrl}"" + Key + "".png"" : string.Empty;

		public {type}EmojiTreeNodeBlock()
		{{
		}}

		public {type}EmojiTreeNodeBlock(string key, int depth) : base(key, depth)
		{{
		}}
	}}
}}";

			context.AddSource($"{type}EmojiNodeBlock", emojiTreeNodeBlockSpecialization);
		}
	}
}
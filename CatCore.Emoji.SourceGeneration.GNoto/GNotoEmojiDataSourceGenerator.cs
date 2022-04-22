using System.Collections.Generic;
using CatCore.Emoji.SourceGeneration.Common;
using Microsoft.CodeAnalysis;

namespace CatCore.Emoji.SourceGeneration.GNoto
{
	[Generator]
	public class GNotoEmojiDataSourceGenerator : EmojiDataSourceGeneratorBase
	{
		protected override string Type => nameof(GNoto);
		protected override string BaseUrl => "https://raw.githubusercontent.com/googlefonts/noto-emoji/main/png/72/emoji_u";
		protected override string CodepointSeparator => "_";
		protected override IEnumerable<string> CodePointKeyExclusions => new[] { "fe0f" };
	}
}
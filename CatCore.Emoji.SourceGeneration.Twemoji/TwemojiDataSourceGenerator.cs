using CatCore.Emoji.SourceGeneration.Common;
using Microsoft.CodeAnalysis;

namespace CatCore.Emoji.SourceGeneration.Twemoji
{
	[Generator]
	public class TwemojiDataSourceGenerator : EmojiDataSourceGeneratorBase
	{
		protected override string Type => nameof(Twemoji);
		protected override string BaseUrl => "https://cdn.jsdelivr.net/gh/twitter/twemoji@latest/assets/72x72/";
		protected override string CodepointSeparator => "-";
	}
}
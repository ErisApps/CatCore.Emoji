using CatCore.Emoji.SourceGeneration.Common;
using Microsoft.CodeAnalysis;

namespace CatCore.Emoji.SourceGeneration.Twemoji
{
	[Generator]
	internal class TwemojiDataSourceGenerator : EmojiDataSourceGeneratorBase
	{
		protected override string Type => nameof(Twemoji);
		protected override string BaseUrl => "https://twemoji.maxcdn.com/v/latest/72x72/";
		protected override string CodepointSeparator => "-";
	}
}
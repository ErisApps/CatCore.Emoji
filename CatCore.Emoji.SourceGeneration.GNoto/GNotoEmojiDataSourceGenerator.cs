using CatCore.Emoji.SourceGeneration.Common;

namespace CatCore.Emoji.SourceGeneration.GNoto
{
	public class GNotoEmojiDataSourceGenerator : EmojiDataSourceGeneratorBase
	{
		protected override string Type => nameof(GNoto);
		protected override string BaseUrl => "https://raw.githubusercontent.com/googlefonts/noto-emoji/main/png/72/emoji_";
		protected override string CodepointSeparator => "_";
	}
}
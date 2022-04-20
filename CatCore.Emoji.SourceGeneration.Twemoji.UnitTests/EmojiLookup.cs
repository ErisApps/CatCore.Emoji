using CatCore.Emoji.Models;
using CatCore.Emoji.SourceGeneration.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace CatCore.Emoji.SourceGeneration.Twemoji.UnitTests
{
	public class EmojiLookup : EmojiSourceGeneratorTestBase
	{
		[Theory]
		[MemberData(nameof(GenerateEmojiTestData), TestDataSet.Unicode13_1)]
#pragma warning disable xUnit1026
		public void EmojiLookupTestsUnicode13_1(string line, string codepointsRepresentation, Status status, string emojiRepresentation, string unicodeVersionIntroduced, string emoteDescription)
#pragma warning restore xUnit1026
		{
			EmojiLookupTestsInternal(CatCore.Emoji.Twemoji.EmojiTesting13_1.EmojiReferenceData, line, codepointsRepresentation, status, emojiRepresentation, unicodeVersionIntroduced,
				emojiRepresentation);
		}

		[Theory]
		[MemberData(nameof(GenerateEmojiTestData), TestDataSet.Unicode14_0)]
#pragma warning disable xUnit1026
		public void EmojiLookupTestsUnicode14_0(string line, string codepointsRepresentation, Status status, string emojiRepresentation, string unicodeVersionIntroduced, string emoteDescription)
#pragma warning restore xUnit1026
		{
			EmojiLookupTestsInternal(CatCore.Emoji.Twemoji.EmojiTesting14_0.EmojiReferenceData, line, codepointsRepresentation, status, emojiRepresentation, unicodeVersionIntroduced,
				emojiRepresentation);
		}

		private static void EmojiLookupTestsInternal(EmojiTreeRoot emojiTreeRoot, string line, string codepointsRepresentation, Status status, string emojiRepresentation,
			string unicodeVersionIntroduced, string emoteDescription)
		{
			// Arrange
			// NOP

			// Act
			IEmojiTreeLeaf? foundEmojiLeaf = null;
			for (var i = 0; i < line.Length; i++)
			{
				foundEmojiLeaf = emojiTreeRoot.LookupLeaf(line, i);
				if (foundEmojiLeaf != null)
				{
					break;
				}
			}

			// Assert
			foundEmojiLeaf.Should().NotBeNull();
			foundEmojiLeaf!.Key.Should().Be(codepointsRepresentation.Replace(" ", "-").ToLowerInvariant());
			foundEmojiLeaf.Depth.Should().Be(emojiRepresentation.ToCharArray().Length - 1);
			foundEmojiLeaf.Url.Should().Be($"https://twemoji.maxcdn.com/v/latest/72x72/{codepointsRepresentation.Replace(" ", "-").ToLowerInvariant()}.png");

			// Not asserting failure conditions because it might either result in no matches or in a match of a fully-qualified subset emote
		}
	}
}
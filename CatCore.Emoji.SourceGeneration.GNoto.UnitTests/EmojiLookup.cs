using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CatCore.Emoji.Models;
using FluentAssertions;
using Xunit;

namespace CatCore.Emoji.SourceGeneration.GNoto.UnitTests
{
	public class EmojiLookup
	{
		public enum Status
		{
			Component,
			FullyQualified,
			MinimallyQualified,
			Unqualified
		}

		public static IEnumerable<object[]> EmojiTestData =>
			File.ReadLines(Path.Combine(Environment.CurrentDirectory, "Resources", "Unicode14_0_EmojiTest.txt"))
				.Where(line => !string.IsNullOrWhiteSpace(line) && line[0] != '#')
				.Select(line =>
				{
					var splitEntries = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

					var splitEntriesIndexCursor = splitEntries.IndexOf(";");
					var codepointsRepresentation = string.Join(" ", splitEntries.Take(splitEntriesIndexCursor));

					var status = Enum.Parse<Status>(splitEntries[++splitEntriesIndexCursor].Replace("-", string.Empty), true);

					var emojiRepresentation = splitEntries[splitEntriesIndexCursor += 2];
					var unicodeVersionIntroduced = splitEntries[++splitEntriesIndexCursor];

					var emoteDescription = string.Join(" ", splitEntries.Skip(splitEntriesIndexCursor));

					return new object[] { line, codepointsRepresentation, status, emojiRepresentation, unicodeVersionIntroduced, emoteDescription };
				})
				.Where(x => ((Status)x[2]) == Status.FullyQualified);

		[Theory]
		[MemberData(nameof(EmojiTestData))]
#pragma warning disable xUnit1026
		public void EmojiLookupTests(string line, string codepointsRepresentation, Status status, string emojiRepresentation, string unicodeVersionIntroduced, string emoteDescription)
#pragma warning restore xUnit1026
		{
			// Arrange
			// NOP

			// Act
			IEmojiTreeLeaf? foundEmojiLeaf = null;
			for (var i = 0; i < line.Length; i++)
			{
				foundEmojiLeaf = CatCore.Emoji.GNoto.EmojiTesting.EmojiReferenceData.LookupLeaf(line, i);
				if (foundEmojiLeaf != null)
				{
					break;
				}
			}

			// Assert
			foundEmojiLeaf.Should().NotBeNull();
			foundEmojiLeaf!.Key.Should().Be(codepointsRepresentation.Replace(" ", "_").ToLowerInvariant());
			foundEmojiLeaf.Depth.Should().Be(emojiRepresentation.ToCharArray().Length - 1);

			// Not asserting failure conditions because it might either result in no matches or in a match of a fully-qualified subset emote
		}
	}
}
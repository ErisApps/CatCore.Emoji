using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace CatCore.Emoji.SourceGeneration.UnitTests.Shared
{
	public abstract class EmojiSourceGeneratorTestBase
	{
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public enum TestDataSet
		{
			Unicode13_1,
			Unicode14_0
		}

		public enum Status
		{
			Component,
			FullyQualified,
			MinimallyQualified,
			Unqualified
		}

		private static string ConvertTestDataSetToFileName(TestDataSet testDataSet) => testDataSet switch
		{
			TestDataSet.Unicode13_1 => "Unicode13_1_EmojiTest.txt",
			TestDataSet.Unicode14_0 => "Unicode14_0_EmojiTest.txt",
			_ => throw new ArgumentOutOfRangeException(nameof(testDataSet), testDataSet, null)
		};

		protected static IEnumerable<object[]> GenerateEmojiTestData(TestDataSet testDataSet) =>
			File.ReadLines(Path.Combine(Environment.CurrentDirectory, "Resources", ConvertTestDataSetToFileName(testDataSet)))
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
	}
}
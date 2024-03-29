﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CatCore.Emoji.Models;
using CatCore.Emoji.SourceGeneration.Common.Extensions;
using CatCore.Emoji.SourceGeneration.Common.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CatCore.Emoji.SourceGeneration.Common
{
	public abstract class EmojiDataSourceGeneratorBase : ISourceGenerator
	{
		protected abstract string Type { get; }
		protected abstract string BaseUrl { get; }
		protected abstract string CodepointSeparator { get; }
		protected virtual IEnumerable<string> CodePointKeyExclusions { get; } = Array.Empty<string>();

		public virtual void Initialize(GeneratorInitializationContext context)
		{
			// NOP - No initialization required for source generator
			context.RegisterForPostInitialization(initializationContext => SpecializedClassBuilderHelper.GenerateSpecializedModels(initializationContext, Type, BaseUrl));
		}

		public virtual void Execute(GeneratorExecutionContext context)
		{
			var additionalFiles = GetLoadOptions(context);
			var sourceFilesFromAdditionalFiles = SourceFilesFromAdditionalFiles(additionalFiles);
			foreach (var (className, code) in sourceFilesFromAdditionalFiles)
			{
				context.AddSource(className, SourceText.From(code, Encoding.UTF8));
			}
		}

		private static IEnumerable<(string className, AdditionalText file)> GetLoadOptions(GeneratorExecutionContext context)
		{
			foreach (var file in context.AdditionalFiles)
			{
				if (Path.GetExtension(file.Path).EndsWith("txt", StringComparison.OrdinalIgnoreCase))
				{
					context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.additionalfiles.ClassName", out var className);
					if (!string.IsNullOrWhiteSpace(className))
					{
						yield return (className!, file);
					}
				}
			}
		}

		private IEnumerable<(string, string)> SourceFilesFromAdditionalFiles(IEnumerable<(string className, AdditionalText file)> fileEntries)
			=> fileEntries.Select(entry => SourceFilesFromAdditionalFile(entry.className, entry.file));

		private (string, string) SourceFilesFromAdditionalFile(string className, AdditionalText file)
		{
			var emojiTree = BuildReferenceEmojiTree(file);
			return (className, GenerateClassFile(className, emojiTree));
		}

		private EmojiTreeRoot BuildReferenceEmojiTree(AdditionalText file)
		{
			IEnumerable<(string codepointRepresentation, EmojiStatus emojiStatus, char[] emojiCharRepresentation)> fullyQualifiedEmotes = file.GetText()!
				.Lines
				.Select(line => line!.ToString())
				.Where(line => !string.IsNullOrWhiteSpace(line) && line[0] != '#')
				.Select(line =>
				{
					var splitEntries = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

					var splitEntriesIndexCursor = splitEntries.IndexOf(";");
					var codepointsRepresentation = string.Join(CodepointSeparator, splitEntries
						.Take(splitEntriesIndexCursor)
						.Select(x => x.ToLowerInvariant())
						.Where(x => !CodePointKeyExclusions.Contains(x)));

					Enum.TryParse<EmojiStatus>(splitEntries[++splitEntriesIndexCursor].Replace("-", string.Empty), true, out var emojiStatus);

					var emojiCharRepresentation = splitEntries[splitEntriesIndexCursor + 2].ToArray();

					return (codepointsRepresentation, emojiStatus, emojiCharRepresentation);
				})
				.Where(emojiData => emojiData.emojiStatus == EmojiStatus.FullyQualified);

			var emojiTreeRoot = new EmojiTreeRoot();
			foreach (var (codepointRepresentation, _, emojiCharRepresentation) in fullyQualifiedEmotes)
			{
				emojiTreeRoot.AddToTree(codepointRepresentation, emojiCharRepresentation);
			}

			return emojiTreeRoot;
		}

		private string GenerateClassFile(string className, EmojiTreeRoot emojiTreeRoot)
		{
			var sourceBuilder = new StringBuilder();

			//// Usings
			sourceBuilder.Append(@$"#nullable enable
using System.Collections.Generic;
using CatCore.Emoji.Models;

namespace CatCore.Emoji.{Type}
{{
	public static class {className}
	{{
		public static {nameof(EmojiTreeRoot)} EmojiReferenceData {{ get; }} = new()
		{{
");

			emojiTreeRoot.AddToSourceBuilder(sourceBuilder, Type, 3);

			sourceBuilder.Append(
@"		};
	}
}");

			return sourceBuilder.ToString();
		}
	}
}
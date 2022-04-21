# CatCore.Emoji

CatCore.Emoji is a project that consists of a group of packages that provide source generators to generate emoji lookup tables at compile-time.
At the time of writing, has support for both [Google Noto (GNoto)](https://github.com/googlefonts/noto-emoji) and [Twemoji](https://github.com/twitter/twemoji) emojis.

## Usage
1. Install either [CatCore.Emoji.SourceGeneration.GNoto](https://github.com/ErisApps/CatCore.Emoji/packages/1121298) or [CatCore.Emoji.SourceGeneration.Twemoji](https://github.com/ErisApps/CatCore.Emoji/packages/1121297) from the GitHub package registry.
(Make sure not to install both, they will conflict with each other)

2. Get the emoji reference file (`emoji-test.txt`) for the Unicode version you want to get the lookup table for. It can be found [here](https://unicode.org/Public/emoji/), also make sure to double-check whether either GNoto or Twemoji project already supports the Unicode standard you want to use.

3. Drop the emoji reference file from the previous step in a folder of your project and add the following snippet in your .csproj file as well.
Make sure that the path to the file is correct. Also, make sure to define a name for the generated class as well, it will be generated in the `CatCore.Emoji` namespace.
```xml
<ItemGroup>
	<PackageReference Include="CatCore.Emoji.SourceGeneration.Twemoji" Version="1.0.1" OutputItemType="Analyzer" />
</ItemGroup>

<ItemGroup>
	<AdditionalFiles Include=".\Resources\Unicode14_0_EmojiTest.txt" ClassName="{{Choose the name of the generated class}}" />
</ItemGroup>
```

4. If everything went well, you should be able to use the generated lookup table from code.
The example below should give you an idea on how to use it.
```csharp
var emojiReferenceData = CatCore.Emoji.Twemoji.EmojiTesting14_0.EmojiReferenceData;
var testEntries = new[] { "😸", "I 🧡 Twemoji! 🥳", "I've eaten Chinese food 😱😍🍱🍣🍥🍙🍘🍚🍜🍱🍣🍥🍙🍘🍚🍜", "🧝‍♀️", "🏳️‍⚧️" };

foreach (var entry in testEntries)
{
    for (var i = 0; i < entry.Length; i++)
    {
        var emojiTreeLeaf = emojiReferenceData.LookupLeaf(entry, i);
        if (emojiTreeLeaf != null)
        {
            Console.WriteLine($"Found emote between indexes [{i}-{i + emojiTreeLeaf.Depth}[ (Length: {emojiTreeLeaf.Depth})\n" +
                              $"  Twemoji url: {emojiTreeLeaf.Url}");
            i += emojiTreeLeaf.Depth;
        }
    }
}
```
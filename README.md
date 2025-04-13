# mb_MusicBrainzSync
MusicBee plugin to sync your tags/ratings to your account on MusicBrainz.

## Rationale
I wanted something that would let me send data to MusicBrainz on Linux, but I use MusicBee through WINE and WINE apps don't play well with native apps with stuff like drag-and-drop (which was my workflow on Windows). So I thought, why not do it through MusicBee itself? WINE handles .NET pretty well nowadays.

Yeah, it still means I'm dependent on Windows for a bit, but at least my workflow will no longer be broken on Linux and other people can have that flexibility.

## Installation
Move all of the DLLs from the MusicBrainz Sync ZIP into your MusicBee plugins directory.

## Compatibility
This plugin is compatible with MusicBee 3.5 and later.

It should be compatible with plugins that use:
- Newtonsoft.Json 13.0.3
- TagLibSharp 2.3.0

Plugins that use older versions may need to be updated.

## Features
- Send track and album ratings from your local library to MusicBrainz (recordings and release groups).
- Send tags to recordings, releases and release groups.
- Choose to have separate tags to send for recordings, releases and release groups.
- Find and replace while sending. (e.g. mapping your genre tags to MusicBrainz's default genre tags like "synth-pop" vs. "synthpop")

### Planned
- Import/export of plugin settings
- The ability to get data from MusicBrainz as well as set.
- "Smarter" way of handling tags. Right now it's either all or nothing on keeping your tags.
- Automatic submission of tags/ratings when you change them in MusicBee.

### Not planned
- Submission of artist ratings/tags
    - I implemented this on another plugin I wrote - Submit Folksonomy Tags, for Picard - but it didn't exactly make much sense. I feel it'd be better to do this via the MusicBrainz web UI itself.
- Collections
    - Doable, but I'd need to see if I can use any alternative to WinForms because WinForms is bad.

## Building
Visual Studio 2022 is the recommended IDE for this plugin. Untested on other IDEs with `dotnet`.

Before attempting to build, make sure you edit the following parameters in plugin.csproj:
- `ProgramExePath`: Location of your MusicBee install 
    - It's generally recommended to develop on a portable copy of MusicBee so you don't affect your main installation.
- `ProgramPluginsDir`: Location of the MusicBee plugin directory

Once cloned, just load `plugin.sln` in Visual Studio and build inside it.

# mb_MusicBrainzSync
MusicBee plugin to sync your tags/ratings to your account on MusicBrainz.

## Rationale
I wanted something that would let me send data to MusicBrainz on Linux, but I use MusicBee through WINE and WINE apps don't play well with native apps with stuff like drag-and-drop (which was my workflow on Windows) and no player native to Linux has been able to replace MusicBee for me.

So I thought, why not do it through MusicBee itself? WINE handles .NET pretty well nowadays.

Yeah, it still means I'm dependent on Windows for a bit, but at least my workflow will no longer be broken on Linux and other people can have that flexibility.

## Features
- Send track and album ratings from your local library to MusicBrainz (recordings and release groups).
- Send tags to recordings, releases and release groups.
- Choose to have separate tags to send for recordings, releases and release groups.
- Find and replace while sending. (e.g. mapping your genre tags to MusicBrainz's default genre tags like "synth-pop" vs. "synthpop")
- Import/export of plugin settings

### Planned
- The ability to get data from MusicBrainz as well as set.
  - Retrieval of ratings is in development.
- Automatic submission of tags/ratings when you change them in MusicBee.

### Not planned
- Submission of artist ratings/tags
    - I implemented this on another plugin I wrote - Submit Folksonomy Tags, for Picard - but it didn't exactly make much sense. I feel it'd be better to do this via the MusicBrainz web UI itself.
- Collections
    - Doable, but I'd need to see if I can use any alternative to WinForms because WinForms is bad.

## Installation
Move all of the DLLs from the MusicBrainz Sync ZIP into your MusicBee plugins directory.

Note that installing via MusicBee doesn't work, possibly due to the requirement of other libraries.

## Compatibility
This plugin is compatible with MusicBee 3.5 and later.

It should be compatible with plugins that use:
- Newtonsoft.Json 13.0.3
- TagLibSharp 2.3.0

Plugins that use older versions will need to be updated.

## Limitations
- Custom tracks limited up to 16 even on MusicBee 3.6 which bumped the custom tag limit to 20.
    - Will be fixed by bumping the MusicBee API up when it's released.
- Rating feature will malfunction if user saves multiple ratings using the Tag Editor instead of rating each track individually - submitting the previously saved rating instead of the new rating.
    - Likely a bug with MusicBee. Workaround is to change the ratings individually when MusicBee ingests new files into your library.
- .TAK files are not supported due to limitations with TagLib#.

## Building
Visual Studio 2022 is the recommended IDE for this plugin. Untested on other IDEs with `dotnet`.

Before attempting to build, make sure you edit the following parameters in plugin.csproj:
- `ProgramExePath`: Location of your MusicBee install 
    - It's generally recommended to develop on a portable copy of MusicBee so you don't affect your main installation.
- `ProgramPluginsDir`: Location of the MusicBee plugin directory

Once cloned, just load `plugin.sln` in Visual Studio and build inside it.

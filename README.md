# mb_MusicBrainzSync
MusicBee plugin to sync your tags/ratings to your account on MusicBrainz.

## Building
Visual Studio 2022 is the recommended IDE for this plugin. Untested on other IDEs with `dotnet`.

Before attempting to build, make sure you edit the following parameters in plugin.csproj:
- `ProgramExePath`: Location of your MusicBee install 
    - It's generally recommended to develop on a portable copy of MusicBee so you don't affect your main installation.
- `ProgramPluginsDir`: Location of the MusicBee plugin directory

Once cloned, just load `plugin.sln` in Visual Studio and build inside it.

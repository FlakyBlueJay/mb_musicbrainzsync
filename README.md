# MusicBee Plugin Template

This is a template for creating a MusicBee plugin.
It contains a basic plugin that can be built and run.

Please see the [MusicBee API](https://getmusicbee.com/help/api/) for more information on plugin development.

## Structure

- `plugin` - Contains the plugin source code.
    - `plugin/plugin.csproj` - The project file for the plugin.
    > Here is where you change your plugin's metadata.
    - `plugin/MusicBeeInterface.cs` - Contains the MusicBee interface.
    > It's the interface you download from [MusicBee API](https://getmusicbee.com/help/api/). It's already included in this template, but you can update it if you want.
    - `plugin/Plugin.cs` - Contains the plugin's functionality.

- `console` - Contains a console application that can be used to test the plugin.
    - `console/console.csproj` - The project file for the console application.
    - `console/Program.cs` - Contains the console application's functionality.

## Setup

This template assumes that you're using Visual Studio Code, but it should work with any IDE.
It is recommended that you use [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) tool to build and run the plugin.

Start by modifying the `plugin/plugin.csproj` file to change the plugin's metadata, and setting the path to MusicBee executable and plugin folder.

## Building
> Note that `dotnet` cli tool usage is optional. You can also just open the .sln file in Visual Studio and build the project from there.

To only build the plugin, run the following command:

- `dotnet build plugin/plugin.csproj`

    This will create a `plugin/bin/Debug/net4.8-windows/mb_[PluginName].dll` file.
    
    For convenience, the plugin DLL and its dependencies are copied to the `build/out` folder and zipped into `build/[PluginName].zip`.

    Additionally, if `CopyToProgram` in `plugin/plugin.csproj` is set to `true`, the plugin is copied to the MusicBee plugin folder specified in the `plugin/plugin.csproj` file.

    The MusicBee application will automatically get restarted during this process.
    Console output will then be displayed in the terminal.

To build the plugin and the console application, run the following command:

- `dotnet run --project console/console.csproj`

## Tips & Tricks

- Add NuGet packages with the following command:

    - `dotnet add plugin/plugin.csproj package [PackageName]`

- Build a release version of the plugin with the following command:

    - `dotnet build plugin/plugin.csproj -c Release`

## Contributing

If you have any suggestions or improvements, feel free to open an issue or a pull request.
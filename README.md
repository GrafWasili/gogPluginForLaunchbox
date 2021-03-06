Version 0.1

*Only works on 64-bit installs of Launchbox*

## About

A plugin for Launchbox that allows adding non-installed games from gog.com.
Excuse the sloppy and inconsistent code, it's the first C#/.NET/Visual Studio project for me.

## Features

- login in to gog.com via popup browser
- store authentication token so no further login is needed
- select platform for games import
- option to skip already imported games and games with the same title

At the moment there are two import modes:

1. Start with Galaxy

Sets the application path to gog Galaxy with a game id as starting parameter. This allows to install and start the game via the Galaxy client.

2. Download Installer

Sets the application path to a download link for the game installer of your choice (language, editions etc.) but that's it.
After installing you have to set the path for the game's executable yourself.


## Download and Installation

Download the binaries from [here](https://forums.launchbox-app.com/files/file/1283-gogcom-plugin/) unzip and copy the contents of bin/ to a new folder in the Launchbox plugin folder (e.g. launchbox/plugins/gogplugin).

If necessary unblock the .dll libraries via context menu -> Properties.

You should now find the item "import games from gog.com" in the tools menu of Launchbox.

## Contribute

To use the source code in a new Visual Studio 2017 project follow this steps:

1) Select "New.." -> "Project from existing code" creating a class library project form the folder "gogPlugin"

2) If CefSharp and Json.Net references are not added automatically, run the following command in the Package Manager Console: Update-Package -reinstall

3) In order to make CefSharp work, set your project and soution platform to x64.


In gogData.cs you can find a lot of automatically generated classes for deserializing calls to the gog api with json.net for getting game information. Most of them are yet unused but should provide a good base for additional features.

Unofficial gog API docuentation: https://gogapidocs.readthedocs.io/en/latest/

Tool for generating C# classes from json structures: http://json2csharp.com/


Launchbox Forum thread: https://forums.launchbox-app.com/topic/45589-work-in-progress-gogcom-plugin/

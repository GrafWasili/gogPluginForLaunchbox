Version 0.1

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

Download the binaries from [here](https://forums.launchbox-app.com/files/file/1283-gogcom-plugin/) and copy the gogplugin folder into the Launchbox plugin folder.


## Contribute

Launchbox Forum thread: https://forums.launchbox-app.com/topic/45589-work-in-progress-gogcom-plugin/

To use the source code in a new Visual Studio 2017 project follow this steps:

1) Select "New.." -> "Project from existing code" creating a class library project form the folder "gogPlugin"

2) If CefSharp and Json.Net references are not added automatically, run the following command in the Package Manager Console: Update-Package -reinstall

3) In order to make CefSharp work, set your project and soution platform to x64.

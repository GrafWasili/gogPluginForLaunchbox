To use the source code in a new Visual Studio 2017 project follow this steps:

1) Select "New.." -> "Project from existing code" creating a class library project form the folder "gogPlugin"

2) If CefSharp and Json.Net references are not added automatically, run the following command in the Package Manager Console: Update-Package -reinstall

3) In order to make CefSharp work, set your project and soution platform to x64.
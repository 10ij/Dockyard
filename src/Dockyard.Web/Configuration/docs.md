# What is going on???

This replaces the old config.php where everything was a constant that had to be defined

Rather than constants, everything is passed on to the app, and the app will simply refuse to run without what it needs

Since this is called from the builder in `program.cs` everything does have to be set at startup
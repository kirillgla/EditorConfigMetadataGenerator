# EditorConfig metadata generator

This project can be used to generate EditorConfig metadata for IntelliJ.

### The problem:
Microsoft released [a new set of EditorConfig rules](https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-formatting-conventions).
For IntelliJ-based IDEs to support them, metadata describing those keys and values needs to be provided.
Due to sheer numbers of the new rules, creating the metadata manually is tiresome.

### The solution:
Download the page and use regular expressions to extract rules from it.
Then generate the metadata based on that.

### Usage:
1. Open the [page with rule descriptions](https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-formatting-conventions);
2. Right-click it and select `View page source`;
3. Copy all the code;
4. Create a file called `Data.html` on the disk and paste the code into it;
5. Build and run the project, passing a path to the folder containing `Data.html` as an argument;
6. Open `Output.json` file created near `Data.html`.

The generated metadata can now be moved into the proper place in IntelliJ EditorConfig plugin.

Kirill Glazyrin, JetBrains, 2020
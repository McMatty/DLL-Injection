# DLL-Injection
For a while I wanted to understand how to perform DLL injection. Originally this was with the intent to use it to create a way of accessing Blizzards Starcraft to access BWAPI.
Being stuborn I didnt want to copy and past existing C++ code as I have a fundemental understanding and thats a stretch so I chose to port what I could to C#.
Unfortunately C# and .Net is managed so missing the DLL hooks required.

So what we have here is C++ code that loads the .Net framework and then the C# dll into the process space. There is also some code in C#
looking into performing DLL injection via C#, maybe a way but beyond my skillset and effort I wanted to put in.

# Spring State Generator

This utility allows you to automatically generate immutable classes used for storing state in **Spring** game.
You can describe desired field in text file with *.scs* extension then run the program to do the rest for you.
Input *.scs* should be in the same directory as this program. *.cs* file will be generated in the same directory as well. *.scs.*
file structure is following:

Example structure for *test.scs* file:
```
GeneratedState
bool;AlarmTurnedOff;true
string;ElixirName;"";Name of the chosen elixir
int;AngerLevel;13
```

Name of generated class should be on the first line. Each other line describes one field: *data type*;*name*;*default value*;*comment*

Run using `SpringStateGenerator test`

Generated class will have fields `AlarmTurnedOff`, `ElixirName`, `AngerLevel` to access
these values and setter methods `SetAlarmTurnedOff`, `SetElixirName`, `SetAngerLevel`. Each of these methods will return new
instance of `GeneratedState` object.

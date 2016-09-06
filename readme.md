# ReTracker 0.1 (alpha) #

ReTracker is a plugin (machine) for [Jeskola Buzz](http://jeskola.net/buzz/).  It's a workflow utility for tracking peer note data.

## Features ##

It's a very simple machine:
 
* Send MIDI notes to other machines
* Track MIDI notes with volume data
* Target multiple machines

### Not yet Implemented ###

* Save assignments
* Disable target
* Modify key map per track
* Target other ReTracker instances

## Usage ##

Only machines that accept MIDI notes can be targetted and you may need to enable MIDI via attributes on the target machine.

Some machines simply don't work as targets. 

### Add Target ###

Open ReTracker GUI, select a target from the drop down list and then click 'Add Target' to target a machine.

### Example Pattern ###

```
00 C-4 7F    MIDI note C-4, 127 volume
01 ... ..
02 ... .. 
03 ... .. 
04 ... 50    MIDI note C-4, 80 volume
05 ... .. 
06 ... .. 
07 ... .. 
08 E-4 40    MIDI note E-4, 64 volume
09 ... .. 
10 ... .. 
11 ... .. 
12 ... 50    MIDI note E-4, 80 volume
13 ... .. 
14 ... 10    MIDI note E-4, 16 volume
15 ... ..   
```

### Build ###

Use Visual Studio.  Reference assemblies from Buzz.

### Install ###

Put the dll in Buzz\Gear\Generators directory.
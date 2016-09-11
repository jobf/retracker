# ReTracker 0.4 (alpha) #

ReTracker is a plugin (machine) for [Jeskola Buzz](http://jeskola.net/buzz/).  It's a workflow utility for tracking peer note data.

Use at your own risk, parameters won't change until version 1.0 if required (unlikely).  Targets are now saved to bmx.

## Features ##

It's a very simple machine:
 
* Send notes to other machines
* Track notes with velocity data
* Target multiple machines

### Not yet Implemented ###

* Disable target
* Modify key map per target
* Target other ReTracker instances

## Usage ##

Buzz song must be playing for notes to be sent to targets. 

Don't edit patterns too fast or it crashes, sorry!

### Add Target ###

Open ReTracker GUI, select a target from the drop down list and then click 'Add Target' to target a machine.

### Example Pattern ###

```
00 C-4 7F    note C-4, 127 velocity
01 ... ..
02 ... .. 
03 ... .. 
04 ... 50    note C-4, 80 velocity
05 ... .. 
06 ... .. 
07 ... .. 
08 E-4 40    note E-4, 64 velocity
09 ... .. 
10 ... .. 
11 ... .. 
12 ... 50    note E-4, 80 velocity
13 ... .. 
14 ... 10    note E-4, 16 velocity
15 ... ..   
```

### Build ###

Use Visual Studio.  Reference assemblies from Buzz.

### Install ###

Put the dll in Buzz\Gear\Generators directory.
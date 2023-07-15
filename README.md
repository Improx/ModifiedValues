# ModifiedValues
ModifiedValues is a C# library for Unity that enables modifying (numeric and other) values while keeping track and managing the modifiers affecting them.

Minimum requirement is <strong>Unity 2021.2</strong> (for C# 9 and netstandard2.1).

## Quickstart Example

You're making a buff system for your game. Instead of having a classic float variable on your character

```C#
float Speed = 10;
```

You can create a Modified version of it:

```C#
ModifiedFloat Speed = 10;
```
If the field is public or serialized, it will also appear in the inspector:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/images/speedInspector1.PNG "ModifiedValue Speed visible in the inspector")

For convenience, this `Speed` object can be implicitly cast back into a float. Most of your code can treat it as just a regular float value:

```C#
transform.position += Speed * Time.deltaTime;
```

Let's say your character gets an Energized buff that multiplies base speed by 120%. Your character also equips rollerscates, increasing speed by 5. You apply a these multiplicative and additive modifiers like this:

```C#
Speed.Mul(1.2f);
Speed.Add(5);
```

By default, the multiplicative modifier is applied before the additive one. This results in a final speed value of 17. Your code that uses `Speed` will automatically pick up this update. The current value is also visible in the inspector:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/images/speedInspector2.PNG "Updated value of Speed visible in the inspector")

If you want to be able to remove these buffs later, you need to save the modifier objects:

```C#
Modifier energizedBuff = Speed.Mul(1.2f);
Modifier rollerScatesBuff = Speed.Add(5);

//After some time passes, you want to remove the Energized buff.
energizedBuff.DetachFromAll();

Debug.Log(Speed.Value); //Will print 12
```

Without this library, where `Speed` is just a normal `float`, you would have needed to do something like this:

```C#
Speed *= 1.2f;
Speed += 5;

//After some time passes, you want to remove the Energized buff.
//However, we can't just simply divide by 1.2f to get the correct result, because
//the rollerscates buff is still active
//We need to keep the additive rollerscates effect like this:

Speed -= 5;
Speed /= 1.2f;
Speed += 5;

Debug.Log(Speed); //Will print 12
```
With many different kinds of buffs, doing this manually could get extremely convoluted. One of the main conveniences of this library is that the buffs don't have to know about each other. For each modifier, you just define how it modifies the value, and then you can attach and detach these modifiers independently, while keeping the final value always correct. You also don't need to worry about the temporal order in which you apply modifiers. The ordering, layers and priorities of modifiers are defined in optional parameters (explained further down).

This library provides the following wrapper types with lots of ready functionality, and you can easily create more:

* `ModifiedFloat`
* `ModifiedDouble`
* `ModifiedDecimal`
* `ModifiedInt`
* `ModifiedUint`
* `ModifiedLong`
* `ModifiedUlong`
* `ModifiedBool`
* `ModifiedEnum<YourEnum>`

You can wrap other types without needing to create new classes simply by using `ModifiedValue<MyType>`, it just won't have as much functionality by default. For example `ModifiedFloat` too inherits from `ModifiedValue<float>` and adds a bunch of methods on top, such as `Add` and `Mul`. With a generic wrapper, you can still apply modifiers with any custom operations:

```C#
ModifiedValue<MyType> myValue = new MyType();
Modifier mod = myValue.Modify((v) => v * 1.2f + 5);
```

## Priority, Layer and Order

TODO
IMAGE for explanation
Layers for talents, equipment, temporary buffs

## Initialization
Implicit casting from value to ModValue to create it. Alternative to constructor. Note that it replaces the old modvalue object if there was one.
```C#
ModifiedFloat Speed1 = 5;
//Is the same as:
ModifiedFloat Speed2 = new ModifiedFloat(5);
//Is the same as:
ModifiedFloat Speed3 = new ModifiedFloat(() => 5);
```
BaseValueGetter (can be dynamic. For example it can depend on the value of another ModifiedValue. This external dependency is handled safely.)
UpdateEveryTime

### Undeclared ModifiedValue objects = bad!
```C#
[Serializedfield] private ModifiedFloat Speed; //Set this reference to a new ModifiedFloat before using it!
```
Declaring a serialized ModifiedValue member variable and not assigning anything to it leads to Unity creating a default object out of it, instead of keeping the reference as `null`. In this Unity quirk, the constructor is bypassed and the ModifiedValue is not initialized correctly. Using such ModifiedValue objects will result in errors. Always set it to something when declaring it, or later. If needed, you can check whether a ModifiedValue object was created in this bad way (in that case `ModifiedValue.Init` equals `false`) and replace it with a new object.

## Handling Modifiers

TODO
ATTACHING AND DETACHING
ACTIVE BOOL
MODIFIERGROUPS
ADDING ONE MODIFIER TO MULTIPLE MODVALUES
DIRTY FLAG
TEMPLATE MODIFIERS (NOT YET ATTACHED TO ANY VALUE)
CUSTOM OPERATIONS, IF EXTERNAL DEPENDENCIES, SET TO UPDATEEVERYTIME
COMPOUND AND NONCOMPOUND

## Previewing Values

TODO

## Inspector

TODO
SETTINGS TO PREVIEW FINAL VALUE
SAVED VALUE VS GETTER



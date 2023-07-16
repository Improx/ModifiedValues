# ModifiedValues
[![Unity 2021.2+](https://img.shields.io/badge/unity-2021.2%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/Improx/ModifiedValues/blob/main/LICENSE)

ModifiedValues is a C# library for Unity that enables modifying (numeric and other) values while keeping track and managing the modifiers affecting them.

Minimum requirement is <strong>Unity 2021.2</strong> (for C# 9 and netstandard2.1).

[HeaderDecorator]: https://placehold.co/15x15/00dd00/00dd00.png

 ## ![][HeaderDecorator] Quickstart Example ![][HeaderDecorator]

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
In rare cases where that is not possible, you can get the float value by `Speed.Value`.

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

Debug.Log(Speed); //Will print 12
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

## ![][HeaderDecorator] Initialization ![][HeaderDecorator]
You can create a new ModifiedValue object in many ways. You can create a new object with a contructor, where you pass the base value as a parameter. Implicitly setting a ModifiedValue object to a base value does the same thing. You can also call the constructor with a base value getter function parameter, in which case the base value can have external dependencies (for example, the base value can depend on the value of another ModifiedValue).

```C#
ModifiedFloat Speed1 = 5;
//Is the same as:
ModifiedFloat Speed2 = new ModifiedFloat(5);
//Is the same as:
ModifiedFloat Speed3 = new ModifiedFloat(() => 5);
//Is the same as:
ModifiedFloat Speed4 = new ModifiedFloat(ReturnFive);

private float ReturnFive()
{
  return 5;
}
```
Note that if at a later stage, you set a ModifiedValue object to a new base value implicitly again, the reference will point to a completely new ModifiedValue object.

```C#
ModifiedFloat Speed = 5;
Speed.Add(1);
Speed = 3; //Speed is now a completely new object, with a base value of 3 and the previous Add modifier removed.
```
If you want to update a ModifiedValue's base value, you can update its `BaseValue` or `BaseValueGetter` function directly.

### :warning: Uninitialized ModifiedValue references = bad! :warning:
```C#
[Serializedfield] private ModifiedFloat Speed; //Set this reference to a new ModifiedFloat before using it!
```
Declaring a serialized ModifiedValue member variable and not assigning anything to it leads to Unity creating a default object out of it, instead of keeping the reference as `null`. In this Unity quirk, the constructor is bypassed and the ModifiedValue is not initialized correctly. Using such ModifiedValue objects will result in errors. Always set it to something when declaring it, or later. If needed, you can check whether a ModifiedValue object was created in this bad way (in that case `ModifiedValue.Init` equals `false`) and replace it with a new object.

## ![][HeaderDecorator] Out-of-the-box Modifiers ![][HeaderDecorator]

The following modifying methods are readily available for `ModifiedFloat`, `ModifiedDouble` and `ModifiedDecimal`:
* `Set()`: Forces to this value.
* `AddFraction()`: Adds this fraction of the value (relative to what the value was at the beginning of the layer). Multiple modifiers of this kind stack additively.
* `Mul()`: Multiplies the value by this amount. Multiple modifiers of this kind stack multiplicatively.
* `Add()`: Adds this value. Can be negative.
* `MaxCap()`: Limits value from above.
* `MaxCapFinal()`: Limits value from above. Is applied with priority and layer equaling to int.MaxValue.
* `MinCap()`: Limits value from below.
* `MinCapFinal()`: Limits value from below. Is applied with priority and layer equaling to int.MaxValue.

Available for `ModifiedInt`. `ModifiedUint`, `ModifiedLong` and `ModifiedUlong`:
* `Set()`
* `AddMultiple()`: Adds this multiple of the value (relative to what the value was at the beginning of the layer). Multiple modifiers of this kind stack additively.
* `Mul()`
* `Add()`
* `MaxCap()`
* `MaxCapFinal()`
* `MinCap()`
* `MinCapFinal()`

Available for `ModifiedBool`:
* `Set()`
* `Not()`: Applies a Not logic gate.
* `And()`: Applies a And logic gate.
* `Or()`: Applies a Or logic gate.
* `Xor()`: Applies a Xor logic gate.
* `Imply()`: Applies a Imply logic gate.

The generic `ModifiedEnum<YourEnum>` type only has the `Set()` Modifier readily available.

If many different modifiers are applied that have the same `Priority`and `Layer`, they will all have effect. They will be applied in the same order as they are presented in the lists above (from top to bottom). This ordering is also visible in the `DefaultOrders.cs` class. If you are not happy with some of the default ordering, you can always use a custom order in a modifier. For example: `Speed.Set(99f, order: 50)`.

You can also create your own modifying operations either with an inline function `myValue.Modify((v) => v * 1.2f + 5)` or by using a function defined elsewhere: `myValue.Modify(MyCustomOperation)`. More about custom operations explained further down.

## ![][HeaderDecorator] Priority, Layer and Order ![][HeaderDecorator]

The temporal order in which Modifiers were attached to a ModifiedValue does not matter. Their interrelations are instead defined by optional `priority`, `layer` and `order` parameters:

```C#
//Custom parameters
Speed.Mul(1.2f, priority : 3, layer : 2, order : 3);

//By omitting them, priority and layer default to 0, and order defaults to the
//operation's default order defined in DefaultOrders.cs, if using an out-of-the-box modifier:

Speed.Mul(1.2f); //Priority and layer are 0, and order is DefaultOrders.Mul = 2000

//If using a custom operation, order defaults to 0:
Speed.Modify(CustomOperation); //Priority, layer and order are all 0

//In custom operations we can of course too use non-default parameters, if we want to:
Speed.Modify(CustomOperation, priority : 5, layer : 0, order : DefaultOrders.Mul - 100);
```

This is how these optional parameters affect the final value calculation:

* Value is calculated layer by layer, starting with the lowest and ending with the highest.
* Within each layer, only Modifiers with the highest priority actually have effect.
* If more than one Modifier have the same highest priority within the same layer, they will all have effect. Their ordering is defined by the order parameters, starting from lowest and ending with highest.
* If multiple modifiers have the same layer, priority, and order, there is no guarante on the order they will be executed in (will probably be the same order they were attached in). This situation is against the design of this system: make sure that these situations do not happen. That's why it's handy to use pre-defined order constants for different custom operations, like in DefaultOrders.cs for out-of-the-box operations.

TODO
IMAGE for explanation
Layers for talents, equipment, temporary buffs

DIRTY FLAG. Changing prio, layer, order

## ![][HeaderDecorator] Handling Modifiers ![][HeaderDecorator]

TODO
ATTACHING AND DETACHING
ACTIVE BOOL
MODIFIERGROUPS
ADDING ONE MODIFIER TO MULTIPLE MODVALUES

## ![][HeaderDecorator] Custom Operations ![][HeaderDecorator]
Setting the operation to a new function sets the modifiedvalue to dirty.
TEMPLATE MODIFIERS (NOT YET ATTACHED TO ANY VALUE)
CUSTOM OPERATIONS make sure pure function, IF EXTERNAL DEPENDENCIES, SET TO UPDATEEVERYTIME in constructor or later
COMPOUND AND NONCOMPOUND
the modifier uses either the compound or noncompound operation, whichever was set last. If needed, you can see which one is used by inquiring the `Modifier.Compound` bool.

## ![][HeaderDecorator] Previewing Values ![][HeaderDecorator]

You can preview the value of a ModifiedValue by pretending to attach and/or detach modifiers, without actually affecting the object. A plethora of `PreviewValue` and `PreviewValueDetach` method versions exist for this:

```C#
//Pretend to attach modifier1:
float previewValue = Speed.PreviewValue(modifier1);

//Pretend to attach modifier1 and detach modifier2
float previewValue = Speed.PreviewValue(modifier1, modifier2);

//Pretend to detach modifier2
float previewValue = Speed.PreviewValueDetach(modifier2);

//Pretend to attach a collection of modifiers (modifierCol1)
float previewValue = Speed.PreviewValue(modifierCol1);

//Pretend to attach a collection of modifiers (modifierCol1) and detach modifierCol2
float previewValue = Speed.PreviewValue(modifierCol1, modifierCol2);

//Pretend to detach a collection of modifiers (modifierCol2)
float previewValue = Speed.PreviewValueDetach(modifierCol2);

//Pretend to attach a ModifierGroup modifierGroup1
float previewValue = Speed.PreviewValue(modifierGroup1);

//Pretend to attach a ModifierGroup modifierGroup1 and detach modifierGroup2
float previewValue = Speed.PreviewValue(modifierGroup1, modifierGroup2);

//Pretend to detach modifierGroup2
float previewValue = Speed.PreviewValueDetach(modifierGroup2);
```

Like in regular value calculation, a preview modifier will not have effect on the preview value if it is not Active. A preview modifier will not have effect if it already exists in the ModifiedValue. Also, naturally, pretending to detach a modifier will not have effect if that modifier isn't already contained in the ModifiedValue.

## ![][HeaderDecorator] Inspector ![][HeaderDecorator]

TODO
SETTINGS TO PREVIEW FINAL VALUE
SAVED VALUE VS GETTER

`ModifiedEnum<YourEnum>` does have a custom property drawer and will not appear in the inspector, because Unity property drawers do not support generic types. However, for a specific YourEnum type, you can create your own property drawer by copying any other property drawer class and replacing the type with `ModifiedEnum<YourEnum>`. The same applies for any other class derived from `ModifiedValue` - you can easily create your own drawers by copying from the existing ones.

## ![][HeaderDecorator] Other Notes ![][HeaderDecorator]

In cases where the context is ambiguous, implicit casting of a ModifiedValue object to its wrapped value type may not work. One such example is the switch statement, where you need to specify that you're inquiring the value directly:


```C#
ModifiedEnum<MyEnum> Example = MyEnum.First;

switch(Example.Value)
{
  case (MyEnum.First):
    //Without the usage of .Value, this line would not execute
    break;
}
```

The only things that make this a Unity library are the custom property drawers, the `[SerializeField]` attribute in ModifiedValue.cs, and the small things in the Generator.cs class. If you want to use this as a non-Unity C# library, you can easily do so by removing these Unity things.

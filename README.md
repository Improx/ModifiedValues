# ModifiedValues
ModifiedValues is a C# library for Unity that enables modifying (numeric and other) values while keeping track of the modifiers affecting it.

Minimum requirements:
* C# version 9
* DotNet netstandard2.1

## Example

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
Speed.Add(5f);
```

By default, the multiplicative modifier is applied before the additive one. This results in a final speed value of 17. Your code that uses `Speed` will automatically pick up this update. The current value is also visible in the inspector:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/images/speedInspector2.PNG "Updated value of Speed visible in the inspector")

If you want to be able to remove these buffs later, you need to save the modifier objects:

```C#
var energizedBuff = Speed.Mul(1.2f);
var rollerScatesBuff = Speed.Add(5f);

//After some time passes, you want to remove the Energized buff.
energizedBuff.DetachFromAll();

Debug.Log(Speed.Value); //Will print 12
```

Without this library, where `Speed` is just a normal `float`, you would have needed to do something like this:

```C#
Speed *= 1.2f;
Speed += 5f;

//After some time passes, you want to remove the Energized buff.
//However, we can't just simply divide by 1.2f to get the correct result, because
//the rollerscates buff is still active
//We need to keep the additive rollerscates effect like this:

Speed -= 5f;
Speed /= 1.2f;
Speed += 5f;

Debug.Log(Speed); //Will print 12
```
With many buffs, doing this manually would get extremely convoluted. One of the main conveniences of this library is that the buffs don't have to know about each other. For each modifier, you just define how it modifies the value, optionally give it some priority-related parameters (explained further down), and then you can attach and detach these modifiers independently, while keeping the final value always correct.

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
myValue.Modify((v) => v * 1.2f + 5);
```

# ModifiedValues
ModifiedValues is a C# library for Unity that enables modifying numeric values while keeping track of the modifiers affecting it.

### Quick Example

You're making a buff system for your game. Instead of having a classic float variable on your character

```C#
float Speed = 10;
```

You can create a Modified version of it:

```C#
ModifiedFloat Speed = 10;
```

For convenience, this `Speed` object can be implicitly cast back into a float. Most of your code can treat it as just a regular float value:

```C#
transform.position += Speed * Time.deltaTime;
```

Let's say your character gets an Energized buff that multiplies base speed by 120%. Your character also equips rollerscates, increasing speed by 5. You add a these multiplicative and additive modifiers like this:

```C#
Speed.Mul(1.2f);
Speed.Add(5f);
```

By default, the multiplicative modifier is applied before the additive one. This results in a final speed value of 17. Your code that uses `Speed` will automatically pick up this update.

If you want to be able to remove these buffs later, you need to save the modifier objects:

```C#
var energizedBuff = Speed.Mul(1.2f);
var rollerScatesBuff = Speed.Add(5f);

//After some passes, you want to remove the Energized buff.
energizedBuff.DetachFromAll();

Debug.Log(Speed.Value); //Will print 12
```

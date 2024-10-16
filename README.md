# ModifiedValues
[![Unity 2021.2+](https://img.shields.io/badge/unity-2021.2%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/Improx/ModifiedValues/blob/main/LICENSE)

ModifiedValues is a powerful C# library for Unity that enables modifying (numeric and other) values while keeping track and managing the modifiers affecting them. Great for games with buffs/debuffs, stats, statuses, perks, equipment etc.

This can be especially useful in a buff / stat effects system, where different modifiers affect the value of different numbers, there can be many different modifiers per number, and it doesn't matter in which temporal order the modifiers were added.

This system is designed to be as generic and extendable as possible, enabling a wide variety of use cases. Many out-of-the-box classes and function implementations are provided, to enable the most common use cases quickly.

Minimum requirement is <strong>Unity 2021.2</strong> (for C# 9 and netstandard2.1).

This can also be quite easily used as a non-Unity C# library by removing only a few things. See the last section for more info on this.

[HeaderDecorator]: https://placehold.co/15x15/00dd00/00dd00.png

## ![][HeaderDecorator] Installation ![][HeaderDecorator]

You can install this as a Unity Package by going to Window -> Package Manager, clicking the plus sign, "Add package from git URL" and pasting `https://github.com/Improx/ModifiedValues.git?path=/Assets/ModifiedValues`.

## ![][HeaderDecorator] Quickstart Example ![][HeaderDecorator]

Make sure to use the `ModifiedValues` namespace.

You're making a buff system for your game. Instead of having a classic float variable on your character

```C#
public float Speed = 10;
```

You can create a Modified version of it:

```C#
public ModifiedFloat Speed = 10;
```
If the field is public or private serialized, it will also appear in the inspector:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/Images/speedInspector1.PNG "ModifiedValue Speed visible in the inspector")

For convenience, this `Speed` object can be implicitly cast back into a float. Most of your code can treat it as just a regular float value:

```C#
transform.position += Speed * Time.deltaTime;
```
In rare cases where that is not possible, you can get the float value by `Speed.Value`.

Let's say your character gets an Energized buff that multiplies base speed by 120%. Your character also equips rollerscates, increasing speed by 5. You apply these multiplicative and additive modifiers like this:

```C#
Speed.Add(5);
Speed.Mul(1.2f);
```

By default, in this library the multiplicative modifier is applied before the additive one (although you can easily change this based on your needs, read below about Layer, Priority and Order). The above code results in a final speed value of 17, regardless of the order those two lines of code are written.

Your code that uses `Speed` will automatically pick up the updated value. The current value is also visible in the inspector:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/Images/speedInspector2.PNG "Updated value of Speed visible in the inspector")

If you want to be able to remove these buffs later, you need to save the modifier objects:

```C#
Modifier energizedBuff = Speed.Mul(1.2f);
Modifier rollerSkatesBuff = Speed.Add(5);

//After some time passes, you want to remove the Energized buff.
energizedBuff.DetachFromAll();

Debug.Log(Speed); //Will print 15
```

Without this library, where `Speed` is just a normal `float`, you would have needed to do something like this:

```C#
Speed *= 1.2f;
Speed += 5;

//After some time passes, you want to remove the Energized buff.
//However, we can't just simply divide by 1.2f to get the correct result, because
//the rollerskates buff is still active
//We need to keep the additive rollerskates effect like this:

Speed -= 5;
Speed /= 1.2f;
Speed += 5;

Debug.Log(Speed); //Will print 15
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

You can wrap other types without needing to create new classes simply by using `ModifiedValue<MyType>`, it just won't have as much functionality by default. For example `ModifiedFloat` just inherits from `ModifiedValue<float>` and adds a bunch of methods on top, such as `Add` and `Mul`. You are of course free to use `ModifiedValue<float>`, but `ModifiedFloat` just has so many ready helper methods. See section "Out-of-the-box Modifiers" below for a full list of helper methods to cover the most common use cases.

With a generic wrapper for any type, you can  apply modifiers with any custom operations. The ModifiedValues system is not limited to cover only the above-mentioned types, but you can create ModifiedValues and Modifiers wrapping any type you want (that don't even need to be numeric):

```C#
ModifiedValue<MyType> myValue = new MyType();
Modifier mod = myValue.ModifyLatest((latestValue) => latestValue * 1.2f + 5);
```

## ![][HeaderDecorator] Initialization ![][HeaderDecorator]
You can create a new ModifiedValue object in many ways. You can do it with a contructor, where you pass the base value as a parameter. Implicitly setting a ModifiedValue object to a base value does the same thing. You can also call the constructor with a base value getter function parameter, in which case the base value can have external dependencies (for example, the base value can depend on the value of another ModifiedValue).

```C#
ModifiedFloat Speed1 = 5;
//Has the same base value as:
ModifiedFloat Speed2 = new ModifiedFloat(5);
//Has the same base value as:
ModifiedFloat Speed3 = new ModifiedFloat(() => 5);
//Has the same base value as:
ModifiedFloat Speed4 = new ModifiedFloat(ReturnFive);

private float ReturnFive()
{
  return 5;
}
```
A use case for a base value getter function instead of a hard value is when you want the base value to have an external dependency that can change over time.

Note that if at a later stage, you set a ModifiedValue object to a new base value implicitly again, the reference will point to a completely new ModifiedValue object.

```C#
ModifiedFloat Speed = 5;
Speed.Add(1);
Speed = 3; //Speed is now a completely new object, with a base value of 3 and the previous Add modifier removed.
```
If you want to update a ModifiedValue's base value while keeping all modifiers, you can update its `BaseValue` or `BaseValueGetter` function directly.

### :warning: Uninitialized ModifiedValue references = bad! :warning:
```C#
[Serializedfield] private ModifiedFloat Speed; //Set this reference to a new ModifiedFloat before using it!
```
Declaring a serialized ModifiedValue member variable and not assigning anything to it leads to Unity creating a default object out of it, instead of keeping the reference as `null`. In this Unity quirk, the constructor is bypassed and the ModifiedValue is not initialized correctly. Using such ModifiedValue objects will result in errors. Always set it to something when declaring it, or later. If needed, you can check whether a ModifiedValue object was created in this bad way (in that case `ModifiedValue.Init` equals `false`) and replace it with a new object. The inspector also alerts if a ModifiedValue is uninitialized:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/Images/speedInspectorUninitialized.PNG "Uninitialized ModifiedValue in the inspector")

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

Note: ALL `ModifiedValue<T>` types have the `Set()` Modifier readily available, not just in the abovementioned predefined helper classes.

If many different modifiers are applied that have the same `Priority`and `Layer`, they will all have effect. They will be applied in the same order as they are presented in the lists above (from top to bottom). This ordering is also visible in the `DefaultOrders.cs` class. If you are not happy with some of the default ordering, you can always use a custom order in a modifier. For example: `Speed.Set(99f, order: 50)`.

You can also create your own modifying operations either with an inline function `myValue.ModifyFromLatest((v) => v * 1.2f + 5)` or by using a function defined elsewhere: `myValue.ModifyFromLatest(MyCustomOperation)`. More about custom operations explained further down.

## ![][HeaderDecorator] Priority, Layer and Order ![][HeaderDecorator]

If used correctly, the temporal order in which Modifiers were attached to a ModifiedValue does not matter. Their interrelations are instead defined by optional `priority`, `layer` and `order` parameters:

```C#
//Custom parameters
Speed.Mul(1.2f, priority : 3, layer : 2, order : 3);

//By omitting them, priority and layer default to 0, and order defaults to the
//operation's default order defined in DefaultOrders.cs, if using an out-of-the-box modifier:

Speed.Mul(1.2f); //Priority and layer are 0, and order is DefaultOrders.Mul = 2000

//If using a custom operation, order defaults to 0:
Speed.ModifyFromLatest(CustomOperation); //Priority, layer and order are all 0

//In custom operations we can of course too use non-default parameters, if we want to:
Speed.ModifyFromLatest(CustomOperation, priority : 5, layer : 0, order : DefaultOrders.Mul - 100);
```

This is how these optional parameters affect the final value calculation:

* Value is calculated layer by layer, starting with the lowest and ending with the highest. Final value of a layer is fed as input into the next layer.
* Within each layer, only Modifiers with the highest priority actually have effect.
* If more than one Modifier have the same highest priority within the same layer, they will all have effect. Their ordering is defined by the order parameters, starting from lowest and ending with highest.
* If multiple modifiers have the same layer, priority, and order, there is no guarantee on the order they will be executed in (will probably be the temporal order in code they were attached in). This situation is against the design of this system: make sure that these situations do not happen. That's why it's handy to use pre-defined order constants for different custom operations, like in DefaultOrders.cs for out-of-the-box operations.

A ModifiedValue object uses the dirty flag pattern to re-calculate its value upon inquiry if something in its modifiers has changed (it's also recalculated if the base value has changed). You can change the Modifier objects' `Priority`, `Layer` and `Order` properties after attaching them. The ModifiedValue object will be set dirty and its value will be updated the next time some script inquires `Value`:

```C#
ModifiedFloat Speed = 10;

Modifier energizedBuff = Speed.Mul(1.2f, priority : 1);
Modifier rollerSkatesBuff = Speed.Add(5, priority : 0);

Debug.Log(Speed); //Will print 12. Only the Mul modifier has effect, because it has the higher priority in the shared layer 0.

rollerSkatesBuff.Priority = 2;

Debug.Log(Speed); //Will print 15 because now the Add modifier has the higher priority.

energizedBuff.Priority = 2;

Debug.Log(Speed); //Will print 17 because now both modifiers have effect. Mul is applied first because it has a lower order.

energizedBuff.Layer = 1;

Debug.Log(Speed); //Will print 18 ( =(10+5)*1.2f ) because now energizedBuff has a higher layer, so it's Mul modifier will be applied after the previous layer has been calculated.
```

Let us further elaborate on how priority, layer, and order work with a concrete example. You're making an RPG game where the character's Speed value is modified by many different kinds of effects. Starting from more permanent and ending with less permanent effect types, these are: 1) Level-up Bonuses, 2) Talent Choices, 3) Worn Equipment and 4) Temporary Buffs (such as potions). You choose to design your system so these effect kinds would be calculated in the aforementioned order. So for instance, the effects of all Temporary Buffs are calculated after all Worn Equipment effects have been calculated. In other words, the output Speed value calculated at the end of Worn Equipment layer serves as input for the Temporary Buffs effects. It makes sense to design your effect system with these four different layers:

```C#
const int LayerLevelUp = 1;
const int LayerTalents = 2;
const int LayerEquipment = 3;
const int LayerBuffs = 4;
```
And then use these constants in the optional layer parameters when creating modifiers.

Within each layer, only modifiers with the highest priority actually have effect. For example, we might have a potion buff that increases speed by 5% and a blessing buff that increases speed by adding 3, both with Prioroity 0. However, when another player casts a Control curse on us, it is designed to set our Speed to 8, no matter what other less-important effects say. In that case, the Control curse should use a higher priority (for example Priority 1) than the potion and the blessing buff.

|               |<strong>Priority 0</strong>|<strong>Priority 1</strong>|
| ------------- | ------------- |:-------------:|
| <strong>Layer 4 (Buffs)</strong>      | (Potion) AddFraction(0.05f) & (Blessing) Add(3) | (Control) Set(8) |
| <strong>Layer 3 (Equipment)</strong>  | (Boots) Add(2) &  (Sword) Mul(1.03f)            |                  |
| <strong>Layer 2 (Talents)</strong>    | AddFraction(0.1) & Add(3)                       |                  |
| <strong>Layer 1 (LevelUP)</strong>    | Add(2*Level)                                    |                  |
| <strong>Base Value = 10</strong>    |

Let's say our base speed value is 10. First, the LevelUP bonuses have effect. As an example, if our character's level 3, our speed becomes 10 + 2 * 3 = 16.

Then let's take a look at our talents. We have two modifiers there, with the same priority, so they both have effect. Their order is defined by the modifier's orders. AddFraction has a smaller order, so it has effect first, adding 10% to the value, so our speed becomes 17.6. Then the other talent adds 3, to a result of 20.6.

Then let's look at our equipment. We have Boots and Sword equipped. Again, two modifiers both have effect because they have the same priority. Mul has a lower default order, so it happens before Add. So first, 20.6 * 1.03 => 21.218, and then 21.218 + 2 => 23.218.

Finally, the Buffs layer takes effect. Because the Control modifier has a higher priority than Potion and Blessing, it will be the only Buff actually having an effect. Because it is a Set operation, it simply sets the current value from 23.218 to 8. Because there are no other Buffs with priority 1, and there are no higher layers, the final value of Speed is 8.

If the Control buff were detached, then the Potion and Blessing buffs would have effect.

## ![][HeaderDecorator] BecameDirty Event and Dependencies ![][HeaderDecorator]

To avoid redundant calculations, a ModifiedValue uses a dirty flag pattern to only update its value if something about its modifiers or base value has changed. The value will be updated on the next time some code inquires for the value. However, in some situations you need to know exactly whenever a ModifiedValue became dirty, in order to immediately inquire (and update) its new value. Such a use case is for example a UI displaying the value. Instead of asking for a ModifiedValue's value every frame in case it's changed, you can use its `BecameDirty` event:

```C#
public class HealthBar : MonoBehaviour
{
    TextMeshProUGUI _maxHealthText;

    private void Awake()
    {
        _maxHealthText = GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(Character character)
    {
        //MaxHealth is a character's ModifiedFloat
        character.MaxHealth.BecameDirty += (sender, eventArgs) => UpdateText(((ModifiedFloat) sender).Value);
    }

    private void UpdateText(float value)
    {
        _maxHealthText.text = value.ToString();
    }

}
```

If a ModifiedValue's base value depends on another ModifiedValue's final value, then whenever the dependency's final value changes, the depending ModifiedValue will not become dirty, even though its value would be recalculated correctly upon inquiry (upon each inquiry of `Value`, the ModifiedValue checks whether the current base value is different from the last time it was inquired, and if it is, then it sets itself dirty and recalculates the final value).

However, imagine a situation where a UI needs to know immediately after a ModifiedValue became dirty, to reflect its potentially updated value. If its value has been updated just because its dependency has changed in value, by default, the depending ModifiedValue's BecameDirty event is not invoked. Without an explicit inquiry of the depending ModifiedValue's `Value`, the UI would not know that the base value (and the final value) has changed.

In such a situation, we should set the depending ModifiedValue dirty whenever the dependency ModifiedValue has become dirty. As an example, `AttackSpeed` depends on `Speed`, and we can set up the dependency already in the construction phase, later with a helper method, or completely manually:

```C#
public class Character
{
	public ModifiedFloat Speed;
	public ModifiedFloat AttackSpeed;

	public Character()
	{
		//Initialization
		Speed = 10;

		//Constructing AttackSpeed and declaring a dependency with additional parameter:
		AttackSpeed = new ModifiedFloat(() => Speed, Speed);
		
		//Is the same thing as doing:
		AttackSpeed = new ModifiedFloat(() => Speed);
		AttackSpeed.AddDependency(Speed);

		//Which is almost the same thing as doing:
		AttackSpeed = new ModifiedFloat(() => Speed);
		Speed.BecameDirty += (sender, eventArgs) => AttackSpeed.SetDirty();
	}
}
```

Now a UI element can subscribe to `AttackSpeed.BecameDirty` to be able to react whenever AttackSpeed's final value has changed, even if that change was only due to its dependency's (Speed's) final value change.

You'll need to be careful to not set up circular dependencies.

## ![][HeaderDecorator] Dynamic Modifiers ![][HeaderDecorator]

Sometimes you want to apply a Modifier that uses its own ModifiedValue as an operation parameter, which can change value over time. For example:

```C#

public ModifiedFloat Speed = 10;
public ModifiedFloat MaxSpeed = 20;

Speed.MaxCap(MaxSpeed);
Debug.Log(Speed);
//Will print 10
MaxSpeed.Set(8);
Debug.Log(Speed);
//Will still print 10, because when you called MaxCap(MaxSpeed),
//it cast MaxSpeed into a float at that moment, and that value will be used in
//the operation.

```

If you want to make sure that the Speed  will react if the MaxSpeed  changes, then you will need to explicitly use a "Dynamic" version of the modifier method:

```C#

public ModifiedFloat Speed = 10;
public ModifiedFloat MaxSpeed = 20;

Speed.MaxCapDynamic(MaxSpeed);
Debug.Log(Speed); //Will print 10
MaxSpeed.Set(8);
Debug.Log(Speed); //Will print 8

```

All of the out-of-the-box modifying functions, such as `Add`, `Mul`, etc. have a dynamic version: `AddDynamic`, `MulDynamic`, etc. These methods take as parameter a ModifiedValue instead of a raw value, so it will not cast it into the raw value when creating the operation. Behind the scenes, the Dynamic methods also call `AddDependency(otherModifiedValue)` to ensure that whenever MaxSpeed becomes dirty, Speed would become Dirty as well.

## ![][HeaderDecorator] Handling Modifiers ![][HeaderDecorator]

When a Modifier is attached to a ModifiedValue, it means that it affects its value. When creating modifiers with one of the readily provided methods, such as `Speed.Add(5)`, the modifier returned by this method is automatically attached to Speed.

It's possible to create a Modifier that is not attached to anything, with a constructor. You need to use a more detailed `Modifier<Type>` class, because the constructor takes a typed operation as an argument:

```C#
Modifier<float> mod = Modifier<float>.NewFromLatest((v) => v * v); //Can also set optional priority, layer and order parameters

//Later, attaching this modifier to two different ModifiedFloats:
Speed.Attach(mod);
Strength.Attach(mod);

//After some time passes, we want to detach the modifier only from the Speed buff
//while keeping it on Strength:
Speed.Detach(mod);
```
As the previous example showed, it is possible for a modifier to be attached to more than one ModifiedValue. In such a case, changing the properties of the modifier will affect all ModifiedValues it is attached to. If you want identical, but independent copies of a modifier, the `Copy()` method can be used:

```C#
Modifier<float> mod = Modifier<float>.NewFromLatest((v) => v * v);

//Later, attaching this modifier and its independent copy to two different ModifiedFloats:
Speed.Attach(mod);
Strength.Attach(mod.Copy());
```

The `Copy()` method creates a new Modifier object with all properties identical to the original, except it will not be attached to anything by default.

You can see all ModifiedValues a modifier is attached to:

```C#
Modifier<float> mod = Modifier<float>.NewFromLatest((v) => v * v);

Speed.Attach(mod);
Strength.Attach(mod);

foreach (ModifiedValue modValue in mod.GetAttachedModValues())
{
    //This will happen for Speed and for Strength
    Debug.Log(modValue);
}
```

Another way to create modifiers that are not attached to anything in the beginning is by utilizing an out-of-the-box modified value class's static Template methods. This is handy when the modifier's operation is one of the readily provided ones, and you want its order to match the provided DefaultOrder:

```C#
Modifier<float> mod1 = ModifiedFloat.TemplateAdd(5);
//Is the same as:
Modifier<float> mod2 = Modifier<float>.NewFromLatest((v) => v + 5, order : DefaultOrders.Add);
```

Each Modifier also has an `Active` bool, which is true by default. If you set it to false, then it will no longer have an effect on attached ModifiedValues, while still remaining attached to them. This is just a handy way of turning modifiers off and on, instead of constantly needing to attach & detach modifiers.

If you're making a buff system, it is a common use case that a single buff would affect multiple different stats. As an example, equipping a sword item increases a Character's Damage (ModifiedFloat), JumpCount (ModifiedInt), and Speed (ModifiedFloat). Whenever you equip & unequip the sword, all of these modifiers need to be attached & detached simultaneously. Instead of keeping all Modifieirs in separate member variables or a regular collection, this library provides a handy `ModifierGroup` collection class:

```C#
public class SwordBuff
{
    ModifierGroup modGroup = new();

    public void SwordBuff(Character character)
    {
        modGroup += character.Damage.Add(15,7f);
        modGroup += character.JumpCount.Add(1);
        modGroup += character.Speed.AddFraction(0.1f);
    }

    public void Remove()
    {
        //Detach all modifiers from the character, and clears the ModifierGroup:
        modGroup.ClearAndDetach();
    }

}
```

If a modifier is in a ModifierGroup, it doesn't necessarily mean that it is attached to anything. ModifierGroup is just a collection with the ability to do the same thing for multiple modifiers at once. You can call `modGroup.SetActive()` or `modGroup.SetInactive()` to toggle the Active status of all modifieres, `modGroup.Attach(modValue)` and `modGroup.Detach(modValue)`, and so on. You add and remove modifiers from a group with the `+=` and `-=` operators, or add with an `Add()` method.

`ModifierGroup` can be iterated through with an `foreach (Modifier mod in modGroup)` clause

Some more convenient syntax for `ModifierGroup`:
```C#

//Can create a new ModifierGroup with elements inside curly brackets:
ModifierGroup modGroup = new() {character.JumpCount.Add(1)};

//Can iterate through ModifierGroup like this:
foreach (Modifier mod in modGroup)
{
	//Do something
}

//ForEach method also exists:
modGroup.ForEach(someAction);

```

## ![][HeaderDecorator] Modifier Operations ![][HeaderDecorator]

As some of the previous sections already showed, in addition to the readily provided operations such as `Add` and `Mul`, you can make your modifiers use any custom operations. You can update the operation after the modifier has been created, in which case the ModifiedValues it is attached to become dirty. In that case it is not enought to store the modifier in a `Modifier` type reference, but it needs to be stored (or cast to) in a more specific generically typed `Modifier<Type>` reference, where Type is the same type as what the ModifiedValue wraps. This is because the operation of a modifier needs to know the type it is dealing with:

```C#
//Modifier that squares the value
Modifier<float> mod = Speed.ModifyFromLatest((v) => v * v); 

//At a later point, changing the operation to one that cubes the latest value:
mod.Operation = (_, _, v) => v * v * v;
```

As a general rule, operations should be pure functions, and thus, not have external dependencies. That is because if these external dependencies would change, the ModifiedValue object would not know about it and would not become dirty. If you still want to use external dependencies in an operation, you can either manually track whenever an external dependency changes value and call `modifiedValue.SetDirty()` each time, or you can set `modifiedValue.UpdateEverTime = true`, so that its value would be recalculated on each inquiry, regardless if it's dirty or not. For example:

```C#

Modifier<float> mod = Speed.ModifyFromLatest((v) => v + Time.time);
//Time.time is an external dependency that changes each frame, so we do the following:
Speed.UpdateEveryTime = true;
```
Behind the scenes, a modifier's `Operation` is always a function that takes three inputs: 1) base value, 2) value at the start of the layer, and 3) latest value in the layer, at the moment the modifier takes effect. A modifier's `Operation` can use all or a subset of those inputs.

As an example, when you do `Speed.Add(5f)`, behind the scenes, a Modifier<float> is created whose operation is `(_, _, latestValue) => latestValue + 5f`. In that case, the operation only cares about the latest value. That way, multiple additive modifiers can have effect, each adding a number to the output of the previous modifier. If the operation had been `(baseValue, _, _) => baseValue + 5f`, then only the last Modifier would have the effect, because its operation doesn't care about anything else other than the ModifiedValue's base value.

An example of an operation that uses more than one input is `AddFraction`. It adds a fraction to the value based on what the value was at the start of the layer, instead of the latest value. Because of this, many modifiers of this kind would stack additively instead of multiplicatively.

```C#
Speed.AddFraction(0.2f);
//Is the same as:
Speed.ModifyFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + 0.2f * layerStartValue, order : DefaultOrders.AddFraction);
//Is the same as:
Speed.Modify((_, layerStartValue, latestValue) => latestValue + 0.2f * layerStartValue, order : DefaultOrders.AddFraction);

//There is also a version that adds a fraction based on the base value instead:
Speed.AddFractionBase(0.2f);
//Is the same as:
Speed.ModifyFromBaseAndLatest((baseValue, latestValue) => latestValue + 0.2f * baseValue, order : DefaultOrders.AddFraction);
//Is the same as:
Speed.Modify((baseValue, _, latestValue) => latestValue + 0.2f * baseValue, order : DefaultOrders.AddFraction);
```

The difference becomes apparent when multiple operations stack. As an example, here's how `Mul` stacks (multiplicatively):

```C#
ModifiedFloat Speed = 100;
Speed.Mul(1.2f);
Debug.Log(Speed); //Will print 120
Speed.Mul(1.2f);
Debug.Log(Speed); //Will print 144!

ModifiedFloat Speed2 = 100;
Speed2.AddFraction(0.2f);
Debug.Log(Speed); //Will print 120
Speed2.AddFraction(0.2f);
Debug.Log(Speed); //Will print 140!
```
When creating modifiers with custom operations, you can take advantage of various shorthand methods, instead of having to use the full notation of all three inputs:

```C#
//Creating a non-attached modifier that only cares about the base value
Modifier<float> mod = Modifier<float>.NewFromBase((baseValue) => someOperation(baseValue));
//Is the same as:
Modifier<float> mod = Modifier<float>.New((baseValue, _, _) => someOperation(baseValue));

//Creating a non-attached modifier that only cares about the layer start value
Modifier<float> mod = Modifier<float>.NewFromLayerStart((layerStartValue) => someOperation(layerStartValue));
//Is the same as:
Modifier<float> mod = Modifier<float>.New((_, layerStartValue, _) => someOperation(layerStartValue));

//Creating a non-attached modifier that only cares about the latest value
Modifier<float> mod = Modifier<float>.NewFromLatest((latestValue) => someOperation(latestValue));
//Is the same as:
Modifier<float> mod = Modifier<float>.New((_, _, latestValue) => someOperation(latestValue));

//Creating a non-attached modifier that only cares about the base and layer start values
Modifier<float> mod = Modifier<float>.NewFromBaseAndLayerStart((baseValue, layerStartValue) => someOperation(baseValue, layerStartValue));
//Is the same as:
Modifier<float> mod = Modifier<float>.New((baseValue, layerStartValue, _) => someOperation(baseValue, layerStartValue));

//Creating a non-attached modifier that only cares about the base and layer start values
Modifier<float> mod = Modifier<float>.NewFromBaseAndLatest((baseValue, latestValue) => someOperation(baseValue, latestValue));
//Is the same as:
Modifier<float> mod = Modifier<float>.New((baseValue, _, latestValue) => someOperation(baseValue, latestValue));

//Creating a non-attached modifier that only cares about the base and layer start values
Modifier<float> mod = Modifier<float>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => someOperation(layerStartValue, latestValue));
//Is the same as:
Modifier<float> mod = Modifier<float>.New((_, layerStartValue, latestValue) => someOperation(layerStartValue, latestValue));
```

Similar shorthand methods also exist when modifying a ModifiedValue directly:

```C#
//Modify with an operation that cares about all three inputs
Speed.Modify((baseValue, layerStartValue, latestValue) => someOperation(baseValue, layerStartValue, latestValue));

//Or, if your operation only cares about a subset of the inputs, you can use shorthands:
Speed.ModifyFromBase((baseValue) => someOperation(baseValue));
Speed.ModifyFromLayerStart((layerStartValue) => someOperation(layerStartValue));
Speed.ModifyFromLatest((latestValue) => someOperation(latestValue));

Speed.ModifyFromBaseAndLayerStart((baseValue, layerStartValue) => someOperation(baseValue, layerStartValue));
Speed.ModifyFromBaseAndLatest((baseValue, latestValue) => someOperation(baseValue, latestValue));
Speed.ModifyFromLayerStartAndLatest((layerStartValue, latestValue) => someOperation(layerStartValue, latestValue));
```

## ![][HeaderDecorator] Previewing Values ![][HeaderDecorator]

You can preview the value of a ModifiedValue by pretending to attach and/or detach modifiers, without actually affecting the object. An example of a common use case is in an RPG game, where before equipping an item, you want to preview its effects on your stats in a tooltip.

A plethora of `PreviewValue` and `PreviewValueDetach` method versions exist for this purpose:

```C#
//Pretend to attach modifier1:
float previewValue1 = Speed.PreviewValue(modifier1);

//Pretend to attach modifier1 and detach modifier2
float previewValue2 = Speed.PreviewValue(modifier1, modifier2);

//Pretend to detach modifier2
float previewValue3 = Speed.PreviewValueDetach(modifier2);

//Pretend to attach a collection of modifiers (modifierCol1)
float previewValue4 = Speed.PreviewValue(modifierCol1);

//Pretend to attach a collection of modifiers (modifierCol1) and detach collection modifierCol2
float previewValue5 = Speed.PreviewValue(modifierCol1, modifierCol2);

//Pretend to detach a collection of modifiers (modifierCol2)
float previewValue6 = Speed.PreviewValueDetach(modifierCol2);

//Pretend to attach a ModifierGroup modifierGroup1
float previewValue7 = Speed.PreviewValue(modifierGroup1);

//Pretend to attach a ModifierGroup modifierGroup1 and detach modifierGroup2
float previewValue8 = Speed.PreviewValue(modifierGroup1, modifierGroup2);

//Pretend to detach modifierGroup2
float previewValue9 = Speed.PreviewValueDetach(modifierGroup2);
```

Like in regular value calculation, a preview modifier will not have effect on the preview value if it is not Active. A preview modifier will not have effect if it already exists in the ModifiedValue. Also, naturally, pretending to detach a modifier will not have effect if that modifier isn't already contained in the ModifiedValue.

## ![][HeaderDecorator] Inspector ![][HeaderDecorator]

Like was shown in the Quickstart Example section, serialized ModifiedValues are displayed in the inspector. Its base value can be modified in the inspector at runtime and edit mode. The current final value is also displayed, as long as the current setting allows it. The setting can be changed in Settings.cs by changing `ShowLatestValue`. The possible modes are `Never`, `OnlyRuntime` and `Always` (default).

![alt text](https://github.com/Improx/ModifiedValues/blob/main/Images/speedInspector2.PNG "Updated value of Speed visible in the inspector")

If a ModifiedValue uses a `BaseValueGetter` function instead of a saved base value, then it makes sense that the base value cannot be directly set in the inspector, as the base value depends on whatever the getter returns at any given moment. In such a case the inspector shows that a getter is used, and presents the current base value:

![alt text](https://github.com/Improx/ModifiedValues/blob/main/Images/speedInspectorGetter.PNG "Inspector shows that Speed uses a base value getter").

If you still want to delete the base value getter function in the inspector, you can click on the getter button. A saved value will be used (defaulting to the wrapped type's default value), and can be edited in the inspector again.

⚠️ If not using one of the readily available classes such as  `ModifiedFloar`, but instead using `ModifiedValue<YourType>`, it will have limited functionality in the inspector, because a generic class cannot have a custom property drawer. In these cases only the saved base value will be visible and settable in the inspector. For most use cases this should be ok. Basically, you will only be missing seeing the changing current value at runtime in the inspector. That's because Unity property drawers do not support generic types. However, for a specific YourType, you can easily create your own property drawer by copying any other property drawer class and replacing the type with `ModifiedValue<YourType>`, so that then you can also see the current value in the inspector in addition to the base value..

## ![][HeaderDecorator] Other Notes ![][HeaderDecorator]

* In cases where the context is ambiguous, implicit casting of a ModifiedValue object to its wrapped value type may not work. One such example is the switch statement, where you need to directly specify that you're inquiring the ModifiedValue's `Value` instead of the ModifiedValue object itself:


```C#
public MyEnum {First, Second};

ModifiedValue<MyEnum> Example = MyEnum.First;

switch (Example.Value)
{
    case (MyEnum.First):
        //Without the usage of .Value, this line would not execute
        break;
}
```

* With a few changes this library can also be used as a normal C# library without Unity, by deleting a few things. First of all, just use the Assets/ModifiedValues path (which is anyway the package installation folder). The only things that make this a Unity library are the custom property drawers (just delete the Editor folder), the `[SerializeField]` attribute in ModifiedValue.cs, and the small things in the Generator.cs class. Meta files are also part of this repo but redundant if used without Unity.

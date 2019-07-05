# Unity-Animate

## Animate any Component in the Unity Editor with ease

Animate.cs is a simple script to facilitate simple animations to the properties for any Unity Component. Just add Animate as a component to the GameObject and setup the property/properties to be animated. Invoke it by its animation state index or call them all together.


![Animate Component](https://drive.google.com/uc?export=view&id=1WcttilGOeApi1DhxvMcBnECEnD7ISfpx)

Animations in the GIF:

 - Camera: Zoom out.
 - Button: scale up (ease out), color, scale down (ease in), and gameObject deactivated on animation end.
 - Cube: position up (ease out), position down (ease in) loop, rotation.
 
## Use

### 1- Add "Animate" as a Component to the GameObject:

![Animate Component](https://drive.google.com/uc?export=view&id=1F5d9Cv9anhvpPrhpqAdEhbLmN2To3wpz)

### 2- Add an animation state with it property setup

The animation state needs:

 1. A **state name** (optional).
 2. The *exact* **Component type name** (required): It must be the exact name of the component to animate its property.
 3. The *exact* **property name** of the component to be animated.
 4. **Duration in seconds** of the animation.
 5. The **interpolation curve** of the animation, *default: linear interpolation.*
 6. The **parameter type** of the property to animate.
 7. **Value** *of the parameter type* to animate.
 8. An (optional) **Event** that is invoked once the animation has ended. *(It may invoke another animation state to create loops).*

![Animate Component](https://drive.google.com/uc?export=view&id=1apcO0V3vCe5juosVxRRdUVtO6MB3DKO9)

### 3- Add the code to run the animation

To start all the animation states together:

```csharp
GetComponent<Animate>().RunAnimations();
```

If you prefer to run animation state separated (*example for the first state*):

```csharp
int stateIndex = 0;
GetComponent<Animate>().AnimateState(stateIndex);
```

## What it does not do?
The script can not run nested properties. Ex: Trying to animate the color of the material in a MeshRenderer.

```csharp
GetComponent<MeshRenderer>().material.color
```

## Working parameter types

 1. Vector2
 2. Vector3
 3. Float
 4. Integer
 5. Color

## Recomendations
This component is not recommended for heavy use of animations as it use ***System.Reflection*** inside while loops that decrease performance in comparison with dedicated animation scripts.

The ease use of the component facilitate to create multiple types of animations for UI interactions and very simple 3d animations.

## Acknowledgments
Thanks to [@Deadcows](https://github.com/Deadcows) for his ConditionalFieldAttribute in [MyBox](https://github.com/Deadcows/MyBox)

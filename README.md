
# Unity-Animate

  

## Animate any Component in the Unity Editor with ease

  

Animate.cs is a simple script to facilitate simple animations to the properties for any Unity Component. Just add Animate as a component to the GameObject and setup the property/properties to be animated. Invoke it by its animation state index or call them all together.

  

![Animate Component](https://drive.google.com/uc?export=view&id=1EkVyjXoNt9zEwPtWaOUFFd2jpFwh6I9I)

  

Animations in the GIF:

  

- Camera: Zoom out.

- Button: scale up (ease out), color, scale down (ease in), text color, and gameObject deactivated on animation end.

- Cube: position up (ease out), position down (ease in) loop, rotation.

- Title: text anchorMax resize, icon color alpha

## Use

  

### 1- Add "Animate" as a Component to the GameObject:

  

![Animate Component](https://drive.google.com/uc?export=view&id=1F5d9Cv9anhvpPrhpqAdEhbLmN2To3wpz)

  

### 2- Add an animation state with it property setup

  

The animation state needs:

  

1. A **state name** (optional).

2. The *exact*  **Component type name** (required): It must be the exact name of the component to animate its property.

3. The *exact*  **property name** of the component to be animated.

4.  **Duration in seconds** of the animation.

5. The **interpolation curve** of the animation, *default: linear interpolation.*

6. The **parameter type** of the property to animate.

7.  **Value**  *of the parameter type* to animate.

8. An (optional) **Event** that is invoked once the animation has ended. *(It may invoke another animation state to create loops).*

9.  **Linked GameObject** (optional), the animation can be linked to any GameObject.*

  

![Animate Component](https://drive.google.com/uc?export=view&id=1aLN4Qqq5zj2rhfQPVCeruDU9rWadZF-Q)

  

### 3- Add the code to run the animation

  

To start all the animation states together:

  

```csharp

GetComponent<Animate>().RunAnimations();

```

  

If you prefer to run animation state separated (*example for the first state*):

  

```csharp

int  stateIndex = 0;

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

## Advanced Example

*Building an interactive UI Button with PointerEnter and PointerExit event listeners*

![Animate Component](https://drive.google.com/uc?export=view&id=1UqnkJ_EiV94Y79imKM1Q3vV2Fnz8pUo1)

**1. The hierarchy:**
 *Button with a text and an icon*
 
 ![Animate Component](https://drive.google.com/uc?export=view&id=1i5GVhJvbS8LpTA0pccNsUawYinRTxmV5) 

**2. The animation states**

 - Button **RectTransform**: *offsetMin/offsetMax*
 - Button **UnityEngine.UI.Image**: *color*
 - Button/TextContainer **RectTransform**: *anchorMax*
 - Button/Icon **CanvasGroup**: *alpha*

![Animate Component](https://drive.google.com/uc?export=view&id=15qbgIVhxClkoUjq-iwYVHJhkZOIkpuYC)

**3. Advanced property to reset animations** 
*Enabling "**Advanced**" allows to modify the reset animations method and duration* 

![Animate Component](https://drive.google.com/uc?export=view&id=136KPoLL7j36_lWmIO3jAwYb0PI29HxyJ)

**4. AnimateUIListener Component:** 
*Attaching **AnimateUIListener** to the button allows to interact with PointerEnter/PointerExit events, running the comma separated state indexes once the event occurs.* If "Reset States" is enabled then the event will reset the animations when the event is triggered.

![Animate Component](https://drive.google.com/uc?export=view&id=1K9NPVl62JuxOzpXNay8UAGm8xtSI09_R)

## Acknowledgments

Thanks to [@Deadcows](https://github.com/Deadcows) for his ConditionalFieldAttribute in [MyBox](https://github.com/Deadcows/MyBox)
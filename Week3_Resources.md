# Week 3

### Resources

  * [Aarthificial](https://www.youtube.com/channel/UCtEwVJZABCd0tels2KIpKGQ) - extremely polished, well-explained videos about custom Unity editor tools and how they allow for faster iteration and more direct creative control over the game design.
  * [Unite 2016 - Overthrowing the MonoBehaviour Tyranny in a Glorious Scriptable Object Revolution](https://www.youtube.com/watch?v=6vmRwLYWNRo) - the seminal talk on ScriptableObjects that got everyone on-board. Explains the benefits of doing a lot of your design work in ScriptableObjects, rather than in the inspector or in C# directly.

### Tutorials and Guides

  * [How to make a CUSTOM INSPECTOR in Unity (Brackeys)](https://www.youtube.com/watch?v=RInUu1_8aGw) - an overview of one way of creating custom inspectors in Unity. Note that this method doesn't make use of `serializedObject`, so undo/redo, multi-object editing, etc. will not work.
  * [How to make an EDITOR WINDOW in Unity (Brackeys)](https://www.youtube.com/watch?v=491TSNwXTIg) - we didn't do this in class, but it's very similar to a custom inspector, just not tied to a particular asset or GameObject. It lets you create your own custom windows if you have complex tasks in the editor you'd like to automate in some way (for example).
  * [Custom Editors (Unity Documentation)](https://docs.unity3d.com/ScriptReference/Editor.html) - goes over the three ways of creating a custom inspector in Unity. Note that I didn't go over the first version which makes of UIElements. This is just because it's *SO MUCH MORE CODE TO WRITE* than the other two options and I think it's way overkill unless you're creating a very complex editor tool. For our purposes where we want to iterate quickly, I suggest using the second and third options (`serializedObject` and `target`). **In short: you can ignore the first three code examples on this page.**
  * [SCRIPTABLE OBJECTS in Unity (Brackeys)](https://www.youtube.com/watch?v=aPXvoWVabPY) - an intro to how you can use ScriptableObjects to store data for use in runtime

### Handy Tools from This Week

  * [EditorGUILayout](https://docs.unity3d.com/ScriptReference/EditorGUILayout.html) - contains all the methods you'll use for placing UI components in your custom editors - note that the only difference between `EditorGUILayout` and `EditorGUI` is that `EditorGUI` needs you to explicitly put in bounding rects for every element, whereas `EditorGUILayout` generates those rects for you. Generally, you want to be using `EditorGUILayout`. I strongly recommend just kinda browsing around this page to see what's on offer, there are a zillion awesome methods in here - sliders, horizontal groups, info boxes, foldouts, etc!
  * [EditorGUI](https://docs.unity3d.com/ScriptReference/EditorGUI.html) - contains a few more useful UI components for your inspector. I only ever use `EditorGUI.Button`, but there's probably other useful things in here!
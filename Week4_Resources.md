# Week 4

### Resources

  * [GDC - Practical Procedural Generation for Everyone](https://www.youtube.com/watch?v=WumyfLEa6bU) - Just some inspiration and a good overview! I should have linked this in week one, but I only just found it this week while looking for something else :D
  * [Modular First Person Controller](https://assetstore.unity.com/packages/3d/characters/modular-first-person-controller-189884) - A free, easy to use first person character controller to allow you to walk around your levels. Make sure you're testing the world from the player perspective regularly to make sure things make sense from that view.

### Tutorials and Guides

  * [Simple Random Forest Generator](https://www.youtube.com/watch?v=604lmtHhcQs) - Similar to the start point in-class, places prefabs randomly. Useful if you want to go over it again at your own pace (in a slightly different way, of course)
  * [How to Randomly Generate Levels (and Islands)](https://www.youtube.com/watch?v=O9J_Cfl6HzE) - A *fantastic* video by one of the devs for Islanders. Shows very well how you should approach your assignments: iteratively, working on one system at a time.

### Handy Tools from This Week

  * [Physics.Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) - Casts a 'ray' into the world and lets you know whether it hit anything and, optionally, what it hit and how. Extremely useful for placing objects 'intelligently' given the existing scene context. Also check out `Physics.Linecast`, `Physics.Spherecast`, `Physics.Boxcast` and `Collider.Raycast` (this one is for checking whether a ray hits a particular collider)
  * [Physics.CheckSphere](https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html) - A useful way to check whether there are any colliders in a given area. Also check out `Physics.CheckBox` and a few other similar methods
  * [Physics.ClosestPoint](https://docs.unity3d.com/ScriptReference/Physics.ClosestPoint.html) - Handy when you want to place things on other things - note that it only works on convet mesh colliders.
  * [Physics.ComputePenetration](https://docs.unity3d.com/ScriptReference/Physics.ComputePenetration.html) - an extremely powerful function that takes in two colliders and tells you how to move the first one so that they're no longer colliding. Can be a little tricky to get working, read the documentation page carefully!
  * [Physics.SyncTransforms](https://docs.unity3d.com/ScriptReference/Physics.ClosestPoint.html) - if you want to run physics checks against newly spawned objects on the same frame that they were created (or in the editor), you need to call this after you spawn them. Note that if you're doing this at runtime, it can cause framerate issues so be careful.
  * [Physics.Simulate](https://docs.unity3d.com/ScriptReference/Physics.Simulate.html) - I'm not planning on going over this in class, but you can use this to place objects by just letting them fall and settle where they naturally would. Useful for things like rocks, branches, litter and other small debris.
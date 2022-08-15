# Week 5

### Resources

  * [GDC - Math for Game Programmers: Fast and Funky 1D Nonlinear Tranasformations](https://www.youtube.com/watch?v=mr5xkf6zSzk) - The title sounds scary, but this is a brilliant talk. Focus primarily on the first half, where the speaker is talking about ways of conceptualizing abstract things like 'AI aggression' or 'player progress' as simple 0 -> 1 or -1 -> 1 floating point numbers. The second half of the talk is focused much more on the nitty-gritty details of easing functions which is useful, but not the primary reason I'm linking this.
  * [Juice It or Lose It](https://www.youtube.com/watch?v=Fy0aCDmgnxg) - An all-time classic talk about the benefits of adding 'juice' to gameplay. Focuses a lot on the role of code in this process and is presented in a kind of similar way to how you might go about intuitively and creatively embellishing your game.
  * [Desmos](https://www.desmos.com/calculator) - A SUPER handy online graphing calculator which lets you mess around with parametric functions (as well as all other kinds of functions). Useful if your math skills are rusty and you want to develop some intuition about what things like squaring an input value does, etc.

### Handy Tools from This Week

  * [Gradient](https://docs.unity3d.com/ScriptReference/Gradient.html) - A really easy way to create pretty gradients! You can use `Gradient.Evaluate` to get a color at a given point - note that the input to this function is a float from 0 -> 1 (wink wink)
  * [AnimationCurve](https://docs.unity3d.com/ScriptReference/AnimationCurve.html) - Like a gradient, but for numbers rather than colors. Has a similar `AnimationCurve.Evaluate` function. Incredibly powerful for giving yourself (or people you're working with) direct creative control over how one value should map to another value, without having to worry about the maths of easing functions.
  * [Mathf.Lerp](https://docs.unity3d.com/ScriptReference/Mathf.Lerp.html) - An outrageously useful shortcut to parametrically move from one value to another based on an input value `t` in the range of 0 to 1. Note that you can use `Mathf.LerpAngle` if you want to be able to 'wrap around' from 360° back to 0°.
  * [Mathf.InverseLerp](https://docs.unity3d.com/ScriptReference/Mathf.InverseLerp.html) - The opposite of Mathf.Lerp - allows you to take in an arbitrary number, as well as a known (or expected) range and returns where your input lies in that range (e.g. 0.2 would mean that you are 20% of the way from your minimum value to your maximum value). Can be combined with `Mathf.Lerp` to create a 'remapping' function
  * [Mathf.Clamp01](https://docs.unity3d.com/ScriptReference/Mathf.Clamp01.html) - A handy little function that spits out your input float unchanged if it's between zero and one, and clamps to that range if it isn't.
  * [Mathf.SmoothStep](https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html) - Similar to Mathf.Lerp, but with some nice inbuilt smoothing - saves you having to worry about easing functions if all you want is a quick, easy and smooth transition.
  * [Mathf.Sin](https://docs.unity3d.com/ScriptReference/Mathf.Sin.html) - Literally just the good ol' trigonometric sin function. Especially useful as it turns any value into a smoothly-varying output float in the range -1 -> 1.

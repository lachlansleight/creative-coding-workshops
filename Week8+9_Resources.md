# Week 8 + 9

### Resources

  * [https://thebookofshaders.com/](The Book of Shaders) - an amazing introductory resources into writing fragment shaders. The book is targeted at GLSL so you will need to convert things into Unity's HLSL. Examining the resources below will help you do this.
  * [https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html)(Vertex Fragment Shader Examples) - a high-level overview of how vert/frag shaders work in Unity, what the various parts of a shader file are for, etc.
  * [https://docs.unity3d.com/Manual/SL-SurfaceShaders.html](Writing Surface Shaders) - the equivalent documentation page regarding surface shaders - probably more detailed than you need. Contains several useful sub-pages, but most importantly...
  * [https://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html](Surface Shader Examples) - a more useful and practical page regarding surface shaders. Demonstrates common techniques and (importantly), how to go about including a custom vertex function.
  * [https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html](Build-in Shader Variables) - Unity documentation page that includes all the 'built-in' variables like `_Time`. These kind of just exist ambiently, ready for use in all your shaders!
  * [https://developer.download.nvidia.com/cg/index_stdlib.html](Nvidia CG Documentation) - the docs for all the various functions that you might need when writing shaders. If you want to know how to do something like `Mathf.Lerp` in a shader, check out the page on the `lerp()` function, for example.
  * [https://www.shadertoy.com/results?query=tag%3D2d](Shadertoy) - A huge community of people writing GLSL shaders designed to be run on the web. A lot of cutting-edge shader development happens here, and you can learn all sorts of amazing techniques by studying other people's work. An advanced resource, however, as the shaders are generally written in hyper-optimized, esoterically-written GLSL. **Note that until you're quite advanced, I only recommend trying to learn techniques from the shaders that look 2D. The 3D shaders are generally raytraced and aren't particularly applicable to games**

### Tutorials

  * [https://www.youtube.com/watch?v=kfM-yu0iQBk](Shader Basics, Blending & Textures) - Freya Holmer's amazing, in-depth and thorough tutorial on shaders. An amazing resource if you want to really understand how to make shader magic happen!
  * [https://catlikecoding.com/unity/tutorials/rendering/part-2/](Shader Fundamentals (CatlikeCoding)) - a very, *very* thorough tutorial regarding writing basic shaders with the built-in rendering pipeline. Not a great introduction, but useful to fill in some gaps and details that are often missed by more introductory tutorials.
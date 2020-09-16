# Gridworld Procedural Terrain Generator

<img src="gridworld_slideshow.gif" width="600" alt="Gridworld showcase">

Gridworld is a procedural terrain generator based of of the [Perlin Noise Terrain Generator](https://github.com/ozawatomu/perlin-noise-procedural-terrain-generation.git) from one of my other projects. It can generate terrain with sand, grass, snow, lakes, rivers, mountains, and trees:

<img src="gridworld_demo.gif" width="1200" alt="Gridworld demo">

It samples points from the layered perlin noise and creates blocks the corresponding height. Depending on the height, it will be textured as one of four materials, grass, snow, sand, or water. If the height is below a specified water height, then the height of the block will become equal to the water height. Then copies of tree prefabs are placed anywhere with grass and snow with a probability based on a tree density value.

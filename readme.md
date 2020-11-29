# COMP270 Worksheet 4

![Example](images/preview/moving-example.gif)

> Unity Engine 2019.4.1fs

## Information
- This tool allows you to generate a custom mesh based on the pixel contents of an image. You can either make your own image in greyscale, or use in the in built perlin noise to produce said mesh. 
- The mesh generated will only have the faces most relevant as shown in the figure.
- A Triplanar Shader is also included to give the isometric colour effect like shown in the figure above.
- A Water Shader is also included which includes vertex displacement as well as the ability to create foam and a foam outline in separate colours and sizes.
- As a gameplay example, I have included a small demo where you are able to click which will spawn a random prefab, or remove it if there is already one. This could be expanded heavily to any game involving tiles.

## Sources Used
> Snippets of code used is cited at the appropriate sections in code
### Mesh Generation:
- [Discrete Cell Generation](https://youtu.be/8PlpCbxB6tY)
- [Procedural Grid Generation](https://catlikecoding.com/unity/tutorials/procedural-grid/)

### Perlin Noise:
- [Perlin Noise in Unity](https://youtu.be/bG0uEXV6aHQ)

### Shaders:
- [Unity Triplanar Shader](https://youtu.be/SGXkFYS4f7I)
- [Vertex Displacement](https://youtu.be/4qLJlMpPdK0)

### Resources:
- [Colour Inspiration from Monument Valley](https://play.google.com/store/apps/details?id=com.ustwo.monumentvalley&hl=en_GB&gl=US)

## Acknowledgements
ASSET by '[Free! Low Poly Boxy-Stylized Trees](https://assetstore.unity.com/packages/3d/vegetation/trees/free-low-poly-boxy-stylized-trees-0-67258)' licenced under Standard Terms of the Asset Store End User License Agreement (https://unity3d.com/legal/as_terms, Appendix A) as an "Extension Asset"

## Instructions
### Perlin Noise and Image to Mesh Generation:
|                                                             |
|-------------------------------------------------------------|
|![Perlin Noise](images/information/perlin-noise-settings.png)|

- To create the perlin noise you can get the generator by right clicking in the project then going to 'Create->ScriptableObjects->Tools->PerlinNoiseGenerator'. 
- You can then customise the settings to your desired effect. 
- It will save in the datapath provided from the root of '/Assets'.

|                                                      |                                                      |
|------------------------------------------------------|------------------------------------------------------|
|![Read Write](images/information/read-and-write.png)  |![Script](images/information/drag-and-drop.png)       |

- **IMPORTANT:** With your perlin noise or custom image you will need to **enable Read/Write** to ensure that the file can be read by the script to generate the mesh.
- You can then add the MapToData class to any GameObject in the scene and assign it your image along with any customisation you desired.
- When you run you will see the mesh generated.
- **NOTE:** Currently you cannot saved the mesh outside of runtime.

|                                                      |                                                      |
|------------------------------------------------------|------------------------------------------------------|
|![Triplanar](images/information/triplanar.png)        |![Water](images/information/water.png)                |
#### Triplanar Shader
- The Triplanar shader has 3 colours which equate to the Vector3.Left, Vector.Backwards and Vector3.Up directions in Engine due to the orientation of my camera.
- **NOTE:** This shader currently only works for these three directions as the other sides are just black.
#### Water Shader
- The shader makes use of vertex displacement on the Y axis to simulate a wave effect.
- The water shader has the option to include or exclude foam based on the size.
- You can also change the colour of the main water, foam and foam outline.
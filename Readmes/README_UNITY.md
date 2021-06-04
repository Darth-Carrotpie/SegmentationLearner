If you are not familiar with Unity, sorry, but this is not a Unity tutorial. The engine pretty simple and quite intuitive though. Plus there are tons of learning materials online. Just google terms if you are unfamiliar with any.

## Scene Setup
In the Hierarchy window you will find main objects, which contain components responsible for various parts:
- SegmentationLearner.

This is where the visual level objects live. ```Main Camera``` should not be touched, it is used in Level Generation, whereas ```Directional Light``` and the childen of ```Level``` can be modified to setup your custom scene if necessary.
- EventCoordinator and EventSystem. Don't touch these unless you know what you're doing.
- Canvas. Basically the UI lies here.
- ```All Handlers and Managers```. Most of the logic is placed here!
All of the parameters setup is done in the components under the object ```Bucket```. There are a couple "bucket" objects. As the name implies, they don't do much, just hold variables and make them accessible.
    - ```LabelsBucket``` holds name-color pairs. Adjust them however needed. The labels which are in these lists will be exported to .csv for the model training and the colors will be sent to fastAPI via a request. To add a group you'd have to edit ```LabelsBucket.cs```  itself, while to add new ```LabelName``` values, edit ```LabelName.cs```.
    - ```ConstantsBucket``` holds general global constants, which are used everywhere like output texture resolution, target fps, etc.


## Level Generation
This one is quite trivial, but I feel a need for a short explanation none the less. When you click "Play" button, everything you've put under the aforementioned "Level" gameobject will be essentially duplicated. The original will stay with the original colors, while the copy will get an encoded color. The encoding will be done by recursively checking all ```Renderer``` and ```LabelIdentity``` component pairs. When found the color according to ```LabelsBucket``` will be assigned. The copy objects will also be put to the "Label" layer.

Camera mechanics are a bit different. The main camera will be duplicated twice. One copy for screenshots and another for Label layer view. The original camera will stay as a display camera to show actual visuals in the editor.


## Data Generation
All that is necessary lies as components under ```Generator``` gameobject, but you really don't need to edit it. Most of the variables you'd want to tweak are under ```ConstantsBucket.cs``` component. On the other hand, if you build your own level, you will need to update ```Navmesh```. 

### Navmesh

The ```Navmesh``` is important in this setup, because it is where camera can be placed to take the screenshot during the generation. It will, however be aimed generally center-wise both height and direction.
To open the info view go ```Window>AI>Navigation```
then click "Bake" tab. The properties here control how accessible the edges of the ```Navmesh``` are. More details on the ```Navmesh``` setup can be found here in [Unity Docs](https://docs.unity3d.com/Manual/nav-BuildingNavMesh.html).
After completing tweaks click "Bake" button to generate and save the new mesh.
If you click "Object" tab and select Mesh renderers, you can add them to "Navigation Static" and set them walkable or not. ```Not Walkable``` means the mesh will not be generated there.

### Higher Complexity Level

Since this is not tested in a higher complexity level, many things could need fixing. From the top of my head, I can think of one immediately - camera direction. Currently it is aimed generally center-wise, afterwards adding a delta angle to get variability. If you had lots of room's better approach would be to edit the code, so that each room would have a room-center ```Transform``` and on 'aim point' creation moment it picks the closed one and then proceeds form there normally.

## Inference

There are a few keyboard controls, which are defined in the script ```InputController.cs```.
- Move the camera using "W-A-S-D" keys. Aim with mouse.
- Buttons F1-F4 isolate label groups to see only a single group overlay at a time, i.e. furniture, building, animals.
- With "+" and "-" keys (num-pad) regulate label overlay opacity.
- "Q" key to show/hide background, leaving only label color overlay.
- "E" to force show labels, or to clear them (if they are being shown). This works also in non-inference mode (as a single web request).

For normal inference run the editor (click "Play" button in the middle) and hit spacebar. A loop will start calling API n times per second, depending on what target FPS you have set in the ```ConstantsBucket```. The sent and returned resolution will also depend on the width/height you've chose. The script which controls the loop is ```InferenceController.cs```. Inside there's a coroutine launched whenever inference start is called for. If you're interested in what is happening precisely, start diving into code form there. Most of the functions are static thereon so it should be easy to understand the logics flow.
```ApiCoordinator.cs``` handles the communications, while ```OverlayCoordinator.cs``` uses the parsed response to display the image onto UI canvas. The response itself is just a json being cast into ```DataResponseClass``` object, so if you decide to update the communications with additional properties see that. Whereas for sending the json see ```DataClass``` which is filled with proper data inside ```ApiJsonHandler.cs```.

## Fails and Things to Consider

I made an attempt to build a Shader Graph which would display raw label-encoded png response into colors. I was unsuccessful, however, perhaps failed by my lack of Graph tool know-how. Anyhow, there was not enough time to figure it out. My initial approach was to have a single graph for all labels. It would replace colors depending on input values, but as it was not working, I tried simplifying the problem to do only a single label replacement per graph. This still did not work, though. I tried using ```replace color``` node and then ```step``` node, but the channels in input texture just were not consistent. I clearly outputed correct values from the API, but they kept getting compressed in Unity, no matter what mode or texture type I selected.

It could be similar to the problem with ```sRGB``` on output mode when saving the data, so there might be a correct approach yet, just that I haven't found the correct combo at the time.
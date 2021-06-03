If you are not familiar with Unity, sorry, but this is not a Unity tutorial. It's pretty simple and quite intuitive though. Plus there are tons of learning materials online. Just google terms if you are unfamiliar with any.

## Scene Setup
In the Hierarchy windows you will find main objects, which contain components responsible for various parts:
- SegmentationLearner.

This is where the visual level objects live. ```Main Camera``` should not be touched, it is used in Level Generation, whereas ```Directional Light``` and child of ```Level``` can be modified to setup your custom scene if necessary.
- EventCoordinator and EventSystem. Don't touch these unless you know what you're doing.
- Canvas. Basically the UI lies here.
- ```All Handlers and Managers```. Like the name implies, you do setup here!
All of the parameters setup is done in the components under the object ```Buckets and Data```. There are a couple "bucket" objects. As the name implies, they don't do much, just hold variables and make them accessible.
    - ```LabelsBucket``` holds name-color pairs. Adjust them however needed. The labels which are in these lists will be exported to .csv for the model training and the colors will be sent to fastAPI via a request. To add a group you'd have to edit ```LabelsBucket.cs```  itself, while to add new ```LabelName``` values, edit ```LabelName.cs```.
    - ```ConstantsBucket``` holds general global constants, which are used everywhere like output texture resolution, target fps, etc.


## Level Generation
Level is generation is quite trivial, but I feel needs an explanation none the less. When you click "Play" button, everything you've put under the aforementioned "Level" gameobject will be essentially duplicated. One the original will stay with the original colors, while the copy will get an encoded color. The encoding will be done by recursively checking all ```Renderer``` and ```LabelIdentity``` component pairs. When found the color according to LabelsBucket will be assigned. The copy objects will also be put to the "Label" layer.

Camera mechanics are a bit different. The main camera will be duplicated twice. One copy for screenshots and another for Label layer view. The original camera will stay as a display camera to show actual visuals in the editor.


## Data Generation
All that is necessary lies as components under ```Generator``` gameobject, but you don't need to edit them. Most of the variables you'd want to tweak are under ```ConstanceBucker.cs``` component. On the other hand, if you build your own level, you will need to update ```Navmesh```. 

### Navmesh

The ```Navmesh``` is important in this setup, because it is where camera can be placed to take the screenshot during the generation. It will, however be aimed generally center-wise both height and direction.
To open the info view go ```Window>AI>Navigation```
then click "Bake" tab. The properties here control how accessible the edges of the ```Navmesh``` are. More details on the ```Navmesh``` setup can be found here i.n [Unity Docs](https://docs.unity3d.com/Manual/nav-BuildingNavMesh.html)
After completing tweaks click "Bake" button to save the new mesh.
If you click "Object" tab and select Mesh renderers, you can add them to "Navigation Static" and set them walkable or not. ```Not Walkable``` means the mesh will not be generated there.

### Higher Complexity Level

Since this is not tested in a higher complexity level, many things need fixing might arise. From the top of my head, I can think of one immediately - camera direction. Currently it is aimed generally center-wise, afterwards adding a delta angle to get variability. If you had lots of room's my approach would be to edit the code, so that each room would have a center ```Transform``` and on 'aim point' creation moment it picks the closed one and then proceeds form there normally.

## Inference

## Things to Consider
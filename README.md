# SegmentationLearner
## About
### Segmentation tasks
Image segmentation is a technique to label each individual pixel in an image (or pic sections with geometry) using a deep neural network (DNN).
Segmentation techiques can be of use in object extraction from images in real life aplications.
I.e. a DNN running on a car software using cameras can extract people, roads, road signs, bikes, etc. and later use that info for orientation.
***An example segmentation image:***
![segmentation_image](https://miro.medium.com/max/3200/1*MQCvfEbbA44fiZk5GoDvhA.png)
Image is from [How to do Semantic Segmentation using Deep learning](https://medium.com/nanonets/how-to-do-image-segmentation-using-deep-learning-c673cc5862ef)

### Problem
As in most cases the labeling must be done by a dedicated group of people.
Needless to say, that labeling each pixel in a picture (essentially drawing over) is a hard and mind-numbing grunt work.
But hey, what all those art degrees are doing anyways?? Jk, of course. But seriously, to mitigate the whole lot of boring work, we should try to automate it somehow.
Why not to use a free game engine to build an environment, generate loads of precicely labeled data and try to train a network on that.
Afterwards, if the virtual environment is realistic looking enough, we could try transfer learning to pip up the model on real photos, or even use it right away.

I mean, some video games nowadays look quite realistic, proving the capabilities of building capable environments:
![Senua](https://miro.medium.com/max/2560/1*v3XZZXSo-tdeHfvZAGr_IQ.jpeg)
This is a screenshot from a game released in 2017 called "Hellblade: Senua's Sacrifice", featuring high quality visuals, particularly volumetric lighting and vegetation among other things, which in the game dev community are notorious to pull off good looking.
The game was crafted by Unreal Engine 4, a popular (now also made free) game engine. Graphical engine capabilities have only improved since then by miles, both by UE and Unity, providing us with even better tools to create virtual worlds.

I am by no means first to claim to propose this technique. I.e. my assumption is that [Tesla](https://www.tesla.com/autopilotAI) must have done something similar to kickstart it's own networks in virtual environment.
It just happened that I was learning DNNs and wanted to make something, while also using my other skills (Unity, in this case).

## What this is
This project is an attempt to automate segmentation image labeling, by generating ready-made label-picture pairs automatically.
It is thus made of three parts:
- Virtual environment setup and generator in Unity game engine;
- A prototype Jupyter notebook to train on the generated data;
- Fastapi server to serve the model and feed that data back to Unity and visualize predictions (almost) realtime.

## What this is not
- not a commercial product/tool. Use it at your own disgression.
- not a cure for all. Your project might require different approach. Please do not spam feature requests.

## Setup
1. Download UnityHub and install an appropriate version. Should work on 2021.1.5 or later. Will update this when we get a 2021 LTS release.
2. Install python if your system does not have it. Python version used to develop this was 3.8.1
3. Git pull this repo. Add it your Unity projects.
4. Install python depencencies using requirmeents.txt file form this repo.

## How to use
### Dataset generation
Within Unity you will find a scene. Load it if you have not yet done it. The scene will contain an example level ready to be used. If you hit "play" button in the editor, the level should load and setup the environment for you.
Then just input how many pictures you want to generate and click "Generate" button. You will find your dataset in ```/Assets/Data~``` folder.

### Training
Run Jupyter server. If you do not know how to do it or even what it is, I recommend watching a few tutorials online, there's loads of them. Just google "Jupyter Notebooks".
The notebook itself is with quite detailed descriptions. It is a fastai based prototype by loading a unet resnet34 model (a convolutional neural network, or CNN in short).
![unet](https://www.researchgate.net/profile/Alan-Jackson-2/publication/323597886/figure/fig2/AS:601386504957959@1520393124691/Convolutional-neural-network-CNN-architecture-based-on-UNET-Ronneberger-et-al.png)
Unet picture source: Silburt, Ari & Ali-Dib, Mohamad & Zhu, Chenchong & Jackson, Alan & Valencia, Diana & Kissin, Yevgeni & Tamayo, Daniel & Menou, Kristen. (2018). Lunar Crater Identification via Deep Learning. Icarus. 317. 10.1016/j.icarus.2018.06.022. 

There are two parts in training the notebook:
- minimal training is to run a down-sized pic version to 64x64 resolution
- big training, gradually upscaling while training on top of each model
This saves a resources and delivers adequate results.
Models are saved in /Assets/Data~/models/ folder in the project.
You can preview the notebook without downloading [here in github](https://github.com/Darth-Carrotpie/SegmentationLearner/blob/master/SegmentationLearner.ipynb).
There you will view batch examples, label examples, top losses, training metrics over-time, etc.

### Inference
- If you have started the fastapi server, when in Unity Play mode you should be able to walk around and enable "inference" mode to communicate to API by pressing "Space".
- Walk around using "W-A-S-D" keys.
- The API simply returns labels and overlays them via a shader. With buttons F1-F12 you can isolate labels to see only a single one at a time.
- With "+" and "-" keys (numpad) you can regulate label overlay opacity. Use it if you cannot see objects well enough.
- "Q" key to show/hide background, leaving only label layer.
- "E" to force show labels, or to clear them (if they are being shown). This works even in non-inference mode, but will return nothing if there are no models saved.

### Discussion
- TBD

## To Do / Plans
- Make a YT video walkthrough
- (Make server auto-run on unity launch)

## Kudos To
- [Jeremy Howard](https://www.fast.ai/about/) for great video material and library (and also Rachel Thomas).
- [Dovydas Ceilutka ](https://www.linkedin.com/in/dovydasceilutka/) for taking initiative to create a local AI community and much more.

## License and copyright
Licenced under the [MIT Licence](LICENSE)
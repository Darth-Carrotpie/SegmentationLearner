## About
There are two parts in the training the notebook:
- minimal training process is to run a down-sized to 64x64 resolution pic version;
- big training, gradually upscaling while training on top of each model;

There are total of 4 notebooks in this project:

| Notebook | Description |
|-----|-----|
|[SegmentationLearner](/SegmentationLearner.ipynb)|Train initial low-res model |
|[GoBig](/GoBig.ipynb)| Scale up the model |
|[ONNX](/ONNX.ipynb)| ONNX inference. Load model, predict |
|[RealImagePred](/RealImagePred.ipynb)| Attempt to predict real life images |


## Initial train
Initial training is done in the notebook [SegmentationLearner](/SegmentationLearner.ipynb). Main parts there are:
 - Data loading
 - Data debugging
 - Training
 - Saving 
 - Metrics
 - Predicting

Things to notice in this part of training:
- Check constants in the first code cell, adjust them to your needs.
- Be sure to check ```batch_tmfs``` before training. There are transform parameters to apply on each batch, most of which are augmentations. This extends the dataset also making the model more resistant to image imperfections and better generalized. However, this makes model train longer, so keep in mind when dealing with larger datasets.
- ```Normalize``` transform does what is says - normalizes through batch before input. It doesn't seem to increase metrics significantly while training, but at inference point ONNX seems to like it better.
- You can use transfer learning to train on top of resnet's pre-trained weights. However, seeing as the scene in Unity is likely not realistic enough my advice is not to use it. If you setup a more realistic scene, you're welcome to try it out and share the results.
- If you have problems with generated labels, try debugging them in the Debug section. Use ```debug_label(lnames[0], 1)``` to display a filtered label, to see if they are saved as expected.
- Models are saved in three variants. Comment if you don't need them all. I.e. if you are not doing inference with low res models, you can skip saving ```to_onnx```. However, ```.export``` should be kept if you want to eventually increase resolution via weight transfer.

You can preview the notebooks without downloading here in github. If they are not loading from first attempt, try reloading multiple times. Eventually it should load properly.

Models are saved in /Assets/Data~/models/ folder in the project.

Here are my metrics, where ```epoch_count = 35``` and ```learning_rate = slice(6e-5, 5e-3)```. The slice means the rate will decrease with progress.

![metrics_64](/readme_images/metrics_64.png)

This seems to almost reach peak performance metric-wise. Bumps in the graphs could mean that learning rate is too high, but would actually be a wrong assumption. Since we use [one-cycle](https://towardsdatascience.com/finding-good-learning-rate-and-the-one-cycle-policy-7159fe1db5d6) learning scheme, the bumps are supposed to occur early in the process.

## Upscaling
Upscaling is done by loading the saved 64x64 size model and training on larger Resize(x, y) values instead. Open [GoBig](/GoBig.ipynb) notebook to see the code.
This saves time resources and delivers adequate results. If you wanna learn about this method there's a [video by Jeremy Howard](https://youtu.be/MpZxV6DVsmM?t=5176) and a [supplementing notebook](https://github.com/fastai/course-v3/blob/master/nbs/dl1/lesson3-camvid.ipynb). They are meant for an older fastai version, but the principles stay the same. The whole video is full of useful insights related to this project, therefore I  recommend watching it whole if you have not seen it yet.

![metrics_64](/readme_images/metrics_128.png)

We can see in this metrics graph (up) that in the beginning the lines seem to jump up and down very sporadically, but after a while they smooth up. This again is a sign of one-cycle fitting, but there's a catch here - bumps seem much higher than previously. It makes sense, though! Why? Because this training run does not start from a low value - we train on an already high level metrics (see graph scale). Also, starting on a higher level, means we can start with lower learning rate. Let's see how it looks after reducing starting LR  10 times:

![metrics_64](/readme_images/metrics_128_LR.png)

Much better! The start descent is much faster now. There are still a couple jumps and falls, which could be due to randomness, or perhaps we need to reduce the ending LR too. Let's train on even larger resolution next:

![metrics_246](/readme_images/metrics_256.png)

And then even higher up to even *512 x 512*:

![metrics_512](/readme_images/metrics_512.png)

### Final results

| Resolution | Train Loss | Validation Loss | Foreground Accuracy | Dice Multi  | Epochs | Total Train Time |
| ----------- | ----------- | ----------- | ----------- | ----------- | ----------- | ----------- |
|  64 x 64  | 0.1338 |	0.1115 |  0.9648 |  0.7744 | 35 | 15:41 |
| 128 x 128 | 0.0657 |	0.0586 |	0.9810 |	0.8577 | 20 | 8:56 |
| 256 x 256 | 0.0473 |	0.0317 |	0.9895 |	0.9015 | 25 | 29:42 |
| 512 x 512 | 0.0505 |  0.0429 |  0.9875 | 0.8990 | 19 | 98:03 |


*Note. At some point I retrained with more epochs and forgot to update graphs/tables in this readme, so there might be a slight miss-match (35/30/25/20) in some places, i.e. in the table's row 128 x 128. Nevertheless, that did not make much difference metrics-wise in the end.*


## Common weird errors
Some of the errors are not too intuitive, though if you worked with DNNs before, you have likely seen them.
### Memory error
CUDA out of memory error. This little devil haunts day and night. It is what is says, so no trickery there, but just to discuss a few hints concerning this project.
- Stop all other notebooks. Torch will reserve GPU memory regardles if you are using at the time as long as the kernel is running. So click ```Kernel > Shutdown All Kernels``` to clear it up. Be sure to save the models you have trained fist! Otherwise you might lose valuable data.
- Monitor memory via running ```watch -n 0.5 nvidia-smi``` in terminal. I.e. while training an upscaled model (x256) it gives me:
  

|  GPU |  GI ID| CI ID| PID | Type | Process name | GPU Memory Usage |
| ----------- | ----------- | ----------- | ----------- | ----------- | ----------- | ----------- |
|    0 |  N/A | N/A   |   1209    |  G |  /usr/lib/xorg/Xorg     |           198MiB |
|    0  | N/A | N/A   |   2183 |     G |  /usr/lib/xorg/Xorg   |             624MiB |
|    0  | N/A | N/A   |   2311 |     G |   /usr/bin/gnome-shell    |           90MiB |
|    0  | N/A | N/A   |   4043 |     G |  ...AAAAAAAAA= --shared-files    |  155MiB |
|    0 |  N/A | N/A   |   8767 |     G |  ...AAAAAAAAA= --shared-files   |    51MiB |
|    0 | N/A | N/A   |   9475 |     G |  ...r/2021.1.5f1/Editor/Unity   |   329MiB |
|    0  | N/A | N/A   |  11274  |    C |  ...ssB/classB_env/bin/python   |  9599MiB |

As you can see almost all of the memory (of available 11 Gig) is consumed. Closing some applications might be a solution, i.e. fastAPI server, because it loads a onnx session which in turn eats up some; close Unity.
### Device-side assert
A weird one ```RuntimeError: CUDA error: device-side assert triggered``` which does not really say anything useful. In the case of this project, I've noticed that this error triggers when the model receives a different amount of ```n_classes``` than it has ```out```s. This can happen if labeler camera in Unity sees an undefined label color and saves it. For example, it would save unexpected colors if you forgot to set ```Layer``` or ```LabelIdentity``` to a newly added object (more about setup in [Unity Readme](README_UNITY.md)). To solve this all of "undefined" objects in the level should be of color black, in other words - label color ```0```. If you get this error, try debuging what color codes you get in the label picture. There are helper functions (like ```n_codes``` and ```mask_check```) for that in "Debuging loaded label images" chapter in ```SegmentationLearner.ipynb``` notebook. Because there is an additional 'empty' label, ```out```s will be of length ```len(label_count)+1```.
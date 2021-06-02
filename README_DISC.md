In essence, this was just a simple prototype-ish project to get some production experience for myself, working with DNNs and Unity. Will see if it leads anywhere, though unlikely. For now it is as it is.
## Roadmap story circa post mortem
TL;DR There were naturally arising problems and decisions were made.
### First steps
When I started it was a very simple initial idea, with no actual design of how to accomplish it. I chose fastai for it's fast prototyping capabilities and Unity only because I already was familiar with the engine.

I started with the data of course. No need to make models unless there is no data! So in a weekend I made a simple generator. Prior to that, a friend of mine made a simple room setup scene (kudos to Stasys Vysniauskas a.k.a. in github "Analizatorius").

In first week I had a simple setup running with a base UNET model trained (64x64) and a simple FastAPI server.

### Lessons learned

First major problem I encountered was my own shortcoming as a Unity user - it proved difficult to make a desirable shader graph, which would display the inference of a returned label. I solved this by moving the visualization part to PIL Image lib: the API would send an already made colored representation via a response. How it is being drawn can be seen in the function called ```draw_preds_image``` which can be found in ```server_libs.backend.helper_funcs```. Labels are sent from unity each time when starting the 'inference' mode via the method ```set_label_colors(item: LabelClass)```.

Another lesson learned at this point was to properly control image conversions. PNG is the nicest format, but keep in mind to use sRGB mode everywhere when converting labels. Another hint is to use Color32 object in unity for color encoding, as it will have an integer 0-255 color channel value range as opposed to 0-1 floating point. I used sRGB on screenshots as well as I've notices a significant improvement in accuracy after testing it out. Picture look brighter in the viewer as well.

After some work within Unity a "realtime" inference was achieved. However "realtime" was the wishful thinking part though. It took 300-400 ms for API to respond ;(

[![FIRST_INF](http://img.youtube.com/vi/nrCxr0U5Apw/0.jpg)](http://www.youtube.com/watch?v=nrCxr0U5Apw "Deep neural network Resnet34 inference in Unity")

After further investigation I came to a conclusion that nearly all of the delay time was taken by ```.predict``` method on the fastai's ```Learner``` object. After further investigation it made sense - fastai is not meant for production, only for as fast as possible prototype iterations. This meant that the objects would do a lot of side/helping calculations and save tons of useless data. A nice investigation can be seen in a video by 
Zachary Mueller:

[![FIRST_INF](http://img.youtube.com/vi/-NNXDFwCrTg/0.jpg)](https://youtu.be/-NNXDFwCrTg "A walk with fastinference")

### fastinference and onnx

To increase the response speeds I turned to try using Zachary's [fastinference lib](https://github.com/muellerzr/fastinference). It actually proved to be harder than it looked at first glance as I stumbled on multiple small hiccups:
- CUDA driver version I was using was incompatible. Had to re-setup all of the environment :(
- the ```onnx``` modules proved to be difficult to install properly. It is not clear which ones to get, since there are multiple similar-sounding without elaborate explanations. After multiple trial and error installations I stumbled upon correct ones.
- the examples on fastinference were on tabular data, so the predict and get_preds methods of fastinference module did not work. To make it work I rewrote the ```predict``` function, since this was what I actually needed  - to predict just a single image at a time. Did it while inside a [notebook](ONNX.ipynb) for faster testing. The custom function now resides in [custom_onnx.py](server_libs/custom_onnx.py) and is called ```predict_image```.
- onnx seemed unhappy with my data input. Where fastai loaded Learner would gladly gobble up and spit out a clearly correct prediction, onnx did not like formats, tensor shapes and so on.

Another thing to keep in mind when making inference is to retain the meaningful transforms if any. Fastai seemed to be doing that, while fastinference strips that away. Since I did run a separate Batch-Normalization transform on input, the same should be done before feeding the model to ```ort_session``` (onnx). This rescales the input tensor from any values to from -1 to 1.

After inference ran quick and nice, I upped the request rate 20 times and it seemed to be handling pretty well.

I've put some more notes on common errors in the chapter "Common weird errors" in the [Train Readme](README_TRAIN.md).

### Final speeds
Following are computation times in seconds:

| Resolution | prediction | loss_decodes | draw_time | label_coords | total | 
| ----------- | ----------- | ----------- |----------- |----------- |----------- |
| 64 x 64 | 0.0111 | 0.0108 | 0.0013 | 0.0005 | 0.0239 |
| 128 x 128 | 0.0087 | 0.0067 | 0.0100 | 0.0021 | 0.0277 |
| 256 x 256 | 0.0161 | 0.0306 | 0.0656 | 0.0062 | 0.1186 |
| 512 x 512 | 0.0518 | 0.1573 | 0.2419 | 0.0220 | 0.4733 |

From here we can convert total inference response delay to theoretical max FPS we could pump out in Unity:

| Resolution | Max FPS |
| ----------- | ----------- | 
| 64 x 64 | 41.72 |
| 128 x 128 | 36.04 |
| 256 x 256 | 8.42 |
| 512 x 512 | 2.11 |


This is not very promising. We can see that though total inference time scales up with bigger resolution and there are choke points which don't just scale 2^2 with each increments, but more like 5-6 times instead.
- The biggest culprit here being coloring function, which turns prediction results into a picture ready to send via a json response. If we were to do the same maths inside Unity Shader Graph, it would likely cost 20-100 times less. Needless to say, it would then be negligible.
- Another choke seems to be ```loss_decodes``` part. At the moment of testing these speeds under this section there are actually two actions performed with the raw prediction output:
  - ```self.model.dls.loss_func.activation(tensor(raw_output)).squeeze(0)```
  - ```self.model.dls.loss_func.decodes(tensor(raw_output)).squeeze(0)```

```.activation``` gives us confidences, so that later we could show it as a heat map. This could be a later feature so we can remove it for now. ```.decodes``` is a necessary function as it outputs labels. This is the data we later use PIL to make a colored image for our response, so there is not much we can do about it. We can, however try to run calculations ourselves via numpy, instead of using ```.loss_func```.

**to be updated...**

## Ideas for improvements
- Move inferred image formation from API to Unity side to speed things up.
- For actual real life application tests would need to make a bigger scene, which would include many objects of the same label. For better comparability, these objects should be Resnet label counterparts. This way could check applicability by using same test images, which are Resnet's test set.
  - Perhaps there are readily available free modeled scenes?
  - Are there free or low cost model kits which could be used to kit bash a scene like that?
- Could make server auto-run on unity launch, but with fast iteration dev nature doesn't seem like it would be too useful.

## Summary
Working on the project was really fun. Learned a ton. Despite various choke points managed to make a somewhat running setup. Model train metrics look satisfying, as well as pseudo real-time inference within the game engine.


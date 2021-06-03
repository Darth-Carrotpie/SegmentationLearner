This readme stands in as a short intro to the inference part of this project. Nothing too fancy of resembling documentation, it's actually a simple enough when on its own.

## Server

So the script which runs a localhost ```uvicorn``` server is called ```model_server.py```, you run it as any other python script. At the bottom of it there's server config properties, change them at your needs.
The few API calls that we need are in this script also. To check FastAPI docs on them just open your browser and if you kept default properties, enter ```http://127.0.0.1:4200/docs``` or <...>```/redoc```. You can find many examples and tutorials online on how to use [fastAPI](https://fastapi.tiangolo.com/), so I see no reason to go too much in depth here.
## Structure

There are two manager objects, which do the stateful things themselves: ```LabelCoordinator``` and  ```ModelCoordinator```. It's as intuitive as ever... LabelCoordinator keeps track of labels and their respective colors and ModelCoordinator loads and runs model functions.

All of helper functions used anywhere are placed in ```helper_funcs.py``` script.

All of ```pydantic``` paterns are described in ```comm_classes.py```.

```custom_onnx.py``` contains the ```predict_image``` function which replaces the original predict function from ```fastinference > onnx.py```.

That's kind of it. Most of the heavy lifting is done by the ModelCoordinator object, so see it's code inside ```backend>onnx_coordinator.py``` for insights.

## Previously
If interested, you could find an older commit, before introducing fastinference and checkout at that point to see the performance that barebones fastai delivered. I.e. perhaps ```8b3b2b974e0fda931158db0eeec57ca66ca6d728``` commit should do.
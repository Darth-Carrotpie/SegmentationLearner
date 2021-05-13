import os
import pathlib
import numpy as np
from PIL import Image
from io import BytesIO
from fastai.vision.all import PILImage

path = pathlib.Path().absolute()


def label_func(fn):
    return os.path.join(path, "labels", f"{fn.stem}{fn.suffix}")


def load_image(data):
    return np.array(PILImage.create(BytesIO(data)))


def path_to_image_bytes(path):
    byteImgIO = BytesIO()
    byteImg = Image.open(path)
    byteImg.save(byteImgIO, "PNG")
    byteImgIO.seek(0)
    byteImg = byteImgIO.read()
    # dataBytesIO = BytesIO(byteImg)

    return byteImg


def pred_to_bytes(pred):
    byteImgIO = BytesIO()
    byteImg = Image.fromarray(np.array(pred[0]))
    byteImg.save(byteImgIO, "PNG")
    byteImgIO.seek(0)
    byteImg = byteImgIO.read()
    # dataBytesIO = BytesIO(byteImg)

    return byteImg

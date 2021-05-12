import os
from fastai.vision.all import resnet34, create_unet_model
from fastai.learner import load_learner
import pandas as pd
import pathlib
import cv2
import numpy as np
import torch

path = ""


def label_func(self, fn):
    return os.path.join(path, "labels", f"{fn.stem}{fn.suffix}")


class ModelCoordinator:
    def load_info(self):
        path = pathlib.Path().absolute()
        self.path = os.path.join(path, "Assets", "Data~")
        self.codes = pd.read_csv(os.path.join(self.path, "labels.csv"))
        self.img_size = (256, 256)

    def loadModel(self, filename):
        # self.model = load_model(filename, resnet34)
        print(os.path.join(self.path, "models", filename))
        self.model = load_learner(os.path.join(self.path, "models", filename), filename)
        # self.model = create_unet_model(
        #    resnet34,
        #    (len(self.codes["LabelName"]) + 1),
        #    self.img_size,

    #     pretrained=False,
    # )

    # self.model.load(os.path.join(self.path, "models", filename))

    @staticmethod
    def _prepare_img(inputImage):
        arr = np.asarray(bytearray(inputImage), dtype=np.uint8)
        img = cv2.imdecode(arr, -1)
        img = cv2.resize(img, (28, 28)) - 255
        # img = albumentations.Normalize()(image=img)["image"]
        return img

    @staticmethod
    def _prepare_batch(img):
        img = np.moveaxis(img, -1, 0)
        vec = torch.from_numpy(img)
        batch = torch.unsqueeze(vec, 0)
        return batch

    def predict(self, inputImage):
        self.prediction = self.model.predict(inputImage)
        ##img = self._prepare_img(inputImage)
        # batch = self._prepare_batch(img)
        # out = self.model.forward(batch)
        # out = out.detach().cpu().numpy()
        return self.prediction

    def __init__(self):
        self.load_info()
        self.loadModel("unity_resnet34")

    def __str__(self):
        outs = ""
        outs += self.path
        outs += " | "
        outs += self.model
        return outs

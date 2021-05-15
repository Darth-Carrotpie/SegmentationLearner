import os
from fastai.vision.all import resnet34
from fastai.learner import load_learner, load_model
import pandas as pd
import pathlib
import cv2
import numpy as np
import torch
from PIL import Image

from server_libs.backend.helper_funcs import serve_pil_image


class ModelCoordinator:
    def load_info(self):
        self.path = pathlib.Path().absolute()
        self.path = os.path.join(self.path, "Assets", "Data~")

        self.codes = pd.read_csv(os.path.join(self.path, "labels.csv"))
        self.img_size = (256, 256)

    def loadModel(self, filename):
        self.filename = os.path.join(self.path, "models", filename)
        self.model = load_learner(self.filename, resnet34)
        print(os.path.join(self.path, "models", self.filename))

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

    def debug_label(self, im, pixel_id):
        res, im = cv2.imencode(".png", im)
        pix = im.load()
        for i in range(len(list(im.getdata(band=0)))):
            x = i % 640
            y = int(i / 640)
            if pix[x, y] == pixel_id:
                pix[x, y] = 255
        im.show()

    def predict(self, inputImage):
        prediction = self.model.predict(inputImage)[0]
        # self.debug_label(np.array(prediction[0]), 1)
        # prediction.show()
        print("predicted shape:", np.array(prediction).shape)
        im = Image.fromarray(np.uint8(prediction))
        res, im_png = cv2.imencode(".png", np.array(prediction).astype(np.uint8))
        # return serve_pil_image(im)
        print(im_png.shape)
        return im_png.tobytes()

    def __init__(self):
        self.load_info()
        self.loadModel("unity_resnet34")

    def __str__(self):
        outs = ""
        outs += self.path
        outs += " | "
        outs += self.model
        return outs

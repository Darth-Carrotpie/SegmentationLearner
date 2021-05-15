import os
from fastai.vision.all import resnet34, PILImage
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
        (y, x, z) = inputImage.shape
        model_output = self.model.predict(inputImage)
        prediction = model_output[0]
        confidences = model_output[2]
        print("confidences : ", confidences.shape)
        # print("confidences : ", confidences[:, 1, 1])
        conf_max_cats = np.argmax(confidences, axis=0)
        print("conf_max_cats : ", conf_max_cats.shape)
        # print("conf_max_cats : ", conf_max_cats)

        conf_max = np.amax(np.array(confidences), axis=0)
        conf_max = np.around(conf_max, 2)
        print("conf_max : ", conf_max.shape)
        # print("conf_max : ", conf_max)

        pred_img = PILImage.create(prediction)
        labels = np.unique(prediction)
        print("labels found: ", labels)
        p_resized_back = pred_img.resize((x, y), resample=Image.BOX)
        # self.debug_label(np.array(prediction[0]), 1)
        # prediction.show()
        (y, x) = np.array(prediction).shape
        print("predicted shape:", np.array(prediction).shape)
        print("resized pred shape:", np.array(p_resized_back).shape)
        # im = Image.fromarray(np.uint8(prediction))
        # res, im_png = cv2.imencode(".png", np.array(prediction).astype(np.uint8))
        return serve_pil_image(p_resized_back), labels, conf_max
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

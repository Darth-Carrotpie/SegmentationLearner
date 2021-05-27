import os
from fastai.vision.all import resnet34, PILImage
from fastai.learner import load_learner, load_model
from fastinference.onnx import fastONNX
import pandas as pd
import pathlib
import cv2
import numpy as np
import torch
from PIL import Image
import time

from server_libs.backend.helper_funcs import (
    serve_pil_image,
    serve_confidence_map,
    draw_preds_image,
    label_coords,
)


class ModelCoordinator:
    def load_info(self):
        self.path = pathlib.Path().absolute()
        self.path = os.path.join(self.path, "Assets", "Data~")

        self.codes = pd.read_csv(os.path.join(self.path, "labels.csv"))
        self.img_size = (256, 256)

    def loadModel(self, filename):
        self.filename = os.path.join(self.path, "models", filename)
        # self.model = load_learner(self.filename, resnet34)

        self.model = fastONNX(self.filename)
        print(self.filename)

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
        (_, z, y, x) = inputImage.shape

        start_time = time.time()
        # single_dl = self.model.test_dl(inputImage)
        print(
            type(inputImage), " shape: ", inputImage.shape
        )  # could be wrong model... or wrong input. how to figure out?
        confidences = self.model.predict(inputImage)
        activation = self.model.loss_func.activation(confidences)
        print("activation : ", activation.shape)
        print("activation : ", activation[:50])
        decoded = self.model.loss_func.decodes(confidences)
        print("decoded : ", decoded.shape)
        print("decoded : ", decoded[:50])

        pred_time = time.time()
        conf_max_cats = np.argmax(confidences, axis=0)
        print("conf_max_cats : ", conf_max_cats.shape)
        print("conf_max_cats : ", conf_max_cats[:50])

        conf_max = np.amax(np.array(confidences), axis=0)
        conf_max = np.around(conf_max, 2)
        print("conf_max : ", conf_max.shape)
        print("conf_max : ", conf_max[:50])
        labels = np.unique(conf_max_cats)
        pred_img = draw_preds_image(conf_max, self.labelCoord.colors, labels)
        draw_time = time.time()

        p_resized_back = pred_img.resize((x, y), resample=Image.BOX)
        (y, x) = np.array(conf_max).shape
        # print("predicted shape:", np.array(prediction).shape)
        # print("resized pred shape:", np.array(p_resized_back).shape)
        coords = label_coords(conf_max, labels)
        coords_time = time.time()
        print("prediction computations time:", str(pred_time - start_time))
        print("draw_preds_image computations time:", str(draw_time - pred_time))
        print("label_coords computations time:", str(coords_time - draw_time))
        return serve_pil_image(p_resized_back), coords, serve_confidence_map(conf_max)

    def __init__(self, _labelCoord):
        self.load_info()
        # self.loadModel("unity_resnet34")
        self.loadModel("unity_resnet34_onnx_64x")
        self.labelCoord = _labelCoord

    def __str__(self):
        outs = ""
        outs += self.path
        outs += " | "
        outs += self.model
        return outs

import os
from fastai.vision.all import resnet34, PILImage
from fastai.learner import load_learner, load_model
from fastinference.onnx import fastONNX
import pandas as pd
import pathlib
import cv2
import numpy as np
from torch import tensor
import torch
from PIL import Image
import time
from server_libs.custom_onnx import predict_image

from server_libs.backend.helper_funcs import (
    serve_pil_image,
    serve_confidence_map,
    draw_preds_image,
    label_coords,
    image64_to_tensor,
)


class ModelCoordinator:
    def predict(self, inputImage):
        inputImage = image64_to_tensor(inputImage, self.MODEL_SIZE)

        (_, z, y, x) = inputImage.shape
        start_time = time.time()
        # print(inputImage[:20])
        # raw_output = self.model.predict(inputImage)
        raw_output = predict_image(self.model.ort_session, inputImage)
        pred_time = time.time()
        # confidences = self.model.dls.loss_func.activation(tensor(raw_output)).squeeze(0)
        # print("confidences : ", confidences.shape)
        # print("confidences : ", confidences[:5])
        # conf_max_vals, conf_max_indeces = torch.max(confidences, 0)
        # conf_max_cats = np.argmax(confidences, axis=0)
        # print("conf_max_cats : ", conf_max_indeces.shape)
        # print("conf_max_cats : ", conf_max_indeces)

        # labels = np.unique(conf_max_indeces)
        # print("labels:", labels)

        # conf_max = np.amax(np.array(confidences), axis=0)
        # conf_max = np.around(conf_max, 2)
        # print("conf_max : ", conf_max_vals.shape)
        # print("conf_max : ", conf_max_vals)

        predictions = self.model.dls.loss_func.decodes(tensor(raw_output))
        predictions = predictions.numpy().squeeze(0)
        loss_decodes = time.time()

        print("predictions : ", predictions.shape)
        # print("predictions : ", predictions[:5])

        labels = np.unique(predictions)
        # print("labels:", labels)
        # pred_img = draw_preds_image(conf_max, self.labelCoord.colors, labels)
        pred_img = draw_preds_image(predictions, self.labelCoord.colors, labels)
        draw_time = time.time()

        p_resized_back = pred_img.resize((x, y), resample=Image.BOX)
        (y, x) = np.array(predictions).shape
        # print("predictions shape:", np.array(predictions).shape)
        # print("resized pred shape:", np.array(p_resized_back).shape)
        coords = label_coords(predictions, labels)
        coords_time = time.time()
        print("prediction computations time:", str(pred_time - start_time))
        print("loss_decodes computations time:", str(loss_decodes - pred_time))
        print("draw_time computations time:", str(draw_time - loss_decodes))
        print("label_coords computations time:", str(coords_time - draw_time))
        print("total time:", str(coords_time - start_time))
        return (
            serve_pil_image(p_resized_back),
            coords,
        )  # , serve_confidence_map(conf_max)

    def load_info(self):
        self.path = pathlib.Path().absolute()
        self.path = os.path.join(self.path, "Assets", "Data~")
        self.codes = pd.read_csv(os.path.join(self.path, "labels.csv"))

    def loadModel(self, filename):
        self.filename = os.path.join(self.path, "models", filename)
        self.model = fastONNX(self.filename)
        print("loaded: ", self.filename)

    def __init__(self, _labelCoord, _MODEL_SIZE):
        self.MODEL_SIZE = _MODEL_SIZE
        self.labelCoord = _labelCoord

        self.load_info()
        load_name = "unity_resnet34" + "_" + str(_MODEL_SIZE) + "x_onnx"
        self.loadModel(load_name)

    def __str__(self):
        outs = ""
        outs += self.path
        outs += " | "
        outs += self.model
        return outs

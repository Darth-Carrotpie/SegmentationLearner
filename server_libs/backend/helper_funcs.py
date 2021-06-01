import os
import pathlib
import numpy as np
from PIL import Image
from io import BytesIO
from fastai.vision.all import PILImage
from torchvision.transforms.functional import (
    pil_to_tensor,
    normalize,
)
from torchvision.transforms import Resize
import torch

path = pathlib.Path().absolute()


def label_func(fn):
    return os.path.join(path, "labels", f"{fn.stem}{fn.suffix}")


def normalize_tensor(inp_tensor):
    mean = torch.mean(inp_tensor)
    std = torch.std(inp_tensor)
    # print("pre norm: mean: ", mean, " std: ", std)
    return normalize(inp_tensor, mean, std)


def image64_to_tensor(data, size):
    resize = Resize([size, size])
    res = resize(PILImage.create(BytesIO(data)))
    tn = pil_to_tensor(res).to(torch.float)
    tn = normalize_tensor(tn)
    # print("post norm: mean: ", mean, " std: ", std)

    tn.unsqueeze_(0)
    return tn


def path_to_image_bytes(path):
    byteImgIO = BytesIO()
    byteImg = Image.open(path)
    byteImg.save(byteImgIO, "PNG")
    byteImgIO.seek(0)
    byteImg = byteImgIO.read()
    return byteImg


def pred_to_bytes(pred):
    byteImgIO = BytesIO()
    byteImg = Image.fromarray(np.array(pred[0]))
    byteImg.save(byteImgIO, "PNG")
    byteImgIO.seek(0)
    byteImg = byteImgIO.read()
    return byteImg


def serve_pil_image(pil_img):
    img_io = BytesIO()
    pil_img.save(img_io, "PNG", quality=95)
    img_io.seek(0)
    return img_io


def serve_confidence_map(confs):
    neg_img = np.subtract(1, confs)
    neg_img = np.clip(neg_img, 0.6, 1)
    heatmap_image = PILImage.create(np.uint8(neg_img * 255))

    return serve_pil_image(heatmap_image)


def draw_preds_image(preds, colors, labels):

    # preds_4d = np.repeat(preds[..., np.newaxis], 4, axis=2)
    preds_4d = np.repeat(preds, 4, axis=0)
    preds_4d = np.array(preds_4d).astype(np.uint8)
    # print(type(preds_4d), " preds_4d shape: ", preds_4d.shape)
    for label in labels:
        # print(label, " label, color : ", colors[label - 1])
        preds_4d = replace_color(
            preds_4d, label, np.array(colors[label - 1].GetTuple())
        )
    trans = preds_4d.transpose(1, 2, 0)
    # print(type(trans), " preds_4d shape: ", trans.shape)
    return PILImage.create(np.uint8(trans))


def replace_color(img_arr, source, target):
    return np.where(img_arr == source, target[..., np.newaxis, np.newaxis], img_arr)


def label_coords(preds, labels):
    payload = []

    for label in labels:
        rows, cols = np.where(preds == label)
        lb = {
            "label": int(label - 1),
            "labelPositionX": int(cols.mean()),
            "labelPositionY": int(rows.mean()),
        }
        payload.append(lb)
    return payload

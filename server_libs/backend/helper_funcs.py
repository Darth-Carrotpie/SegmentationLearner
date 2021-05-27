import os
import pathlib
import numpy as np
from PIL import Image
from io import BytesIO
from fastai.vision.all import PILImage
from torchvision.transforms.functional import pil_to_tensor, convert_image_dtype
from torchvision.transforms import Resize, ToTensor

path = pathlib.Path().absolute()


def label_func(fn):
    return os.path.join(path, "labels", f"{fn.stem}{fn.suffix}")


def load_image_onnx(data):
    resize = Resize([64, 64])
    to_tensor = ToTensor()
    res = resize(PILImage.create(BytesIO(data)))
    tn = to_tensor(res)
    print("before unsqeeze: ", tn.shape)
    tn.unsqueeze_(0)
    print("after unsqeeze: ", tn.shape)
    return tn
    # return to_tensor(resize()).unsqueeze_(0)


def load_image(data):
    return convert_image_dtype(pil_to_tensor(PILImage.create(BytesIO(data))))


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
    preds_4d = np.repeat(preds[..., np.newaxis], 4, axis=2)
    preds_4d = np.array(preds_4d).astype(np.uint8)
    for label in labels:
        preds_4d = replace_color(preds_4d, label, colors[label - 1].GetTuple())
    return PILImage.create(np.uint8(preds_4d))


def replace_color(img_arr, source, target):
    return np.where(img_arr == source, np.array(target), img_arr)


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

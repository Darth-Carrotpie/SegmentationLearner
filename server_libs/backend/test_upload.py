from os.path import sameopenfile
import requests
import base64

import os
import pathlib
import numpy as np
from PIL import Image
from io import BytesIO
from server_libs.backend.helper_funcs import path_to_image_bytes


def pretty_print_req(req):
    """
    At this point it is completely built and ready
    to be fired; it is "prepared".

    However pay attention at the formatting used in
    this function because it is programmed to be pretty
    printed and may differ from the actual request.
    """
    info = (req.body[:75]) if len(req.body) > 75 else req.body
    print(
        "{}\n{}\r\n{}\r\n\r\n{}".format(
            "-----------START-----------",
            req.method + " " + req.url,
            "\r\n".join("{}: {}".format(k, v) for k, v in req.headers.items()),
            info,
        )
    )


def test_upload():
    path = pathlib.Path().absolute()
    path = os.path.join(path, "Assets", "Data~", "screenshots")
    _test_upload_file = ""
    for root, dirs, files in os.walk(
        path, topdown=True, onerror=None, followlinks=False
    ):
        _test_upload_file = os.path.join(root, files[0])
    print(_test_upload_file)

    file = {
        "file": (
            "screenshot.png",
            base64.b64encode(path_to_image_bytes(_test_upload_file)),
            "image/png",
        )
    }

    url = "http://127.0.0.1:4200/predict_image"

    response = requests.post(url, files=file)
    pretty_print_req(response.request)
    return response


def test_text_upload():
    path = pathlib.Path().absolute()
    path = os.path.join(path, "Assets", "Data~", "screenshots")
    _test_upload_file = ""
    for root, dirs, files in os.walk(
        path, topdown=True, onerror=None, followlinks=False
    ):
        _test_upload_file = os.path.join(root, files[0])
    print(_test_upload_file)

    data = {"string_stream": base64.b64encode(path_to_image_bytes(_test_upload_file))}
    url = "http://127.0.0.1:4200/predict_image_text"

    response = requests.post(url, json=data)
    pretty_print_req(response.request)
    return response

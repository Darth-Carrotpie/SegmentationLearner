from datetime import datetime
import uvicorn
from io import BytesIO
import json
from fastapi.responses import JSONResponse
from fastapi import FastAPI, UploadFile, File, Request, Response
from fastapi.middleware.cors import CORSMiddleware
import base64
from pydantic import BaseModel
from server_libs.backend.mcoordinator import ModelCoordinator
from server_libs.backend.helper_funcs import label_func, load_image

from server_libs.backend.test_upload import (
    test_upload,
    test_text_upload,
    pretty_print_req,
)

from starlette.responses import StreamingResponse

api = FastAPI()

models = ModelCoordinator()


origins = [
    "http://127.0.0.1",
]
api.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # change this in production to a concrete list for security
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


@api.get("/")
def read_root():
    return {
        "root_response": "OK!",
    }


@api.get("/predict_test")
def predict_item():
    test = test_upload()
    print("test --- sent")
    resp = {"predict_test": test.text}
    return resp


@api.get("/predict_test_text")
def predict_item():
    test = test_text_upload()
    print("test --- sent")
    resp = {"predict_test": test.text}
    return resp


class Data(BaseModel):
    string_stream: str


@api.post("/predict_image_text")
async def image_endpoint_text(request: Request):
    data = await request.json()
    print(data)
    print("request received, waiting to read...")
    # data = byte_stream.read()
    print("image read!")
    loaded_image = load_image(base64.b64decode(data["string_stream"]))
    print("received image, shape: ", loaded_image.shape)
    pred_img_bytes = models.predict(loaded_image)
    print("making a StreamingResponse()...")
    return StreamingResponse(BytesIO(pred_img_bytes), media_type="image/png")


@api.post("/predict_image")
async def image_endpoint(file: UploadFile = File(...)):
    print("request received, waiting to read...")
    data = await file.read()
    print("image read!")
    loaded_image = load_image(base64.b64decode(data))
    print("received image, shape: ", loaded_image.shape)
    pred_img_bytes = models.predict(loaded_image)
    print("making a StreamingResponse()...")
    prep_to_send = base64.b64encode(pred_img_bytes).decode("ascii")
    # return StreamingResponse(BytesIO(pred_img_bytes), media_type="image/png")
    payload = json.dumps({"mime": "image/png", "image": prep_to_send}, indent=4)
    return JSONResponse(content=payload, media_type="image/png")


if __name__ == "__main__":
    print("starting...")
    uvicorn.run(api, host="0.0.0.0", port=4200)

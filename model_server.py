import uvicorn
from fastapi.responses import JSONResponse
from fastapi import FastAPI, UploadFile, File, Request, Response
from fastapi.middleware.cors import CORSMiddleware

from server_libs.comm_classes import DataClass, LabelClass

import base64
from server_libs.backend.onnx_coortinator import ModelCoordinator
from server_libs.backend.lcoordinator import LabelCoordinator
from server_libs.backend.helper_funcs import label_func, image64_to_tensor

from server_libs.backend.test_upload import test_upload, test_text_upload

from starlette.responses import StreamingResponse

MODEL_SIZE = 64

api = FastAPI()

labelCoord = LabelCoordinator()
models = ModelCoordinator(labelCoord, MODEL_SIZE)

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
    return {"root_response": "OK!"}


@api.put("/set_label_colors")
def set_label_colors(item: LabelClass):
    print(item.mime)
    print(item.labels[:5])
    print(item.colors[:5])
    labelCoord.UpdateMaskVals(item.labels)
    labelCoord.SetupColors(item.colors)
    return {
        "set_label_colors": "OK!",
    }


@api.put("/predict_image_text")
async def image_endpoint_text(item: DataClass):
    print("request received, waiting to predict...")
    pred_img_bytes, labels = models.predict(base64.b64decode(item.image64))
    prep_to_send = base64.b64encode(pred_img_bytes.read()).decode("ascii")
    # confidences = base64.b64encode(confidences.read()).decode("ascii")
    print("encoded image len:", len(prep_to_send))
    payload = {
        "mime": "image/png",
        "labels": labels,
        "image64": prep_to_send,
        # "confidences": confidences,
    }
    # print(str(payload)[:150])
    return JSONResponse(content=payload, media_type="application/json")


if __name__ == "__main__":
    print("starting...")
    uvicorn.run(api, host="0.0.0.0", port=4200)

from datetime import datetime

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from fastapi.responses import HTMLResponse


from server_libs.constants import hero_names, map_names, game_types
from server_libs.item import Item

from server_libs.backend.mcoordinator import ModelCoordinator

from starlette.responses import StreamingResponse

api = FastAPI()
##preds = PredCoordinator()
models = ModelCoordinator()


# hard coded for now, later, extract from loaded model:

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


@api.get("/test")
def read_root_test():
    return {
        "test_response": "OK!",
        # "map_names": map_names,
        # "game_types": game_types,
        # "available_models": available_models,
    }


@api.get("/")
def read_root():
    return {
        "hero_names": hero_names,
        "map_names": map_names,
        "game_types": game_types,
        "available_models": models.getAvailableModels(),
    }


# @api.get("/items/{item_id}")
# def read_item(item_id: int, q: Optional[str] = None):
#    return {"item_id": item_id, "q": q}


@api.put("/predict/")
def predict_item(item: Item):
    pred = preds.get_pred(item=item, models=models)
    return {
        "pred_id": pred.pred_id,
        "pred_value": pred.value,
        "current_selections": pred.current_selections,
        "top_pick_recs": pred.top_pick_recs,
        "top_ban_recs": pred.top_ban_recs,
    }


@api.post("/predict_image")
def image_endpoint(*, screenshot):
    # Returns a cv2 image array from the document vector
    pred = preds.get_pred(item=screenshot, models=models)
    cv2img = my_function(screenshot)
    res, im_png = cv2.imencode(".png", cv2img)
    return StreamingResponse(io.BytesIO(im_png.tobytes()), media_type="image/png")


@api.get("/shap/{prediction_id}")
async def get_shap(prediction_id: str):
    pred = preds.get_pred(prediction_id=prediction_id)
    if pred is not None:
        return HTMLResponse(content=pred.shap, status_code=200)
    else:
        return None


@api.get("/waterfalls/{prediction_id}")
async def get_waterfalls(prediction_id: str):
    pred = preds.get_pred(prediction_id=prediction_id)
    if pred is not None:
        return HTMLResponse(content=pred.waterfalls, status_code=200)
    else:
        return None

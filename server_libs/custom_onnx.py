import numpy as np
from torch import tensor


def to_numpy(t: tensor):
    return t.detach.cpu().numpy() if t.requires_grad else t.cpu().numpy()


def predict_image(ort_session, inps):
    "Predict a single tensor item"
    print(type(inps), " predict shape: ", inps.shape)
    ort_inputs = {
        ort_session.get_inputs()[0].name: (np.array(to_numpy(inps), dtype=np.float32))
    }
    outs = ort_session.run(None, ort_inputs)
    return outs[0]

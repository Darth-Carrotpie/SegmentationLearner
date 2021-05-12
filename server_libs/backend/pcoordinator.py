from objects.prediction import Prediction
import secrets

class PredCoordinator():
    PREDICTIONS_MAX = 100
    predictions = []
    pred_ids = []


    def add_prediction(self, prediction_id, item, models):
        print("add new pred: "+prediction_id)
        if prediction_id not in self.pred_ids:
            self.predictions.append((prediction_id, Prediction(prediction_id, item, models)))
            self.pred_ids.append(prediction_id)
        else:
            [pred.__init__(prediction_id, item) for ID, pred in self.predictions if ID == prediction_id]

        if len(self.predictions) > self.PREDICTIONS_MAX:
            self.predictions.pop(0)


    def get_pred(self, prediction_id = None, item = None, models = None):
        if prediction_id == None:
            prediction_id = secrets.token_urlsafe(16)

        if prediction_id not in self.pred_ids:
            self.add_prediction(prediction_id, item, models)

        return [pred for ID, pred in self.predictions if ID == prediction_id][0]

    def __init__(self):
        pass
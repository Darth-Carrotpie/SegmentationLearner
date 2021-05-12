
from random import random, sample
from constants import hero_names, map_names, game_types, available_models
from backend.feature_helpers import hero_hot_transform, hero_table_variants, force_plot_pred_filtered

import matplotlib.pyplot as plt
import numpy as np
import mpld3
import shap

def mockup_fig(input):
    fig = plt.figure()
    ax = fig.gca()
    ax.plot([1,2,3,4] * input)
    #plt.show()
    return mpld3.fig_to_html(fig)

class Prediction():
    def __init__(self, _prediction_id, _item, modelCoord):
        self.pred_id = _prediction_id
        if _item.has_min_vals():
            self.setup(_item, modelCoord)
        else:
            self.setup_mockup()

    def setup(self, _item, modelCoord):
        self.item = _item
        self.m_name, self.m_type, self.m = modelCoord.getModel()
        self.explainer = shap.TreeExplainer(self.m)
        input_row = hero_hot_transform(game_map = self.item.map_name,
                                        game_type = self.item.game_type,
                                        winners = self.item.your_team,
                                        losers = self.item.opponent_team)
        self.value = (self.m.predict(input_row)).tolist()
        #shap = embed_map(shap.force_plot(explainer.expected_value, shap_values[1000,:], X_train.iloc[0,:]), "shap.html")
        #waterfalls = embed_map(draw_interpretation_waterfalls(bias, prog ,rest))

        #self.rand_heroes = sample(hero_names, 3)
        #self.top_recommendations = {self.rand_heroes[0]:random(), self.rand_heroes[1]:random(),self.rand_heroes[2]:random()}

        self.top_pick_recs = self.find_top_picks(input_row, amount = self.item.rec_size if self.item.rec_size else 3)
        self.top_ban_recs = self.find_top_bans(input_row)
        self.current_selections, self.shap = self.show_current_sel(input_row)
        #self.shap = mockup_fig(1)
        self.waterfalls = mockup_fig(-1)

    def show_current_sel(self, input_row):
        vals, shap_plot = force_plot_pred_filtered(self.explainer, input_row)
        #curr_heroes = self.item.your_team + self.item.opponent_team
        #x = np.random.uniform(low=-1, high=1, size=len(curr_heroes)).tolist()
        return vals, shap_plot

    def find_top_picks(self, input_row, amount = 3):
        unav_heroes = self.item.your_team + self.item.opponent_team + self.item.ban_names
        all_variants_df, available_heroes = hero_table_variants(input_row, game_map = self.item.map_name, game_type = self.item.game_type, unavailable_heroes=unav_heroes)
        all_preds = (self.m.predict(all_variants_df)).tolist()
        outcomes = list(zip(available_heroes, all_preds))
        outcomes.sort(key=lambda t: t[1], reverse=True)
        #print("shape of all_variants_df: ", all_variants_df.shape, " len names: ",len(available_heroes))
        return outcomes[:amount]

    def find_top_bans(self, input_row, amount = 3):
        unav_heroes = self.item.your_team + self.item.opponent_team + self.item.ban_names
        all_variants_df, available_heroes = hero_table_variants(input_row, game_map = self.item.map_name, game_type = self.item.game_type, unavailable_heroes=unav_heroes)
        all_preds = (self.m.predict(all_variants_df)).tolist()
        outcomes = list(zip(available_heroes, all_preds))
        outcomes.sort(key=lambda t: t[1])
        #print("shape of all_variants_df: ", all_variants_df.shape, " len names: ",len(available_heroes))
        return outcomes[:amount]

    def setup_mockup(self):
        #mockup values:
        self.value = random()
        self.rand_heroes = sample(hero_names, 3)
        self.top_recommendations = {self.rand_heroes[0]:random(), self.rand_heroes[1]:random(),self.rand_heroes[2]:random()}
        self.current_selections = {}
        self.shap = mockup_fig(1)
        self.waterfalls = mockup_fig(-1)
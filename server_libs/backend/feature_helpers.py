import numpy as np
import pandas as pd
import os.path
import shap

thisfile = os.path.abspath(__file__)
programs = os.path.dirname(thisfile)
project1 = os.path.dirname(programs)

game_map_cats_path = os.path.join(project1, "info/game_map_cats.csv")
game_type_cats_path = os.path.join(project1, "info/game_type_cats.csv")
hero_name_cats_path = os.path.join(project1, "info/hero_name_cats.csv")

m_names = pd.read_csv(game_map_cats_path,header=None, index_col=0, squeeze=True).to_dict()
gt_names = pd.read_csv(game_type_cats_path,header=None, index_col=0, squeeze=True).to_dict()
h_names = pd.read_csv(hero_name_cats_path,header=None, index_col=0, squeeze=True).to_dict()

m_names_swapped = dict([(value, key) for key, value in m_names.items()])
gt_names_swapped = dict([(value, key) for key, value in gt_names.items()])
hero_names = list([value for key, value in h_names.items()])

def hero_hot_transform(game_map = '', game_type = '', winners = [], losers = [], bans = []):
    new_row = pd.Series(index = hero_names, dtype = 'int8')
    for x in hero_names:
        if x in winners:
            new_row[x] = 2
        if x in losers:
            new_row[x] = 1
    #print(list(m_names.keys())[0], " : ",m_names[list(m_names.keys())[0]])  
    #print(list(m_names_swapped.keys())[0], " : ",m_names_swapped[list(m_names_swapped.keys())[0]])
    if game_map not in m_names_swapped.keys():
        game_map = 'Tomb of the Spider Queen'
    if game_type not in gt_names_swapped.keys():
        game_type = 'UnrankedDraft'
        
    game_map_index = m_names_swapped[game_map]
    game_type_index = gt_names_swapped[game_type]    
        
    new_row = new_row.append(pd.Series([game_map_index, game_type_index], index = ['game_map','game_type']))
    return pd.DataFrame(new_row).transpose()
    #adding turn into a dafatrams and a transpose for XGBoost to understand input

def hero_table_variants(input_df, game_map = '', game_type = '', unavailable_heroes = []):
    available_heroes = [i for i in hero_names if i not in unavailable_heroes]
    for i in range(1, len(available_heroes)):
        input_df.loc[i] = input_df.loc[0]
        input_df.loc[i][available_heroes[i]] = 2
    input_df.drop([0])
    return input_df, available_heroes

def hero_ban_variants(input_df, game_map = '', game_type = '', unavailable_heroes = []):
    available_heroes = [i for i in hero_names if i not in unavailable_heroes]
    for i in range(1, len(available_heroes)):
        input_df.loc[i] = input_df.loc[0]
        input_df.loc[i][available_heroes[i]] = 1
    input_df.drop([0])
    return input_df, available_heroes

def force_plot_pred_filtered(explainer, input_row):
    pred_shap_values = explainer.shap_values(input_row)[0]
    feature_names = input_row.columns
    features = input_row.iloc[0,:]
    
    shaps_ziped = list(zip(pred_shap_values, features, feature_names))
    shaps_ziped.sort(key=lambda t: t[0], reverse=True)
    
    shaps_filtered = [(s, i, n) for (s, i, n) in shaps_ziped if i > 0]
    rest_shaps_val_total = sum([s for (s, i, n) in shaps_ziped if i == 0])
    
    shaps_filtered.append((rest_shaps_val_total, 0, "Other"))
    
    shap_v_filt = np.array([s for (s, i, n) in shaps_filtered])
    feat_filt = [i for (s, i, n) in shaps_filtered]
    names_filt = [n for (s, i, n) in shaps_filtered]

    return [(str(n),float(s)) for (s, i, n) in shaps_filtered], shap.force_plot(explainer.expected_value, shap_values=shap_v_filt, features=feat_filt, feature_names=names_filt)
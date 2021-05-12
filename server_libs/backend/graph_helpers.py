
import waterfall_chart

def draw_interpretation_waterfalls(bias, interpretations, rest):
    winner_team_interps = [(a, b, c) for (a, b, c) in interpretations if (a in hero_names and b == 2)]
    loser_team_interps = [(a, b, c) for (a, b, c) in interpretations if (a in hero_names and b == 1)]

    colnames_winner = [a for (a, b, c) in winner_team_interps]
    colnames_loser = [a for (a, b, c) in loser_team_interps]
    conts_winner = [c for (a, b, c) in winner_team_interps]
    conts_loser = [c for (a, b, c) in loser_team_interps]
    
    colname_totals = ['bias','your team', 'opponents','map','type', 'rest']
    map_interp = [c for (a, b, c) in interpretations if (a == 'game_map')]
    type_interp = [c for (a, b, c) in interpretations if (a == 'game_type')]
    rest_interp = [c for (a, b, c) in rest]
    conts_totals = [bias[0],sum(conts_winner),sum(conts_loser), sum(map_interp), sum(type_interp), sum(rest_interp)]

    winner_plot = waterfall_chart.plot(colnames_winner,conts_winner, rotation_value=90, threshold=0.0,formatting='{:,.4f}')
    loser_plot = waterfall_chart.plot(colnames_loser,conts_loser, rotation_value=90, threshold=0.0,formatting='{:,.4f}')
    totals_plot = waterfall_chart.plot(colname_totals,conts_totals, rotation_value=90, threshold=0.0,formatting='{:,.4f}')


def prognoses(model, game_map, game_type, winners, losers):
    input_row = np.asarray([hero_hot_transform(game_map = game_map, game_type=game_type, winners = winners, losers=losers)])
    prediction, bias, contributions_input = ti.predict(model, input_row)
    idxs = np.argsort(contributions_input[0])
    interpretations = [(a, b, c) for (a, b, c) in zip(dummies_data.columns[idxs], input_row[0][idxs], contributions_input[0][idxs])  if b != 0]
    rest = [(a, b, c) for (a, b, c) in zip(dummies_data.columns[idxs], input_row[0][idxs], contributions_input[0][idxs])  if b == 0]

    change = sum([c for c in contributions_input[0][idxs]])

    outcome = bias[0] + change
    #print(interpretations)
    #print(bias, change, [c for (a, b, c) in interpretations])
    if bias[0] + change > 0.5:
        print("your team is expected to win (>0.5): ",outcome)
    else:
        print("your team is expected to lose (<0.5): ",outcome)
    return (bias, interpretations, rest)
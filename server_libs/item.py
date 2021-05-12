from pydantic import BaseModel
from typing import Optional, List

class Item(BaseModel):
    ban_names: List[str] = ["Auriel","Diablo", "Nazeebo"]
    your_team: List[str] = ["Murky","Brightwing"]
    opponent_team: List[str] = ['Johanna', 'Malfurion', 'Azmodan', 'Kerrigan', 'Blaze']
    map_name: Optional[str] = "Tomb of the Spider Queen"
    game_type: Optional[str] = "UnrankedDraft"
    pred_model: Optional[str] = "default"
    rec_size: Optional[int] = 3

    def has_min_vals(self):
        return (len(self.your_team) > 0 or len(self.opponent_team) > 0) and len(self.ban_names) > 2
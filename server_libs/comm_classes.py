from typing import Optional, List
from pydantic import BaseModel


class DataClass(BaseModel):
    mime: Optional[str] = None
    image64: Optional[str] = None
    # imageBytes: List[bytes] = None
    # class Config:
    #    arbitrary_types_allowed = True


class Color32(BaseModel):
    r: Optional[int] = None
    g: Optional[int] = None
    b: Optional[int] = None
    a: Optional[int] = None

    def GetTuple(self):
        return (self.r, self.g, self.b, self.a)


class LabelClass(BaseModel):
    mime: Optional[str] = None
    labels: List[int] = []
    colors: List[Color32] = []
    # class Config:
    #    arbitrary_types_allowed = True

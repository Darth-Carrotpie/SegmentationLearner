class LabelCoordinator:
    def __init__(self):
        self.labelMask = []
        self.colors = []

    def UpdateMaskVals(self, _labelMasks):
        self.labelMask = _labelMasks

    def SetupColors(self, _colors):
        self.colors = _colors

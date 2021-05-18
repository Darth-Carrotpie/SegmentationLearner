#from fastai.vision.all import PILImage
from io import BytesIO
#from nbdev.showdoc import *
#from fastcore.test import test_eq
from fastai.vision.all import *

#from fastcore.test import *
#from nbdev.showdoc import *
#from fastcore.nb_imports import *


#Data loading and converting:
def load_image_from_bytes(data):
    return np.array(PILImage.create(BytesIO(data)))

def path_to_image_bytes(path):
    byteImgIO = BytesIO()
    byteImg = Image.open(path)
    byteImg.save(byteImgIO, "PNG")
    byteImgIO.seek(0)
    byteImg = byteImgIO.read()
    
    return byteImg

#show images in various ways for debuging and inestigating:
def show_masked_original_size(base_img, learner):
    pred = learner.predict(np.array(base_img))[0]
    pred_img = PILImage.create(pred)
    y, x = base_img.shape
    print(pred_img.shape, " resizing back to ",base_img.shape)
    p_resized_back = pred_img.resize((x, y), resample=Image.BOX)
    plt.imshow(np.array(base_img))
    plt.imshow(np.array(p_resized_back), alpha=0.25)
    
def show_superimposed(cam_fn, mask_fn):
    #test how mask is shown, from: https://docs.fast.ai/vision.core.html
    cam_img = PILImage.create(cam_fn)
    #test_eq(cam_img.size, (640,480))
    tmask = Transform(PILMask.create)
    mask = tmask(mask_fn)
    test_eq(type(mask), PILMask)
    #test_eq(mask.size, (640,480))
    
    _,axs = plt.subplots(1,3, figsize=(12,3))
    cam_img.show(ctx=axs[0], title='image')
    mask.show(alpha=1, ctx=axs[1], vmin=0, vmax=25, title='mask')
    cam_img.show(ctx=axs[2], title='superimposed')
    mask.show(ctx=axs[2], vmin=1, vmax=30);

def show_cropped(base_img):
    h, w = base_img.shape
    max_size = max(h, w)
    min_size = min(h, w)
    offset = (max_size-min_size)/2
    if w > h:
        box = (offset, 0, max_size-offset, min_size)
    else:
        box = (0, offset, min_size, max_size-offset)
    b_cropped = base_img.crop(box)
    plt.imshow(np.array(b_cropped))
    
#Debuging mask values
def n_code(fname):
    "Gather the codes from a single `fname`"
    vals = set()
    msk = np.array(load_image(fname)) #PILMask.create
    for val in np.unique(msk):
        if val not in vals:
            vals.add(val)
    return vals

def n_codes(fnames, is_partial=True):
    "Gather the codes from a list of `fnames`"
    vals = set()
    if is_partial:
        random.shuffle(fnames)
        fnames = fnames[:100]
    for fname in fnames:
        vals.update(n_code(fname))
    vals = list(vals)
    p2c = dict()
    for i,val in enumerate(vals):
        p2c[i] = vals[i]
    return p2c

def mask_check(lname):
    msk = np.array(PILMask.create(lname))
    return np.unique(msk)

def debug_label(label_img, pixel_id):
    im = Image.open(label_img)
    pix = im.load()
    print(im.size)
    print(im.getdata())
    for i in range(len(list(im.getdata(band=0)))):
        x = i % 640
        y = int(i / 640)
        if(pix[x, y] == pixel_id):
            pix[x, y] = 255
            #pix[x, y] = (0, 255, 0, 255)
    im.show()
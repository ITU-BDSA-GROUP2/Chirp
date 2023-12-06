    var images = new Array("images/bird1.webp",
"images/Bird2.png",
"images/Bird3.png",
"images/Image4.png");

function nextImg(curImg) {
    let imgNum = 0;

    for (let i = 0; i < images.length; i++) {
        
        if (curImg === images[i]) {
            imgNum = checkOverFlow(i+1);
            break;
        }
    }
    let container = document.getElementById('imgid');
    let container1 = document.getElementById('curImg');
    container.value = images[imgNum];
    container1.value = images[imgNum];
    container.src = images[imgNum];
}

function prevImg(curImg) {
    let imgNum = 0;

    for (let i = 0; i < images.length; i++) {
        
        if (curImg === images[i]) {
            imgNum = checkOverFlow(i-1);
            break;
        }
    }
    let container = document.getElementById('imgid');
    let container1 = document.getElementById('curImg');
    container.value = images[imgNum];
    container1.value = images[imgNum];
    container.src = images[imgNum];
}

function checkOverFlow(i) {
    if (i > 3) {
        return 0;
    } else if (i < 0) {
        return 3;
    } 
    return i;
}
    var images = new Array(
        "images/bird1.png",
        "images/bird2.png",
        "images/bird3.png",
        "images/bird4.png",
        "images/bird5.png",
        "images/bird6.png",
        "images/bird7.png",
        "images/bird8.png",
        "images/bird9.png",
        "images/bird10.png",
    );
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
    if (i > 9) {
        return 0;
    } else if (i < 0) {
        return 9;
    } 
    return i;
}
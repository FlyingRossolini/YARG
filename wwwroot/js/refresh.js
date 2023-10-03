var time = new Date().getTime();

$(document.body).bind("mousemove keypress", function (e) {
    time = new Date().getTime();
});

function refresh() {
    if (new Date().getTime() - time >= 60000)
        window.location.reload(true);
    else
        setTimeout(refresh, 10000);
}

setTimeout(refresh, 10000);

function doDefault() {
    document.body.style.cursor = 'default';
    setCursor("default");
};

function doHourglass() {
    document.body.style.cursor = 'wait';
    setCursor("wait");
};

function setCursor(cursor) {
    var x = document.querySelectorAll("*");

    for (var i = 0; i < x.length; i++) {
        x[i].style.cursor = cursor;
    }
}
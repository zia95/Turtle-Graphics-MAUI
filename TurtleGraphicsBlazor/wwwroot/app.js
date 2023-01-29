function refresh_canvas() {
    var elm = document.getElementsByTagName("canvas")[0];
    elm.width = elm.offsetWidth;
    elm.height = elm.offsetHeight;
    return [elm.width, elm.height];
}

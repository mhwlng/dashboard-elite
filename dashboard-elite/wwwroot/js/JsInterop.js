
document.onclick = function (evt) {

    evt = evt || window.event;

    DotNet.invokeMethodAsync('dashboard-elite', 'JsClick');

};

document.onpointermove = function (evt) {

    evt = evt || window.event;

    //console.log("aaa");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

document.onpointerleave = function (evt) {

    evt = evt || window.event;

    //console.log("191");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

document.onlostpointercapture = function (evt) { 

    evt = evt || window.event;

    //console.log("555");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

document.onpointercancel = function (evt) {

    evt = evt || window.event;

    //console.log("201");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

document.ongotpointercapture = function (evt) {

    evt = evt || window.event;

    //console.log("a1461");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

document.onpointerout = function (evt) {

    evt = evt || window.event;

    //console.log("a4461");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

document.onpointerover = function (evt) {

    evt = evt || window.event;

    //console.log("a5461");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};

/*
document.onmouseup = function (evt) { // button clicked and released

    evt = evt || window.event;

    //console.log("191");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

}; */


/*
document.onpointerup = function (evt) { // button clicked and released

    evt = evt || window.event;

    console.log("261");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};



document.onpointerleave = function (evt) {

    evt = evt || window.event;

    console.log("221");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};


document.onmouseleave = function (evt) {

    evt = evt || window.event;

    console.log("151");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};


document.ontouchcancel = function (evt) {

    evt = evt || window.event;

    console.log("351");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};
document.ontouchstart = function (evt) {

    evt = evt || window.event;

    console.log("361");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

};


document.ontouchend = function (evt) {

    evt = evt || window.event;

    console.log("461");

    DotNet.invokeMethodAsync('dashboard-elite', 'JsMouseUp');

}; */





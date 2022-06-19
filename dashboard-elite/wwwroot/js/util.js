
export function ScrollTableToTop(className ) {

    var x = document.getElementsByClassName(className)[0];

    var y = x.getElementsByClassName("mud-table-container")[0];

    y.scrollTop = 0;
}

export function ScrollTableToBottom(className) {

    var x = document.getElementsByClassName(className)[0];

    var y = x.getElementsByClassName("mud-table-container")[0];

    y.scrollTop = y.scrollHeight;
}

export function IframeReload() {

    parent.location.reload();

}

export function IframeName() {

    var iFrame = window.frameElement;
    if (iFrame) {
       return iFrame.name;
    }
    return "";
}


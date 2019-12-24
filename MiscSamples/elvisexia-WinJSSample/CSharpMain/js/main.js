// Your code here!
(function () {

    /*
     let dialog = new Windows.UI.Popups.MessageDialog("main");
     dialog.showAsync();
     */
    "use strict"
    var w;
    if (typeof Worker !== "undefined") {
        document.getElementById("status").innerText = "Worker exists"
        var w = new Worker("ms-appx-web:///js/webWorkerDemo.js");
        if (w) {
            document.getElementById("status").innerText = "Worker rolls"
        }
        w.onmessage = function (evt) {
            document.getElementById("status").innerText = "Worker beeps"
            document.getElementById("myContent").innerText = evt.data;
            w.terminate();
        }
    }
    document.getElementById("hello").innerText = "Hello Main!"
    var str = MyRuntimeComponent.Example.getAnswer();
    document.getElementById("answer").innerText = str;
})();
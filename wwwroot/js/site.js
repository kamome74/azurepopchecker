// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var countdown = 10;
var MyTimer;
var retry = false;

function setCountdown(flag) {
    MyTimer = setInterval(alertFunc, 1000);
    retry = flag;
}
function alertFunc() {

    if(!retry)
        var message = "<h3>Will check after " + countdown + " seconds....</h3>";
    else
        var message = "<h3>Waiting again for " + countdown + " seconds....</h3>";

    document.getElementById("countdownMsg").innerHTML = message;

    if (countdown == 0) {
        clearInterval(MyTimer);
        document.getElementById("runQuery").submit();
    }

    countdown--;
}
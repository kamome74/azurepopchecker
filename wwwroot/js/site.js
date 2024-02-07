// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var countdown = 60;
var MyTimer;

function setCountdown() {
    MyTimer = setInterval(alertFunc, 1000);
}
function alertFunc() {
    document.getElementById("countdownMsg").innerHTML = "<h3>Will check after " + countdown + " seconds....</h3>";

    if (countdown == 0) {
        clearInterval(MyTimer);
        document.getElementById("runQuery").submit();
    }

    countdown--;
}
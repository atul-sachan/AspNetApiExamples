"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var btn = document.getElementById("incrementView");
var viewCountSpan = document.getElementById("viewCount");
// create connection
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/view")
    .build();
btn.addEventListener("click", function (evt) {
    // send to hub
    connection.invoke("IncrementServerView");
});
// client events
connection.on("incrementView", function (val) {
    viewCountSpan.innerText = val;
    if (val % 10 === 0)
        connection.off("incrementView");
});
// start the connection
function startSuccess() {
    console.log("Connected.");
    connection.invoke("IncrementServerView");
}
function startFail() {
    console.log("Connection failed.");
}
connection.start().then(startSuccess, startFail);

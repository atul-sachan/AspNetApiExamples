"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var custom_logger_1 = require("./custom-logger");
var counter = document.getElementById("viewCounter");
// create connection
var connection = new signalR.HubConnectionBuilder()
    //.configureLogging(signalR.LogLevel.Trace)
    .configureLogging(new custom_logger_1.CustomLogger())
    .withUrl("/hub/view", {
    transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.ServerSentEvents
})
    .build();
// on view update message from client
connection.on("viewCountUpdate", function (value) {
    counter.innerText = value.toString();
});
// notify server we're watching
function notify() {
    connection.send("notifyWatching");
}
// start the connection
function startSuccess() {
    console.log("Connected.");
    notify();
}
function startFail() {
    console.log("Connection failed.");
}
connection.start().then(startSuccess, startFail);

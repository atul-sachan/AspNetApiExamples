import * as signalR from "@microsoft/signalr";
import { CustomLogger } from "./custom-logger";
import CustomRetryPolicy from "./custom-retry-policy";

var counter = document.getElementById("viewCounter");

// create connection
let connection = new signalR.HubConnectionBuilder()
    //.configureLogging(signalR.LogLevel.Trace)
    .configureLogging(new CustomLogger())
    .withUrl("/hub/view", {
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.ServerSentEvents
    })
    //.withAutomaticReconnect([0,10,30,60, 90]) // 0, 10 etc are time interval in second 
    .withAutomaticReconnect(new CustomRetryPolicy())
    .build();

// on view update message from client
connection.on("viewCountUpdate", (value: number) => {
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

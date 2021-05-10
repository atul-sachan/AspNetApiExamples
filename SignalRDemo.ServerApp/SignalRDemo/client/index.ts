/************************************************************************************/

//import * as signalR from "@microsoft/signalr";
//import { CustomLogger } from "./custom-logger";

//var counter = document.getElementById("viewCounter");

//// create connection
//let connection = new signalR.HubConnectionBuilder()
//    //.configureLogging(signalR.LogLevel.Trace)
//    .configureLogging(new CustomLogger())
//    .withUrl("/hub/view", {
//        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.ServerSentEvents
//    })
//    .build();

//// on view update message from client
//connection.on("viewCountUpdate", (value: number) => {
//    counter.innerText = value.toString();
//});

//// notify server we're watching
//function notify(){
//    connection.send("notifyWatching");
//}

//// start the connection
//function startSuccess(){
//    console.log("Connected.");
//    notify();
//}
//function startFail(){
//    console.log("Connection failed.");
//}

//connection.start().then(startSuccess, startFail);

/************************************************************************************/

//import * as stringTools from "./string-tools";
//let connection = stringTools.connection;


//let btn = document.getElementById("btnGetFullName");
//btn.addEventListener("click", event => {
//    var firstName = (document.getElementById("inputFirstName") as HTMLInputElement).value;
//    var lastName = (document.getElementById("inputLastName") as HTMLInputElement).value;

//    connection.invoke("getFullName", firstName, lastName)
//        .then(val => {
//            alert(val);
//        });
//});

/************************************************************************************/

import * as signalR from "@microsoft/signalr";

let btn = document.getElementById("incrementView");
let viewCountSpan = document.getElementById("viewCount");

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/view")
    .build();

btn.addEventListener("click", function (evt) {
    // send to hub
    connection.invoke("IncrementServerView");
});

// client events
connection.on("incrementView", (val) => {
    viewCountSpan.innerText = val;

    if (val % 10 === 0) connection.off("incrementView");
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

/************************************************************************************/
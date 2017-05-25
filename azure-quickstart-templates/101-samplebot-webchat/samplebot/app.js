'use strict';

var restify = require('restify');
//Include the library botbuilder
var builder = require('botbuilder');

var server = restify.createServer();

// Create console connector and listen to the command prompt/terminal/console for messages 
var connector = new builder.ConsoleConnector().listen();



//Run the server continuously
var port = process.env.port || process.env.PORT || 3978;
server.listen(port, function () {
    console.log('The server is running on ', server.name, server.url)
});
// Create chat connector with the default id and password
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
})

//When the server posts to /api/messages, make the connector listen to it.
server.post('/api/messages', connector.listen())



// Create your bot with a function to receive messages from the user
var bot = new builder.UniversalBot(connector, function (session) {
    //Send a message back to the user via command prompt/terminal/console
    //console.log(session);
    console.log(JSON.stringify(session.message));
    var name = session.message.user.name;
    var message = session.message.text;
    session.send("Hi " + name + " you said " + message);
});

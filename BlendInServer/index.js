"use strict";
const WebSocket = require('ws');
const Lobby = require('./Lobby.js')
const User = require('./User.js')
const StartMsg = require('./StartMsg.js')
const Location = require('./Location.js')
const ErrorMsg = require('./ErrorMsg.js')
const wss = new WebSocket.Server({ port: 8080 });

// TODO maybe redis?
var lobbies = [];

function getLobbyByName(lobbyname) {
    let lobby = null;
    lobbies.forEach(l => {
        if (l.name == lobbyname) {
            lobby = l;
            return;
        }
    });
    return lobby;
}

/**
 * Login a client
 * needs to contain:
 * > message.username (Username)
 * > message.lobby (opt. Lobby Name)
 * @param {WebSocket} client 
 * @param {object} message 
 */
function login(client, message) {
    var event = "login"
    var lobby;
    if (message.lobby == null) {
        lobby = new Lobby();
        lobbies.push(lobby);
    } else {
        lobby = getLobbyByName(message.lobby)
        if (lobby == null) {
            client.send(JSON.stringify(
                new ErrorMsg(event, "Lobby with this name does not exist")
            ))
            return;
        }
    }
    try {
        lobby.addUser(new User(client, message.username))
        lobby.sendToMembersExcept({ event: "join", user: message.username }, [ message.username ])
    } catch(exists) {
        
        client.send(JSON.stringify(
            new ErrorMsg(event, exists)
        ))
        return;
    }    
    console.log("Created new lobby:")
    console.log(lobby)
    client.send(JSON.stringify({
        event: event,
        status: "success",
        lobby_name: lobby.name,
        lobby_size: lobby.users.length
     }))
}

function location(client, message) {
    var event = "location"
    var lobby = getLobbyByName(message.lobby)
    if (lobby == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby with this name does not exist")
        ))
        return;
    }
    var location = new Location(message.latitude, message.longitude);
    
}

function start(client, message) {
    var event = "start"
    var lobby = getLobbyByName(message.lobby)
    if (lobby == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby with this name does not exist")
        ))
        return;
    }
    var hunter = lobby.users[Math.floor(Math.random() * lobby.users.length)];
    hunter.isHunter = true;
    lobby.start(15);
}

wss.on('connection', function connection(ws) {
    // This event fires when any connected socket sends a message
    ws.on('message', function incoming(jsonmessage) {
        try {
            try {
                var message = JSON.parse(jsonmessage);
                console.log('received: %s', message);
            } catch (error) {
                ws.send(JSON.stringify({ error: error }));
            }
            console.log(lobbies)
            switch (message.event) {
                case "login":
                    login(ws, message)
                    break;
                case "start":
                    start(ws, message)
                    break;
                case "location":
                    location(ws, message)
                    break;
                default:
                    ws.send(JSON.stringify({ error: "Undefind event!" }));
                    break;
            }

        } catch (err) {
            console.error(err)
        }
    });

    ws.on('close', function close(code, reason) {
        lobbies.forEach(lobby => {
            lobby.users.forEach(user => {
                if(user.socket == ws) {
                    lobby.sendToMembers({ 
                        event: "leave", 
                        user: user.name
                    })
                    user.connected = false;
                    console.log(user.name + " disconnected")
                    return;
                }
            });
            // remove disconnected users
            lobby.users = lobby.users.filter(function(user, index, arr){
                return user.connected == true;
            });
            // remove empty lobbies
            lobbies = lobbies.filter(function(lobby, index, arr){
                return lobby.users.length > 0
            });
            console.log(lobby)
            console.log(lobbies)
        });
        console.log("[WS] WebSocket disconnected")
    });

    // Send on connected
    ws.send(JSON.stringify({ event: "connected", info: 'You successfully connected!' }));
    console.log("[WS] User connected!")
});
"use strict";
const WebSocket = require('ws');
const Lobby = require('./Lobby.js')
const User = require('./User.js')
const ErrorMsg = require('./ErrorMsg.js')
const wss = new WebSocket.Server({ port: 8080 });

// TODO maybe redis?
var lobbies = [];

function getLobbyByName(lobbyname) {
    let lobby = null;
    lobbies.forEach(l => {
        if (l.name == lobbyname) {
            lobby = l;
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
        getLobbyByName(message.lobby)
        if (lobby == null) {
            client.send(JSON.stringify(
                new ErrorMsg(event, "Lobby with this name does not exist")
            ))
            return;
        }
    }
    try {
        lobby.addUser(new User(client, message.username))
        lobby.sendToMembersExcept({ event: "lobby", lobby: lobby }, [ message.username ])
    } catch(exists) {
        console.log(exists)
        client.send(JSON.stringify(
            new ErrorMsg(event, exists)
        ))
    }
    //reply(ws, JSON.stringify(lobby));       
    client.send(JSON.stringify({
        event: event,
        status: "success",
        lobby_name: lobby.name
     }))
}

wss.on('connection', function connection(ws) {
    ws.on('message', function incoming(jsonmessage) {
        try {
            try {
            var message = JSON.parse(jsonmessage);
                console.log('received: %s', message);
            } catch (error) {
                ws.send(JSON.stringify({ error: error }));
            }
            
            switch (message.event) {
                case "login":
                    login(ws, message)
                    break;

                default:
                    ws.send(JSON.stringify({ error: "Undefind event!" }));
                    break;
            }

        } catch (err) {
            console.error(err)
        }

    });

    ws.send(JSON.stringify({ event: "connected", info: 'You successfully connected!' }));
});
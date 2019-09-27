"use strict";
const WebSocket = require('ws');
const Lobby = require('./Lobby.js')
const User = require('./User.js')
const Location = require('./Location.js')
const ErrorMsg = require('./ErrorMsg.js')
const Tick = require('./Tick.js')
const Calculations = require('./Calculations.js')
const ExposeMsg = require('./ExposeMsg.js')

const wss = new WebSocket.Server({ port: 8080 });

// TODO maybe redis?
var lobbies = [];
var tickrate = .5;

function getLobbyByName(lobbyname) {
    lobbyname = lobbyname.toUpperCase();
    let lobby = null;
    lobbies.forEach(l => {
        if (l.name == lobbyname) {
            lobby = l;
            return;
        }
    });
    return lobby;
}

function getUserInLobbyByName(lobby, username) {
    let foundUser = null;
    lobby.users.forEach(user => {
        if (user.name == username) {
            foundUser = user;
        }
    });
    return foundUser;
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
    var user = getUserInLobbyByName(lobby, message.username)
    if (user == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User with this name does not exist in your lobby")
        ))
        return;
    }
    user.location = new Location(message.latitude, message.longitude);
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
    if (lobby.playing == true) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby is already running")
        ))
        return;
    }
    lobby.start(15);
}

function stun(client, message) {
    var event = "stun"
    var lobby = getLobbyByName(message.lobby)
    if (lobby == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby with this name does not exist")
        ))
        return;
    }
    if (lobby.playing == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby is not playing yet! How the fuck did you???")
        ))
        return;
    }
    var user = getUserInLobbyByName(lobby, message.username)
    if (user == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User with this name does not exist in your lobby")
        ))
        return;
    }
    if (user.isHunter == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User is not hunter and cannot use Stun")
        ))
        return;
    }
    var stunnedPrey = [];
    lobby.users.forEach(prey => {
        if(prey.isHunter == false) {
            var dist = Calculations.distance(user.location, prey.location);
            if (dist < 10) {
                stunnedPrey.push({ user: prey, distance: dist })
            }
        }
    });

    stunnedPrey.forEach(stunned => {
        stunned.user.socket.send(new StunMsg(stunned.distance))
    });
}

function catchPlayer(client, message) {
    var event = "catch"
    var lobby = getLobbyByName(message.lobby)
    if (lobby == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby with this name does not exist")
        ))
        return;
    }
    if (lobby.playing == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby is not playing yet! fukcing hakcre!1!uno")
        ))
        return;
    }
    var user = getUserInLobbyByName(lobby, message.username)
    if (user == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User with this name does not exist in your lobby")
        ))
        return;
    }
    if (user.isHunter == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User is not hunter and cannot use Stun")
        ))
        return;
    }
    
    coughtPlayer = getUserInLobbyByName(lobby, message.cought)
    if (user.isCought == true) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User is already cought")
        ))
        return;
    }
    coughtPlayer.isCought = true;
}

function expose(client, message) {
    var event = "expose"
    var lobby = getLobbyByName(message.lobby)
    if (lobby == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby with this name does not exist")
        ))
        return;
    }
    if (lobby.playing == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby is not playing yet! U l33t haX0r?")
        ))
        return;
    }
    var user = getUserInLobbyByName(lobby, message.username)
    if (user == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User with this name does not exist in your lobby")
        ))
        return;
    }
    if (user.isHunter == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User is not hunter and cannot use Expose")
        ))
        return;
    }
    var exposedPrey = [];
    lobby.users.forEach(prey => {
        if(prey.isHunter == false) {
            var dist = Calculations.distance(user.location, prey.location);
            exposedPrey.push({ user: prey, duration: Math.floor(dist) })
        }
    });

    //expose all the prey for (distance in m) * 1 seconds
    exposedPrey.forEach(exposed => {
        exposed.user.socket.send(new ExposeMsg(exposed.duration))
    });
}

function cloak(client, message) {
    var event = "expose"
    var lobby = getLobbyByName(message.lobby)
    if (lobby == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby with this name does not exist")
        ))
        return;
    }
    if (lobby.playing == false) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "Lobby is not playing yet! U l33t haX0r?")
        ))
        return;
    }
    var user = getUserInLobbyByName(lobby, message.username)
    if (user == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User with this name does not exist in your lobby")
        ))
        return;
    }
    if (user.isHunter == true) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User is hunter and cannot use Cloak")
        ))
        return;
    }
    
    user.isCloaked = true;
    delayUncloak(user)

    // Server ticking
    async function delayUncloak (usr) {
        await timeout(5000)
        usr.isCloaked = false;
    }
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
            // console.log(lobbies)
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
                case "stun":
                    stun(ws, message)
                    break;
                case "expose":
                    expose(ws, message)
                    break;
                case "catch":
                    catchPlayer(ws, message)
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
                    console.log("[INFO] " + user.name + " disconnected from " + lobby.name)
                    return;
                }
            });
            // remove disconnected users
            lobby.users = lobby.users.filter(function(user, index, arr){
                return user.connected == true;
            });
            // remove empty lobbies
            lobbies = lobbies.filter(function(lobby, index, arr){
                if(lobby.users.length == 0) console.log("[INFO] Dropping empty lobby " +  lobby.name)
                return lobby.users.length > 0
            });
        });
        console.log("[WS] WebSocket disconnected")
    });

    // Send on connected
    ws.send(JSON.stringify({ event: "connected", info: 'You successfully connected!' }));
    console.log("[WS] User connected!")
});

// Server ticking
const interval = setInterval(function tick() {
    lobbies.forEach(lobby => {
        if(lobby.playing) {
            console.log("[%s] Lobby tick.", lobby.name)
            var visibleUsers = lobby.users.filter(function(user, index, arr){
                return user.isCloaked == false
            });

            lobby.users.forEach(user => {
                user.socket.send(JSON.stringify(new Tick(visibleUsers)))
            });
        }
    });
  }, 1000 / tickrate);
"use strict";
const WebSocket = require('ws');
const Lobby = require('./Lobby.js')
const User = require('./User.js')
const Location = require('./Location.js')
const ErrorMsg = require('./ErrorMsg.js')
const Tick = require('./Tick.js')
const Calculations = require('./Calculations.js')
const ExposeMsg = require('./ExposeMsg.js')
const StunMsg = require('./StunMsg.js')
const EndMsg = require('./EndMsg.js')
const express = require('express');
const path = require('path');
const { createServer } = require('http');

const port = 8080;

// Observer debug server:

const app = express();
app.use(express.static(path.join(__dirname, '/public')));

const server = createServer(app);
server.listen(port, function() {
    console.log('Listening on http://localhost:8080');
});


console.log(".-. .-\')               (\'-.       .-\') _  _ .-\') _                      .-\') _  \r\n\\  ( OO )            _(  OO)     ( OO ) )( (  OO) )                    ( OO ) ) \r\n ;-----.\\  ,--.     (,------.,--.\/ ,--,\'  \\     .\'_         ,-.-\') ,--.\/ ,--,\'  \r\n | .-.  |  |  |.-\')  |  .---\'|   \\ |  |\\  ,`\'--..._)        |  |OO)|   \\ |  |\\  \r\n | \'-\' \/_) |  | OO ) |  |    |    \\|  | ) |  |  \\  \'        |  |  \\|    \\|  | ) \r\n | .-. `.  |  |`-\' |(|  \'--. |  .     |\/  |  |   \' |        |  |(_\/|  .     |\/  \r\n | |  \\  |(|  \'---.\' |  .--\' |  |\\    |   |  |   \/ :       ,|  |_.\'|  |\\    |   \r\n | \'--\'  \/ |      |  |  `---.|  | \\   |   |  \'--\'  \/      (_|  |   |  | \\   |   \r\n `------\'  `------\'  `------\'`--\'  `--\'   `-------\'         `--\'   `--\'  `--\'   ")

const wss = new WebSocket.Server({ server });

// TODO maybe redis?
var lobbies = [];
var observer = null;

// IMPORTANT CONFIG:
var tickrate = .5;
var cloakTime = 10 * 1000 // in ms
var stunDistance = 12 // in m

const scale = (num, in_min, in_max, out_min, out_max) => {
    return (num - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}

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
    if(message.observe == true) {
        observer = client;
        console.log("[DEBUG] Observer was set!!!");
        
        return;
    }
    var lobby;
    if (message.lobby == null) {
        lobby = new Lobby();
        lobbies.push(lobby);
        console.log("[EVENT] Created new lobby: " +  lobby.name)
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
        if(message.username == "Veit") {
            var north = new User(client, "north");
            north.location = new Location(90.0000, 135.0000)
            lobby.addUser(north)
            console.log(lobby)
        }
    } catch(exists) {
        client.send(JSON.stringify(
            new ErrorMsg(event, exists)
        ))
        console.error(exists)
        return;
    }    
    console.log("[EVENT] " + message.username + " joined lobby: " +  lobby.name + " with " + lobby.users.length + " users: " + lobby.users.join(", "))
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
    console.log("[EVENT] Started lobby " + lobby.name)
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
            
            console.log(dist)
            if (dist < stunDistance) {
                stunnedPrey.push({ user: prey, distance: dist })
            }
        }
    });
    console.log("[EVENT] Hunter used stun; affected prey: " + stunnedPrey.join(", "))
    
    stunnedPrey.forEach(stunned => {
        stunned.user.socket.send(JSON.stringify(new StunMsg(stunned.distance)))
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
            new ErrorMsg(event, "User is not hunter and cannot use Catch")
        ))
        return;
    }
    
    var caughtPlayer = getUserInLobbyByName(lobby, message.caught)
    if (caughtPlayer == null) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User with this name does not exist in your lobby")
        ))
        return;
    }
    if (caughtPlayer.isCaught == true) {
        client.send(JSON.stringify(
            new ErrorMsg(event, "User is already caught")
        ))
        return;
    }
    console.log("[EVENT] " + caughtPlayer.name +  " got cought!")
    caughtPlayer.socket.send(JSON.stringify({ event: event }))
    caughtPlayer.isCaught = true;

    var survivors = 0;
    lobby.users.forEach(user => {
        if(user.isHunter == false && user.isCaught == false) {
            survivors++;
        }
    });
    if(survivors == 0) {
        console.log("[INFO] Winner by killing all : " + "Hunter");

        var endMsg = new EndMsg(lobby.name, "Hunter")
        lobby.sendToMembers(endMsg); 
    }
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
            dist = dist / 100;
            dist = Math.max(1, Math.min(dist, 5))
            console.log("[INFO] Clamped stun duration: " + dist);
            exposedPrey.push({ user: prey, duration: Math.floor(dist) })
        }
    });

    console.log("[EVENT] Expose successfully started")
    console.log(exposedPrey);

    //expose all the prey for (distance in m) * 1 seconds
    exposedPrey.forEach(exposed => {
        exposed.user.socket.send(JSON.stringify(new ExposeMsg(exposed.duration)))
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
    console.log("[EVENT] Cloaked " + user.name)
    user.isCloaked = true;
  
    setTimeout(function() {
        console.log("[EVENT] Uncloaked " + user.name)
        user.isCloaked = false;
    }, cloakTime);
}

wss.on('connection', function connection(ws) {
    // This event fires when any connected socket sends a message
    ws.on('message', function incoming(jsonmessage) {
        try {
            try {
                var message = JSON.parse(jsonmessage);
                // console.log('received: %s', message);
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
                case "cloak":
                    cloak(ws, message)
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
            // console.log("[%s] Lobby tick.", lobby.name)
            var visibleUsers = lobby.users.filter(function(user, index, arr){
                return user.isCloaked == false && user.isCaught == false
            });

            lobby.users.forEach(user => {
                user.socket.send(JSON.stringify(new Tick(visibleUsers)))
            });

            if(observer != null) {
                observer.send(JSON.stringify(new Tick(lobby.users)))
            }
        }
    });
  }, 1000 / tickrate);
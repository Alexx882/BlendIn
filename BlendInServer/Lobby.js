const StartMsg = require('./StartMsg.js')
class Lobby {

    makeid(length) {
        var result           = '';
        var characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        var charactersLength = characters.length;
        for ( var i = 0; i < length; i++ ) {
           result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
     }

    constructor() {
        this._name = this.makeid(5);
        this._users = [];
        this._playing = false;
    }

    get name() {
        return this._name;
    }

    addUser(user) {
        if(this._playing == true) {
            throw "Cannot join a lobby that is already playing..."
        }
        this._users.forEach(currUsr => {
            if(currUsr.name == user.name) {
                console.error(user + " exists in " + this._name)
                throw "User with that name already exists"
            }
        });
        this._users.push(user)
    }

    get users() {
        return this._users;
    }

    set users(users) {
        this._users = users;
    }

    /**
     * Sends a message to all members of this lobby
     * @param {object} message object which will be JSON stringified
     */
    sendToMembers(message) {
        this.sendToMembersExcept(message, [])
    }

    /**
     * 
     * @param {object} message object which will be JSON stringified
     * @param {string[]} ignored names of all ignored users
     */
    sendToMembersExcept(message, ignored) {
        this._users.forEach(usr => {
            if(!ignored.includes(usr.name)) {
                usr.socket.send(JSON.stringify(message))
            }
        });
    }

    toJSON() {
        return {
            name: this._name,
            users: this._users
        }
    }

    /**
     * Starts the game and notifies all members of starting time
     * @param {Integer} secondsToStart 
     */
    start(secondsToStart) {
        this._playing = true;
        var startTime = (new Date).getTime() + secondsToStart * 1000;
        var hunter = this.users[Math.floor(Math.random() * this.users.length)];
        hunter.isHunter = true;
        var startMsg = new StartMsg(this.name, startTime, hunter.name)
        this.sendToMembers(startMsg);
    }

    stop() {
        this._playing = false;
    }
    
}

module.exports = Lobby;
const StartMsg = require('./StartMsg.js')
const EndMsg = require('./EndMsg.js')
class Lobby {

    makeid(length) {
        var result           = '';
        var characters       = 'ACDEFGHIJKLMNOQRSTUVWXYZ';
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
        var hunter = this.users[0] //this.users[Math.floor(Math.random() * this.users.length)];
        hunter.isHunter = true;

        var gamelength = this.users.length * 5 * 60;

        var startMsg = new StartMsg(this.name, startTime, hunter.name, gamelength)
        this.sendToMembers(startMsg);

        var that = this;
        setTimeout(function() { 
            console.log("[EVENT] ("+ that.name + ") Lobby is finished, time is up!");
            // console.log(that.users)
            // console.log(that)
            var winner;
            var survivors = 0;
            that.users.forEach(user => {
                if(user.isHunter == false && user.isCaught == false) {
                    survivors++;
                }
            });
            console.log("[INFO] Game over: "+ survivors + "/" + (that.users.length - 1) + " survived.");
            
            if (survivors > Math.floor((that.users.length-1)/2)) {
                winner = "Prey"
            } else {
                winner = "Hunter"
            }
            console.log("[INFO] Winner : " + winner);

            var endMsg = new EndMsg(that.name, winner)
            that.sendToMembers(endMsg); 

            // that.users.forEach(user => {
            //     user.socket.close();
            // });
        }, gamelength * 1000);
    }

    stop() {
        this._playing = false;
    }

    get playing() {
        return this._playing;
    }

    getHunter() {
        for (let i = 0; i < this.users.length; i++) {
            if(this.users[i].isHunter) return this.users[i];
        }
        return null;
    }
}

module.exports = Lobby;
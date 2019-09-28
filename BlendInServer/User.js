const Location = require('./Location.js')

class User {
    constructor(socket, name) {
        this.socket = socket;
        this.name = name;
        this.isHunter = false;
        this.location = new Location(null, null);
        this.connected = true;
        this.isCaught = false;
        this.isCloaked = false;
    }

    toJSON(){
        return {
            name: this.name,
            lat: this.location.lat,
            long: this.location.long
        };
    }

    toString() {
        return this.name
    }
}
module.exports = User;
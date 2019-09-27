class User {
    constructor(socket, name) {
        this.socket = socket;
        this.name = name;
        this.isHunter = false;
        this.location = null;
    }

    toJSON(){
        return {
            name: this.name,
            location: this.location
        };
    }
}
module.exports = User;
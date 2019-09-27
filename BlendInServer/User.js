class User {
    constructor(socket, name) {
        this.socket = socket;
        this.name = name;
        this.isHunter = false;
    }

    toJSON(){
        return {
            name: this.name,
            //isHunter: this.isHunter
        };
    }
}
module.exports = User;
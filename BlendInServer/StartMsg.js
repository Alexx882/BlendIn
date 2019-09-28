class StartMsg {
    constructor(lobbyname, starttime, hunter_username, gamelength) {
        this.event = "start";
        this.status = "success",
        this.lobbyname = lobbyname;
        this.starttime = starttime;
        this.hunter_username = hunter_username;
        this.gamelength = gamelength;
    }

    toJSON(){
        return {
            event: this.event,
            status: this.status,
            starttime: this.starttime,
            hunter_username: this.hunter_username,
            gamelength: this.gamelength
        };
    }
}
module.exports = StartMsg;
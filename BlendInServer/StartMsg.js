class StartMsg {
    constructor(lobbyname, starttime) {
        this.event = "start";
        this.status = "success",
        this.lobbyname = lobbyname;
        this.starttime = starttime;
    }

    toJSON(){
        return {
            event: this.event,
            status: this.status,
            starttime: this.starttime
        };
    }
}
module.exports = StartMsg;
class StartMsg {
    constructor(lobbyname, winner) {
        this.event = "end";
        this.status = "success",
        this.lobbyname = lobbyname;
        this.winner = winner;
    }
}
module.exports = StartMsg;
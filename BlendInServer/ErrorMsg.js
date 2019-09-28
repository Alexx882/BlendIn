class ErrorMsg {
    constructor(event, error) {
        console.error("[ERROR] in " + event + " : " + error)
        this.event = event;
        this.status = "error",
        this.error = error;
    }
}

module.exports = ErrorMsg;
class ErrorMsg {
    constructor(event, error) {
        this.event = event;
        this.status = "error",
        this.error = error;
    }

    // serialize() {
    //     return JSON.stringify(this);
    // }
}

module.exports = ErrorMsg;
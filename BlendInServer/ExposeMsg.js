class ExposeMsg {
    constructor(duration) {
        this.event = "expose";
        this.status = "success";
        this.duration = duration;
    }
}
module.exports = ExposeMsg;
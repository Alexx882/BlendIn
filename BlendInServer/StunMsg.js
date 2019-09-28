class StunMsg {
    constructor(distance) {
        this.event = "stun";
        this.status = "success";
        this.distance = Math.floor(distance);
    }
}
module.exports = StunMsg;
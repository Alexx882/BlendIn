class StunMsg {
    constructor(distance) {
        this.event = "stun";
        this.status = "success";
        this.distance = distance;
    }
}
module.exports = StunMsg;
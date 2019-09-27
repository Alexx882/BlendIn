class StunMsg {
    constructor(distance) {
        this.event = "stun";
        this.status = "success";
        this.distance = distance;
    }

    // toJSON(){
    //     return {
    //         event: this.event,
    //         status: this.status,
    //         starttime: this.starttime,
    //         hunter_username: this.hunter_username
    //     };
    // }
}
module.exports = StunMsg;
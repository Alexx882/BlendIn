class Calculations {
    /**
     * Calculates the distance between two locations
     * @param {double} lat1
     * @param {double} lon1
     * @param {double} lat2
     * @param {double} lon2
     */
    static distance(lat1, lon1, lat2, lon2) {
        var R = 6371e3; // metres
        var φ1 = lat1.toRadians();
        var φ2 = lat2.toRadians();
        var Δφ = (lat2-lat1).toRadians();
        var Δλ = (lon2-lon1).toRadians();

        var a = Math.sin(Δφ/2) * Math.sin(Δφ/2) +
            Math.cos(φ1) * Math.cos(φ2) *
            Math.sin(Δλ/2) * Math.sin(Δλ/2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));

        var d = R * c;
        return d;
    }

    static distance(loc1, loc2) {
        return this.distance(loc1.lat, loc1.long, loc2.lat, loc2.long)
    }
}
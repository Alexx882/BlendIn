class Calculations {
    
    /**
     * Calculates the distance between two locations
     * @param {double} lat1
     * @param {double} lon1
     * @param {double} lat2
     * @param {double} lon2
     */
    static distanceLatLong(lat1, lon1, lat2, lon2) {
        function degrees_to_radians(degrees)
        {
            var pi = Math.PI;
            return degrees * (pi/180);
        }
        var R = 6371e3; // metres
        var φ1 = degrees_to_radians(lat1);
        var φ2 = degrees_to_radians(lat2);
        var Δφ = degrees_to_radians(lat2-lat1);
        var Δλ = degrees_to_radians(lon2-lon1);

        var a = Math.sin(Δφ/2) * Math.sin(Δφ/2) +
            Math.cos(φ1) * Math.cos(φ2) *
            Math.sin(Δλ/2) * Math.sin(Δλ/2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));

        var d = R * c;
        return d;
    }

    static distance(loc1, loc2) {
        if(loc1 == null || loc2 == null || loc1.lat == null || loc1.long == null || 
            loc2.lat == null || loc2.long == null) {
            console.error("invalid distance")
            return Infinity;
        } 
        return this.distanceLatLong(loc1.lat, loc1.long, loc2.lat, loc2.long)
    }
} 
module.exports = Calculations;
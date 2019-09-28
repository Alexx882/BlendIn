var mymap = L.map('mapid').setView([51.505, -0.09], 13);
var data = null;
var markerLayer = L.layerGroup().addTo(mymap);
var markers = [];

L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
    maxZoom: 18,
    id: 'mapbox.streets',
    accessToken: 'pk.eyJ1IjoidzNkMyIsImEiOiJjazEzbXd1YmkwYTk4M2lxamF0cWM5OTRnIn0.LiiKjNkzvWQBCLWBkDtqhQ'
}).addTo(mymap);



// var circle = L.circle([51.508, -0.11], {
// 	color: 'red',
// 	fillColor: '#f03',
// 	fillOpacity: 0.5,
// 	radius: 500
// }).addTo(mymap);
function start() {
    
    
    var url = document.getElementById("url").value;
    console.log("connecting to " + url);
    var server = new WebSocket("ws://"+ url +"/");
    server.onopen = function (event) {
        server.send(JSON.stringify({ event: "login", observe: true }));
    };

    server.onmessage = function (event) {
        data = JSON.parse(event.data);
        
        if (data.userlist == null) return;

        markers = [];
        markerLayer.clearLayers();

        data.userlist.forEach(user => {
            console.log(user);


            //mymap.setView([user.lat, user.long], 13);
            // var circle = L.circle([user.lat,user.long], {
            //     color: 'red',
            //     fillColor: '#f03',
            //     fillOpacity: 0.5,
            //     radius: 10
            // }).addTo(markerLayer);

            var marker = L.marker([user.lat, user.long]).addTo(markerLayer);
            marker.bindPopup("Username: " + user.name);
            markers.push(marker);
        });
    }
}

function center() {
    var group = new L.featureGroup(markers);

    mymap.fitBounds(group.getBounds());
    // if(data != null && data.userlist != null) {
    //     user = data.userlist[0];
    //     mymap.setView([user.lat, user.long], 20);
    // } 
}

// mymap.locate({
//     watch: true,
//     setView: true,
//     maxZoom: 16
// }).on('locationfound', (e) => {
//     if (!this.marker) {
//         this.marker = leaflet.marker([e.latitude, e.longitude], { icon: carIcon }).addTo(this.map);
//     } else {
//         this.marker.setLatLng([e.latitude, e.longitude]);
//     }
// });
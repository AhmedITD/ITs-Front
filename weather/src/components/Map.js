import { useEffect, useRef } from 'react';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import './Map.css';

// Fix for default marker icons in Leaflet
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
  iconUrl: require('leaflet/dist/images/marker-icon.png'),
  shadowUrl: require('leaflet/dist/images/marker-shadow.png'),
});

function Map({ onLocationSelect }) {
  const mapContainer = useRef(null);
  const map = useRef(null);
  const markerRef = useRef(null);

  useEffect(() => {
    if (!mapContainer.current) return;

    // Initialize map centered on Baghdad
    map.current = L.map(mapContainer.current).setView([33.3157, 44.3615], 10);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors',
      maxZoom: 19,
    }).addTo(map.current);

    // Handle map clicks
    map.current.on('click', (e) => {
      const { lat, lng } = e.latlng;
      onLocationSelect(lat, lng);

      // Remove previous marker
      if (markerRef.current) {
        map.current.removeLayer(markerRef.current);
      }

      // Add new marker
      markerRef.current = L.marker([lat, lng])
        .bindPopup(`<b>Selected Location</b><br>Lat: ${lat.toFixed(4)}<br>Lng: ${lng.toFixed(4)}`)
        .addTo(map.current)
        .openPopup();
    });

    return () => {
      if (map.current) {
        map.current.remove();
      }
    };
  }, [onLocationSelect]);

  return (
    <div className="map-container">
      <div ref={mapContainer} className="map" id="map"></div>
      <div className="map-hint">📍 Click on any location to see the weather</div>
    </div>
  );
}

export default Map;

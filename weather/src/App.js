import { useState } from 'react';
import Map from './components/Map';
import Weather from './components/Weather';
import './App.css';

function App() {
  const [latitude, setLatitude] = useState(null);
  const [longitude, setLongitude] = useState(null);

  const handleLocationSelect = (lat, lng) => {
    setLatitude(lat);
    setLongitude(lng);
  };

  return (
    <div className="App">
      <header className="app-header">
        <h1>🌍 Weather Map</h1>
        <p>Click on any location to view the weather</p>
      </header>

      <div className="app-content">
        <div className="map-section">
          <Map onLocationSelect={handleLocationSelect} />
        </div>
        <div className="weather-section">
          <Weather latitude={latitude} longitude={longitude} />
        </div>
      </div>

      <footer className="app-footer">
        <p>Weather data provided by <a href="https://open-meteo.com/" target="_blank" rel="noreferrer">Open-Meteo</a></p>
        <p>Map data © <a href="https://www.openstreetmap.org/copyright" target="_blank" rel="noreferrer">OpenStreetMap</a></p>
      </footer>
    </div>
  );
}

export default App;

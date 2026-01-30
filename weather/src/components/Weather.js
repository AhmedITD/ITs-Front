import React, { useEffect, useState } from 'react';
import './Weather.css';

const getWeatherEmoji = (code) => {
  if (code === 0) return '☀️';
  if (code === 1 || code === 2) return '🌤️';
  if (code === 3) return '☁️';
  if (code === 45 || code === 48) return '🌫️';
  if (code === 51 || code === 53 || code === 55) return '🌧️';
  if (code === 61 || code === 63 || code === 65) return '🌧️';
  if (code === 71 || code === 73 || code === 75) return '❄️';
  if (code === 80 || code === 81 || code === 82) return '⛈️';
  if (code === 85 || code === 86) return '❄️';
  if (code === 95 || code === 96 || code === 99) return '⛈️';
  return '🌍';
};

const getWindDirection = (degrees) => {
  const directions = ['N', 'NNE', 'NE', 'ENE', 'E', 'ESE', 'SE', 'SSE',
                      'S', 'SSW', 'SW', 'WSW', 'W', 'WNW', 'NW', 'NNW'];
  const index = Math.round(degrees / 22.5) % 16;
  return directions[index];
};

const Weather = ({ latitude, longitude }) => {
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (latitude === null || longitude === null) {
      setLoading(false);
      return;
    }

    const fetchWeather = async () => {
      setLoading(true);
      try {
        const url = `https://api.open-meteo.com/v1/forecast?latitude=${latitude}&longitude=${longitude}&current_weather=true&hourly=temperature_2m,relative_humidity_2m,weather_code&timezone=auto`;
        const response = await fetch(url);
        const data = await response.json();
        setWeather(data);
        setError(null);
      } catch (err) {
        setError('Failed to fetch weather data');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchWeather();
  }, [latitude, longitude]);

  if (!latitude || !longitude) {
    return (
      <div className="weather-container">
        <div className="weather-placeholder">
          <div className="placeholder-emoji">📍</div>
          <p>Click on the map to select a location</p>
        </div>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="weather-container">
        <div className="weather-placeholder">
          <div className="spinner"></div>
          <p>Loading weather...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="weather-container">
        <div className="weather-placeholder error">
          <p>⚠️ {error}</p>
        </div>
      </div>
    );
  }

  if (!weather || !weather.current_weather) {
    return (
      <div className="weather-container">
        <div className="weather-placeholder">
          <p>No data available</p>
        </div>
      </div>
    );
  }

  const current = weather.current_weather;
  const emoji = getWeatherEmoji(current.weathercode);
  const windDir = getWindDirection(current.winddirection);
  const time = new Date(current.time).toLocaleTimeString();

  return (
    <div className="weather-container">
      <div className="weather-header">
        <div className="location-coords">
          <span>📍 {latitude.toFixed(2)}°, {longitude.toFixed(2)}°</span>
        </div>
        <div className="current-time">{time}</div>
      </div>

      <div className="weather-main">
        <div className="weather-icon">{emoji}</div>
        <div className="weather-info">
          <div className="temperature">
            <span className="temp-value">{Math.round(current.temperature)}°</span>
            <span className="temp-unit">C</span>
          </div>
          <div className="weather-code">Code: {current.weathercode}</div>
        </div>
      </div>

      <div className="weather-details">
        <div className="detail-item">
          <span className="detail-label">💨 Wind</span>
          <span className="detail-value">
            {Math.round(current.windspeed)} km/h {windDir}
          </span>
        </div>
        <div className="detail-item">
          <span className="detail-label">🌡️ Feels Like</span>
          <span className="detail-value">
            {Math.round(current.temperature - (current.windspeed / 10))}°C
          </span>
        </div>
      </div>

      <div className="weather-footer">
        <p>Click map to select another location</p>
      </div>
    </div>
  );
};

export default Weather;

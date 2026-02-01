# Rent-A-Ride Mini App

A Svelte-based mini app for RentARide that runs inside SuperQi, using hylid-bridge for authentication (`my.getAuthCode`) and payment (`my.tradePay`).

## Setup

1. Copy `.env.example` to `.env` and set `VITE_API_URL` to your RentARide API base URL (e.g. `http://localhost:5022`).
2. Run `npm install` and `npm run build`.
3. Deploy the `dist` folder to your mini app host (or use `npm run dev` for development).

## Backend Configuration

The RentARide API must have the `auth-with-superQi` endpoint and SuperQi configuration:

- **Development**: Set `SuperQi:UseDemoUser: true` in `appsettings.Development.json` to use a demo user without external validation.
- **Production**: Configure `SuperQi:ValidationUrl` to your SuperQi auth validation service, or set `UseDemoUser` for testing.

## Features

- **Browse vehicles**: Filter by type, paginate
- **Book a car**: Select vehicle, dates, amenities; create invoice and pay via `my.tradePay`
- **My rentals**: View rental history (requires SuperQi login)

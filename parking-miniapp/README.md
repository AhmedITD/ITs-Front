# Parking Payment Mini App

A Svelte-based parking payment mini app for the Qineo/SuperQi platform (Iraq). Park owners (Admin) can charge car owners via QR codes, and car owners can scan and pay.

## Features

- **Park Owner (Admin)**: Create charge sessions, display QR codes, view session history
- **Car Owner (User)**: Scan QR to pay, view payment history
- **Qineo API adapter**: getAuthCode, tradePay, device.scan with mock fallback for local development

## Tech Stack

- Svelte 4 + Vite 5
- svelte-spa-router for routing
- qrcode for QR generation
- localStorage for session/payment persistence (no backend required for MVP)

## Development

```bash
npm install --legacy-peer-deps
npm run dev
```

Open http://localhost:5173

## Build

```bash
npm run build
```

Output is in `dist/` – deploy as static assets to your mini app host.

## Backend Integration

- **Auth**: `POST /api/auth-with-superQi` with body `{ "token": "<SuperQi auth code>" }`
- **Payment**: `POST /api/payment` with `Authorization: <token>` and body `{ orderId, amount, subject }`. Returns `{ url }` for `my.tradePay({ paymentUrl: url })`.

Configure `VITE_API_URL` in `.env` (default: `https://its.mouamle.space`).

**SuperQi / Qineo (`my` global):**

| Adapter function | SuperQi API |
|------------------|-------------|
| `getAuthCode()`  | `my.getAuthCode` → authCode/token |
| `tradePay()`     | Backend `/api/payment` → `my.tradePay({ paymentUrl })` |
| `scan()`         | `my.scan` or `my.device.scan` |

**IDs** (in `src/lib/api/config.js`): Client ID, Merchant ID.

## Project Structure

```
src/
├── lib/
│   ├── qineo/        # Qineo API adapter + mock
│   ├── qr/           # QR encode/decode utilities
│   └── stores/       # auth, session, payments
├── routes/           # Page components
├── components/       # QRDisplay, RoleSwitcher, PaymentForm
├── App.svelte
└── main.js
```

## Flow

1. **Admin**: Log in → Create session (amount IQD) → Show QR
2. **User**: Log in → Scan QR → Confirm and pay (tradePay)
3. Admin sees session marked paid; User sees payment in history

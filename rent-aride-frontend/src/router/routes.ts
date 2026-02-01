import type { RouteRecordRaw } from 'vue-router'
import { requireAuth, requireGuest, requireAdmin } from './guards'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'home',
    component: () => import('@/pages/HomePage.vue'),
    meta: { title: 'Browse Vehicles' },
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/pages/LoginPage.vue'),
    meta: { title: 'Login', guest: true },
    beforeEnter: requireGuest,
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('@/pages/RegisterPage.vue'),
    meta: { title: 'Register', guest: true },
    beforeEnter: requireGuest,
  },
  {
    path: '/rentals/new',
    name: 'rental-new',
    component: () => import('@/pages/CreateRentalPage.vue'),
    meta: { title: 'Book a Car', auth: true },
    beforeEnter: requireAuth,
  },
  {
    path: '/rentals',
    name: 'rentals',
    component: () => import('@/pages/MyRentalsPage.vue'),
    meta: { title: 'My Rentals', auth: true },
    beforeEnter: requireAuth,
  },
  {
    path: '/payment/finish',
    name: 'payment-finish',
    component: () => import('@/pages/PaymentFinishPage.vue'),
    meta: { title: 'Payment complete', auth: true },
    beforeEnter: requireAuth,
  },
  {
    path: '/admin/vehicle-types',
    name: 'admin-vehicle-types',
    component: () => import('@/pages/admin/VehicleTypesPage.vue'),
    meta: { title: 'Vehicle Types', admin: true },
    beforeEnter: [requireAuth, requireAdmin],
  },
  {
    path: '/admin/amenities',
    name: 'admin-amenities',
    component: () => import('@/pages/admin/AmenitiesPage.vue'),
    meta: { title: 'Amenities', admin: true },
    beforeEnter: [requireAuth, requireAdmin],
  },
  {
    path: '/admin/vehicles',
    name: 'admin-vehicles',
    component: () => import('@/pages/admin/VehiclesPage.vue'),
    meta: { title: 'Vehicles', admin: true },
    beforeEnter: [requireAuth, requireAdmin],
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('@/pages/NotFoundPage.vue'),
    meta: { title: 'Not Found' },
  },
]

export default routes

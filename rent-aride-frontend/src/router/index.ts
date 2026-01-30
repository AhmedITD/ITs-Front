import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'home', component: () => import('@/views/HomeView.vue'), meta: { title: 'Browse Vehicles' } },
    { path: '/login', name: 'login', component: () => import('@/views/LoginView.vue'), meta: { title: 'Login', guest: true } },
    { path: '/register', name: 'register', component: () => import('@/views/RegisterView.vue'), meta: { title: 'Register', guest: true } },
    { path: '/rentals/new', name: 'rental-new', component: () => import('@/views/CreateRentalView.vue'), meta: { title: 'Book a Car', auth: true } },
    { path: '/rentals', name: 'rentals', component: () => import('@/views/MyRentalsView.vue'), meta: { title: 'My Rentals', auth: true } },
    { path: '/admin/vehicle-types', name: 'admin-vehicle-types', component: () => import('@/views/admin/VehicleTypesView.vue'), meta: { title: 'Vehicle Types', admin: true } },
    { path: '/admin/amenities', name: 'admin-amenities', component: () => import('@/views/admin/AmenitiesView.vue'), meta: { title: 'Amenities', admin: true } },
    { path: '/admin/vehicles', name: 'admin-vehicles', component: () => import('@/views/admin/VehiclesView.vue'), meta: { title: 'Vehicles', admin: true } },
  ],
})

router.beforeEach(async (to, _from, next) => {
  const auth = useAuthStore()
  if (auth.token && !auth.user) await auth.fetchUser()

  if (to.meta.auth && !auth.isLoggedIn) return next({ name: 'login', query: { redirect: to.fullPath } })
  if (to.meta.guest && auth.isLoggedIn) return next({ name: 'home' })
  if (to.meta.admin && !auth.isAdmin) return next({ name: 'home' })

  next()
})

export default router

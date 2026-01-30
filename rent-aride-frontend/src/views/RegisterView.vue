<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const firstName = ref('')
const lastName = ref('')
const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

async function onSubmit() {
  error.value = ''
  loading.value = true
  try {
    await auth.register(firstName.value, lastName.value, email.value, password.value)
    await router.push('/')
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Registration failed'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="auth-page">
    <h1>Register</h1>
    <form class="auth-form" @submit.prevent="onSubmit">
      <div v-if="error" class="error">{{ error }}</div>
      <div class="row">
        <div class="field">
          <label for="firstName">First name</label>
          <input id="firstName" v-model="firstName" type="text" required autocomplete="given-name" />
        </div>
        <div class="field">
          <label for="lastName">Last name</label>
          <input id="lastName" v-model="lastName" type="text" required autocomplete="family-name" />
        </div>
      </div>
      <div class="field">
        <label for="email">Email</label>
        <input id="email" v-model="email" type="email" required autocomplete="email" />
      </div>
      <div class="field">
        <label for="password">Password</label>
        <input id="password" v-model="password" type="password" required autocomplete="new-password" />
      </div>
      <button type="submit" class="btn btn-primary" :disabled="loading">
        {{ loading ? 'Creating account…' : 'Create account' }}
      </button>
    </form>
    <p class="foot">
      Already have an account? <router-link to="/login">Login</router-link>
    </p>
  </div>
</template>

<style scoped>
.auth-page {
  max-width: 360px;
  margin: 2rem auto;
}
.auth-page h1 {
  margin-bottom: 1.5rem;
  font-size: 1.5rem;
}
.auth-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
.row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}
.error {
  padding: 0.5rem 0.75rem;
  background: #fef2f2;
  color: #b91c1c;
  border-radius: 6px;
  font-size: 0.9rem;
}
.field label {
  display: block;
  margin-bottom: 0.25rem;
  font-size: 0.9rem;
  font-weight: 500;
}
.field input {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--color-border);
  border-radius: 6px;
  font-size: 1rem;
}
.btn-primary {
  margin-top: 0.5rem;
  padding: 0.6rem 1rem;
  background: var(--vt-c-indigo);
  color: white;
  border: none;
  cursor: pointer;
}
.btn-primary:hover:not(:disabled) {
  opacity: 0.9;
}
.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.foot {
  margin-top: 1.5rem;
  font-size: 0.9rem;
  color: var(--color-text);
}
.foot a {
  color: var(--vt-c-indigo);
}
</style>

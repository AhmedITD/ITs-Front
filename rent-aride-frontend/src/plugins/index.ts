import type { App } from 'vue'
import piniaPlugin from './pinia'
import routerPlugin from '../router'

export default {
  install(app: App) {
    app.use(piniaPlugin)
    app.use(routerPlugin)
  },
}

const store = new Vuex.createStore({
    state() {
        return {
            count: 1
        }
    },
    mutations: {
        increment (state) {
          state.count++;
        }
    },
    actions: {
        increment ({ commit }) {
            commit('increment')
        }
    }
})
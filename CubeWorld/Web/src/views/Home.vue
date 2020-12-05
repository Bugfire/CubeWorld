<template>
  <div class="home">
    <div>
      <router-link to="/game">
        <img class="logo" src="@/assets/title.png" alt="Cube World" />
      </router-link>
      <router-link to="/license">
        License
      </router-link>
    </div>
  </div>
</template>

<style>
.home {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  min-height: 70vh;
}

.logo {
  width: 95%;
}
</style>

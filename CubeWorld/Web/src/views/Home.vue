<template>
  <div class="home">
    <div>
      <router-link to="/game">
        <img class="logo" src="@/assets/title.png" alt="Cube World" />
      </router-link>
      <ul class="list">
        <li>
          <router-link to="/license">License</router-link>
        </li>
        <li>
          このアプリは<a
            href="https://qiita.com/advent-calendar/2020/crowdworks"
            >クラウドワークスアドベントカレンダー2020</a
          >
          の記事
          <a href="https://qiita.com/bugfire/items/cddf34504b80b2b525ba">
            Unity と Vue3 と GitHub Pages と GitHub Actions のお話</a
          >
          のために作成されました。
        </li>
        <li>
          ゲーム部分の制作元のレポジトリはこちらです。<br />
          <a href="https://github.com/federicodangelo/CubeWorld"
            >GitHub federicodangelo/CubeWorld</a
          ><br />とても素晴らしいです。
        </li>
        <li>
          今回の改造実装のレポジトリはこちらです。<br />
          <a href="https://github.com/Bugfire/CubeWorld"
            >Github Bugfire/CubeWorld</a
          >
        </li>
        <li>
          ネットワークのありがたいサービスはこちらの無料プランを使わさせていただきました。<br />
          <a href="https://www.photonengine.com/ja/photon">Photon REALTIME</a><br />
          楽に実装できました。
        </li>
      </ul>
    </div>
  </div>
</template>

<style>
.home {
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

.list li {
  margin: 1rem;
  text-align: left;
}
</style>

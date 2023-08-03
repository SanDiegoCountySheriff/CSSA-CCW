<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div class="loader">
    <img
      src="/img/icons/agency_logo.svg"
      alt="loader"
      width="494"
      height="196"
    />
    <h1>
      <span class="let1">C</span>
      <span class="let2">O</span>
      <span class="let3">N</span>
      <span class="let4">C</span>
      <span class="let5">E</span>
      <span class="let6">A</span>
      <span class="let7">L</span>
      <span class="let8">E</span>
      <span class="let9">D</span>&nbsp;
      <span class="let10">C</span>
      <span class="let11">A</span>
      <span class="let12">R</span>
      <span class="let13">R</span>
      <span class="let14">Y</span>&nbsp;
      <span class="let15">W</span>
      <span class="let16">E</span>
      <span class="let17">A</span>
      <span class="let18">P</span>
      <span class="let19">O</span>
      <span class="let20">N</span>
    </h1>
  </div>
</template>
<style lang="scss" scoped>
.loader {
  position: absolute;
  top: 40%;
  left: 40%;
  margin: 0;
  transform: translateY(-50%);

  h1 {
    padding: 0;
    margin: 0;
    color: #1d2939;
    font-family: 'Work Sans', sans-serif;
    font-size: 1.1em;
    font-weight: 400;
  }

  span {
    position: relative;
    top: 0.63em;
    display: inline-block;
    opacity: 0;
    text-transform: uppercase;
    transform: rotateX(-90deg);
  }

  .let1 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.2s;
  }

  .let2 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.3s;
  }

  .let3 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.4s;
  }

  .let4 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.5s;
  }

  .let5 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.6s;
  }

  .let6 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.7s;
  }

  .let7 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.8s;
  }

  .let8 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 1.9s;
  }

  .let9 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2s;
  }

  .let10 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.1s;
  }

  .let11 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.2s;
  }

  .let12 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.3s;
  }

  .let13 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.4s;
  }

  .let14 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.5s;
  }

  .let15 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.6s;
  }

  .let16 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.7s;
  }

  .let17 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.8s;
  }

  .let18 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 2.9s;
  }

  .let19 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 3s;
  }

  .let20 {
    animation: drop 3.2s ease-in-out infinite;
    animation-delay: 3.1s;
  }

  @keyframes drop {
    10% {
      opacity: 0.5;
    }

    20% {
      top: 3.78em;
      opacity: 1;
      transform: rotateX(-360deg);
    }

    80% {
      top: 3.78em;
      opacity: 1;
      transform: rotateX(-360deg);
    }

    90% {
      opacity: 0.5;
    }

    100% {
      top: 6.94em;
      opacity: 0;
    }
  }
}
</style>

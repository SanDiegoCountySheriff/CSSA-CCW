// eslint-disable-next-line node/no-extraneous-import
import { Workbox } from 'workbox-window';

let wb;

if ('serviceWorker' in navigator) {
  wb = new Workbox(`${process.env.BASE_URL}service-worker.js`);
  wb.addEventListener('controlling', () => {
    window.location.reload();
  });
  wb.addEventListener('fetch', () => {
    // it can be empty if you just want to get rid of that error
  });
  wb.register();
} else {
  wb = null;
}

export default wb;

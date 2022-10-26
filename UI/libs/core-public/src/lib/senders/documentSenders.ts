import axios from 'axios';

export async function sendPostImage(doc: any, url: string) {
  const res = await axios
    .post(`${url}uploadImage`, doc)
    .catch(err => console.warn(err));

  return res?.data;
}

export async function sendPostFile(doc: any, url: string) {
  const res = await axios
    .post(`${url}/uploadFile`, doc)
    .catch(err => console.warn(err));

  return res?.data;
}

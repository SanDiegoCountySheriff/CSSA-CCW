import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';

export async function sendPostImage(doc: any) {
  const res = await axios
    .post(Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT, doc)
    .catch(err => console.warn(err));

  return res?.data;
}

export async function sendPostFile(doc: any) {
  const res = await axios
    .post(Endpoints.POST_DOCUMENT_FILE_ENDPOINT, doc)
    .catch(err => console.warn(err));

  return res?.data;
}

import { useDocumentsStore } from '@core-admin/stores/documentsStore'

const documentStore = useDocumentsStore()

export async function openPdf(item) {
  documentStore
    .getUserDocument(item.name)
    .then(response => {
      if (response.type === 'application/pdf') {
        const pdfBlob = new Blob([response], { type: 'application/pdf' })
        // eslint-disable-next-line node/no-unsupported-features/node-builtins
        const pdfUrl = URL.createObjectURL(pdfBlob)
        const newWindow = window.open(pdfUrl, '_blank')

        if (newWindow) {
          // eslint-disable-next-line node/no-unsupported-features/node-builtins
          URL.revokeObjectURL(pdfUrl)
        } else {
          alert(
            'The PDF could not be opened in a new window. Please check your pop-up blocker settings.'
          )
        }
      } else if (response.type === 'text/plain') {
        response.text().then(base64String => {
          fetch(base64String)
            .then(res => res.blob())
            .then(blob => {
              // eslint-disable-next-line node/no-unsupported-features/node-builtins
              const imgUrl = URL.createObjectURL(blob)

              window.open(imgUrl, '_blank')
            })
        })
      }
    })
    .catch(error => {
      console.error('Error fetching the PDF:', error)
    })
}

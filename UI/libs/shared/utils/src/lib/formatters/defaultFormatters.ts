import { parse } from 'date-fns'
/**
 * Function to format the entered social security number into a more readable version.
 * Expects a validated form entry.
 * @param {string} ssn
 * @returns {string}
 */
export function formatSSN(ssn: string): string {
  const part1 = ssn.slice(0, 3)
  const part2 = ssn.slice(3, 5)
  const part3 = ssn.slice(5)

  return `${part1}-${part2}-${part3}`
}

/**
 * Function to format the entered phone number into a more readable version. Expects a validated form entry.
 * @param {string} number
 * @returns {string}
 */
export function formatPhoneNumber(number: string): string {
  if (number) {
    const areaCode = number.slice(0, 3)
    const first = number.slice(3, 6)
    const last = number.slice(6)

    return `(${areaCode}) ${first}-${last}`
  }

  return ''
}

export function unformatNumber(number: string): string {
  const arr = number.split('')

  for (let i = 0; i < arr.length; i++) {
    if (!/\d/.test(arr[i]) || arr[i] === ' ') {
      arr.splice(i, 1)
    }
  }

  return arr.join('')
}

/**
 * Function to format UTC DateString to Date string (MM/DD/YY)
 * @param {string} dateStr
 * @returns {string}
 */
export function formatDate(dateStr: string): string {
  return (
    new Date(dateStr).toLocaleDateString('en-US', {
      year: '2-digit',
      month: '2-digit',
      day: '2-digit',
    }) || dateStr
  )
}

/**
 * Function to format UTC DateString to Time string (HH:MM AM/PM)
 * @param {string} dateStr
 * @returns {string}
 */
export function formatTime(dateStr: string): string {
  return (
    new Date(dateStr).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
    }) || dateStr
  )
}

/**
 * Function to format local time to UTC time string
 * @param {string} time
 * @returns {string} utcTimeString
 */
export function formatLocalTimeStringToUtcTimeString(time: string): string {
  return parse(time, 'HH:mm', new Date()).toISOString()
}

export function formatUtcTimeStringToLocalTimeString(time: Date): string {
  window.console.log('time in formatter', time)

  return parse(time.toISOString(), 'HH:mm', new Date()).toLocaleTimeString()
}

/**
 * Function to format local date and time into UTC dateTime string
 * @param {string} date
 * @returns {string} parsedDate
 */
export function formatLocalDateAndTimeStringToUtcDateTime(
  date: string,
  time: string
): string {
  const [year, month, day] = date.split('-')
  const [hours, minutes] = time.split(':')

  return new Date(
    parseInt(year),
    parseInt(month) - 1,
    parseInt(day),
    parseInt(hours),
    parseInt(minutes)
  ).toISOString()
}

/**
 * Function to format persons's first name and last ame
 * @param {object} data
 * @returns {string}
 */
export function formatFullName(data): string {
  return (
    `${data?.application?.personalInfo?.lastName}, ${data?.application?.personalInfo?.firstName}` ||
    ''
  )
}

/**
 * Function to format persons's name
 * @param {object} data
 * @returns {string}
 */
export function formatName(data): string {
  const { firstName, lastName } = data || {}

  return `${lastName}, ${firstName}` || ''
}

/**
 * Function to format persons's address
 * @param {object} data
 * @returns {string}
 */
export function formatAddress(data): string {
  const {
    currentAddress: { streetAddress, city, state, country, zip },
  } = data || {}

  return streetAddress && city && state && country && zip
    ? `${streetAddress}, ${city}, ${state},
        ${country}, ${zip}
      `
    : ''
}

/**
 * Function to format persons initials
 * @param {object} data
 * @returns {string}
 */
export function formatInitials(firstName, lastName): string {
  const initials = `${firstName?.charAt(0) || ''}${lastName?.charAt(0) || ''}`

  return initials?.toUpperCase() || ''
}

export function formatInitialsFromEmail(email: string): string {
  const names = email
  const initials = names[0][0] + names[1][0]

  return initials?.toUpperCase() || ''
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const capitalize = ([firstLetter, ...restOfWord]: any) =>
  (firstLetter?.toUpperCase() || '') + restOfWord.join('')

/**
 * Function to format the entered social security number into a more readable version.
 * Expects a validated form entry.
 * @param {string} ssn
 * @returns {string}
 */
export function formatSSN(ssn: string): string {
  const part1 = ssn.slice(0, 3);
  const part2 = ssn.slice(3, 5);
  const part3 = ssn.slice(5);

  return `${part1}-${part2}-${part3}`;
}

/**
 * Function to format the entered phone number into a more readable version. Expects a validated form entry.
 * @param {string} number
 * @returns {string}
 */
export function formatPhoneNumber(number: string): string {
  if (number) {
    const areaCode = number.slice(0, 3);
    const first = number.slice(3, 6);
    const last = number.slice(6);

    return `(${areaCode}) ${first}-${last}`;
  }

  return '';
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
  );
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
  );
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
  );
}

/**
 * Function to format persons's address
 * @param {object} data
 * @returns {string}
 */
export function formatAddress(data): string {
  const {
    application: {
      currentAddress: { addressLine1, addressLine2, city, state, country, zip },
    },
  } = data;

  return (
    `${addressLine1}, ${addressLine2}, ${city}, ${state},
        ${country}, ${zip}
      ` || ''
  );
}

/**
 * Function to format persons initials
 * @param {object} data
 * @returns {string}
 */
export function formatInitials(data): string {
  return (
    `${data?.application?.personalInfo?.firstName.charAt(
      0
    )}${data?.application?.personalInfo?.lastName.charAt(0)}` || ''
  );
}

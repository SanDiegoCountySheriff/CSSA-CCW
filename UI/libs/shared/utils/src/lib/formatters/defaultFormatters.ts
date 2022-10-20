/**
 * Function to format the entered social security number into a more readable version.
 * Expects a validated form entry.
 * @param {string} ssn
 * @returns {string}
 */
export function formatSSN(ssn: string): string {
  const part1 = ssn.slice(0, 3);
  const part2 = ssn.slice(3, 6);
  const part3 = ssn.slice(6);

  return part1 + '-' + part2 + '-' + part3;
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

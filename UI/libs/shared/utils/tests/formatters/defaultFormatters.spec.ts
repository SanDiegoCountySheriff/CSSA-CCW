import {
  formatPhoneNumber,
  formatSSN,
} from '@shared-utils/formatters/defaultFormatters';

describe('formatSSN', () => {
  it('Should return a correctly formatted SSN', () => {
    const result = formatSSN('111223333');

    expect(result).toBe('111-22-3333');
  });
});

describe('formatPhoneNumber', () => {
  it('should return a correctly formatted Phone number', () => {
    const result = formatPhoneNumber('1112223333');

    expect(result).toBe('(111) 222-3333');
  });

  it('Should return empty if no number', () => {
    const result = formatPhoneNumber('');

    expect(result).toBe('');
  });
});

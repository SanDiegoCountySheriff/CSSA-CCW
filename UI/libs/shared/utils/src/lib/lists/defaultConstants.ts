import {
  CompleteApplication,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes';

export const hairColors = [
  'Black',
  'Blond',
  'Brown',
  'Light brown',
  'Gray',
  'White',
  'Unnatural',
];

export const eyeColors = [
  'Black',
  'Brown',
  'Blue',
  'Green',
  'Gray',
  'Hazel',
  'Mixed',
];

export const formOneStepNames = [
  'Personal',
  'CitizenShip',
  'Address',
  'Appearance',
  'Contact',
];

export const formTwoStepName = [
  'Employment & Weapons',
  'Application Type',
  'Files',
  'Signature',
];

export const employmentStatus = ['Employed', 'Unemployed', 'Retired'];

export const defaultPermitState: CompleteApplication = {
  application: {
    dob: {
      birthDate: '',
      birthCity: '',
      birthCountry: '',
      birthState: '',
    },
    aliases: [],
    applicationType: '',
    citizenship: { citizen: true, militaryStatus: '' },
    contact: {
      cellPhoneNumber: '',
      faxPhoneNumber: '',
      primaryPhoneNumber: '',
      textMessageUpdates: false,
      workPhoneNumber: '',
    },
    currentAddress: {
      addressLine1: '',
      addressLine2: '',
      city: '',
      country: '',
      county: '',
      state: '',
      zip: '',
    },
    differentMailing: false,
    differentSpouseAddress: false,
    employment: '',
    idInfo: {
      idNumber: '',
      issuingState: '',
    },
    immigrantInformation: {
      countryOfCitizenship: '',
      countryOfBirth: '',
      immigrantAlien: false,
      nonImmigrantAlien: false,
    },
    isComplete: false,
    license: {
      permitNumber: '',
      issuingCounty: '',
      expirationDate: '',
    },
    mailingAddress: {
      addressLine1: '',
      addressLine2: '',
      city: '',
      country: '',
      county: '',
      state: '',
      zip: '',
    },
    personalInfo: {
      lastName: '',
      firstName: '',
      middleName: '',
      noMiddleName: false,
      maidenName: '',
      suffix: '',
      ssn: '',
      maritalStatus: '',
    },
    physicalAppearance: {
      eyeColor: '',
      gender: '',
      hairColor: '',
      heightFeet: '',
      heightInch: '',
      physicalDesc: '',
      weight: '',
    },
    previousAddresses: [],
    qualifyingQuestions: {
      questionOne: null,
      questionOneExp: '',
      questionTwo: null,
      questionTwoExp: '',
      questionThree: null,
      questionThreeExp: '',
      questionFour: null,
      questionFourExp: '',
      questionFive: null,
      questionFiveExp: '',
      questionSix: null,
      questionSixExp: '',
      questionSeven: null,
      questionSevenExp: '',
      questionEight: null,
      questionEightExp: '',
      questionNine: null,
      questionNineExp: '',
      questionTen: null,
      questionTenExp: '',
      questionEleven: null,
      questionElevenExp: '',
      questionTwelve: null,
      questionTwelveExp: '',
      questionThirteen: null,
      questionThirteenExp: '',
      questionFourteen: null,
      questionFourteenExp: '',
      questionFifteen: null,
      questionFifteenExp: '',
      questionSixteen: null,
      questionSixteenExp: '',
      questionSeventeen: null,
      questionSeventeenExp: '',
    },
    spouseInformation: {
      lastName: '',
      firstName: '',
      middleName: '',
      maidenName: '',
    },
    spouseAddressInformation: {
      addressLine1: '',
      addressLine2: '',
      city: '',
      country: '',
      county: '',
      state: '',
      zip: '',
    },
    userEmail: '',
    weapons: [],
    workInformation: {
      employerName: '',
      employerAddressLine1: '',
      employerAddressLine2: '',
      employerCity: '',
      employerCountry: 'United States',
      employerPhone: '',
      employerState: '',
      employerZip: '',
      occupation: '',
    },
    currentStep: 1,
    status: 1,
    appointmentStatus: false,
    orderId: '',
    uploadedDocuments: [] as Array<UploadedDocType>,
    appointmentDateTime: '',
  },
  history: [],
  id: '',
};

export const defaultAllPermitsState = {
  orderID: '',
  name: '',
  address: defaultPermitState.application.currentAddress,
  appointmentStatus: false,
  email: '',
  status: 0,
  isComplete: false,
};

export const states = [
  'Alabama',
  'Alaska',
  'Arizona',
  'Arkansas',
  'California',
  'Colorado',
  'Connecticut',
  'Delaware',
  'Florida',
  'Georigia',
  'Hawaii',
  'Idaho',
  'Illinois',
  'Indiana',
  'Iowa',
  'Kansas',
  'Kentucky',
  'Louisiana',
  'Maine',
  'Maryland',
  'Massachusetts',
  'Michigan',
  'Minnesota',
  'Mississippi',
  'Missouri',
  'Montana',
  'Nebraska',
  'Nevada',
  'New Hampshire',
  'New Jersey',
  'New Mexico',
  'New York',
  'North Carolina',
  'North Dakota',
  'Ohio',
  'Oklahoma',
  'Oregon',
  'Pennsylvania',
  'Rhode Island',
  'South Carolina',
  'South Dakota',
  'Tennessee',
  'Texas',
  'Utah',
  'Vermont',
  'Virginia',
  'Washington',
  'West Virginia',
  'Wisconsin',
  'Wyoming',
];

export const countries = [
  'United States',
  'United Kingdom',
  'Canada',
  'Mexico',
  'Afghanistan',
  'Albania',
  'Algeria',
  'Andorra',
  'Angola',
  'Antigua and Barbuda',
  'Argentina',
  'Armenia',
  'Australia',
  'Austria',
  'Azerbaijan',
  'Bahamas',
  'Bangladesh',
  'Barbados',
  'Belarus',
  'Belgium',
  'Belize',
  'Benin',
  'Bhutan',
  'Bolivia',
  'Bosnia and Herzegovina',
  'Botswana',
  'Brazil',
  'Brunei',
  'Bulgaria',
  'Burkina Faso',
  'Burundi',
  'Cabo Verde',
  'Cambodia',
  'Cameroon',
  'Central African Republic',
  'Chad',
  'Chile',
  'China',
  'Colombia',
  'Comoros',
  'Congo',
  'Costa Rica',
  "Cote d'lvoire",
  'Croatia',
  'Cuba',
  'Cyprus',
  'Czech Republic',
  'Denmark',
  'Djibouti',
  'Dominica',
  'Dominican Republic',
  'East Timor',
  'Ecuador',
  'Egypt',
  'El Salvador',
  'Equatorial Guinea',
  'Eritrea',
  'Estonia',
  'Eswatini',
  'Ethiopia',
  'Fiji',
  'Finland',
  'France',
  'Gabon',
  'The Gambia',
  'Georgia',
  'Ghana',
  'Greece',
  'Grenada',
  'Guatemala',
  'Guinea',
  'Guinea-Bissau',
  'Guyana',
  'Haiti',
  'Honduras',
  'Hungary',
  'Iceland',
  'India',
  'Indonesia',
  'Iran',
  'Iraq',
  'Ireland',
  'Israel',
  'Italy',
  'Jamaica',
  'Japan',
  'Jordan',
  'Kazakhstan',
  'Kenya',
  'Kiribati',
  'Korea, North',
  'Korea, South',
  'Kosovo',
  'Kuwait',
  'Kyrgyzstan',
  'laos',
  'latvia',
  'lebanon',
  'Lesotho',
  'Liberia',
  'Libya',
  'Liechtenstein',
  'Lithuania',
  'Luxembourg',
  'Luxembourg',
  'Madagascar',
  'Malawi',
  'Malaysia',
  'Maldives',
  'Mali',
  'Malta',
  'Marshall Islands',
  'Mauritania',
  'Mauritius',
  'Mexico',
  'Micronesia',
  'Moldova',
  'Monaco',
  'Mongolia',
  'Montenegro',
  'Morocco',
  'Mozambique',
  'Myanmar',
  'Myanmar',
  'Namibia',
  'Nauru',
  'Nepal',
  'Netherlands',
  'New Zealand',
  'Nicaragua',
  'Niger',
  'Nigeria',
  'North Macedonia',
  'Norway',
  'Oman',
  'Pakistan',
  'Palau',
  'Panama',
  'Papua New Guinea',
  'Paraguay',
  'Peru',
  'Philippines',
  'Poland',
  'Portugal',
  'Qatar',
  'Romania',
  'Russia',
  'Rwanda',
  'Saint Kitts and Nevis',
  'Saint Lucia',
  'Saint Vincent and the Grenadines',
  'Samoa',
  'San Marino',
  'Sao Tome and Principe',
  'Saudi Arabia',
  'Senegal',
  'Serbia',
  'Seychelles',
  'Sierra Leone',
  'Singapore',
  'Slovakia',
  'Slovenia',
  'Solomon Islands',
  'Somalia',
  'South Africa',
  'Spain',
  'Sri Lanka',
  'Sudan',
  'Sudan, South',
  'Suriname',
  'Sweden',
  'Switzerland',
  'Syria',
  'Taiwan',
  'Tajikistan',
  'Tanzania',
  'Thailand',
  'Tonga',
  'Trinidad and Tobago',
  'Tunisia',
  'Turkey',
  'Turkmenistan',
  'Tuvalu',
  'Uganda',
  'Ukraine',
  'United Arab Emirates',
  'Uruguay',
  'Uzbekistan',
  'Vanuatu',
  'Vatican City',
  'Venezuela',
  'Vietnam',
  'Yemen',
  'Zambia',
  'Zimbabwe',
];

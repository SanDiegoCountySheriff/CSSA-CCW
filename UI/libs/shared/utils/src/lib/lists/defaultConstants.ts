import {
  ApplicationStatus,
  AppointmentStatus,
  CommentType,
  CompleteApplication,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'

export const adminFileTypes = [
  { name: 'Portrait', value: 'portrait' },
  { name: 'Thumbprint', value: 'thumbprint' },
  { name: 'Signature', value: 'signature' },
  { name: 'Driver License', value: 'driverlicense' },
  { name: 'Proof of Residency', value: 'proofresidency' },
  { name: 'Military Documents', value: 'militarydoc' },
  { name: 'Citizenship', value: 'citizenship' },
  { name: 'Supporting', value: 'supporting' },
  { name: 'Name Change', value: 'namechange' },
  { name: 'Judicial', value: 'judicial' },
  { name: 'Reserve', value: 'reserve' },
  { name: '8 Hour Safety Course', value: 'eightHourSafetyCourse' },
]

export const userFileTypes = [
  { name: 'Driver License', value: 'driverlicense' },
  { name: 'Proof of Residency', value: 'proofresidency' },
  { name: 'Military Documents', value: 'militarydoc' },
  { name: 'Citizenship', value: 'citizenship' },
  { name: 'Supporting', value: 'supporting' },
  { name: 'Name Change', value: 'namechange' },
  { name: 'Judicial Documents', value: 'judicial' },
  { name: 'Reserve Documents', value: 'reserve' },
  { name: '8 Hour Safety Course', value: 'eightHourSafetyCourse' },
]

export const hairColors = [
  'Black',
  'Blond',
  'Brown',
  'Light brown',
  'Gray',
  'White',
  'Unnatural',
]

export const eyeColors = [
  'Black',
  'Brown',
  'Blue',
  'Green',
  'Gray',
  'Hazel',
  'Mixed',
]

export const formOneStepNames = [
  'Personal',
  'CitizenShip',
  'Address',
  'Appearance',
  'Contact',
]

export const formTwoStepName = [
  'Employment & Weapons',
  'Application Type',
  'Files',
  'Signature',
]

export const employmentStatus = ['Employed', 'Unemployed', 'Retired']

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
    ciiNumber: '',
    startOfNinetyDayCountdown: null,
    citizenship: { citizen: true, militaryStatus: '' },
    comments: new Array<CommentType>(),
    cost: {
      new: {
        standard: 1,
        judicial: 1,
        reserve: 1,
      },
      renew: {
        standard: 1,
        judicial: 1,
        reserve: 1,
      },
      issuance: 1,
      modify: 1,
      creditFee: 1,
      convenienceFee: 1,
    },
    contact: {
      cellPhoneNumber: '',
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
      issueDate: '',
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
      questionOneAgency: '',
      questionOneIssueDate: '',
      questionOneNumber: '',
      questionTwo: null,
      questionTwoAgency: '',
      questionTwoDenialDate: '',
      questionTwoDenialReason: '',
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
      phoneNumber: '',
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
    status: ApplicationStatus.Incomplete,
    submittedToLicensingDateTime: null,
    paymentStatus: 0,
    appointmentStatus: AppointmentStatus['Not Scheduled'],
    orderId: '',
    uploadedDocuments: [] as Array<UploadedDocType>,
    adminUploadedDocuments: new Array<UploadedDocType>(),
    appointmentDateTime: null,
    appointmentId: null,
    assignedTo: '',
    backgroundCheck: {
      proofOfID: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      proofOfResidency: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      ncicWantsWarrants: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      locals: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      probations: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      dmvRecord: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      akasChecked: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      coplink: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      trafficCourtPortal: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      propertyAssesor: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      voterRegistration: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      dojApprovalLetter: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      ciiNumber: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      doj: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      fbi: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      sR14: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      firearms: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      sidLettersReceived: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      safetyCertificate: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
      restrictions: {
        changeDateTimeUtc: null,
        changeMadeBy: null,
        value: null,
      },
    },
    agreements: {
      goodMoralCharacterAgreed: false,
      goodMoralCharacterAgreedDate: null,
      conditionsForIssuanceAgreed: false,
      conditionsForIssuanceAgreedDate: null,
      falseInfoAgreed: false,
      falseInfoAgreedDate: null,
    },
  },

  history: [],
  paymentHistory: [],
  id: '',
  userId: '',
}

export const defaultAllPermitsState = {
  orderId: '',
  name: '',
  address: defaultPermitState.application.currentAddress,
  appointmentStatus: AppointmentStatus.Available,
  email: '',
  status: 0,
  isComplete: false,
}

export const defaultAdminUser = {
  id: '',
  badgeNumber: '',
  name: '',
  jobTitle: '',
  uploadedDocuments: [],
}

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
  'Georgia',
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
]

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
  'Germany',
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
]

export const weaponMake = [
  'Barrett',
  'Benelli',
  'Beretta',
  'Browning',
  'Colt',
  'Glock',
  'Heckler & Koch',
  'Hi-Point',
  'Kel-Tech',
  'Kimber',
  'Mauser',
  'Mossberg',
  'Remington',
  'Ruger',
  'Sig Sauer',
  'Smith & Wesson',
  'Springfield',
  'Taurus',
  'Wilson Combat',
  'Winchester',
]

export const calibers = [
  '.22 LR',
  '.223/5.56mm',
  '.308/7.62mm',
  '32 ACP',
  '357 Magnum',
  '.380 ACP',
  '38 Special',
  '40 S&W',
  '45 ACP',
  '9mm',
  '10mm',
]

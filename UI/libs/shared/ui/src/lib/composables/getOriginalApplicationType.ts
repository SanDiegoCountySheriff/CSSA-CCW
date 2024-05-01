import { ApplicationType } from '@shared-utils/types/defaultTypes'

export function getOriginalApplicationTypeModification(
  applicationType: ApplicationType
): ApplicationType {
  switch (applicationType) {
    case ApplicationType['Modify Standard']: {
      return ApplicationType.Standard
    }
    case ApplicationType['Modify Judicial']: {
      return ApplicationType.Judicial
    }
    case ApplicationType['Modify Reserve']: {
      return ApplicationType.Reserve
    }
    case ApplicationType['Modify Employment']: {
      return ApplicationType.Employment
    }
    default: {
      return ApplicationType.Standard
    }
  }
}

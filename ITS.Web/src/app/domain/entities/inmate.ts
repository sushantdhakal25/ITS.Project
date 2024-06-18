export class Inmate {
    constructor(
      public inmateId: number,
      public name: string,
      public identificationNumber: string,
      public currentFacilityId: number,
      public currentFacilityName: string
    ) {}
  }
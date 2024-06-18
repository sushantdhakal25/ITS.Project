export class Transfer {
    constructor(
      public transferId: number,
      public inmateId: number,
      public inmateName: string,
      public sourceFacilityId: number,
      public sourceFacilityName: string,
      public destinationFacilityId: number,
      public destinationFacilityName: string,
      public departureTime: Date,
      public arrivalTime: Date
    ) {}
  }
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FacilityRepository } from 'src/app/data/repositories/facility.repository';
import { Facility } from 'src/app/domain/entities/facility';

@Injectable({
  providedIn: 'root'
})
export class FacilityService {
  constructor(private facilityRepository: FacilityRepository) {}

  getFacilities(searchText? : string): Observable<Facility[]> {
    return this.facilityRepository.getFacilities(searchText);
  }

  createFacility(facility: Facility): Observable<Facility> {
    return this.facilityRepository.createFacility(facility);
  }

  updateFacility(facility: Facility): Observable<Facility> {
    return this.facilityRepository.updateFacility(facility);
  }

  deleteFacility(facility: Facility): Observable<void> {
    return this.facilityRepository.deleteFacility(facility);
  }
}

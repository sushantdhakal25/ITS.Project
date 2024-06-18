import { Observable } from 'rxjs';
import { Facility } from 'src/app/domain/entities/facility';

export abstract class FacilityRepository {
  abstract getFacilities(searchText? : string): Observable<Facility[]>;
  abstract createFacility(facility: Facility): Observable<Facility>;
  abstract updateFacility(facility: Facility): Observable<Facility>;
  abstract deleteFacility(facility: Facility): Observable<void>;
}
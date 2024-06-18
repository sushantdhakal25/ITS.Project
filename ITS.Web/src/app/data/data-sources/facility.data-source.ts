import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Facility } from '../../domain/entities/facility';
import { FacilityRepository } from '../repositories/facility.repository';
import { environment } from '../../../environments/environment';
import { WebApiService } from 'src/app/core/services/web-api.service';

@Injectable({
  providedIn: 'root'
})
export class FacilityDataSource extends FacilityRepository {
  private readonly facilityApiUrl = `${environment.apiUrl}/facility/`;

  constructor(private webApiService: WebApiService) {
    super();
  }

  getFacilities(searchText? : string): Observable<Facility[]> {
    const params = searchText ? { searchText } : {};
    return this.webApiService.get(this.facilityApiUrl + 'getfacility', params)
  }

  createFacility(facility: Facility): Observable<Facility> {
    return this.webApiService.post(this.facilityApiUrl + 'add', { json: JSON.stringify(facility) });
  }

  updateFacility(facility: Facility): Observable<Facility> {
    return this.webApiService.put(this.facilityApiUrl + 'update', { json: JSON.stringify(facility) });
  }

  deleteFacility(facility: Facility): Observable<void> {
    return this.webApiService.delete(this.facilityApiUrl + 'delete', { json: JSON.stringify(facility) });
  }
}

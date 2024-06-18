import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Officer } from '../../domain/entities/officer';
import { OfficerRepository } from '../repositories/officer.repository';
import { environment } from '../../../environments/environment';
import { WebApiService } from 'src/app/core/services/web-api.service';

@Injectable({
  providedIn: 'root'
})
export class OfficerDataSource extends OfficerRepository {
  private readonly officerApiUrl = `${environment.apiUrl}/officer/`;

  constructor(private webApiService: WebApiService) {
    super();
  }

  getOfficers(searchText? : string): Observable<Officer[]> {
    const params = searchText ? { searchText } : {};
    return this.webApiService.get(this.officerApiUrl + 'getofficer', params)
  }

  createOfficer(officer: Officer): Observable<Officer> {
    return this.webApiService.post(this.officerApiUrl + 'add', { json: JSON.stringify(officer) });
  }

  updateOfficer(officer: Officer): Observable<Officer> {
    return this.webApiService.put(this.officerApiUrl + 'update', { json: JSON.stringify(officer) });
  }

  deleteOfficer(officer: Officer): Observable<void> {
    return this.webApiService.delete(this.officerApiUrl + 'delete', { json: JSON.stringify(officer) });
  }
}

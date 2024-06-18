// src/app/data/data-sources/inmate.data-source.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Inmate } from '../../domain/entities/inmate';
import { InmateRepository } from '../repositories/inmate.repository';
import { environment } from '../../../environments/environment';
import { WebApiService } from 'src/app/core/services/web-api.service';

@Injectable({
  providedIn: 'root'
})
export class InmateDataSource extends InmateRepository {
  private readonly inmateApiUrl = `${environment.apiUrl}/inmate/`;

  constructor(private webApiService: WebApiService) {
    super();
  }

  getInmates(searchText?: string): Observable<Inmate[]> {
    const params = searchText ? { searchText } : {};
    return this.webApiService.get(this.inmateApiUrl + 'getinmate', params);
  }

  createInmate(inmate: Inmate): Observable<Inmate[]> {
    return this.webApiService.post(this.inmateApiUrl + 'add', { json: JSON.stringify(inmate) });
  }

  updateInmate(inmate: Inmate): Observable<Inmate[]> {
    return this.webApiService.put(this.inmateApiUrl + 'update', { Json: JSON.stringify(inmate) });
  }

  deleteInmate(inmate: Inmate): Observable<void> {
    return this.webApiService.delete(this.inmateApiUrl + 'delete', { json: JSON.stringify(inmate) });
  }
}

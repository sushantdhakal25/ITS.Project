import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OfficerRepository } from 'src/app/data/repositories/officer.repository';
import { Officer } from 'src/app/domain/entities/officer';

@Injectable({
  providedIn: 'root'
})
export class OfficerService {
  constructor(private officerRepository: OfficerRepository) {}

  getOfficers(searchText? : string): Observable<Officer[]> {
    return this.officerRepository.getOfficers(searchText);
  }

  createOfficer(officer: Officer): Observable<Officer> {
    return this.officerRepository.createOfficer(officer);
  }

  updateOfficer(officer: Officer): Observable<Officer> {
    return this.officerRepository.updateOfficer(officer);
  }

  deleteOfficer(officer: Officer): Observable<void> {
    return this.officerRepository.deleteOfficer(officer);
  }
}

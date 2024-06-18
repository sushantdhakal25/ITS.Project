import { Observable } from 'rxjs';
import { Officer } from 'src/app/domain/entities/officer';

export abstract class OfficerRepository {
  abstract getOfficers(searchText? : string): Observable<Officer[]>;
  abstract createOfficer(officer: Officer): Observable<Officer>;
  abstract updateOfficer(officer: Officer): Observable<Officer>;
  abstract deleteOfficer(officer: Officer): Observable<void>;
}
import { Observable } from 'rxjs';
import { Inmate } from 'src/app/domain/entities/inmate';

export abstract class InmateRepository {
  abstract getInmates(search?: string): Observable<Inmate[]>;
  abstract createInmate(inmate: Inmate): Observable<Inmate[]>;
  abstract updateInmate(inmate: Inmate): Observable<Inmate[]>;
  abstract deleteInmate(inmate: Inmate): Observable<void>;
}
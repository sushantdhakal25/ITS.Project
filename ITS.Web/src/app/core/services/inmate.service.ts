import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Inmate } from '../../domain/entities/inmate';
import { InmateRepository } from '../../data/repositories/inmate.repository';

@Injectable({
  providedIn: 'root'
})
export class InmateService {
  constructor(private inmateRepository: InmateRepository) {}

  getInmates(searchText? : string): Observable<Inmate[]> {
    return this.inmateRepository.getInmates(searchText);
  }

  createInmate(inmate: Inmate): Observable<Inmate[]> {
    return this.inmateRepository.createInmate(inmate);
  }

  updateInmate(inmate: Inmate): Observable<Inmate[]> {
    return this.inmateRepository.updateInmate(inmate);
  }

  deleteInmate(inmate: Inmate): Observable<void> {
    return this.inmateRepository.deleteInmate(inmate);
  }
}

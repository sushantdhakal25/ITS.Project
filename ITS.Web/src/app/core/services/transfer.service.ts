import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TransferRepository } from 'src/app/data/repositories/transfer.repository';
import { Transfer } from 'src/app/domain/entities/transfer';

@Injectable({
  providedIn: 'root'
})
export class TransferService {
  constructor(private transferRepository: TransferRepository) {}

  getTransfers(searchText? : string): Observable<Transfer[]> {
    return this.transferRepository.getTransfers(searchText);
  }

  createTransfer(transfer: Transfer): Observable<Transfer> {
    return this.transferRepository.createTransfer(transfer);
  }

  updateTransfer(transfer: Transfer): Observable<Transfer> {
    return this.transferRepository.updateTransfer(transfer);
  }

  deleteTransfer(transfer: Transfer): Observable<void> {
    return this.transferRepository.deleteTransfer(transfer);
  }
}

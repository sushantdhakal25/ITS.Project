import { Observable } from 'rxjs';
import { Transfer } from 'src/app/domain/entities/transfer';

export abstract class TransferRepository {
  abstract getTransfers(searchText? : string): Observable<Transfer[]>;
  abstract createTransfer(transfer: Transfer): Observable<Transfer>;
  abstract updateTransfer(transfer: Transfer): Observable<Transfer>;
  abstract deleteTransfer(transfer: Transfer): Observable<void>;
}
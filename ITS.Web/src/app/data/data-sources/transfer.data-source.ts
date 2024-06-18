import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Transfer } from 'src/app/domain/entities/transfer';
import { TransferRepository } from '../repositories/transfer.repository';
import { WebApiService } from 'src/app/core/services/web-api.service';

@Injectable({
  providedIn: 'root'
})
export class TransferDataSource extends TransferRepository {
  private readonly transferApiUrl = `${environment.apiUrl}/transfer/`;

  constructor(private webApiService: WebApiService) {
    super();
  }

  getTransfers(searchText? : string): Observable<Transfer[]> {
    const params = searchText ? { searchText } : {};
    return this.webApiService.get(this.transferApiUrl + 'gettransfer', params)
  }

  createTransfer(transfer: Transfer): Observable<Transfer> {
    return this.webApiService.post(this.transferApiUrl + 'add', { json: JSON.stringify(transfer) });
  }

  updateTransfer(transfer: Transfer): Observable<Transfer> {
    return this.webApiService.put(this.transferApiUrl + 'update', { json: JSON.stringify(transfer) });
  }

  deleteTransfer(transfer: Transfer): Observable<void> {
    return this.webApiService.delete(this.transferApiUrl + 'delete', { json: JSON.stringify(transfer) });
  }
}

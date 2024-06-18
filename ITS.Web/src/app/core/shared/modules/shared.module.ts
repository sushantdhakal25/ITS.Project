import { NgModule } from '@angular/core';
import { FacilityDataSource } from 'src/app/data/data-sources/facility.data-source';
import { InmateDataSource } from 'src/app/data/data-sources/inmate.data-source';
import { OfficerDataSource } from 'src/app/data/data-sources/officer.data-source';
import { TransferDataSource } from 'src/app/data/data-sources/transfer.data-source';
import { FacilityRepository } from 'src/app/data/repositories/facility.repository';
import { InmateRepository } from 'src/app/data/repositories/inmate.repository';
import { OfficerRepository } from 'src/app/data/repositories/officer.repository';
import { TransferRepository } from 'src/app/data/repositories/transfer.repository';

@NgModule({
  providers: [
    { provide: FacilityRepository, useClass: FacilityDataSource },
    { provide: InmateRepository, useClass: InmateDataSource },
    { provide: OfficerRepository, useClass: OfficerDataSource },
    { provide: TransferRepository, useClass: TransferDataSource }
  ]
})
export class SharedModule { }

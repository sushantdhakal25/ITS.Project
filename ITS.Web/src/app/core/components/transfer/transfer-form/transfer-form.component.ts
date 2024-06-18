import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FacilityService } from 'src/app/core/services/facility.service';
import { InmateService } from 'src/app/core/services/inmate.service';
import { TransferService } from 'src/app/core/services/transfer.service';
import { Facility } from 'src/app/domain/entities/facility';
import { Inmate } from 'src/app/domain/entities/inmate';
import { Transfer } from 'src/app/domain/entities/transfer';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-transfer-form',
  templateUrl: './transfer-form.component.html',
  styleUrls: ['./transfer-form.component.scss']
})
export class TransferFormComponent implements OnInit {

  formObject: Transfer = {
    transferId: 0,
    inmateId: 0,
    inmateName: '',
    sourceFacilityId: 0,
    sourceFacilityName: '',
    destinationFacilityId: 0,
    destinationFacilityName: '',
    arrivalTime: new Date(),
    departureTime: new Date(),
  }
  formType: string = '';
  facilities: Facility[] = [];
  inmates: Inmate[] = [];

  constructor(
    private dialogRef: MatDialogRef<TransferFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private transferService: TransferService,
    private facilityService: FacilityService,
    private inmateService: InmateService,
    private datePipe: DatePipe) {
    if (data) {
      this.formObject = { ...data.data };
      if(data.formType == 'edit'){
        this.formObject.arrivalTime = new Date(data.data.arrivalTime);
        this.formObject.departureTime = new Date(data.data.departureTime);
      }
      this.formType = data.formType;
    }
  }

  ngOnInit() {
    this.getFacilities();
    this.getInmates();
  }

  getFacilities() {
    this.facilityService.getFacilities().subscribe(resp => {
      this.facilities = resp ? [...resp] : [];
    })
  }

  getInmates() {
    this.inmateService.getInmates().subscribe(resp => {
      this.inmates = resp ? [...resp] : [];
    })
  }

  onFormSubmit() {
    if (this.formType == 'edit') {
      this.transferService.updateTransfer(this.formObject).subscribe(resp => {
        this.dialogRef.close(resp);
      });
    }
    else {
      this.transferService.createTransfer(this.formObject).subscribe(resp => {
        this.dialogRef.close(resp);
      });
    }
  }

  onReset(form: any) {
    form.resetForm();
    this.formObject = {
      transferId: 0,
      inmateId: -1,
      inmateName: '',
      sourceFacilityId: -1,
      sourceFacilityName: '',
      destinationFacilityId: -1,
      destinationFacilityName: '',
      arrivalTime: new Date(),
      departureTime: new Date(),
    };
  }

}

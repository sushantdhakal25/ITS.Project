import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FacilityService } from 'src/app/core/services/facility.service';
import { Facility } from 'src/app/domain/entities/facility';

@Component({
  selector: 'app-facility-form',
  templateUrl: './facility-form.component.html',
  styleUrls: ['./facility-form.component.scss']
})
export class FacilityFormComponent implements OnInit {

  formObject: Facility = {
    facilityId: 0,
    name: '',
    address: '',
    email: '',
    phone: '',

  }

  formType: string = '';

  facilities :Facility [] = [];

  constructor(
    private dialogRef: MatDialogRef<FacilityFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private facilityService: FacilityService) {
    if (data) {
      this.formObject = { ...data.data };
      this.formType = data.formType;
    }
  }

  ngOnInit() {
  }

  onFormSubmit() {
    if (this.formType=='edit') {
      this.facilityService.updateFacility(this.formObject).subscribe( resp =>{
        this.dialogRef.close(resp);
      });
    }
    else {
        this.facilityService.createFacility(this.formObject).subscribe( resp =>{
        this.dialogRef.close(resp);
      });
    }
  }

  onReset(form: any) {
    form.resetForm();
    this.formObject = {
      facilityId: 0,
      name: '',
      address: '',
      email: '',
      phone: ''
    };
  }

}

import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FacilityService } from 'src/app/core/services/facility.service';
import { InmateService } from 'src/app/core/services/inmate.service';
import { Facility } from 'src/app/domain/entities/facility';
import { Inmate } from 'src/app/domain/entities/inmate';

@Component({
  selector: 'app-inmate-form',
  templateUrl: './inmate-form.component.html',
  styleUrls: ['./inmate-form.component.scss']
})
export class InmateFormComponent implements OnInit {

  formObject: Inmate = {
    inmateId: 0,
    name: '',
    identificationNumber: '',
    currentFacilityId: 0,
    currentFacilityName: '',

  }

  formType: string = '';

  facilities :Facility [] = [];

  constructor(
    private dialogRef: MatDialogRef<InmateFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private inmateService: InmateService,
    private facilityService: FacilityService) {
    if (data) {
      this.formObject = { ...data.data };
      this.formType = data.formType;
    }
  }

  ngOnInit() {
    this.getFacilities();
  }

  getFacilities() {
    this.facilityService.getFacilities().subscribe(resp => {
      this.facilities  = [...resp]
    })
  }

  onFormSubmit() {
    if (this.formType=='edit') {
      this.inmateService.updateInmate(this.formObject).subscribe( resp =>{
        this.dialogRef.close(resp);
      });
    }
    else {
        this.inmateService.createInmate(this.formObject).subscribe( resp =>{
        this.dialogRef.close(resp);
      });
    }
  }

  onReset(form: any) {
    form.resetForm();
    this.formObject = {
      inmateId: 0,
      name: '',
      identificationNumber: '',
      currentFacilityId: -1,
      currentFacilityName: ''
    };
  }

}

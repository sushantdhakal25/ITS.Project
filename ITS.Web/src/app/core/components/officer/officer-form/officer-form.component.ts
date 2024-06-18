import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FacilityService } from 'src/app/core/services/facility.service';
import { OfficerService } from 'src/app/core/services/officer.service';
import { Facility } from 'src/app/domain/entities/facility';
import { Officer } from 'src/app/domain/entities/officer';

@Component({
  selector: 'app-officer-form',
  templateUrl: './officer-form.component.html',
  styleUrls: ['./officer-form.component.scss']
})
export class OfficerFormComponent implements OnInit {

  formObject: Officer = {
    officerId: 0,
    name: '',
    identificationNumber: '',
    password: ''
  }

  formType: string = '';

  facilities :Facility [] = [];

  constructor(
    private dialogRef: MatDialogRef<OfficerFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private officerService: OfficerService) {
    if (data) {
      this.formObject = { ...data.data };
      this.formType = data.formType;
    }
  }

  ngOnInit() {
  }

  onFormSubmit() {
    if (this.formType=='edit') {
      this.officerService.updateOfficer(this.formObject).subscribe( resp =>{
        this.dialogRef.close(resp);
      });
    }
    else {
        this.officerService.createOfficer(this.formObject).subscribe( resp =>{
        this.dialogRef.close(resp);
      });
    }
  }

  onReset(form: any) {
    form.resetForm();
    this.formObject = {
      officerId: 0,
      name: '',
      identificationNumber: '',
      password: ''
    };
  }

}

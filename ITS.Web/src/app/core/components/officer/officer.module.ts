import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfficerComponent } from './officer.component';
import { OfficerService } from '../../services/officer.service';
import { RouterModule, Routes } from '@angular/router';
import {MatTableModule} from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { ConfirmDialogModule } from '../../shared/components/confirm-dialog/confirm-dialog.module';
import { OfficerFormModule } from './officer-form/officer-form.module';

const routes: Routes =[
  {
    path: '',
    component: OfficerComponent
  }
];

@NgModule({
  declarations: [OfficerComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSortModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    FormsModule,
    MatSnackBarModule,
    ConfirmDialogModule,
    OfficerFormModule
  ],
  providers: [OfficerService
  ]
})
export class OfficerModule { }

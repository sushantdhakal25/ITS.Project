import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InmateComponent } from './inmate.component';
import { InmateService } from '../../services/inmate.service';
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
import { InmateFormModule } from './inmate-form/inmate-form.module';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { ConfirmDialogModule } from '../../shared/components/confirm-dialog/confirm-dialog.module';

const routes: Routes =[
  {
    path: '',
    component: InmateComponent
  }
];

@NgModule({
  declarations: [InmateComponent],
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
    InmateFormModule,
    MatSnackBarModule,
    ConfirmDialogModule
  ],
  providers: [InmateService
  ]
})
export class InmateModule { }

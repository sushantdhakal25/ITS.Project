import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { OfficerService } from '../../services/officer.service';
import { Officer } from 'src/app/domain/entities/officer';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from '../../shared/models/confirm-dialog-data';
import { OfficerFormComponent } from './officer-form/officer-form.component';

@Component({
  selector: 'app-officer',
  templateUrl: './officer.component.html',
  styleUrls: ['./officer.component.scss']
})
export class OfficerComponent {

  searchText: string = '';
  selectedIndex: number = 0;
  noData: boolean = false;
  selectedRow: Officer | null = null;

  displayedColumns: string[] = ['IdentificationNumber', 'Name'];
  dataSource = new MatTableDataSource<Officer>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private matDialog: MatDialog,
    private snackBar: MatSnackBar,
    private officerService: OfficerService) { }

  ngOnInit() {
    this.getOfficers();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  getOfficers(searchText?: string) {
    this.officerService.getOfficers(searchText).subscribe(resp => {
      if(resp){
        this.dataSource.data = [...resp];
        this.noData = resp.length === 0;
      }else{
        this.noData = true;
      }
    })
  }

  onRowClick(row: any, index: number) {
    this.selectedIndex = index;
    this.selectedRow = row;
  }

  onActionClick(action: string) {
    switch (action) {
      case 'add':
        this.openAddEditForm(action);
        break;
      case 'edit':
        if (this.selectedRow) {
          this.openAddEditForm(action);
        }
        else {
          this.openSnackBar("Please Select Row to Edit", "Warning!!");
        }
        break;
      case 'delete':
        if (this.selectedRow) {
          this.onDeleteClick();
        }
        else {
          this.openSnackBar("Please Select Row to Delete", "Warning!!");
        }
        break;
    }
  }

  openAddEditForm(action: string) {
    var formData = {
      formType: action,
      data: action == 'add' ? null : this.selectedRow
    }
    const dialogRef = this.matDialog.open(OfficerFormComponent, { width: "50%", data: formData });
    dialogRef.afterClosed().subscribe(response => {
      if (response) {
        if (action == 'add') {
          const newData = [...this.dataSource.data];
          newData.unshift(response[0]);
          this.dataSource.data = newData;
          this.openSnackBar("Added Sucessfully", "Sucess!!");
        }
        else {
          const newData = [...this.dataSource.data];
          newData[this.selectedIndex] = { ...response[0] };
          this.dataSource.data = newData;
          this.openSnackBar("Edited Sucessfully", "Sucess!!");
        }
      }
    });
  }

  onDeleteClick() {
    const dialogData: ConfirmDialogData = {
      message: 'Are you sure you want to delete this item?',
      confirmButtonText: 'Delete',
      cancelButtonText: 'Cancel'
    };

    const dialogRef = this.matDialog.open(ConfirmDialogComponent,{ data: dialogData});
      dialogRef.afterClosed().subscribe(response => {
        if (response == true) {
          this.officerService.deleteOfficer(this.selectedRow!).subscribe(response => {
            const newData = [...this.dataSource.data];
            newData.splice(this.selectedIndex, 1);
            this.dataSource.data = newData;
            this.selectedIndex = 0;
            this.openSnackBar("Deleted Sucessfully", "Sucess!!");
          });
        }
      });
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

  search(e: any) {
    this.getOfficers(e);
  }

  referesh() {
    this.searchText = '';
    this.getOfficers();
    this.noData = false;
  }
}

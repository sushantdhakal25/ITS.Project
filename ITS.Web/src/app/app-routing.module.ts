import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const approutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./core/components/dashboard/dashboard.module').then(x => x.DashboardModule)
  },
  {
    path: 'inmate',
    loadChildren: () => import('./core/components/inmate/inmate.module').then(x => x.InmateModule)
  },
  {
    path: 'facility',
    loadChildren: () => import('./core/components/facility/facility.module').then(x => x.FacilityModule)
  },
  {
    path: 'officer',
    loadChildren: () => import('./core/components/officer/officer.module').then(x => x.OfficerModule)
  },
  {
    path: 'transfer',
    loadChildren: () => import('./core/components/transfer/transfer.module').then(x => x.TransferModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(approutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

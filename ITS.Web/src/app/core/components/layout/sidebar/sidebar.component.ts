import { Component, Signal, computed, signal } from '@angular/core';
import { LayoutService } from 'src/app/core/services/layout.service';

interface MenuItem {
  icon: string;
  label: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
 
  collapsed: Signal<boolean>;
  sidenavWidth: Signal<string>;
  profileImageSize: Signal<string>;

  constructor(private layoutService: LayoutService) {
    this.collapsed = this.layoutService.collapsed;
    this.sidenavWidth = computed(() => (this.collapsed() ? '65px' : '250px'));
    this.profileImageSize = computed(() => (this.collapsed() ? '32' : '100'));
  }

  
  
  menuItems = signal<MenuItem[]>([
    {
      icon: "dashboard",
      label: "Dashboard",
      route: "dashboard",
    },
    {
      icon: "personal_injury",
      label: "Inmate",
      route: "inmate",
    },
    {
      icon: "ballot",
      label: "Facility",
      route: "facility",
    },
    {
      icon: "local_police",
      label: "Officer",
      route: "officer",
    },
    {
      icon: "low_priority",
      label: "Transfer",
      route: "transfer",
    }
  ]);
}

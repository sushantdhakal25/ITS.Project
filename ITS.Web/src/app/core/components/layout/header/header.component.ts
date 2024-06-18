import { Component, computed, signal } from '@angular/core';
import { LayoutService } from 'src/app/core/services/layout.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  
  constructor(private layoutService: LayoutService) {}
  
  toggleSidenav(): void {
    this.layoutService.toggle();
  }
}

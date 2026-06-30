import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Sidebar } from '../../shared/sidebar/sidebar';
import { Topbar } from '../../shared/topbar/topbar';

@Component({
  selector: 'app-dashboard-layout',
  imports: [RouterOutlet, Sidebar, Topbar],
  templateUrl: './dashboard-layout.html',
  styleUrl: './dashboard-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardLayout {}
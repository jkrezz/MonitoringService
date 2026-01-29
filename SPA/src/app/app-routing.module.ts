import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DevicesPageComponent } from './pages/devices/devices-page.component';
import { DevicePageComponent } from './pages/device/device-page.component';

const routes: Routes = [
  { path: '', component: DevicesPageComponent, pathMatch: 'full' },
  { path: 'devices/:id', component: DevicePageComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}


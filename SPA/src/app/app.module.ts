import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { DevicesPageComponent } from './pages/devices/devices-page.component';
import { DevicePageComponent } from './pages/device/device-page.component';

@NgModule({
  declarations: [
    AppComponent,
    DevicesPageComponent,
    DevicePageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

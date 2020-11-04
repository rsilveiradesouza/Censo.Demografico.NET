import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { EscolaridadePipe } from './pipe/escolaridade.pipe';
import { GeneroPipe } from './pipe/generos.pipe';
import { EtniaPipe } from './pipe/etnias.pipe';
import { RegioesPipe } from './pipe/regioes.pipe';
import { HubService } from './service/hub.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    NgApexchartsModule,
    HttpClientModule
  ],
  providers: [
    EscolaridadePipe,
    GeneroPipe,
    EtniaPipe,
    RegioesPipe,
    HubService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { Component, NgZone, OnInit } from '@angular/core';
import { EscolaridadePipe } from './pipe/escolaridade.pipe';
import { EtniaPipe } from './pipe/etnias.pipe';
import { GeneroPipe } from './pipe/generos.pipe';
import { RegioesPipe } from './pipe/regioes.pipe';
import { HubService } from './service/hub.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public chartOptions = {
    chart: {
      width: 380,
      type: 'pie',
    },
    legend: {
      show: true,
      position: 'bottom'
    },
    labels: ['Team A', 'Team B', 'Team C', 'Team D', 'Team E'],
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 200
          },
          legend: {
            show: false
          }
        }
      }
    ]
  };

  public data = {
    regiao: {
      labels: [],
      data: [],
    },
    generos: {
      labels: [],
      data: [],
    },
    escolaridade: {
      labels: [],
      data: [],
    },
    etnias: {
      labels: [],
      data: [],
    }
  };
  public loading = false;

  title = 'censo-web';

  constructor(
    public hubService: HubService,
    private _ngZone: NgZone,
    private escolaridadepipe: EscolaridadePipe,
    private generosPipe: GeneroPipe,
    private etniasPipe: EtniaPipe,
    private regiaoPipe: RegioesPipe,
    private http: HttpClient) {
  }

  ngOnInit() {
    this.obterData();
    this.hubService.start();
    this.hubService.verification.subscribe((data: { data: any, status: 'update' }) => {
      this._ngZone.run(async () => {
        if (data.status === 'update') {
          this.data = {
            escolaridade: {
              labels: data.data?.escolaridades ? data.data?.escolaridades.map(e => this.escolaridadepipe.enum(Number(e.chave))) : [],
              data: data.data?.escolaridades ? data.data?.escolaridades.map(e => e.quantidade) : []
            },
            etnias: {
              labels: data.data?.etnias ? data.data?.etnias.map(e => this.etniasPipe.enum(Number(e.chave))) : [],
              data: data.data?.etnias ? data.data?.etnias.map(e => e.quantidade) : []
            },
            generos: {
              labels: data.data?.generos ? data.data?.generos.map(e => this.generosPipe.enum(Number(e.chave))) : [],
              data: data.data?.generos ? data.data?.generos.map(e => e.quantidade) : []
            },
            regiao: {
              labels: data.data?.regioes ? data.data?.regioes.map(e => this.regiaoPipe.enum(Number(e.chave))) : [],
              data: data.data?.regioes ? data.data?.regioes.map(e => e.quantidade) : []
            },
          };
        }
      });
    });
  }

  obterData() {
    this.loading = true;
    this.http.get('http://localhost:8080/api/pesquisa').toPromise().then((response: any[]) => {
      const escolaridade = this.removeDuplicate(response, 'escolaridade');
      const etnias = this.removeDuplicate(response, 'etnia');
      const generos = this.removeDuplicate(response, 'genero');
      const regiao = this.removeDuplicate(response, 'regiao');
      this.data = {
        escolaridade: {
          labels: escolaridade.map(e => this.escolaridadepipe.enum(Number(e))),
          data: escolaridade.map(e => response.filter(f => f.pesquisa.escolaridade === e).length)
        },
        etnias: {
          labels: etnias.map(e => this.etniasPipe.enum(Number(e))),
          data: etnias.map(e => response.filter(f => f.pesquisa.etnia === e).length)
        },
        generos: {
          labels: generos.map(e => this.generosPipe.enum(Number(e))),
          data: generos.map(e => response.filter(f => f.pesquisa.genero === e).length)
        },
        regiao: {
          labels: regiao.map(e => this.regiaoPipe.enum(Number(e))),
          data: regiao.map(e => response.filter(f => f.pesquisa.regiao === e).length)
        },
      };
    }).catch(err => { })
      .finally(() => {
        this.loading = false;
      });
  }

  removeDuplicate(original, key) {
    return original.map(e => e.pesquisa[key]).filter((item, pos) => {
      return original.map(e => e.pesquisa[key]).indexOf(item) === pos;
    });
  }
}

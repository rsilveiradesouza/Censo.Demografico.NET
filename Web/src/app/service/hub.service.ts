import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType, LogLevel } from '@aspnet/signalr';

@Injectable({
    providedIn: 'root'
})
export class HubService {
    verification = new EventEmitter<{data: any, status: 'update'}>();
    connectionEstablished = new EventEmitter<boolean>();

    private _hubConnection: HubConnection;

    constructor() { }

    public start() {
        this.createConnection();
        this.registerOnServerEvents();
        this.startConnection();
    }

    public hasConnection() {
        return this._hubConnection;
    }

    private createConnection(): void {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:8080/hub/dashboard', {
                transport: HttpTransportType.LongPolling,
      })
            .configureLogging(LogLevel.Information)
            .build();
    }

    private startConnection(): void {
        Object.defineProperty(WebSocket, 'OPEN', { value: 1, });
        this._hubConnection
            .start()
            .then(() => {
                this.connectionEstablished.emit(true);
            })
            .catch(err => {
            });
    }

    public cancelConnection(timedOut = false) {
        this._hubConnection.stop();
    }

    private registerOnServerEvents(): void {
        this._hubConnection.on('AtualizarDashboard', (data: any) => {
            this.verification.emit({data, status: 'update'});
        });
    }
}
